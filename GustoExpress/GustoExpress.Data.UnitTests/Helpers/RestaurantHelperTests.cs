namespace GustoExpress.Services.Data.UnitTests.Helpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using GustoExpress.Services.Data.Helpers.Restaurant;

    [TestFixture]
    public class RestaurantHelperTests
    {
        [Test]
        public void Test_GetRestaurantSortingValues_ShouldWork()
        {
            var actual = RestaurantHelper.GetRestaurantSortingValues();

            Assert.That(actual.GetType(), Is.EqualTo(typeof(List<SelectListItem>)));
        }

        [Test]
        public void Test_GetTimeDifference_ShouldWork()
        {
            string time = "20-30";

            int actual = RestaurantHelper.GetTimeDifference(time);
            int expected = 10;

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
