namespace GustoExpress.Web.ViewModels
{
    using GustoExpress.Web.ViewModels.Enums.Restaurant;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AllRestaurantViewModel
    {
        public List<RestaurantViewModel> Restaurants { get; set; }
        public string CityName { get; set; } = null!;
        public IEnumerable<SelectListItem> SortingItems { get; set; }
        public RestaurantSorting Sort { get; set; }
        public string? SearchString { get; set; }
    }
}
