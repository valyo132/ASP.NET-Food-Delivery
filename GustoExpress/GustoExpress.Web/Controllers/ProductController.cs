using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GustoExpress.Web.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService,
            IWebHostEnvironment webHostEnvironment,
            ICategoryService categoryService)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateProduct(string id)
        {
            ViewData["restaurantId"] = id;
            CreateProductViewModel productVm = new CreateProductViewModel()
            {
                CategoryList = _categoryService.GetCategories()
            };

            return View(productVm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(IFormFile? file, CreateProductViewModel obj)
        {
            if (ModelState.IsValid)
            {
                ProductViewModel product = await _productService.CreateProductAsync(obj);

                if (file != null)
                    await SaveImage(file, product);

                TempData["success"] = "Successfully created product!";
                return RedirectToAction("RestaurantPage", "Restaurant", new {id = product.RestaurantId});
            }

            return View(obj);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(string id)
        {
            CreateProductViewModel createProductVm = await _productService.ProjectToModel<CreateProductViewModel>(id);
            createProductVm.CategoryList = _categoryService.GetCategories();

            ViewData["restaurantId"] = createProductVm.RestaurantId;

            return View(createProductVm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(IFormFile? file, string id, CreateProductViewModel obj)
        {
            if (ModelState.IsValid)
            {
                ProductViewModel editedProduct = await _productService.EditProductAsync(id, obj);

                if (file != null)
                {
                    if (editedProduct.ImageURL != null)
                        DeleteImage(editedProduct.ImageURL);

                    await SaveImage(file, editedProduct);
                }

                TempData["success"] = "Successfully updated product!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = editedProduct.RestaurantId });
            }

            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            ProductViewModel deletedProduct = await _productService.DeleteAsync(id);

            TempData["success"] = "Successfully deleted product!";
            return RedirectToAction("RestaurantPage", "Restaurant", new { id = deletedProduct.RestaurantId });
        }

        private async Task SaveImage(IFormFile file, ProductViewModel product)
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
