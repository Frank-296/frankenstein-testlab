#nullable disable
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Data;

namespace Api.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class ExecutionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ExecutionsController(ApplicationDbContext context) => _context = context;

    // GET: api/Executions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Execution>>> GetExecutions([FromQuery] Guid? testCaseId, Guid userId, ExecutionStatus? executionStatus)
    {
        var user = await _context.Users.Where(u => u.Id == userId)
            .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
            .Include(u => u.Company)
            .FirstOrDefaultAsync();

        if (executionStatus == null)
        {
            if (testCaseId == null)
            {
                if (user.UserRoles.FirstOrDefault().Role.Name == "Root" || user.UserRoles.FirstOrDefault().Role.Name == "Manager")
                {
                    return Ok(await _context.Executions.Where(e => e.SuiteExecutionId == null)
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
                            e.ExecutionDate,
                            e.TestEnvironment,
                            e.BrowserDriver,
                            e.ExecutionStatus,
                            e.TestCase,
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
                        return Ok(await _context.Executions.Where(e => e.User.Company.CompanyId == user.CompanyId && e.SuiteExecutionId == null)
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
                                e.ExecutionDate,
                                e.TestEnvironment,
								e.BrowserDriver,
								e.ExecutionStatus,
                                e.TestCase,
                                e.User
                            })
                            .OrderByDescending(e => e.ExecutionDate)
                            .ToListAsync()
                        );
                    }
                    else
                    {
                        return Ok(await _context.Executions.Where(e => e.UserId == user.Id && e.SuiteExecutionId == null)
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
                                e.ExecutionDate,
                                e.TestEnvironment,
								e.BrowserDriver,
								e.ExecutionStatus,
                                e.TestCase,
                                e.User
                            })
                            .OrderByDescending(e => e.ExecutionDate)
                            .ToListAsync()
                        );
                    }
                }
            }
            else
            {
                if (user.UserRoles.FirstOrDefault().Role.Name == "Root" || user.UserRoles.FirstOrDefault().Role.Name == "Manager")
                {
                    return Ok(await _context.Executions.Where(e => e.TestCaseId == testCaseId && e.SuiteExecutionId == null)
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
                            e.ExecutionDate,
                            e.TestEnvironment,
							e.BrowserDriver,
							e.ExecutionStatus,
                            e.TestCase,
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
                        return Ok(await _context.Executions.Where(e => e.TestCaseId == testCaseId && e.User.Company.CompanyId == user.CompanyId && e.SuiteExecutionId == null)
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
                                e.ExecutionDate,
                                e.TestEnvironment,
								e.BrowserDriver,
								e.ExecutionStatus,
                                e.TestCase,
                                e.User
                            })
                            .OrderByDescending(e => e.ExecutionDate)
                            .ToListAsync()
                        );
                    }
                    else
                    {
                        return Ok(await _context.Executions.Where(e => e.TestCaseId == testCaseId && e.UserId == user.Id && e.SuiteExecutionId == null)
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
                                e.ExecutionDate,
                                e.TestEnvironment,
								e.BrowserDriver,
								e.ExecutionStatus,
                                e.TestCase,
                                e.User
                            })
                            .OrderByDescending(e => e.ExecutionDate)
                            .ToListAsync()
                        );
                    }
                }
            }
        }
        else
        {
            if (user.UserRoles.FirstOrDefault().Role.Name == "Root" || user.UserRoles.FirstOrDefault().Role.Name == "Manager")
            {
                return Ok(await _context.Executions.Where(e => e.ExecutionStatus == executionStatus && e.SuiteExecutionId == null)
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
                        e.ExecutionDate,
                        e.TestEnvironment,
						e.BrowserDriver,
						e.ExecutionStatus,
                        e.TestCase,
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
                    return Ok(await _context.Executions.Where(e => e.ExecutionStatus == executionStatus && e.User.CompanyId == user.CompanyId && e.SuiteExecutionId == null)
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
                            e.ExecutionDate,
                            e.TestEnvironment,
							e.BrowserDriver,
							e.ExecutionStatus,
                            e.TestCase,
                            e.User
                        })
                        .OrderByDescending(e => e.ExecutionDate)
                        .ToListAsync()
                    );
                }
                else
                {
                    return Ok(await _context.Executions.Where(e => e.ExecutionStatus == executionStatus && e.UserId == user.Id && e.SuiteExecutionId == null)
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
                            e.ExecutionDate,
                            e.TestEnvironment,
							e.BrowserDriver,
							e.ExecutionStatus,
                            e.TestCase,
                            e.User
                        })
                        .OrderByDescending(e => e.ExecutionDate)
                        .ToListAsync()
                    );
                }
            }
        }
    }

    // POST: api/Executions
    [HttpPost]
    public async Task<ActionResult<Execution>> PostExecution(Execution execution)
    {
        _context.Executions.Add(execution);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetExecution", new { id = execution.ExecutionId }, execution);
    }

	// GET: api/Executions/5
	[HttpGet("{id}/{loadTestReport}/{loadTestData}")]
    public async Task<ActionResult<Execution>> GetExecution(Guid id, Boolean loadTestReport, Boolean loadTestData)
	{
		var execution = await _context.Executions.Where(e => e.ExecutionId == id)
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
				TestData = loadTestData ? e.TestData : null,
				TestReport = loadTestReport ? e.TestReport : null,
				e.ExecutionStatus,
				e.Remarks,
				e.TestCase,
				e.SuiteExecution,
				e.Defect,
				e.User
			})
			.FirstOrDefaultAsync();

		if (execution == null)
			return NotFound();

		return Ok(execution);
	}

    // PUT: api/Executions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutExecution(Guid id, Execution execution)
    {
        if (id != execution.ExecutionId)
            return BadRequest();

        _context.Entry(execution).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            if (exception.GetBaseException() is SqlException sqlException)
            {
                if (sqlException.Number == 2601)
                    return Conflict();
            }
            else
            {
                if (!ExecutionExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok(execution);
    }

    // DELETE: api/Executions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExecution(Guid id)
    {
        try
        {
            var execution = await _context.Executions.FindAsync(id);

            if (execution == null)
                return NotFound();

            _context.Executions.Remove(execution);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            if (exception.GetBaseException() is SqlException sqlException)
                if (sqlException.Number == 547)
                    return Conflict();
        }
        catch (Exception)
        {
            throw;
        }

        return Ok();
    }

    private Boolean ExecutionExists(Guid id) => _context.Executions.Any(e => e.ExecutionId == id);
}