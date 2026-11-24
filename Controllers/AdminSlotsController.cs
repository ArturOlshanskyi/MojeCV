using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MojeCV.Data;
using MojeCV.Models;

namespace MojeCV.Controllers;

[Authorize(Roles = "Admin")]
public class AdminSlotsController : Controller
{
    private readonly CvDbContext _context;
    public AdminSlotsController(CvDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var slots = await _context.ReservationSlots.Include(s => s.Service).OrderBy(s => s.StartTime).ToListAsync();
        return View(slots);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Services = new SelectList(await _context.Services.Where(s => s.IsActive).ToListAsync(), "Id", "Name");
        return View(new ReservationSlot { StartTime = DateTime.Today.AddDays(1).AddHours(9), Capacity = 1 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationSlot slot)
    {
        if (slot.StartTime <= DateTime.Now)
            ModelState.AddModelError("StartTime", "Termin musi być w przyszłości.");
        if (!await _context.Services.AnyAsync(s => s.Id == slot.ServiceId && s.IsActive))
            ModelState.AddModelError("ServiceId", "Wybierz aktywną usługę.");
        if (!ModelState.IsValid)
        {
            ViewBag.Services = new SelectList(await _context.Services.Where(s => s.IsActive).ToListAsync(), "Id", "Name", slot.ServiceId);
            return View(slot);
        }
        _context.ReservationSlots.Add(slot);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var slot = await _context.ReservationSlots.FindAsync(id);
        if (slot == null) return NotFound();
        var hasReservations = await _context.Reservations.AnyAsync(r => r.ReservationSlotId == id);
        if (hasReservations)
        {
            slot.IsAvailable = false;
        }
        else
        {
            _context.ReservationSlots.Remove(slot);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
