#nullable disable
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Data;

namespace Api.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ReportsController(ApplicationDbContext context) => _context = context;

    //GET: api/Reports

    public async Task<ActionResult<IEnumerable<Execution>>> GetExecutions([FromQuery] DateTime startDate, DateTime endDate, Guid userId)
    {
        var user = await _context.Users.Where(u => u.Id == userId)
            .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
            .Include(u => u.Company)
            .FirstOrDefaultAsync();

        if (user.UserRoles.FirstOrDefault().Role.Name == "Root" || user.UserRoles.FirstOrDefault().Role.Name == "Manager")
        {
            return Ok(await _context.Executions.Where(e => e.ExecutionDate.Date >= startDate.Date && e.ExecutionDate.Date <= endDate.Date)
                .Include(e => e.Defect)
                    .ThenInclude(e => e.User)
                .Include(e => e.TestCase)
                    .ThenInclude(e => e.BusinessProcess)
                        .ThenInclude(e => e.TestApplication)
                .Include(e => e.User)
                    .ThenInclude(e => e.UserRoles)
                        .ThenInclude(e => e.Role)
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
                    e.Remarks,
                    e.TestCase,
                    e.SuiteExecution,
                    e.Defect,
                    e.User
                })
                .OrderByDescending(e => e.ExecutionDate)
                .ToListAsync()
            );
        }
        else
        {
            if (user.UserRoles.FirstOrDefault().Role.Name == "Administrator")
            {
                return Ok(await _context.Executions.Where(e => e.ExecutionDate.Date >= startDate.Date &&
                        e.ExecutionDate.Date <= endDate.Date && e.User.Company.CompanyId == user.CompanyId)
                    .Include(e => e.Defect)
                        .ThenInclude(e => e.User)
                    .Include(e => e.TestCase)
                        .ThenInclude(e => e.BusinessProcess)
                            .ThenInclude(e => e.TestApplication)
                    .Include(e => e.User)
                        .ThenInclude(e => e.UserRoles)
                            .ThenInclude(e => e.Role)
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
					    e.Remarks,
					    e.TestCase,
					    e.SuiteExecution,
					    e.Defect,
					    e.User
				    })
					.OrderByDescending(e => e.ExecutionDate)
                    .ToListAsync()
                );
            }
            else
            {
                return Ok(await _context.Executions.Where(e => e.ExecutionDate.Date >= startDate.Date && e.ExecutionDate.Date <= endDate.Date && e.UserId == user.Id)
                    .Include(e => e.Defect)
                        .ThenInclude(e => e.User)
                    .Include(e => e.TestCase)
                        .ThenInclude(e => e.BusinessProcess)
                            .ThenInclude(e => e.TestApplication)
                    .Include(e => e.User)
                        .ThenInclude(e => e.UserRoles)
                            .ThenInclude(e => e.Role)
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
					    e.Remarks,
					    e.TestCase,
					    e.SuiteExecution,
					    e.Defect,
					    e.User
				    })
					.OrderByDescending(e => e.ExecutionDate)
                    .ToListAsync()
                );
            }
        }
    }
}