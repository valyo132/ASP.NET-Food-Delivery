namespace GustoExpress.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Data;
    using GustoExpress.Web.ViewModels;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> GetUserEmailByUsername(string username)
        {
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.FirstName.ToLower() == username.Split()[0].ToLower());

            return user.Email;
        }

        public async Task<List<UserViewModel>> AllUsersAsync()
        {
            return await _context.ApplicationUsers
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
