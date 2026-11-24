using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojeCV.Data;
using MojeCV.Models;
using MojeCV.ViewModels;

namespace MojeCV.Controllers;

[Authorize]
public class ReservationsController : Controller
{
    private readonly CvDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ReservationsController(CvDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var reservations = await _context.Reservations
            .Include(r => r.Service)
            .Include(r => r.ReservationSlot)
            .Where(r => r.UserId == user.Id)
            .OrderByDescending(r => r.ReservationSlot!.StartTime)
            .ToListAsync();
        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int serviceId)
    {
        var service = await _context.Services.FindAsync(serviceId);
        if (service == null || !service.IsActive) return NotFound();

        var bookedSlotIds = await _context.Reservations
            .Where(r => r.Status != ReservationStatus.Cancelled && r.Status != ReservationStatus.Rejected)
            .Select(r => r.ReservationSlotId)
            .ToListAsync();

        var slots = await _context.ReservationSlots
            .Where(s => s.ServiceId == serviceId && s.IsAvailable && s.StartTime > DateTime.Now && !bookedSlotIds.Contains(s.Id))
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        ViewBag.Slots = slots;
        return View(new ReservationCreateViewModel { ServiceId = serviceId, ServiceName = service.Name });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationCreateViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        var service = await _context.Services.FindAsync(model.ServiceId);
        if (user == null) return Challenge();
        if (service == null) return NotFound();

        var slot = await _context.ReservationSlots
            .FirstOrDefaultAsync(s => s.Id == model.ReservationSlotId && s.ServiceId == model.ServiceId && s.IsAvailable);
        if (slot == null)
        {
            ModelState.AddModelError("", "Wybrany termin jest niedostępny.");
        }

        var duplicate = await _context.Reservations.AnyAsync(r => r.ReservationSlotId == model.ReservationSlotId
            && r.Status != ReservationStatus.Cancelled && r.Status != ReservationStatus.Rejected);
        if (duplicate)
        {
            ModelState.AddModelError("", "Ten termin został już zarezerwowany.");
        }

        if (!ModelState.IsValid)
        {
            model.ServiceName = service.Name;
            ViewBag.Slots = await _context.ReservationSlots
                .Where(s => s.ServiceId == model.ServiceId && s.IsAvailable && s.StartTime > DateTime.Now)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
            return View(model);
        }

        _context.Reservations.Add(new Reservation
        {
            ReservationSlotId = slot!.Id,
            ServiceId = service.Id,
            UserId = user.Id,
            Notes = model.Notes,
            Status = ReservationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Rezerwacja została wysłana i oczekuje na zatwierdzenie.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var reservation = await _context.Reservations
            .Include(r => r.Service)
            .Include(r => r.ReservationSlot)
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == user!.Id);
        if (reservation == null) return NotFound();
        if (reservation.Status == ReservationStatus.Approved) return Forbid();

        var bookedIds = await _context.Reservations
            .Where(r => r.Id != id && r.Status != ReservationStatus.Cancelled && r.Status != ReservationStatus.Rejected)
            .Select(r => r.ReservationSlotId).ToListAsync();
        ViewBag.Slots = await _context.ReservationSlots
            .Where(s => s.ServiceId == reservation.ServiceId && s.IsAvailable && s.StartTime > DateTime.Now && !bookedIds.Contains(s.Id))
            .OrderBy(s => s.StartTime).ToListAsync();
        return View(reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, int reservationSlotId, string? notes)
    {
        var user = await _userManager.GetUserAsync(User);
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id && r.UserId == user!.Id);
        if (reservation == null) return NotFound();

        var slotAvailable = await _context.ReservationSlots.AnyAsync(s => s.Id == reservationSlotId
            && s.ServiceId == reservation.ServiceId && s.IsAvailable && s.StartTime > DateTime.Now);
        var booked = await _context.Reservations.AnyAsync(r => r.Id != id && r.ReservationSlotId == reservationSlotId
            && r.Status != ReservationStatus.Cancelled && r.Status != ReservationStatus.Rejected);
        if (!slotAvailable || booked)
        {
            TempData["Error"] = "Wybrany termin jest niedostępny.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        reservation.ReservationSlotId = reservationSlotId;
        reservation.Notes = notes;
        reservation.Status = ReservationStatus.Pending;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id && r.UserId == user!.Id);
        if (reservation == null) return NotFound();

        reservation.Status = ReservationStatus.Cancelled;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Rezerwacja została anulowana.";
        return RedirectToAction(nameof(Index));
    }
}
