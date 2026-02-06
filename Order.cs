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
    class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double OrderTotal { get; set; }
        private double orderTotal;
        public string OrderStatus { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string OrderPaymentMethod { get; set; }
        public bool OrderPaid { get; set; }

        public List<OrderedFoodItem> OrderedFoodItems { get; set; }

        public Order(int orderId, DateTime orderDateTime, string status,
             string deliveryAddress, string paymentMethod)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderStatus = status;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = paymentMethod;
            OrderPaid = false;
            OrderedFoodItems = new List<OrderedFoodItem>();
        }

        public Order(int id)
        {
            OrderId = id;
            OrderDateTime = DateTime.Now;
            OrderStatus = "Pending";
            OrderPaid = false;
        }
        public Order(int orderId, DateTime orderDateTime, string status,
             string deliveryAddress, string paymentMethod, double orderTotal)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderStatus = status;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = paymentMethod;
            OrderPaid = false;
            OrderedFoodItems = new List<OrderedFoodItem>();
            OrderTotal = orderTotal;
        }

        public double CalculateOrderTotal()
        {
            OrderTotal = 0;

            foreach (OrderedFoodItem item in OrderedFoodItems)
            {
                OrderTotal += item.CalculateSubtotal();
            }

            return OrderTotal;
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
            foreach (OrderedFoodItem item in OrderedFoodItems)
            {
                Console.WriteLine(item);
            }
        }

        public override string ToString()
        {
            return $"Order {OrderId} - Total: ${OrderTotal}";
        }
    }
}