using Microsoft.AspNetCore.Mvc.Rendering;

namespace GustoExpress.Services.Data.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<SelectListItem> GetCategories();
    }
}
