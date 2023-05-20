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
public class CompaniesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CompaniesController(ApplicationDbContext context) => _context = context;

    // GET: api/Companies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanies() => await _context.Companies.OrderBy(c => c.Name).ToListAsync();

    // POST: api/Companies
    [HttpPost]
    public async Task<ActionResult<Company>> PostCompany(Company company)
    {
        if (_context.Companies.Any(c => c.Name == company.Name))
            return Conflict();

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
    }

    // GET: api/Companies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(Guid id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null)
            return NotFound();

        return company;
    }

    // PUT: api/Companies/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCompany(Guid id, Company company)
    {
        if (id != company.CompanyId)
            return BadRequest();

        _context.Entry(company).State = EntityState.Modified;

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
                if (!CompanyExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok(company);
    }

    // DELETE: api/Companies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        try
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                return NotFound();

            _context.Companies.Remove(company);
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

    private Boolean CompanyExists(Guid id) => _context.Companies.Any(e => e.CompanyId == id);
}