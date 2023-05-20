#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Data;

using System.Linq.Dynamic.Core;
using System.IO.Compression;

namespace Api.Controllers;

[AllowAnonymous]
public class ExecutionsController : Controller
{
	private readonly ApplicationDbContext _context;

	public ExecutionsController(ApplicationDbContext context) => _context = context;

	public async Task<IActionResult> Index(Guid? testCaseId)
	{
		var testCase = await _context.TestCases.FindAsync(testCaseId);

		ViewData["TestCase"] = testCase;
		return View();
	}

	// POST: Executions
	[HttpPost]
	public async Task<IActionResult> List(Guid? id)
	{
		var totalRecord = 0;
		var filterRecord = 0;

		var draw = Request.Form["draw"].FirstOrDefault();
		var tableLength = Convert.ToInt32(Request.Form["tableLength"].FirstOrDefault());
		var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
		var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
		var searchValue = Request.Form["search[value]"].FirstOrDefault();

		var pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
		var skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

		var query = _context.Executions
			.Where(e => e.TestCaseId == id && e.SuiteExecutionId == null && e.User.UserRoles.FirstOrDefault().Role.Name != "Root")
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
			.Select(e => new
			{
				e.ExecutionId,
				e.ExecutionDate,
				e.TestEnvironment,
				e.ExecutionStatus,
				testCase = e.TestCase.Name,
				executor = e.User.Name + " " + e.User.LastName + " " + e.User.MothersLastName,
				company = e.User.Company.Name,
				role = e.User.UserRoles.FirstOrDefault().Role.Name
			})
			.OrderByDescending(e => e.ExecutionDate)
			.AsQueryable();

		totalRecord = query.Count();

		if (!String.IsNullOrEmpty(searchValue) && !String.IsNullOrWhiteSpace(searchValue))
			query = query.Where(e =>
				e.testCase.ToLower().Contains(Convert.ToString(searchValue).ToLower()) ||
				e.executor.ToLower().Contains(Convert.ToString(searchValue).ToLower()) ||
				e.company.ToLower().Contains(Convert.ToString(searchValue).ToLower()) ||
				e.role.ToLower().Contains(Convert.ToString(searchValue).ToLower())
			);

		for (var i = 0; i < tableLength; i++)
		{
			var columnName = Request.Form[$"columns[{i}][data]"];
			var columnValue = Request.Form[$"columns[{i}][search][value]"];

			if (!String.IsNullOrEmpty(columnValue) && !String.IsNullOrWhiteSpace(columnValue))
			{
				switch (columnName)
				{
					case "testCase":
						query = query.Where(e => e.testCase.ToLower().Contains(Convert.ToString(columnValue).ToLower()));
						break;

					case "executionDate":
						query = query.Where(e => e.ExecutionDate.Date == DateTime.Parse(columnValue).Date);
						break;

					case "testEnvironment":
						query = query.Where(e => e.TestEnvironment == (TestEnvironment)Enum.Parse(typeof(TestEnvironment), Convert.ToString(columnValue)));
						break;

					case "executor":
						query = query.Where(e => e.executor.ToLower().Contains(Convert.ToString(columnValue).ToLower()));
						break;

					case "role":
						query = query.Where(e => e.role.ToLower().Contains(Convert.ToString(columnValue)));
						break;

					case "company":
						query = query.Where(e => e.company.ToLower().Contains(Convert.ToString(columnValue)));
						break;

					case "executionStatus":
						query = query.Where(e => e.ExecutionStatus == (ExecutionStatus)Enum.Parse(typeof(ExecutionStatus), Convert.ToString(columnValue)));
						break;
				}
			}
		}

		filterRecord = query.Count();

		if (!String.IsNullOrEmpty(sortColumn) && !String.IsNullOrEmpty(sortColumnDirection))
			query = query.OrderBy($"{sortColumn} {sortColumnDirection}");

		var list = await query.Skip(skip).Take(pageSize).ToListAsync();
		var executions = new { draw, recordsTotal = totalRecord, recordsFiltered = filterRecord, data = list };

		return Json(executions);
	}

	// GET: Executions/Get/5
	public async Task<IActionResult> Get(Guid? id)
	{
		if (id == null || _context.Executions == null)
			return NotFound();

		var execution = await _context.Executions
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
			.Select(e => new
			{
				e.ExecutionId,
				e.TestCaseId,
				e.SuiteExecutionId,
				e.UserId,
				e.ExecutionDate,
				e.BrowserDriver,
				e.TestEnvironment,
				e.TestData,
				TestReport = Decompress(e.TestReport),
				e.ExecutionStatus,
				e.Remarks,
				e.TestCase,
				e.SuiteExecution,
				e.Defect,
				e.User
			})
			.FirstOrDefaultAsync(e => e.ExecutionId == id);

		if (execution == null)
			return NotFound();

		return Json(execution);
	}

	public static Byte[] Decompress(Byte[] testReport)
	{
		var input = new MemoryStream(testReport);
		var output = new MemoryStream();

		using (var deflateStream = new DeflateStream(input, CompressionMode.Decompress))
		{
			deflateStream.CopyToAsync(output).Wait();
		}

		return output.ToArray();
	}
}