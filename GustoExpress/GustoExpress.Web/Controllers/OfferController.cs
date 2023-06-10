using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GustoExpress.Web.Controllers
{
    [Authorize]
    public class OfferController : BaseController
    {
        private readonly IOfferService _offerService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OfferController(IOfferService offerService,
            IWebHostEnvironment webHostEnvironment)
        {
            _offerService = offerService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOffer(string id)
        {
            ViewData["RestaurantId"] = id;
            CreateOfferViewModel model = new CreateOfferViewModel()
            {
                ProductsToChoose = await _offerService.GetProductsByRestaurantIdAsync(id)
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOffer(IFormFile? file, string id, CreateOfferViewModel obj)
        {
            if (ModelState.IsValid)
            {
                OfferViewModel offer = await _offerService.CreateOfferAsync(id, obj);

                if (file != null)
                {
                    if (offer.ImageURL != null)
                        DeleteImage(offer.ImageURL);

                    await SaveImage(file, offer);
                }

                TempData["success"] = "Successfully created offer!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = id });
            }

            return View(obj);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditOffer(string id)
        {
            CreateOfferViewModel model = await _offerService.ProjectToModel<CreateOfferViewModel>(id);
            model.ProductsToChoose = await _offerService.GetProductsByRestaurantIdAsync(model.RestaurantId.ToString());

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditOffer(IFormFile? file, string id, CreateOfferViewModel obj)
        {
            if (ModelState.IsValid)
            {
                OfferViewModel offer = await _offerService.EditOfferAsync(id, obj);

                if (file != null)
                {
                    if (offer.ImageURL != null)
                        DeleteImage(offer.ImageURL);

                    await SaveImage(file, offer);
                }

                TempData["success"] = "Successfully updated offer!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = offer.RestaurantId });
            }

            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOffer(string id)
        {
            OfferViewModel deletedOffer = await _offerService.DeleteOfferAsync(id);

            TempData["success"] = "Successfully deleted offer!";
            return RedirectToAction("RestaurantPage", "Restaurant", new { id = deletedOffer.RestaurantId });
        }

        private async Task SaveImage(IFormFile file, OfferViewModel offer)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string imagePath = Path.Combine(wwwRootPath, @"images/Offers");

            using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string imageURL = @"/images/Offers/" + fileName;
            await _offerService.SaveImageURL(imageURL, offer);
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
