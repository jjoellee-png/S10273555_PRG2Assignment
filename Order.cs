using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Student Number: S10275515
// Student Name: Nur Tiara Nasha
// Partner Name: Joelle
namespace PRG2Assignment
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double OrderTotal { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; } = "";
        public string OrderPaymentMethod { get; set; } = "";
        public bool OrderPaid { get; set; }

        // Class associations
        public Customer Customer { get; set; }  // links to the customer
        public Restaurant Restaurant { get; set; }  // links to the restaurant

        public List<OrderedFoodItem> OrderedFoodItems { get; set; }

        // Constructor with just ID
        public Order(int id)
        {
            OrderId = id;
            OrderDateTime = DateTime.Now;
            OrderStatus = "Pending";
            OrderPaid = false;
            OrderedFoodItems = new List<OrderedFoodItem>();
        }

        // Constructor with full info (without order total)
        public Order(int orderId, DateTime orderDateTime, string status,
                     string deliveryAddress, string paymentMethod,
                     Customer customer = null, Restaurant restaurant = null)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderStatus = status;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = paymentMethod;
            OrderPaid = false;
            OrderedFoodItems = new List<OrderedFoodItem>();
            Customer = customer;
            Restaurant = restaurant;
        }

        // Constructor including order total
        public Order(int orderId, DateTime orderDateTime, string status,
                     string deliveryAddress, string paymentMethod, double orderTotal,
                     Customer customer = null, Restaurant restaurant = null)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderStatus = status;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = paymentMethod;
            OrderPaid = false;
            OrderedFoodItems = new List<OrderedFoodItem>();
            OrderTotal = orderTotal;
            Customer = customer;
            Restaurant = restaurant;
        }

        // Calculate total dynamically based on OrderedFoodItems
        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach (var item in OrderedFoodItems)
            {
                total += item.CalculateSubtotal();
            }
            OrderTotal = total;
            return total;
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            OrderedFoodItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            return OrderedFoodItems.Remove(item);
        }

        public void DisplayOrderedFoodItems()
        {
            int count = 1;
            foreach (var item in OrderedFoodItems)
            {
                Console.WriteLine($"{count}. {item.ItemName} - {item.QtyOrdered} x ${item.ItemPrice:0.00} = ${item.CalculateSubtotal():0.00}");
                count++;
            }
        }

        public override string ToString()
        {
            string customerName = Customer != null ? Customer.CustomerName : "Unknown Customer";
            string restaurantName = Restaurant != null ? Restaurant.RestaurantName : "Unknown Restaurant";

            return $"Order {OrderId} ({customerName} at {restaurantName}) - Total: ${OrderTotal:0.00} - Status: {OrderStatus}";
        }
    }
}