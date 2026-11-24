using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojeCV.Data;
using MojeCV.ViewModels;

namespace MojeCV.Controllers;

public class CvController : Controller
{
    private readonly CvDbContext _context;

    public CvController(CvDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var osoba = await _context.Osoby.FirstOrDefaultAsync();
        if (osoba == null)
            return RedirectToAction("Create", "Osoby");

        var model = new CvViewModel
        {
            Osoba = osoba,
            Doswiadczenia = await _context.Doswiadczenia
                .OrderByDescending(d => d.DataOd)
                .ToListAsync(),
            Wyksztalcenia = await _context.Wyksztalcenia
                .OrderByDescending(w => w.DataOd)
                .ToListAsync(),
            Umiejetnosci = await _context.Umiejetnosci
                .OrderByDescending(u => u.Poziom)
                .ToListAsync()
        };

        return View(model);
    }
}
