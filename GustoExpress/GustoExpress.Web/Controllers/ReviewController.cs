﻿namespace GustoExpress.Web.Controllers
{
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(CreateReviewViewModel obj)
        {
            if (ModelState.IsValid)
            {
                string userId = GetUserId();
                ReviewViewModel review = await _reviewService.CreateReview(userId, obj);

                TempData["success"] = "Successfully created review!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = review.RestaurantId.ToString() });
            }

            TempData["danger"] = "Invalid operation!";
            return RedirectToAction("RestaurantPage", "Restaurant", new { id = obj.RestaurantId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(string id)
        {
            ReviewViewModel deletedReview = await _reviewService.DeleteAsync(id);

            TempData["success"] = "Successfully deleted review!";
            return RedirectToAction("RestaurantPage", "Restaurant", new { id = deletedReview.RestaurantId });
        }
    }
}
