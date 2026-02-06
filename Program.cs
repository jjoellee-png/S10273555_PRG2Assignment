<<<<<<< HEAD
﻿using PRG2Assignment;
using S10273555_PRG2Assignment;
using System.Globalization;

// Nur Tiara Nasha - Feature 1
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
Restaurant r1 = new Restaurant("R001", "Grill House", "grillhouse@email.com");
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

// Joelle Heng - Feature 2
List<Customer> customersList = new List<Customer>();
List<Order> ordersList = new List<Order>();
void LoadCustomers()
{
    using (StreamReader sr = new StreamReader("customer.csv"))
    {
        string header = sr.ReadLine(); // reads the first line, skips the header
        string? line; // allows the line to hold any null value (if any)

        while ((line = sr.ReadLine()) != null)
        {
            string[] parts = line.Split(',');
            string name = parts[0];
            string email = parts[1];

            Customer customer = new Customer(name, email);
            customersList.Add(customer);
=======
﻿// Nur Tiara Nasha 
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
>>>>>>> 9fd0981ee77b6da10d178a7246b6dd65472333dc
        }
    }
}

<<<<<<< HEAD
void LoadOrders()
{
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        string header = sr.ReadLine();
        string? line;

        while ((line = sr.ReadLine()) != null)
        {
            string[] firstParts = line.Split('"');

            string[] parts = firstParts[0].Split(",");

            int orderId = Convert.ToInt32(parts[0]);
            string custEmail = parts[1];
            string restaurantId = parts[2];

            string deliveryDate = parts[3];
            string deliveryTime = parts[4];
            string deliveryAddress = parts[5];

            var createdDateTimeString = parts[6].Split(" ");
            string createdDateString = createdDateTimeString[0];
            string createdTimeString = createdDateTimeString[1];

            double totalAmount = Convert.ToDouble(parts[7]);
            string status = parts[8];

            // CultureInfo.InvariantCulture helps to avoid confusion with different formats.
            DateTime deliveryDateTime = DateTime.ParseExact(deliveryDate + " " + deliveryTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime createdDateTime = DateTime.ParseExact(createdDateString + " " + createdTimeString, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            Order order = new Order(orderId);
            order.DeliveryDateTime = deliveryDateTime;
            order.DeliveryAddress = deliveryAddress;
            order.OrderDateTime = createdDateTime;
            order.OrderTotal = totalAmount;
            order.OrderStatus = status;

            // here - added foreach and addorderedfooditem
            var items = firstParts[1].Split("|");

            foreach (string item in items)
            {
                string[] itemsPart = item.Split(","); // changed items[0] to item so that item wont repeat -Mahima
                string itemName = itemsPart[0].Trim();
                int qty = Convert.ToInt32(itemsPart[1].Trim()); //added trim()

                foreach (var foodItem in fooditem)
                {
                    if (foodItem.ItemName == itemName)
                    {
                        OrderedFoodItem orderedFoodItem = new OrderedFoodItem(itemName, foodItem.ItemDesc, foodItem.ItemPrice, "", qty);
                        order.AddOrderedFoodItem(orderedFoodItem);
                    }
                }
            }

            ordersList.Add(order);

            foreach (Customer customer in customersList)
            {
                if (customer.EmailAddress == custEmail)
                {
                    customer.AddOrder(order);
                    break;
                }
            }

            foreach (Restaurant restaurant in restaurants)
            {
                if (restaurant.RestaurantId == restaurantId)
                {
                    restaurant.orders.Enqueue(order);
                }
            }
        }
    }
}
=======
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
>>>>>>> 9fd0981ee77b6da10d178a7246b6dd65472333dc
