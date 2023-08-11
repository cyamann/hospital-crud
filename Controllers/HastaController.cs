using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hastane.Context;
using Hastane.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Numerics;

namespace Hastane.Controllers
{
    public class HastaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HastaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hasta
        public async Task<IActionResult> Index()
        {
            return _context.hasta != null ?
                        View(await _context.hasta.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.hasta'  is null.");
        }

        // GET: Hasta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.hasta == null)
            {
                return NotFound();
            }

            var hasta = await _context.hasta
                .FirstOrDefaultAsync(m => m.hastaid == id);
            if (hasta == null)
            {
                return NotFound();
            }

            return View(hasta);
        }

        // GET: Hasta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hasta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("hastaid,tc,ad_soyad,dogum")] Hasta hasta)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlRaw("CALL sp_addnewpatient({0}, {1}, {2},{3})",hasta.hastaid, hasta.ad_soyad, hasta.dogum, hasta.tc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hasta);
        }

        // GET: Hasta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.hasta == null)
            {
                return NotFound();
            }

            var hasta = await _context.hasta.FindAsync(id);
            if (hasta == null)
            {
                return NotFound();
            }
            return View(hasta);
        }

        // POST: Hasta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("hastaid,tc,ad_soyad,dogum")] Hasta hasta)
        {
            if (id != hasta.hastaid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlRaw("CALL sp_updatepatient({0}, {1}, {2},{3})", hasta.hastaid, hasta.tc, hasta.ad_soyad, hasta.dogum );
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HastaExists(hasta.hastaid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hasta);
        }

        // GET: Hasta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.hasta == null)
            {
                return NotFound();
            }

            var hasta = await _context.hasta
                .FirstOrDefaultAsync(m => m.hastaid == id);
            if (hasta == null)
            {
                return NotFound();
            }

            return View(hasta);
        }

        // POST: Hasta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.hasta == null)
            {
                return Problem("Entity set 'ApplicationDbContext.hastalar'  is null.");
            }
            var hasta = await _context.hasta.FindAsync(id);
            if (hasta != null)
            {
                _context.Database.ExecuteSqlRaw("CALL sp_deletepatient({0})", hasta.hastaid);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HastaExists(int id)
        {
            return (_context.hasta?.Any(e => e.hastaid == id)).GetValueOrDefault();
        }
    }
}
