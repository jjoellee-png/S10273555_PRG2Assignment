namespace PRG2Assignment
{
    class Restaurant
    {
        public string RestaurantId { get; set; } = "";
        public string RestaurantName { get; set; } = "";
        public string RestaurantEmail { get; set; } = "";

        public List<Menu> Menus { get; set; }
        public List<SpecialOffer> Offers { get; set; }
        public Queue<Order> Orders { get; set; }

        public Restaurant()
        {
            Menus = new List<Menu>();
            Offers = new List<SpecialOffer>();
            Orders = new Queue<Order>();
        }

        public Restaurant(string rId, string rName, string rEmail)
        {
            RestaurantId = rId;
            RestaurantName = rName;
            RestaurantEmail = rEmail;
            Menus = new List<Menu>();
            Offers = new List<SpecialOffer>();
            Orders = new Queue<Order>();
        }

        public void DisplayOrders()
        {
            foreach (var order in Orders)
            {
                Console.WriteLine(order);
            }
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