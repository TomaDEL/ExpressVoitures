using ExpressVoitures.Services;
using ExpressVoitures.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExpressVoitures.Models;

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

            var viewModel = new CarIndexViewModel
            {
                Cars = cars,
                SellingPrices = await _carService.GetAllSellingPricesAsync(cars)
            };

            return View(viewModel);

            //// Calcule le prix de vente pour chaque voiture et le stocke dans un dictionnaire Id -> Prix
            //var sellingPrices = new Dictionary<int, decimal>();
            //foreach (var car in cars)
            //{
            //    sellingPrices[car.Id] = await _carService.GetSellingPriceAsync(car.Id);
            //}
            ////var carsWithPrices = cars.Select(car => new
            ////{
            ////    Car = car,
            ////    SellingPrice = sellingPrices[car.Id]
            ////}).ToList();
            //ViewBag.SellingPrices = sellingPrices;
            //return View(cars);
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
        public async Task<IActionResult> Create(
    Car car,
    IFormFile? PhotoFile,
    string? NewBrand,
    string? NewCarModel,
    string? NewTrim,
    int? BrandId,
    string? CarModelIdStr,
    string? TrimIdStr)
        {
            // ── Validation manuelle ───────────────────────────────
            var brandIdValue = Request.Form["BrandId"].ToString();
            var carModelIdValue = Request.Form["CarModelId"].ToString();
            var trimIdValue = Request.Form["TrimId"].ToString();

            if (string.IsNullOrEmpty(brandIdValue) ||
                (brandIdValue != "new" && brandIdValue == ""))
            {
                ModelState.AddModelError("BrandId",
                    "La marque est obligatoire.");
            }

            if (string.IsNullOrEmpty(brandIdValue) == false &&
                brandIdValue == "new" &&
                string.IsNullOrEmpty(NewBrand))
            {
                ModelState.AddModelError("NewBrand",
                    "Veuillez saisir le nom de la nouvelle marque.");
            }

            if (string.IsNullOrEmpty(carModelIdValue) ||
                carModelIdValue == "0")
            {
                ModelState.AddModelError("CarModelId",
                    "Le modèle est obligatoire.");
            }

            if (carModelIdValue == "new" &&
                string.IsNullOrEmpty(NewCarModel))
            {
                ModelState.AddModelError("NewCarModel",
                    "Veuillez saisir le nom du nouveau modèle.");
            }

            if (string.IsNullOrEmpty(trimIdValue) ||
                trimIdValue == "0")
            {
                ModelState.AddModelError("TrimId",
                    "La finition est obligatoire.");
            }

            if (trimIdValue == "new" &&
                string.IsNullOrEmpty(NewTrim))
            {
                ModelState.AddModelError("NewTrim",
                    "Veuillez saisir le nom de la nouvelle finition.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await _brandService.GetAllBrandsAsync();
                return View(car);
            }
            try
            {
                // ── Gestion de la marque ──────────────────────────
                int finalBrandId;
                if (!string.IsNullOrEmpty(NewBrand))
                {
                    // Nouvelle marque → on la crée
                    var brand = new Brand { Name = NewBrand.Trim() };
                    await _brandService.AddBrandAsync(brand);
                    finalBrandId = brand.Id;
                }
                else
                {
                    finalBrandId = BrandId ?? 0;
                }

                // ── Gestion du modèle ─────────────────────────────
                int finalCarModelId;
                if (!string.IsNullOrEmpty(NewCarModel))
                {
                    // Nouveau modèle → on le crée lié à la marque
                    var carModel = new CarModel
                    {
                        Name = NewCarModel.Trim(),
                        BrandId = finalBrandId
                    };
                    await _carModelService.AddCarModelAsync(carModel);
                    finalCarModelId = carModel.Id;
                }
                else
                {
                    finalCarModelId = car.CarModelId;
                }

                // ── Gestion de la finition ────────────────────────
                int finalTrimId;
                if (!string.IsNullOrEmpty(NewTrim))
                {
                    // Nouvelle finition → on la crée liée au modèle
                    var trim = new Trim
                    {
                        Name = NewTrim.Trim(),
                        CarModelId = finalCarModelId
                    };
                    await _trimService.AddTrimAsync(trim);
                    finalTrimId = trim.Id;
                }
                else
                {
                    finalTrimId = car.TrimId;
                }

                // ── Gestion de la photo ───────────────────────────
                if (PhotoFile != null && PhotoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(PhotoFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoFile.CopyToAsync(stream);
                    }

                    car.PhotoUrl = "/uploads/" + fileName;
                }

                // ── Création de la voiture ────────────────────────
                car.CarModelId = finalCarModelId;
                car.TrimId = finalTrimId;
                car.IsAvailable = true;

                await _carService.AddCarAsync(car);
                return RedirectToAction("CreateSuccess");
            }
            catch
            {
                ViewBag.Brands = await _brandService.GetAllBrandsAsync();
                return View(car);
            }
        }

        // GET /Car/CreateSuccess
        [HttpGet]
        [Authorize]
        public IActionResult CreateSuccess()
        {
            return View();
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
        public async Task<IActionResult> Edit(Car car, IFormFile? PhotoFile)
        {
            if (ModelState.IsValid)
            {
                if (PhotoFile != null && PhotoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(PhotoFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoFile.CopyToAsync(stream);
                    }

                    // Remplace l'ancienne photo par la nouvelle
                    car.PhotoUrl = "/uploads/" + fileName;
                }

                await _carService.UpdateCarAsync(car);
                return RedirectToAction("Details", new { id = car.Id });
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
            var car = await _carService.GetCarByIdAsync(id);
            await _carService.SetUnavailableAsync(id);

            ViewBag.CarName = $"{car?.Year} {car?.CarModel?.Brand?.Name} {car?.CarModel?.Name}";
            return View("DeleteSuccess");
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
