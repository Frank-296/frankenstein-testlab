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
public class TestApplicationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TestApplicationsController(ApplicationDbContext context) => _context = context;

    // GET: api/TestApplications
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestApplication>>> GetTestApplications() =>
        await _context.TestApplications.OrderBy(a => a.Name).ToListAsync();

    // POST: api/TestApplications
    [HttpPost]
    public async Task<ActionResult<TestApplication>> PostTestApplication(TestApplication testApplication)
    {
        if (_context.TestApplications.Any(a => a.Name == testApplication.Name))
            return Conflict();

        _context.TestApplications.Add(testApplication);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTestApplication", new { id = testApplication.TestApplicationId }, testApplication);
    }

    // GET: api/TestApplications/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TestApplication>> GetTestApplication(Guid id)
    {
        var testApplication = await _context.TestApplications.FindAsync(id);

        if (testApplication == null)
            return NotFound();

        return testApplication;
    }

    // PUT: api/TestApplications/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTestApplication(Guid id, TestApplication testApplication)
    {
        if (id != testApplication.TestApplicationId)
            return BadRequest();

        _context.Entry(testApplication).State = EntityState.Modified;

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
                if (!TestApplicationExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok(testApplication);
    }

    // DELETE: api/TestApplications/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestApplication(Guid id)
    {
        try
        {
            var testApplication = await _context.TestApplications.FindAsync(id);

            if (testApplication == null)
                return NotFound();

            _context.TestApplications.Remove(testApplication);
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

    private Boolean TestApplicationExists(Guid id) => _context.TestApplications.Any(e => e.TestApplicationId == id);
}