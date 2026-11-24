using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojeCV.Data;
using MojeCV.Models;

namespace MojeCV.Controllers;

[Authorize(Roles = "Admin")]
public class AdminServicesController : Controller
{
    private readonly CvDbContext _context;
    public AdminServicesController(CvDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Services.OrderBy(s => s.Name).ToListAsync());

    public IActionResult Create() => View(new Service());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Service service)
    {
        if (!ModelState.IsValid) return View(service);
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Usługa została dodana.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var service = await _context.Services.FindAsync(id);
        return service == null ? NotFound() : View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Service service)
    {
        if (id != service.Id) return NotFound();
        if (!ModelState.IsValid) return View(service);
        _context.Update(service);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Usługa została zaktualizowana.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null) return NotFound();
        service.IsActive = false;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
