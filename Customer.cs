using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Student Number: S10275515
// Student Name: Nur Tiara Nashape
// Partner Name: Joelle
namespace PRG2Assignment
{
    public class Customer
    {
        public string EmailAddress { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public List<Order> Orders { get; set; } = new List<Order>();

        public Customer() { }

        public Customer(string custName, string eAdd)
        {
            CustomerName = custName.Trim();
            EmailAddress = eAdd.Trim();
        }

        public void AddOrder(Order order)
        {
            if (Orders == null) Orders = new List<Order>();
            Orders.Add(order);
            if (order != null) order.Customer = this;
        }

        public bool RemoveOrder(Order order)
        {
            if (Orders == null) return false;
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
