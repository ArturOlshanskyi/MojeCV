using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojeCV.Data;
using MojeCV.Models;

namespace MojeCV.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly CvDbContext _context;

    public AdminController(CvDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        ViewBag.ActiveReservations = await _context.Reservations.CountAsync(r => r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Approved);
        ViewBag.PendingReservations = await _context.Reservations.CountAsync(r => r.Status == ReservationStatus.Pending);
        ViewBag.ServicesCount = await _context.Services.CountAsync(s => s.IsActive);
        ViewBag.TodayReservations = await _context.Reservations.CountAsync(r => r.ReservationSlot!.StartTime.Date == DateTime.Today);

        var recent = await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Service)
            .Include(r => r.ReservationSlot)
            .OrderByDescending(r => r.CreatedAt)
            .Take(10)
            .ToListAsync();
        return View(recent);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetStatus(int id, ReservationStatus status)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null) return NotFound();
        reservation.Status = status;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
