namespace GustoExpress.Services.Data.Helpers.Order
{
    using GustoExpress.Data.Models;

    public static class OrderHelper
    {
        public static decimal GetOrderTotalCost(Order order)
        {
            return order.OrderItems.Sum(oi => oi.TotalCost);
        }
    }
}
