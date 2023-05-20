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
public class TestCasesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TestCasesController(ApplicationDbContext context) => _context = context;

    // GET: api/TestCases
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestCase>>> GetTestCases([FromQuery] Guid id) =>
        Ok(await _context.TestCases.Where(c => c.BusinessProcessId == id)
            .Include(c => c.BusinessProcess)
                .ThenInclude(c => c.TestApplication)
            .OrderBy(c => c.Name)
            .ToListAsync()
        );

    // POST: api/TestCases
    [HttpPost]
    public async Task<ActionResult<TestCase>> PostTestCase(TestCase testCase)
    {
        if (_context.TestCases.Any(c => c.Name == testCase.Name && c.BusinessProcessId == testCase.BusinessProcessId))
            return Conflict();

        _context.TestCases.Add(testCase);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTestCase", new { id = testCase.TestCaseId }, testCase);
    }

    // GET: api/TestCases/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TestCase>> GetTestCase(Guid id)
    {
        var testCase = await _context.TestCases.Where(c => c.TestCaseId == id)
            .Include(c => c.BusinessProcess)
                .ThenInclude(c => c.TestApplication)
            .FirstOrDefaultAsync();

        if (testCase == null)
            return NotFound();

        return testCase;
    }

    // PUT: api/TestCases/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTestCase(Guid id, TestCase testCase)
    {
        if (id != testCase.TestCaseId)
            return BadRequest();

        _context.Entry(testCase).State = EntityState.Modified;

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
                if (!TestCaseExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok(testCase);
    }

    // DELETE: api/TestCases/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestCase(Guid id)
    {
        try
        {
            var testCase = await _context.TestCases.FindAsync(id);

            if (testCase == null)
                return NotFound();

            _context.TestCases.Remove(testCase);
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

    private Boolean TestCaseExists(Guid id) => _context.TestCases.Any(e => e.TestCaseId == id);
}