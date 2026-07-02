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
        private readonly ICarModelService _carModelService;
        private readonly ITrimService _trimService;

        public CarController(ICarService carService, IBrandService brandService, ICarModelService carModelService, ITrimService trimService)
        {
            _carService = carService;
            _brandService = brandService;
            _carModelService = carModelService;
            _trimService = trimService;
        }

        // GET /Car - catalogue public
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cars = await _carService.GetAllCarsAsync();
            return View(cars);
        }

        // GET /Car/Details/5 - fiche détail public
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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

        // GET /Car/GetModelsByBrand/5
        // Renvoie la liste des modèles pour une marque donnée (utilisé par AJAX)
        [HttpGet]
        public async Task<IActionResult> GetModelsByBrand(int brandId)
        {
            var models = await _carModelService.GetCarModelsByBrandIdAsync(brandId);
            var result = models.Select(m => new { m.Id, m.Name }).ToList();

            return Json(result);
        }

        // Finition selon le modèle choisi
        [HttpGet]
        public async Task<IActionResult> GetTrimsByModel(int carModelId)
        {
            var trims = await  _trimService.GetTrimsByCarModelIdAsync(carModelId);

            var result = trims
                .Select(t => new { t.Id, t.Name })
                .ToList();

            return Json(result);
        }
    }
}
