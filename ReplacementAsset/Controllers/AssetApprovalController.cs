using ReplacementAsset.Data;
using Microsoft.AspNetCore.Mvc;
using ReplacementAsset.Models;
using Microsoft.EntityFrameworkCore;

namespace ReplacementAsset.Controllers
{
    public class AssetApprovalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssetApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssetRequest
        public IActionResult Index()
        {
            return View();
        }

        ///API ENDPOINT
        [HttpGet]
        public IActionResult GetData()
        {
            var AssetRequests = _context.AssetRequest
                .Where(g => g.Status == null) // Filter untuk status null
                .Select(g => new
                {
                    id = g.Id,
                    name = g.Name,
                    departement = g.Departement,
                    type = g.Type,
                    serialNumber = g.SerialNumber,
                    baseline = g.Baseline,
                    usageLocation = g.UsageLocation,
                    requestDate = g.RequestDate.HasValue ? g.RequestDate.Value.ToString("dd MMM yyyy") : null,
                    reason = g.Reason,
                    status = g.Status,
                    approvalDate = g.ApprovalDate.HasValue ? g.ApprovalDate.Value.ToString("dd MMM yyyy") : null,
                    justify = g.Justify,
                    typeReplace = g.TypeReplace,
                }).ToList();

            return Json(new { rows = AssetRequests });
        }

        //APPROVE MANAGER
        [HttpPost]
        public async Task<IActionResult> Approve(int id, string justify, string typeReplace)
        {
            var assetRequest = await _context.AssetRequest.FindAsync(id);

            if (assetRequest == null)
            {
                return NotFound();
            }

            assetRequest.Status = true;
            assetRequest.Justify = justify;
            assetRequest.TypeReplace = typeReplace;
            assetRequest.ApprovalDate = DateTime.Now;

            if (typeReplace == "NewAssetReplacement")
            {
                var newAssetReplacement = new NewAssetReplacement
                {
                    AssetRequestId = assetRequest.Id,
                    Name = assetRequest.Name ?? string.Empty,
                    NewType = string.Empty, // Menyediakan nilai default (string kosong)
                    NewSerialNumber = string.Empty, // Menyediakan nilai default (string kosong)
                    
                };
                _context.NewAssetReplacement.Add(newAssetReplacement);
            }
            else if (typeReplace == "ComponentAssetReplacement")
            {
                var componentAssetReplacement = new ComponentAssetReplacement
                {

                    AssetRequestId = assetRequest.Id,
                    Name = assetRequest.Name ?? string.Empty
                };
                _context.ComponentAssetReplacement.Add(componentAssetReplacement);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Asset request approved successfully!" });
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details
                Console.WriteLine(ex.ToString());
                return Json(new { success = false, message = "Error approving the request: " + (ex.InnerException?.Message ?? "No additional details available.") });
            }
        }




        //REJECT MANAGER
        [HttpPost]
        public async Task<IActionResult> Reject(int id, string justify)
        {
            var assetRequest = await _context.AssetRequest.FindAsync(id);
            if (assetRequest == null)
            {
                return NotFound();
            }

            assetRequest.Status = false; // Mengubah status menjadi false (0) untuk rejected
            assetRequest.Justify = justify;
            assetRequest.ApprovalDate = DateTime.Now;
            _context.Update(assetRequest);
            await _context.SaveChangesAsync();


            return Json(new { success = true, message = "Asset request rejected successfully!" });
        }

        private bool AssetRequestExists(int id)
        {
            return _context.AssetRequest.Any(e => e.Id == id);
        }
    }
}

