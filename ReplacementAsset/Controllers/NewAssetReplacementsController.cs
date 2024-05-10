using System;
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
    public class NewAssetReplacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewAssetReplacementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NewAssetReplacement
        public IActionResult Index()
        {
            var newAssetReplacements = _context.NewAssetReplacement.Include(n => n.AssetRequest).ToList();
            return View(newAssetReplacements);
        }

        // API ENDPOINT
        [HttpGet]
        public IActionResult GetData()
        {
            var newAssetReplacements = _context.NewAssetReplacement
                .Select(n => new
                {
                    id = n.Id,
                    assetRequestName = n.AssetRequest.Name,
                    assetRequestDepartment = n.AssetRequest.Departement,
                    assetRequestType = n.AssetRequest.Type,
                    assetRequestSerialNumber = n.AssetRequest.SerialNumber,
                    assetRequestBaseline = n.AssetRequest.Baseline,
                    assetRequestUsageLocation = n.AssetRequest.UsageLocation,
                    assetRequestReason = n.AssetRequest.Reason,
                    assetRequestJustify = n.AssetRequest.Justify,
                    assetRequestTypeReplace = n.AssetRequest.TypeReplace,
                    assetRequestApprovalDate = n.AssetRequest.ApprovalDate.HasValue ? n.AssetRequest.ApprovalDate.Value.ToString("dd MMM yyyy") : null,
                    name = n.Name,
                    newType = n.NewType ?? string.Empty, // Menangani nilai null
                    newSerialNumber = n.NewSerialNumber ?? string.Empty, // Menangani nilai null
                    dateReplace = n.DateReplace.HasValue ? n.DateReplace.Value.ToString("dd MMM yyyy") : null
                })
                .ToList();

            return Json(new { rows = newAssetReplacements });
        }


        // GET: NewAssetReplacements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newAssetReplacement = await _context.NewAssetReplacement.FindAsync(id);
            if (newAssetReplacement == null)
            {
                return NotFound();
            }
            return View(newAssetReplacement);
        }

        // POST: NewAssetReplacements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssetRequestId,Name,NewType,NewSerialNumber,DateReplace")] NewAssetReplacement newAssetReplacement)
        {
            if (id != newAssetReplacement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newAssetReplacement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewAssetReplacementExists(newAssetReplacement.Id))
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
            return View(newAssetReplacement);
        }

        private bool NewAssetReplacementExists(int id)
        {
            return _context.NewAssetReplacement.Any(e => e.Id == id);
        }
    }
}



/*// GET: NewAssetReplacements/Details/5
public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var newAssetReplacement = await _context.NewAssetReplacement
        .Include(n => n.AssetRequest)
        .FirstOrDefaultAsync(m => m.Id == id);
    if (newAssetReplacement == null)
    {
        return NotFound();
    }

    return View(newAssetReplacement);
}

// GET: NewAssetReplacements/Create
public IActionResult Create()
{
    ViewData["AssetRequestId"] = new SelectList(_context.AssetRequest, "Id", "Name");
    return View();
}

// POST: NewAssetReplacements/Create
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,AssetRequestId,Name,NewType,NewSerialNumber,DateReplace")] NewAssetReplacement newAssetReplacement)
{
    if (ModelState.IsValid)
    {
        _context.Add(newAssetReplacement);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    ViewData["AssetRequestId"] = new SelectList(_context.AssetRequest, "Id", "Name", newAssetReplacement.AssetRequestId);
    return View(newAssetReplacement);
}

// GET: NewAssetReplacement/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var newAssetReplacement = await _context.NewAssetReplacement
        .Include(n => n.AssetRequest)
        .FirstOrDefaultAsync(m => m.Id == id);
    if (newAssetReplacement == null)
    {
        return NotFound();
    }
    return View(newAssetReplacement);
}

/// POST: NewAssetReplacement/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,AssetRequestId,Name,NewType,NewSerialNumber,DateReplace")] NewAssetReplacement newAssetReplacement)
{
    if (id != newAssetReplacement.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(newAssetReplacement);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NewAssetReplacementExists(newAssetReplacement.Id))
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
    return View(newAssetReplacement);
}

// GET: NewAssetReplacements/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var newAssetReplacement = await _context.NewAssetReplacement
        .Include(n => n.AssetRequest)
        .FirstOrDefaultAsync(m => m.Id == id);
    if (newAssetReplacement == null)
    {
        return NotFound();
    }

    return View(newAssetReplacement);
}

// POST: NewAssetReplacements/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var newAssetReplacement = await _context.NewAssetReplacement.FindAsync(id);
    if (newAssetReplacement != null)
    {
        _context.NewAssetReplacement.Remove(newAssetReplacement);
    }

    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}
*
private bool NewAssetReplacementExists(int id)
{
    return _context.NewAssetReplacement.Any(e => e.Id == id);
}
    }
}*/
