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
        public List<Order> Orders { get; set; }

        public Customer()
        {
            Orders = new List<Order>();
        }

        public Customer(string eAdd, string custName)
        {
            EmailAddress = eAdd;
            CustomerName = custName;
            Orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        public bool RemoveOrder(Order order)
        {
            return Orders.Remove(order);
        }

        public void DisplayAllOrders()
        {
            foreach (var order in Orders)
            {
                Console.WriteLine(order);
            }
        }

        public override string ToString()
        {
            return $"{CustomerName} ({EmailAddress})";
        }
    }

}
