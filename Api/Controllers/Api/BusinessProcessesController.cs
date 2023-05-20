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
public class BusinessProcessesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BusinessProcessesController(ApplicationDbContext context) => _context = context;

    // GET: api/BusinessProcesses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BusinessProcess>>> GetBusinessProcesses([FromQuery] Guid id) =>
        await _context.BusinessProcesses.Where(p => p.TestApplicationId == id)
            .Include(p => p.TestApplication)
            .OrderBy(p => p.Name)
            .ToListAsync();

    // POST: api/BusinessProcesses
    [HttpPost]
    public async Task<ActionResult<BusinessProcess>> PostBusinessProcess(BusinessProcess businessProcess)
    {
        if (_context.BusinessProcesses.Any(p => p.Name == businessProcess.Name && p.TestApplicationId == businessProcess.TestApplicationId))
            return Conflict();

        _context.BusinessProcesses.Add(businessProcess);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBusinessProcess", new { id = businessProcess.BusinessProcessId }, businessProcess);
    }

    // GET: api/BusinessProcesses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BusinessProcess>> GetBusinessProcess(Guid id)
    {
        var businessProcess = await _context.BusinessProcesses.Where(p => p.BusinessProcessId == id)
            .Include(p => p.TestApplication)
            .FirstOrDefaultAsync();

        if (businessProcess == null)
            return NotFound();

        return businessProcess;
    }

    // PUT: api/BusinessProcesses/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBusinessProcess(Guid id, BusinessProcess businessProcess)
    {
        if (id != businessProcess.BusinessProcessId)
            return BadRequest();

        _context.Entry(businessProcess).State = EntityState.Modified;

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
                if (!BusinessProcessExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok(businessProcess);
    }

    // DELETE: api/BusinessProcesses/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBusinessProcess(Guid id)
    {
        try
        {
            var businessProcess = await _context.BusinessProcesses.FindAsync(id);

            if (businessProcess == null)
                return NotFound();

            _context.BusinessProcesses.Remove(businessProcess);
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

    private Boolean BusinessProcessExists(Guid id) => _context.BusinessProcesses.Any(e => e.BusinessProcessId == id);
}