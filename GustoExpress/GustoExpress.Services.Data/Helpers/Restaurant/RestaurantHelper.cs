﻿namespace GustoExpress.Services.Data.Helpers.Restaurant
{
    using GustoExpress.Web.ViewModels.Enums.Restaurant;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class RestaurantHelper
    {
        public static IEnumerable<SelectListItem> GetRestaurantSortingValues()
           => Enum.GetValues(typeof(RestaurantSorting))
               .Cast<RestaurantSorting>()
               .Select(r => new SelectListItem()
               {
                   Value = r.ToString(),
                   Text = r.ToString()
               })
               .ToList();

        public static int GetTimeDifference(string timeToDeliver)
        {
            string[] times = timeToDeliver.Split('-');
            int startTime = int.Parse(times[0]);
            int endTime = int.Parse(times[1]);

            return endTime - startTime;
        }
    }
}
