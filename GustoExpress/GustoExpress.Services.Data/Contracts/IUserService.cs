namespace GustoExpress.Services.Data.Contracts
{
    using GustoExpress.Web.ViewModels;

    public interface IUserService
    {
        Task<List<UserViewModel>> AllUsersAsync();
    }
}
