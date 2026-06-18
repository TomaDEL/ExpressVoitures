using ExpressVoitures.Services;
using ExpressVoitures.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExpressVoitures.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;

        public CarController(ICarService carService, IBrandService brandService)
        {
            _carService = carService;
            _brandService = brandService;
        }

        // GET /Car/Details/5 - fiche détail public
        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            ViewBag.SellingPrice = await _carService.GetSellingPriceAsync(id);
            return View(car);
        }

        // GET /Car/Create - formulaire d'ajout (admin)
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            return View();
        }

        // POST /Car/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                await _carService.AddCarAsync(car);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            return View(car);
        }

        // GET /Car/Edit/5 - formulaire de modification (admin)
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            return View(car);
        }

        // POST /Car/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                await _carService.UpdateCarAsync(car);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            return View(car);
        }

        // POST /Car/SetUnavailable/5 - marquer commme vendue
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetUnavailable(int id)
        {
            await _carService.SetUnavailableAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET /Car/GetModelsByBrand/5 - endpoint AJAX pour les menus déroulants
        public async Task<IActionResult> GetModelsByBrand(int brandId)
        {
            var brand = await _brandService.GetBrandByIdAsync(brandId);
            if (brand == null)
            {
                return Json(new List<object>());
            }
            var models = brand.CarModels.Select(m => new { m.Id, m.Name }).ToList();

            return Json(models);
        }
    }
}
