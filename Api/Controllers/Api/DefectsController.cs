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
public class DefectsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DefectsController(ApplicationDbContext context) => _context = context;

    // GET: api/Defects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Defect>>> GetDefects([FromQuery] Guid id)
    {
        var user = await _context.Users.Where(u => u.Id == id)
            .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
            .Include(u => u.Company)
            .FirstOrDefaultAsync();

        if (user.UserRoles.FirstOrDefault().Role.Name == "Root" || user.UserRoles.FirstOrDefault().Role.Name == "Manager")
        {
            return await _context.Defects
                .Include(d => d.Execution)
                    .ThenInclude(d => d.TestCase)
                        .ThenInclude(d => d.BusinessProcess)
                            .ThenInclude(d => d.TestApplication)
                .Include(d => d.Execution)
                    .ThenInclude(d => d.User)
                        .ThenInclude(d => d.UserRoles)
                            .ThenInclude(d => d.Role)
                .Include(d => d.User)
                    .ThenInclude(d => d.UserRoles)
                        .ThenInclude(d => d.Role)
                .ToListAsync();
        }
        else
        {
            if (user.UserRoles.FirstOrDefault().Role.Name == "Administrator")
            {
                return await _context.Defects.Where(d => d.Execution.User.Company.CompanyId == user.CompanyId)
                    .Include(d => d.Execution)
                        .ThenInclude(d => d.TestCase)
                            .ThenInclude(d => d.BusinessProcess)
                                .ThenInclude(d => d.TestApplication)
                    .Include(d => d.Execution)
                        .ThenInclude(d => d.User)
                            .ThenInclude(d => d.UserRoles)
                                .ThenInclude(d => d.Role)
                    .Include(d => d.User)
                        .ThenInclude(d => d.UserRoles)
                            .ThenInclude(d => d.Role)
                    .ToListAsync();
            }
            else
            {
                if (user.UserRoles.FirstOrDefault().Role.Name == "Automator")
                {
                    return await _context.Defects.Where(d => d.UserId == user.Id)
                        .Include(d => d.Execution)
                            .ThenInclude(d => d.TestCase)
                                .ThenInclude(d => d.BusinessProcess)
                                    .ThenInclude(d => d.TestApplication)
                        .Include(d => d.Execution)
                            .ThenInclude(d => d.User)
                                .ThenInclude(d => d.UserRoles)
                                    .ThenInclude(d => d.Role)
                        .Include(d => d.User)
                            .ThenInclude(d => d.UserRoles)
                                .ThenInclude(d => d.Role)
                        .ToListAsync();
                }
                else
                {
                    return await _context.Defects.Where(d => d.Execution.UserId == user.Id)
                        .Include(d => d.Execution)
                            .ThenInclude(d => d.TestCase)
                                .ThenInclude(d => d.BusinessProcess)
                                    .ThenInclude(d => d.TestApplication)
                        .Include(d => d.Execution)
                            .ThenInclude(d => d.User)
                                .ThenInclude(d => d.UserRoles)
                                    .ThenInclude(d => d.Role)
                        .Include(d => d.User)
                            .ThenInclude(d => d.UserRoles)
                                .ThenInclude(d => d.Role)
                        .ToListAsync();
                }
            }
        }
    }

    // POST: api/Defects
    [HttpPost]
    public async Task<ActionResult<Defect>> PostDefect(Defect defect)
    {
        if (_context.Defects.Any(d => d.ExecutionId == defect.ExecutionId))
            return Conflict();

        _context.Defects.Add(defect);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDefect", new { id = defect.DefectId }, defect);
    }

    // GET: api/Defects/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Defect>> GetDefect(Guid id)
    {
        var defect = await _context.Defects.Where(d => d.ExecutionId == id)
        .Include(d => d.Execution)
            .ThenInclude(d => d.TestCase)
                .ThenInclude(d => d.BusinessProcess)
                    .ThenInclude(d => d.TestApplication)
        .Include(d => d.Execution)
            .ThenInclude(d => d.User)
                .ThenInclude(d => d.UserRoles)
                    .ThenInclude(d => d.Role)
        .Include(d => d.User)
            .ThenInclude(d => d.UserRoles)
                .ThenInclude(d => d.Role)
        .FirstOrDefaultAsync();

        if (defect == null)
            return NotFound();

        return Ok(defect);
    }

    // PUT: api/Defects/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDefect(Guid id, Defect defect)
    {
        if (id != defect.DefectId)
            return BadRequest();

        _context.Entry(defect).State = EntityState.Modified;

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
                if (!DefectExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok(defect);
    }

    // DELETE: api/Defects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDefect(Guid id)
    {
        try
        {
            var defect = await _context.Defects.FindAsync(id);

            if (defect == null)
                return NotFound();

            _context.Defects.Remove(defect);
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

    private Boolean DefectExists(Guid id) => _context.Defects.Any(e => e.DefectId == id);
}