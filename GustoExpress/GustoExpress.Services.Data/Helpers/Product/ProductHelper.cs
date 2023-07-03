namespace GustoExpress.Services.Data.Helpers.Product
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using GustoExpress.Data.Models.Enums;

    public static class ProductHelper
    {
        public static IEnumerable<SelectListItem> GetCategories()
            => Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();
    }
}
