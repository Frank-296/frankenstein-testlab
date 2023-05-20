#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using ClosedXML.Excel;

using Api.Data;

using System.Data;

namespace Api.Controllers;

[AllowAnonymous]
public class ReportsController : Controller
{
	private readonly ApplicationDbContext _context;

	public ReportsController(ApplicationDbContext context) => _context = context;

	public IActionResult Index() => View();

	//POST: api/Reports
	[HttpPost]
	public async Task<IActionResult> Create(DateTime startDate, DateTime endDate) =>
		Json(await _context.Executions.Where(e => e.ExecutionDate.Date >= startDate.Date &&
			e.ExecutionDate.Date <= endDate.Date &&
			e.User.UserRoles.FirstOrDefault().Role.Name != "Root")
				.OrderByDescending(e => e.ExecutionDate)
				.Select(e => new
				{
					e.ExecutionId,
					e.TestCaseId,
					e.SuiteExecutionId,
					e.UserId,
					e.ExecutionDate,
					e.BrowserDriver,
					e.TestEnvironment,
					e.ExecutionStatus,
					e.Remarks
				})
				.GroupBy(e => e.ExecutionStatus)
				.ToListAsync());

	//POST: api/Reports/Download
	public async Task<IActionResult> Download(DateTime startDate, DateTime endDate, String reportTitle)
	{
		var executions = await _context.Executions.Where(e => e.ExecutionDate.Date >= startDate.Date &&
			e.ExecutionDate.Date <= endDate.Date &&
			e.User.UserRoles.FirstOrDefault().Role.Name != "Root")
				.Include(e => e.Defect)
					.ThenInclude(e => e.User)
				.Include(e => e.TestCase)
					.ThenInclude(e => e.BusinessProcess)
						.ThenInclude(e => e.TestApplication)
				.Include(e => e.User)
					.ThenInclude(e => e.UserRoles)
						.ThenInclude(e => e.Role)
				.Include(e => e.User)
					.ThenInclude(e => e.Company)
				.OrderByDescending(e => e.ExecutionDate)
				.Select(e => new
				{
					TestApplication = e.TestCase.BusinessProcess.TestApplication.Name,
					BusinessProcess = e.TestCase.BusinessProcess.Name,
					e.TestCase.TestType,
					TestCase = e.TestCase.Name,
					TestCaseDescription = e.TestCase.Description,
					e.ExecutionDate,
					e.TestEnvironment,
					e.BrowserDriver,
					Executor = e.User.DisplayName,
					Role = e.User.UserRoles.FirstOrDefault().Role.Name,
					e.ExecutionStatus,
					e.Remarks
				})
				.ToListAsync();

		var dataTable = new DataTable(reportTitle);

		dataTable.Columns.AddRange(new DataColumn[13]
		{
			new DataColumn("#"),
			new DataColumn("Application"),
			new DataColumn("Business process"),
			new DataColumn("Test type"),
			new DataColumn("Test case"),
			new DataColumn("Description"),
			new DataColumn("Date and time of execution"),
			new DataColumn("Environment"),
			new DataColumn("Browser"),
			new DataColumn("Executor"),
			new DataColumn("Role"),
			new DataColumn("Status"),
			new DataColumn("Comments")
		});

		foreach (var execution in executions)
			dataTable.Rows.Add(executions.IndexOf(execution) + 1,
				execution.TestApplication,
				execution.BusinessProcess,
				execution.TestType,
				execution.TestCase,
				execution.TestCaseDescription,
				execution.ExecutionDate,
				execution.TestEnvironment,
				execution.BrowserDriver,
				execution.Executor,
				execution.Role,
				execution.ExecutionStatus,
				execution.Remarks
			);

		var workBook = new XLWorkbook();
		var workSheet = workBook.Worksheets.Add(dataTable);

		workSheet.RangeUsed().Style.Font.FontName = "Cambria";
		workSheet.ColumnsUsed().AdjustToContents();

		var memoryStream = new MemoryStream();

		workBook.SaveAs(memoryStream);

		var content = memoryStream.ToArray();
		var fileName = $"{reportTitle}.xlsx";
		var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

		return File(content, contentType, fileName);
	}
}