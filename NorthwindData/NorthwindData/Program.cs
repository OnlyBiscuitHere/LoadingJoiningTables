using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace NorthwindData
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new NorthwindContext())
            {
                var ordersQuery = from order in db.Orders.Include(o => o.Customer) where order.Freight > 750 select order;
                foreach (var order in ordersQuery)
                {
                    if (order.Customer != null)
                    {
                        Console.WriteLine(  $"{order.Customer.CompanyName} of {order.Customer.City} paid {order.Freight} for shipping");
                    }
                }
                Console.WriteLine();
                var ordersQuery2 = db.Orders.Include(o => o.Customer).Include(c => c.OrderDetails).Where(o => o.Freight > 750).Select(o=>o);
                foreach (var order in ordersQuery2)
                {
                    Console.WriteLine($"Order {order.OrderId} was made by {order.Customer.CompanyName}");
                    foreach (var od in order.OrderDetails)
                    {
                        Console.WriteLine($"\t ProductID: {od.ProductId}");
                    }
                }
                Console.WriteLine();
                var ordersQuery3 = db.Orders.Include(o => o.Customer).Include(c => c.OrderDetails).ThenInclude(od => od.Product).Where(o => o.Freight > 750).Select(o => o);
                foreach (var o in ordersQuery3)
                {
                    Console.WriteLine($"Order {o.OrderId} was made by {o.Customer.CompanyName}");
                    foreach (var od in o.OrderDetails)
                    {
                        Console.WriteLine($"\t Product {od.ProductId} - Product: {od.Product.ProductName} - Quantity: {od.Quantity}");
                    }
                }
                Console.WriteLine();
                var orderQueryUsingJoin = from order in db.Orders where order.Freight > 750 join customer in db.Customers on order.CustomerId equals customer.CustomerId select new { CustomerContactName = customer.ContactName, City = customer.City, Freight = order.Freight};
                foreach (var result in orderQueryUsingJoin)
                {
                    Console.WriteLine($"{result.CustomerContactName} of {result.City} paid {result.Freight} for shipping");
                }
                Console.WriteLine();
                var orderCustomerBerlinParisQuery = from o in db.Orders join c in db.Customers on o.CustomerId equals c.CustomerId where c.City == "Berlin" || c.City == "Paris" select new { o.OrderId, c.CompanyName };
                foreach (var result in orderCustomerBerlinParisQuery)
                {
                    Console.WriteLine($"{result.CompanyName} and {result.OrderId}");
                }
            }
        }
    }
}
