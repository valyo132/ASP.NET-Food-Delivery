namespace GustoExpress.Services.Data.UnitTests.Helpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using GustoExpress.Services.Data.Helpers.Product;

    [TestFixture]
    public class ProductHelperTests
    {
        [Test]
        public async Task Test_GetCategories_ShouldWork()
        {
            var actual = ProductHelper.GetCategories();

            Assert.That(actual.GetType(), Is.EqualTo(typeof(List<SelectListItem>)));
        }
    }
}
