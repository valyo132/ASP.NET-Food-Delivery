using GustoExpress.Data.Models;
using GustoExpress.Data.Models.Enums;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            CreateProductViewModel productVm = new CreateProductViewModel()
            {
                CategoryList = Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList()
            };

            return View(productVm);
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(string id)
        {
            var product = await _productService.GetById(id);
            ViewData["restaurantId"] = product.RestaurantId;

            var createProductVm = _productService.ProjectTo<CreateProductViewModel>(product);
            createProductVm.CategoryList = Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();

            return View(createProductVm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(IFormFile? file, string id, CreateProductViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var editedProduct = await _productService.EditProduct(id, obj);

                if (file != null)
                {
                    DeleteImage(editedProduct.ImageURL);
                    await SaveImage(file, editedProduct);
                }

                return RedirectToAction("RestaurantPage", "Restaurant", new { id = obj.RestaurantId });
            }

            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            Product deletedProduct = await _productService.DeleteAsync(id);

            return RedirectToAction("RestaurantPage", "Restaurant", new { id = deletedProduct.RestaurantId });
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

        private void DeleteImage(string file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string imagePath = wwwRootPath + file;

            FileInfo fileInfo = new FileInfo(imagePath);
            if (fileInfo != null)
            {
                System.IO.File.Delete(imagePath);
                fileInfo.Delete();
            }
        }
    }
}
