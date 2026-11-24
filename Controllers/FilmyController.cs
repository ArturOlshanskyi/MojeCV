using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojeCV.Data;
using MojeCV.Models;

namespace MojeCV.Controllers;

public class FilmyController : Controller
{
    private readonly CvDbContext _context;

    public FilmyController(CvDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var filmy = await _context.Filmy
            .OrderBy(f => f.Tytul)
            .ToListAsync();

        return View(filmy);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Tytul,DlugoscMinuty,Rezyser")] Film film)
    {
        if (ModelState.IsValid)
        {
            _context.Add(film);
            await _context.SaveChangesAsync();
            TempData["Komunikat"] = "Film został dodany.";
            return RedirectToAction(nameof(Index));
        }

        return View(film);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var film = await _context.Filmy.FindAsync(id);
        if (film == null)
            return NotFound();

        return View(film);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Tytul,DlugoscMinuty,Rezyser")] Film film)
    {
        if (id != film.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(film);
            await _context.SaveChangesAsync();
            TempData["Komunikat"] = "Dane filmu zostały zaktualizowane.";
            return RedirectToAction(nameof(Index));
        }

        return View(film);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var film = await _context.Filmy.FirstOrDefaultAsync(f => f.Id == id);
        if (film == null)
            return NotFound();

        return View(film);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var film = await _context.Filmy.FindAsync(id);
        if (film != null)
        {
            _context.Filmy.Remove(film);
            await _context.SaveChangesAsync();
            TempData["Komunikat"] = "Film został usunięty.";
        }

        return RedirectToAction(nameof(Index));
    }
}
