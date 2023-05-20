#nullable disable
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Data;

namespace Api.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public UsersController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] Guid? id)
    {
        if (id == null)
        {
            return await _context.Users.Where(u => u.UserRoles.FirstOrDefault().Role.Name == "Automator")
                .Include(u => u.UserRoles)
                    .ThenInclude(u => u.Role)
                .Include(u => u.Company)
                .OrderBy(u => u.Email)
                .ToListAsync();
        }
        else
        {
            var user = await _context.Users.Where(u => u.Id == id)
                .Include(u => u.UserRoles)
                    .ThenInclude(u => u.Role)
                .Include(u => u.Company)
                .FirstOrDefaultAsync();

            if (user.UserRoles.FirstOrDefault().Role.Name == "Root" || user.UserRoles.FirstOrDefault().Role.Name == "Manager")
            {
                return await _context.Users.Where(u => u.UserRoles.FirstOrDefault().Role.Name != "Root")
                    .Include(u => u.UserRoles)
                        .ThenInclude(u => u.Role)
                    .Include(u => u.Company)
                    .OrderBy(u => u.Email)
                    .ToListAsync();
            }
            else
            {
                if (user.UserRoles.FirstOrDefault().Role.Name == "Administrator")
                {
                    return await _context.Users.Where(u => u.UserRoles.FirstOrDefault().Role.Name != "Manager" && u.CompanyId == user.CompanyId)
                        .Include(u => u.UserRoles)
                            .ThenInclude(u => u.Role)
                        .Include(u => u.Company)
                        .OrderBy(u => u.Email)
                        .ToListAsync();
                }
                else
                    return BadRequest();
            }
        }
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(RegisterUserViewModel registerUser)
    {
        var password = $"T3$tLab{DateTime.Now.Year}";
        var user = new User
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            RegistrationDate = registerUser.RegistrationDate,
            SetPasswordDate = registerUser.SetPasswordDate,
            IsAuthorized = registerUser.IsAuthorized,
            CompanyId = registerUser.CompanyId
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, registerUser.Role);
        else
        {
            String code = String.Empty;

            foreach (var error in result.Errors)
                code = error.Code;

            if (code == "DuplicateUserName")
                return Conflict();
            else
                return BadRequest();
        }

        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await _context.Users.Where(u => u.Id == id)
            .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
            .Include(u => u.Company)
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return user;
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, UpdateUserViewModel updateUser)
    {
        if (id != updateUser.Id)
            return BadRequest();

        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return NotFound();

        user.IsAuthorized = updateUser.IsAuthorized;

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();

            if (updateUser.Role != null)
            {
                var currentRole = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, currentRole.ToArray());
                await _userManager.AddToRoleAsync(user, updateUser.Role);
            }
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
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok();
    }

    // POST: api/Users/ResetPassword
    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] Guid id)
    {
        var password = $"T3$tLab{DateTime.Now.Year}";
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        try
        {
            _userManager.PasswordHasher.HashPassword(user, password);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Conflict();
        }
        catch (DbUpdateException)
        {
            if (!UserExists(id))
                return NotFound();
            else
                throw;
        }

        return Ok();
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
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

    private Boolean UserExists(Guid id) => _context.Users.Any(u => u.Id == id);
}