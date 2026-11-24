using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojeCV.Data;

namespace MojeCV.Controllers;

public class ServicesController : Controller
{
    private readonly CvDbContext _context;

    public ServicesController(CvDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var services = await _context.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();
        return View(services);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var service = await _context.Services
            .Include(s => s.Slots)
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        if (service == null) return NotFound();

        var bookedSlotIds = await _context.Reservations
            .Where(r => r.Status != Models.ReservationStatus.Cancelled && r.Status != Models.ReservationStatus.Rejected)
            .Select(r => r.ReservationSlotId)
            .Distinct()
            .ToListAsync();

        ViewBag.BookedSlotIds = bookedSlotIds;
        return View(service);
    }
}
