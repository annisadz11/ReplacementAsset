﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReplacementAsset.Data;
using ReplacementAsset.Models;

namespace ReplacementAsset.Controllers
{
    public class ComponentAssetReplacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComponentAssetReplacementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComponentAssetReplacements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ComponentAssetReplacement.Include(c => c.AssetRequest);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ComponentAssetReplacements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var componentAssetReplacement = await _context.ComponentAssetReplacement
                .Include(c => c.AssetRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componentAssetReplacement == null)
            {
                return NotFound();
            }

            return View(componentAssetReplacement);
        }

        // GET: ComponentAssetReplacements/Create
        public IActionResult Create()
        {
            ViewData["AssetRequestId"] = new SelectList(_context.AssetRequest, "Id", "Name");
            return View();
        }

        // POST: ComponentAssetReplacements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssetRequestId,Name,ValidationReplace,ComponentReplaceDate")] ComponentAssetReplacement componentAssetReplacement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(componentAssetReplacement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetRequestId"] = new SelectList(_context.AssetRequest, "Id", "Name", componentAssetReplacement.AssetRequestId);
            return View(componentAssetReplacement);
        }

        // GET: ComponentAssetReplacements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var componentAssetReplacement = await _context.ComponentAssetReplacement.FindAsync(id);
            if (componentAssetReplacement == null)
            {
                return NotFound();
            }
            ViewData["AssetRequestId"] = new SelectList(_context.AssetRequest, "Id", "Name", componentAssetReplacement.AssetRequestId);
            return View(componentAssetReplacement);
        }

        // POST: ComponentAssetReplacements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssetRequestId,Name,ValidationReplace,ComponentReplaceDate")] ComponentAssetReplacement componentAssetReplacement)
        {
            if (id != componentAssetReplacement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(componentAssetReplacement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentAssetReplacementExists(componentAssetReplacement.Id))
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
            ViewData["AssetRequestId"] = new SelectList(_context.AssetRequest, "Id", "Name", componentAssetReplacement.AssetRequestId);
            return View(componentAssetReplacement);
        }

        // GET: ComponentAssetReplacements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var componentAssetReplacement = await _context.ComponentAssetReplacement
                .Include(c => c.AssetRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componentAssetReplacement == null)
            {
                return NotFound();
            }

            return View(componentAssetReplacement);
        }

        // POST: ComponentAssetReplacements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var componentAssetReplacement = await _context.ComponentAssetReplacement.FindAsync(id);
            if (componentAssetReplacement != null)
            {
                _context.ComponentAssetReplacement.Remove(componentAssetReplacement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComponentAssetReplacementExists(int id)
        {
            return _context.ComponentAssetReplacement.Any(e => e.Id == id);
        }
    }
}
