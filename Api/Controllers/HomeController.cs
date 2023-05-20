using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Data;

using System.Diagnostics;

namespace Api.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly IWebHostEnvironment _environment;

	public HomeController(ApplicationDbContext context, IWebHostEnvironment environment)
	{
		_context = context;
		_environment = environment;
	}

	public IActionResult Index()
	{
		var message = TempData["message"];

		if (message != null)
			TempData["alert"] = message;

		ViewData["TestApplications"] = new SelectList(_context.TestApplications.OrderBy(a => a.Name), "TestApplicationId", "Name");
		return View();
	}

	// GET: BusinessProcesses
	public async Task<IActionResult> BusinessProcesses(Guid? id) =>
		Json(await _context.BusinessProcesses.Where(p => p.TestApplicationId == id).OrderBy(p => p.Name).ToListAsync());

	// GET: TestCases
	public async Task<IActionResult> TestCases(Guid? id) =>
		Json(await _context.TestCases.Where(c => c.BusinessProcessId == id).OrderBy(c => c.Name).ToListAsync());

	// GET: Download
	public IActionResult Download()
	{
		var archive = Path.Combine(_environment.WebRootPath, "app", $"Frankenstein Test Lab.zip");

		if (!System.IO.File.Exists(archive))
			return NotFound();

		var file = System.IO.File.ReadAllBytes(archive);
		var contentType = "application/zip";
		var fileName = $"Frankenstein Test Lab.zip";

		return File(file, contentType, fileName);
	}

	public IActionResult Privacy() => View();

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}