using S10273555_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2Assignment
{
    class Restaurant 
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }

        private List<Menu> menu = new List<Menu>();
        private List<SpecialOffer> offers = new List<SpecialOffer>();
        public Restaurant()
        {

        }
        public Restaurant(string rId, string rName, string rEmail)
        {
            RestaurantId = rId;
            RestaurantName = rName;
            RestaurantEmail = rEmail;
        }

        public void DisplayOrders()
        {

        }
        public void DisplaySpecialOffers()
        {
            foreach (var offer in offers)
                Console.WriteLine(offer.ToString());
        }
        public void DisplayMenu()
        {
            foreach (var menu in menu)
                menu.DisplayFoodItems(); //from menu 
        }
        public void AddMenu(Menu menuItem)
        {
            menu.Add(menuItem);
        }
        public bool RemoveMenu(Menu menuItem)
        {
            return menu.Remove(menuItem);
        }
        public override string ToString()
        {
            return $"{RestaurantName} ({RestaurantId}) - {RestaurantEmail}";
        }
    }
}
