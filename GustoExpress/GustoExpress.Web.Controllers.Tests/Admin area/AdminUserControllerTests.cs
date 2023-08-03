namespace GustoExpress.Web.Controllers.Tests.Admin_area
{
    using Microsoft.AspNetCore.Mvc;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Areas.Admin.Controllers;
    using GustoExpress.Web.ViewModels;

    using Moq;

    [TestFixture]
    public class AdminUserControllerTests
    {
        private UserController controller;

        private Mock<IUserService> _userService;

        [SetUp]
        public void SetUp()
        {
            _userService = new Mock<IUserService>();

            controller = new UserController(_userService.Object);
        }

        [Test]
        public async Task Test_All_ShouldWork()
        {
            var expectedModel = new List<UserViewModel>()
            {
                new UserViewModel(){ Email = "test@test.com" },
            };

            _userService
                .Setup(u => u.AllUsersAsync())
                .ReturnsAsync(expectedModel);

            var result = await controller.All();

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }
    }
}
