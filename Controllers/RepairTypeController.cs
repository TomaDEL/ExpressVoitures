using ExpressVoitures.Services;
using ExpressVoitures.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExpressVoitures.Controllers
{
    [Authorize]
    public class RepairTypeController : Controller
    {
        private readonly IRepairTypeService _repairTypeService;
        public RepairTypeController(IRepairTypeService repairTypeService)
        {
            _repairTypeService = repairTypeService;
        }
        // GET /RepairType
        public async Task<IActionResult> Index()
        {
            var repairTypes = await _repairTypeService.GetAllRepairTypesAsync();
            return View(repairTypes);
        }
        // GET /RepairType/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST /RepairType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairType repairType)
        {
            if (ModelState.IsValid)
            {
                await _repairTypeService.AddRepairTypeAsync(repairType);
                return RedirectToAction(nameof(Index));
            }
            return View(repairType);
        }
        // GET /RepairType/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var repairType = await _repairTypeService.GetRepairTypeByIdAsync(id);
            if (repairType == null)
            {
                return NotFound();
            }
            return View(repairType);
        }
        // POST /RepairType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RepairType repairType)
        {
            if (ModelState.IsValid)
            {
                await _repairTypeService.UpdateRepairTypeAsync(repairType);
                return RedirectToAction(nameof(Index));
            }
            return View(repairType);
        }
        // POST /RepairType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _repairTypeService.DeleteRepairTypeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
