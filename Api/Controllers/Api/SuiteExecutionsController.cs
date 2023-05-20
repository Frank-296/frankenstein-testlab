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
public class SuiteExecutionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SuiteExecutionsController(ApplicationDbContext context) => _context = context;

    // GET: api/SuiteExecutions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SuiteExecution>>> GetSuiteExecutions([FromQuery] Guid? businessProcessId, Guid userId)
    {
        var user = await _context.Users.Where(u => u.Id == userId)
            .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
            .Include(u => u.Company)
            .FirstOrDefaultAsync();

        if (businessProcessId == null)
        {
            if (user.UserRoles.FirstOrDefault().Role.Name == "Root" || user.UserRoles.FirstOrDefault().Role.Name == "Manager")
            {
                return Ok(await _context.SuiteExecutions
                    .Include(e => e.User)
                        .ThenInclude(e => e.UserRoles)
                            .ThenInclude(e => e.Role)
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
                    return Ok(await _context.SuiteExecutions.Where(e => e.User.Company.CompanyId == user.CompanyId)
                        .Include(e => e.User)
                            .ThenInclude(e => e.UserRoles)
                                .ThenInclude(e => e.Role)
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
                            e.User
                        })
                        .OrderByDescending(e => e.ExecutionDate)
                        .ToListAsync()
                    );
                }
                else
                {
                    return Ok(await _context.SuiteExecutions.Where(e => e.UserId == user.Id)
                        .Include(e => e.User)
                            .ThenInclude(e => e.UserRoles)
                                .ThenInclude(e => e.Role)
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
                return Ok(await _context.SuiteExecutions.Where(e => e.BusinessProcessId == businessProcessId)
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
                    return Ok(await _context.SuiteExecutions.Where(e => e.BusinessProcessId == businessProcessId && e.User.Company.CompanyId == user.CompanyId)
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
                            e.User
                        })
                        .OrderByDescending(e => e.ExecutionDate)
                        .ToListAsync()
                    );
                }
                else
                {
                    return Ok(await _context.SuiteExecutions.Where(e => e.BusinessProcessId == businessProcessId && e.UserId == user.Id)
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
                            e.User
                        })
                        .OrderByDescending(e => e.ExecutionDate)
                        .ToListAsync()
                    );
                }
            }
        }
    }

    // POST: api/SuiteExecutions
    [HttpPost]
    public async Task<ActionResult<SuiteExecution>> PostSuiteExecution(SuiteExecution suiteExecution)
    {
        _context.SuiteExecutions.Add(suiteExecution);
        await _context.SaveChangesAsync();

        foreach (var execution in suiteExecution.Executions)
            _context.Executions.Add(execution);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSuiteExecution", new { id = suiteExecution.SuiteExecutionId }, suiteExecution);
    }


	// GET: api/SuiteExecutions/5
	[HttpGet("{id}/{loadTestCases}")]
    public async Task<ActionResult<SuiteExecution>> GetSuiteExecution(Guid id, Boolean loadTestCases)
    {
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
                e.User,
                executions = e.Executions.Select(e => new
                {
                    e.ExecutionId,
                    e.ExecutionDate,
                    e.TestEnvironment,
                    e.ExecutionStatus,
                    e.BrowserDriver,
                    TestCase = loadTestCases ? e.TestCase : null,
                    e.User
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (suiteExecution == null)
            return NotFound();

        return Ok(suiteExecution);
    }

    // PUT: api/SuiteExecutions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSuiteExecution(Guid id, SuiteExecution suiteExecution)
    {
        if (id != suiteExecution.SuiteExecutionId)
            return BadRequest();

        _context.Entry(suiteExecution).State = EntityState.Modified;

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
                if (!SuiteExecutionExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok(suiteExecution);
    }

    // DELETE: api/SuiteExecutions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSuiteExecution(Guid id)
    {
        try
        {
            var suiteExecution = await _context.SuiteExecutions.FindAsync(id);

            if (suiteExecution == null)
                return NotFound();

            _context.SuiteExecutions.Remove(suiteExecution);
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

    private Boolean SuiteExecutionExists(Guid id) => (_context.SuiteExecutions?.Any(e => e.SuiteExecutionId == id)).GetValueOrDefault();
}