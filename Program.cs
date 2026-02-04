// Nur Tiara Nasha 
using PRG2Assignment;
using S10273555_PRG2Assignment;

List<Restaurant> restaurants = new List<Restaurant>();

// load restaurants
void LoadRestaurants(string filePath)
{
    using (StreamReader sr = new StreamReader(filePath))
    {
        sr.ReadLine(); // skip header
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(",");

            Restaurant r = new Restaurant(data[0], data[1], data[2]);
            r.AddMenu(new Menu("M001", "Main Menu")); // create default menu
            restaurants.Add(r);
        }
    }
}

// load food items and assign them
void LoadFoodItems(string filePath)
{
    using (StreamReader sr = new StreamReader(filePath))
    {
        sr.ReadLine(); // skip header
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(",");

            string restaurantId = data[0];
            string itemName = data[1];
            string itemDesc = data[2];
            double itemPrice = double.Parse(data[3]);

            FoodItem foodItem = new FoodItem(itemName, itemDesc, itemPrice, "");

            // assign to restaurant menu
            Restaurant r = restaurants.Find(res => res.RestaurantId == restaurantId);
            if (r != null)
            {
                r.Menus[0].AddFoodItem(foodItem);
            }
        }
    }
}

// display all restaurants and menus
void DisplayAllRestaurants()
{
    foreach (Restaurant r in restaurants)
    {
        Console.WriteLine(r);
        r.DisplayMenu();
        Console.WriteLine();
    }
}

// ----------------------------
// call the methods
LoadRestaurants("restaurants.csv");
LoadFoodItems("fooditems.csv");
DisplayAllRestaurants();