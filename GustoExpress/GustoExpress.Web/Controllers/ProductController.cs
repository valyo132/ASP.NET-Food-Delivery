namespace GustoExpress.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Services.Data.Helpers;
    using GustoExpress.Services.Data.Helpers.Product;
    using GustoExpress.Web.ViewModels;
    using GustoExpress.Services.Data;

    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRestaurantService _restaurantService;

        public ProductController(IProductService productService,
            IWebHostEnvironment webHostEnvironment,
            IRestaurantService restaurantService)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(string id)
        {
            if (!await _restaurantService.HasRestaurantWithId(id))
            {
                return GeneralError();
            }

            try
            {
                CreateProductViewModel productVm = new CreateProductViewModel()
                {
                    RestaurantId = id,
                    CategoryList = ProductHelper.GetCategories()
                };

                return View(productVm);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(IFormFile? file, CreateProductViewModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductViewModel product = await _productService.CreateProductAsync(obj);

                    if (file != null)
                        await SaveImage(file, product);

                    TempData["success"] = "Successfully created product!";
                    return RedirectToAction("RestaurantPage", "Restaurant", new { id = product.RestaurantId });
                }
            }
            catch (InvalidOperationException ioe)
            {
                TempData["danger"] = ioe.Message;
            }
            catch (Exception)
            {
                return GeneralError();
            }

            obj.CategoryList = ProductHelper.GetCategories();
            return View(obj);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(string id)
        {
            if (!await _productService.HasProductWithId(id))
            {
                return GeneralError();
            }

            CreateProductViewModel createProductVm = await _productService.ProjectToModel<CreateProductViewModel>(id);
            createProductVm.CategoryList = ProductHelper.GetCategories();

            return View(createProductVm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(IFormFile? file, string id, CreateProductViewModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (obj.Discount > obj.Price)
                    {
                        TempData["danger"] = "Invalid operation!";
                        return RedirectToAction("RestaurantPage", "Restaurant", new { id = obj.RestaurantId });
                    }

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
            }
            catch (Exception)
            {
                return GeneralError();
            }

            obj.CategoryList = ProductHelper.GetCategories();
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (!await _productService.HasProductWithId(id))
            {
                return GeneralError();
            }

            try
            {
                ProductViewModel deletedProduct = await _productService.DeleteAsync(id);

                TempData["success"] = "Successfully deleted product!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = deletedProduct.RestaurantId });
            }
            catch (Exception)
            {
                return GeneralError();
            }
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

            FileHelper.DeleteImage(imagePath);
        }
    }
}
