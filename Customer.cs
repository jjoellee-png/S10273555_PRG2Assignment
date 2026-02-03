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
    class Customer
    {
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }
        public Customer()
        {

        }
        public Customer(string eAdd, string custName)
        {
            EmailAddress = eAdd;
            CustomerName = custName;
        }

        private List<Order> orders = new List<Order>();

        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        public bool RemoveOrder(Order order)
        {
            return orders.Remove(order);
        }

        public void DisplayAllOrders()
        {
            foreach (var order in orders)
                Console.WriteLine(order.ToString());
        }

        public override string ToString()
        {
            return $"{CustomerName} ({EmailAddress})";
        }
    }
}
