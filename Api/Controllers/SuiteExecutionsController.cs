#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Data;

using System.Linq.Dynamic.Core;

namespace Api.Controllers;

[AllowAnonymous]
public class SuiteExecutionsController : Controller
{
	private readonly ApplicationDbContext _context;

	public SuiteExecutionsController(ApplicationDbContext context) => _context = context;

	public async Task<IActionResult> Index(Guid? businessProcessId)
	{
		var businessProcess = await _context.BusinessProcesses.FindAsync(businessProcessId);

		ViewData["BusinessProcess"] = businessProcess;
		return View();
	}

	// POST: SuiteExecutions
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

		var query = _context.SuiteExecutions.Where(e => e.BusinessProcessId == id && e.User.UserRoles.FirstOrDefault().Role.Name != "Root")
			.Include(e => e.BusinessProcess)
			.Include(e => e.User)
				.ThenInclude(e => e.UserRoles)
					.ThenInclude(e => e.Role)
			.Select(e => new
			{
				e.SuiteExecutionId,
				e.BusinessProcessId,
				e.UserId,
				e.ExecutionDate,
				e.Remarks,
				businessProcess = e.BusinessProcess.Name,
				executor = e.User.Name + " " + e.User.LastName + " " + e.User.MothersLastName,
				company = e.User.Company.Name,
				role = e.User.UserRoles.FirstOrDefault().Role.Name
			})
			.OrderByDescending(e => e.ExecutionDate)
			.AsQueryable();

		totalRecord = query.Count();

		if (!String.IsNullOrEmpty(searchValue) && !String.IsNullOrWhiteSpace(searchValue))
			query = query.Where(e =>
				e.businessProcess.ToLower().Contains(Convert.ToString(searchValue).ToLower()) ||
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
					case "businessProcess":
						query = query.Where(e => e.businessProcess.ToLower().Contains(Convert.ToString(columnValue).ToLower()));
						break;

					case "executionDate":
						query = query.Where(e => e.ExecutionDate.Date == DateTime.Parse(columnValue).Date);
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
				}
			}
		}

		filterRecord = query.Count();

		if (!String.IsNullOrEmpty(sortColumn) && !String.IsNullOrEmpty(sortColumnDirection))
			query = query.OrderBy($"{sortColumn} {sortColumnDirection}");

		var list = await query.Skip(skip).Take(pageSize).ToListAsync();
		var suiteExecutions = new { draw, recordsTotal = totalRecord, recordsFiltered = filterRecord, data = list };

		return Json(suiteExecutions);
	}

	// GET: SuiteExecutions/Get/5
	public async Task<IActionResult> Get(Guid? id)
	{
		if (id == null || _context.SuiteExecutions == null)
			return NotFound();

		var suiteExecution = await _context.SuiteExecutions.Where(e => e.SuiteExecutionId == id)
			.Include(e => e.Executions)
				.OrderByDescending(e => e.ExecutionDate)
			.Include(e => e.User)
				.ThenInclude(e => e.UserRoles)
					.ThenInclude(e => e.Role)
			.Include(e => e.User)
				.ThenInclude(e => e.Company)
			.Include(e => e.Executions)
				.ThenInclude(e => e.TestCase)
			.Include(e => e.BusinessProcess)
				.ThenInclude(e => e.TestApplication)
			.Select(e => new
			{
				e.SuiteExecutionId,
				e.BusinessProcessId,
				e.UserId,
				e.ExecutionDate,
				e.Remarks,
				e.BusinessProcess,
				executions = e.Executions.Select(e => new
				{
					e.ExecutionId,
					e.ExecutionDate,
					e.TestEnvironment,
					e.ExecutionStatus,
					executor = e.User.DisplayName,
					company = e.User.Company.Name,
					role = e.User.UserRoles.FirstOrDefault().Role.Name,
					email = e.User.Email,
					phoneNumber = e.User.PhoneNumber,
					testCase = e.TestCase.Name
				}).ToList(),
				executor = e.User.DisplayName,
				company = e.User.Company.Name,
				role = e.User.UserRoles.FirstOrDefault().Role.Name,
				email = e.User.Email,
				phoneNumber = e.User.PhoneNumber,
				passExecutions = e.Executions.Where(e => e.ExecutionStatus == ExecutionStatus.Pass).Count(),
				failExecutions = e.Executions.Where(e => e.ExecutionStatus == ExecutionStatus.Fail).Count(),
				stopExecutions = e.Executions.Where(e => e.ExecutionStatus == ExecutionStatus.Stop).Count(),
				notRunExecutions = e.Executions.Where(e => e.ExecutionStatus == ExecutionStatus.NotRun).Count()
			})
			.FirstOrDefaultAsync();

		if (suiteExecution == null)
			return NotFound();

		return Json(suiteExecution);
	}
}