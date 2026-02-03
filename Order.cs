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
        public string OrderStatus { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string OrderPaymentMethod { get; set; }
        public bool OrderPaid { get; set; }

        private List<OrderedFoodItem> orderedFoodItems = new List<OrderedFoodItem>();

        public double CalculateOrderTotal()
        {
            OrderTotal = 0;
            foreach (var item in orderedFoodItems)
                OrderTotal += item.CalculateSubtotal();
            return OrderTotal;
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            orderedFoodItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            return orderedFoodItems.Remove(item);
        }

        public void DisplayOrderedFoodItems()
        {
            foreach (var item in orderedFoodItems)
                Console.WriteLine(item.ToString());
        }

        public override string ToString()
        {
            return $"Order {OrderId} - Total: ${OrderTotal}";
        }
    }
}
