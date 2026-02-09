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
    public class Customer
    {
        public string EmailAddress { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public List<Order> Orders { get; set; } = new List<Order>();

        public Customer() { }

        public Customer(string eAdd, string custName)
        {
            CustomerName = custName;
            EmailAddress = eAdd;
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
