namespace GustoExpress.Web.Controllers.Tests
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController controller;

        [SetUp]
        public void SetUp()
        {
            controller = new HomeController();
            var context = new DefaultHttpContext();
            context.User = CreateMockPrincipalWithRole("Admin");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
        }

        [Test]
        public void Test_Index_ShouldReturnCorrentViewWhenLoggedInUserIsAdmin()
        {
            var result = controller.Index(null);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void Test_Index_ShouldReturnCorrentViewWhenLoggedInUserIsUser()
        {
            var context = new DefaultHttpContext();
            context.User = CreateMockPrincipalWithRole("User");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            var result = controller.Index("Test city");

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("All"));
        }

        [Test]
        public void Test_Index_ShouldReturnItselfWhenCityIsNull()
        {
            var context = new DefaultHttpContext();
            context.User = CreateMockPrincipalWithRole("User");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            var result = controller.Index(null);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [TestCase(404)]
        [TestCase(400)]
        public void Error_WithStatusCode404Or400_ShouldReturnError404View(int statucCode)
        {
            var result = controller.Error(statucCode);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.AreEqual("Error404", viewResult.ViewName);
        }

        [TestCase(401)]
        public void Error_WithStatusCode401_ShouldReturnError401View(int statucCode)
        {
            var result = controller.Error(statucCode);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.AreEqual("Error401", viewResult.ViewName);
        }

        [TestCase(500)]
        [TestCase(501)]
        public void Test_Error_ShouldReturnItself(int statucCode)
        {
            var result = controller.Error(statucCode);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.IsNull(viewResult.ViewName);
        }

        private ClaimsPrincipal CreateMockPrincipalWithRole(string role)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            return new ClaimsPrincipal(identity);
        }
    }
}
