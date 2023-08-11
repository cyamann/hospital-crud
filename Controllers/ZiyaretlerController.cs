using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hastane.Context;
using Hastane.Models;
using Humanizer;
using static System.Net.Mime.MediaTypeNames;

namespace Hastane.Controllers
{
    public class ZiyaretlerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZiyaretlerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ziyaretler
        public async Task<IActionResult> Index()
        {
            return _context.Ziyaretler != null ?
                        View(await _context.Ziyaretler.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Ziyaretler'  is null.");
        }

        // GET: Ziyaretler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ziyaretler == null)
            {
                return NotFound();
            }

            var ziyaretler = await _context.Ziyaretler
                .FirstOrDefaultAsync(m => m.ziyaret_id == id);
            if (ziyaretler == null)
            {
                return NotFound();
            }

            return View(ziyaretler);
        }

        // GET: Ziyaretler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ziyaretler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ziyaret_id,hasta_id,ziyaret_tarihi,doktor_adi,sikayet,tedavi_sekli")] Ziyaretler ziyaretler)
        {
            if (ModelState.IsValid)
            {
                
                _context.Database.ExecuteSqlRaw("CALL sp_addnewziyaret({0}, {1}, {2},{3},{4},{5})", ziyaretler.ziyaret_id, ziyaretler.hasta_id, ziyaretler.ziyaret_tarihi, ziyaretler.doktor_adi, ziyaretler.sikayet, ziyaretler.tedavi_sekli);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ziyaretler);
        }

        // GET: Ziyaretler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ziyaretler == null)
            {
                return NotFound();
            }

            var ziyaretler = await _context.Ziyaretler.FindAsync(id);
            if (ziyaretler == null)
            {
                return NotFound();
            }
            return View(ziyaretler);
        }

        // POST: Ziyaretler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ziyaret_id,hasta_id,ziyaret_tarihi,doktor_adi,sikayet,tedavi_sekli")] Ziyaretler ziyaretler)
        {
            if (id != ziyaretler.ziyaret_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlRaw("CALL sp_updateziyaret({0}, {1}, {2},{3},{4},{5})", ziyaretler.ziyaret_id,ziyaretler.hasta_id,ziyaretler.ziyaret_tarihi,ziyaretler.doktor_adi,ziyaretler.sikayet,ziyaretler.tedavi_sekli);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZiyaretlerExists(ziyaretler.ziyaret_id))
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
            return View(ziyaretler);
        }

        // GET: Ziyaretler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ziyaretler == null)
            {
                return NotFound();
            }

            var ziyaretler = await _context.Ziyaretler
                .FirstOrDefaultAsync(m => m.ziyaret_id == id);
            if (ziyaretler == null)
            {
                return NotFound();
            }

            return View(ziyaretler);
        }

        // POST: Ziyaretler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ziyaretler == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ziyaretler'  is null.");
            }
            var ziyaretler = await _context.Ziyaretler.FindAsync(id);
            if (ziyaretler != null)
            {
                _context.Database.ExecuteSqlRaw("CALL sp_deleteziyaret({0})", ziyaretler.ziyaret_id);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZiyaretlerExists(int id)
        {
            return (_context.Ziyaretler?.Any(e => e.ziyaret_id == id)).GetValueOrDefault();
        }
    }
}
