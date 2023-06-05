using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GustoExpress.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductService productService,
            IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateProduct(string id)
        {
            ViewData["restaurantId"] = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(IFormFile? file, CreateProductViewModel obj)
        {
            if (ModelState.IsValid)
            {
                Product product = await _productService.CreateProduct(obj);

                if (file != null)
                    await SaveImage(file, product);

                return RedirectToAction("Index", "Home");
            }

            return View(obj);
        }

        private async Task SaveImage(IFormFile file, Product product)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string imagePath = Path.Combine(wwwRootPath, @"images/Products");

            using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string imageURL = @"/images/Products/" + fileName;
            await _productService.SaveImageURL(imageURL, product);
        }
    }
}
