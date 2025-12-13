using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FitnessCenterApp.Models;
using FitnessCenterApp.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApp.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var fitnessCenters = await _context.FitnessCenters
            .Where(fc => fc.IsActive)
            .Take(3)
            .ToListAsync();
        return View(fitnessCenters);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Test()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
