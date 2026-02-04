// Nur Tiara Nasha
//load restaurant.csv
using PRG2Assignment;
using S10273555_PRG2Assignment;
List<Restaurant> restaurants = new List<Restaurant>();

using (StreamReader sr = new StreamReader("restaurants.csv"))
{
    string line;
    sr.ReadLine();

    while ((line = sr.ReadLine()) != null)
    {
        string[] data = line.Split(",");

        string id = data[0];
        string name = data[1];
        string email = data[2];

        Restaurant r = new Restaurant(id, name, email);
        restaurants.Add(r);
    }

}

//creating restaurants objects
Restaurant r1 = new Restaurant("R001","Grill House","grillhouse@email.com");
Restaurant r2 = new Restaurant("R002", "Noodle Palace", "noodlepalace@email.com");
Restaurant r3 = new Restaurant("R003", "Bento Express", "bentoexpress@email.com");
Restaurant r4 = new Restaurant("R004", "Pizza Corner", "pizzacorner@email.com");
Restaurant r5 = new Restaurant("R005", "Salad Bar", "saladbar@email.com");
Restaurant r6 = new Restaurant("R006", "Sushi Zen", "sushizen@email.com");
Restaurant r7 = new Restaurant("R007", "Burger Barn", "burgerbarn@email.com");
Restaurant r8 = new Restaurant("R008", "Taco Town", "tacotown@email.com");
Restaurant r9 = new Restaurant("R009", "Pasta Pronto", "pastapronto@email.com");
Restaurant r10 = new Restaurant("R010", "Steak Station", "steakstation@email.com");
Restaurant r11 = new Restaurant("R011", "Curry Corner", "currycorner@email.com");
Restaurant r12 = new Restaurant("R012", "Dim Sum Den", "dimsumden@email.com");
Restaurant r13 = new Restaurant("R013", "Waffle World", "waffleworld@email.com");
Restaurant r14 = new Restaurant("R014", "Soup Shack", "soupshack@email.com");
Restaurant r15 = new Restaurant("R015", "Donut Delight", "donutdelight@email.com");

//load fooditems.csv
List<FoodItem> fooditem = new List<FoodItem>();

using (StreamReader sr = new StreamReader("fooditems.csv"))
{
    string line;
    sr.ReadLine(); // skip header

    while ((line = sr.ReadLine()) != null)
    {
        string[] data = line.Split(",");

        string restaurantId = data[0];    // restaurant ID
        string itemName = data[1];        // item name
        string itemDesc = data[2];        // item description
        double itemPrice = double.Parse(data[3]); //change string to double

        // Step 3: create FoodItem object
        FoodItem foodItem = new FoodItem(itemName, itemDesc, itemPrice, "");

        // Step 4: find the correct restaurant
        Restaurant r = restaurants.Find(res => res.RestaurantId == restaurantId);

        // Step 5: assign the food item to that restaurant's menu
        if (r != null)
        {
            r.Menus[0].AddFoodItem(foodItem);
        }
    }
}

