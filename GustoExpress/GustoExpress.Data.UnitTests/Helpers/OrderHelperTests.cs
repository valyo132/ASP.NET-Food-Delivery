using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Helpers.Order;

namespace GustoExpress.Services.Data.UnitTests.Helpers
{
    [TestFixture]
    public class OrderHelperTests
    {
        private Order order = new Order()
        {
            IsCompleted = true,
            UserId = "order-id",
            OrderItems = new List<OrderItem>() 
            { 
                new OrderItem() { TotalCost = 10 },
                new OrderItem() { TotalCost = 15 },
            }
        };

        [Test]
        public void Test_GetOrderTotalCost_ShouldWork()
        {
            decimal actual = OrderHelper.GetOrderTotalCost(order);
            decimal expected = 25m;

            Assert.AreEqual(expected, actual);
        }
    }
}
