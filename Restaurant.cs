using S10273555_PRG2Assignment;
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
    class Restaurant
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }

<<<<<<< HEAD
        public List<Menu> menu = new List<Menu>();
        public List<SpecialOffer> offers = new List<SpecialOffer>();
        public Queue<Order> orders = new Queue<Order>();
=======
        public List<Menu> Menus { get; set; }
        public List<SpecialOffer> Offers { get; set; }
        public Queue<Order> orders = new Queue<Order>();

>>>>>>> origin/master
        public Restaurant()
        {
            Menus = new List<Menu>();
            Offers = new List<SpecialOffer>();
        }

        public Restaurant(string rId, string rName, string rEmail)
        {
            RestaurantId = rId;
            RestaurantName = rName;
            RestaurantEmail = rEmail;
            Menus = new List<Menu>();
            Offers = new List<SpecialOffer>();
        }

        public void DisplayOrders()
        {
            
        }

        public void DisplaySpecialOffers()
        {
            foreach (SpecialOffer offer in Offers)
            {
                Console.WriteLine(offer);
            }
        }

        public void DisplayMenu()
        {
            foreach (Menu m in Menus)
            {
                m.DisplayFoodItems();
            }
        }

        public void AddMenu(Menu menu)
        {
            Menus.Add(menu);
        }

        public bool RemoveMenu(Menu menu)
        {
            return Menus.Remove(menu);
        }

        public override string ToString()
        {
            return $"{RestaurantName} ({RestaurantId}) - {RestaurantEmail}";
        }
    }
}

