using GustoExpress.Data.Models.Enums;
using GustoExpress.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GustoExpress.Services.Data
{
    public class CategoryService : ICategoryService
    {
        public IEnumerable<SelectListItem> GetCategories()
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
