#nullable disable
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Data;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers.Api;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, ApplicationDbContext context, IConfiguration config)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _config = config;
    }

    // POST: api/Account/Login
    [AllowAnonymous]
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> PostLogin(LoginViewModel login)
    {
        var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, login.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMonths(1);

            JwtSecurityToken token = new(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            var user = await _userManager.FindByEmailAsync(login.Email);
            var role = await _userManager.GetRolesAsync(user);

            if (user.IsAuthorized)
                return Ok(new
                {
                    user.Id,
                    user.CompanyId,
                    Role = role.FirstOrDefault(),
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration,
                });
			else
				return Unauthorized();
		}
        else
            return Unauthorized();
    }

    // GET: api/Account/5
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUser(Guid id) =>
        Ok(await _context.Users.Where(u => u.Id == id)
            .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
            .Include(u => u.Company)
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.LastName,
                u.MothersLastName,
                u.DisplayName,
                Role = u.UserRoles.FirstOrDefault().Role.Name,
                Company = u.Company.Name,
                u.Email,
                u.PhoneNumber,
                u.ProfilePicture,
                u.RegistrationDate,
                u.SetPasswordDate,
                u.IsAuthorized
            })
            .FirstOrDefaultAsync()
        );

    // PUT: api/Account/UpdateProfile
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProfile(Guid id, UpdateProfileViewModel updateProfile)
    {
        if (id != updateProfile.Id)
            return BadRequest();

        var user = await _context.Users.FindAsync(id);

        user.Name = updateProfile.Name;
        user.LastName = updateProfile.LastName;
        user.MothersLastName = updateProfile.MothersLastName;
        user.PhoneNumber = updateProfile.PhoneNumber;
        user.ProfilePicture = updateProfile.ProfilePicture;

        _context.Entry(user).State = EntityState.Modified;

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
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok();
    }

    // POST: api/Account/ChangePassword
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    [Route("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
    {
        var user = await _userManager.FindByIdAsync(changePassword.Id.ToString());

        if (user == null)
            return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);

        if (!result.Succeeded)
        {
            String code = String.Empty;

            foreach (var error in result.Errors)
                code = error.Code;

            if (code == "PasswordMismatch")
                return Conflict();
            else
                return BadRequest();
        }

        return Ok();
    }

    private Boolean UserExists(Guid id) => _context.Users.Any(u => u.Id == id);
}