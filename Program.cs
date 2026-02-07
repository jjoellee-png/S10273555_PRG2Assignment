
using PRG2Assignment;
using S10273555_PRG2Assignment;
using System.ComponentModel.Design;
using System.Globalization;

// Nur Tiara Nasha - Feature 1
List<Restaurant> restaurants = new List<Restaurant>();
List<OrderedFoodItem> orderedFoodItems = new List<OrderedFoodItem>();

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
        }
    }
}

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

// Joelle Heng - Feature 3
void ListRestaurantAndMenuItems()
{
    Console.WriteLine("All Restaurants and Menu Items");
    Console.WriteLine("==============================");
    if (restaurants.Count == 0)
    {
        Console.WriteLine("No restaurants available.");
    }
    else
    {
        foreach (Restaurant r in restaurants)
        {
            Console.WriteLine($"Restaurant: {r.RestaurantName} ({r.RestaurantId})");
            if (fooditem == null || fooditem.Count == 0)
            {
                Console.WriteLine(" - No food items available.");
            }
            else
            {
                foreach (FoodItem fi in fooditem)
                {
                    Console.WriteLine($" - {fi.ItemName} : {fi.ItemDesc} - ${fi.ItemPrice:F2}");
                }
            }
            Console.WriteLine();
        }
    }
}

// Joelle Heng - Feature 5
void CreateNewOrder(List<Customer> customers, List<Restaurant> restaurants)
{
    Console.WriteLine("Create New Order");
    Console.WriteLine("================");

    string custEmail = "";
    while (custEmail.Trim() == "")
    {
        Console.Write("Enter Customer Email: ");
        custEmail = Console.ReadLine();
        if (custEmail == null) custEmail = "";
        custEmail = custEmail.Trim(); // Trim whitespace
        if (custEmail == "")
        {
            Console.WriteLine("Customer email cannot be empty. Please try again.");
        }
    }
    string restId = "";
    while (restId.Trim()  == "")
    {
        Console.Write("Enter Restaurant ID: ");
        restId = Console.ReadLine();
        if (restId == null) restId = "";
        restId  = restId.Trim();
        if (restId == "")
            Console.WriteLine("Restaurant ID cannot be empty.");
    }

    DateTime deliveryDate;
    while (true)
    {
        Console.WriteLine("Enter Delivery Date (dd/mm/yyyy): ");
        string dd = Console.ReadLine();

        if (dd == null) dd = "";
        string[] parts = dd.Split(',');
        if (parts.Length == 3)
        {
            try
            {
                int day = Convert.ToInt32(parts[0]);
                int month = Convert.ToInt32(parts[1]);
                int year = Convert.ToInt32(parts[2]);

                deliveryDate = new DateTime(year,month,day);
                break;
            }
            catch
            {
                Console.WriteLine("Invalid date. Please try again");
            }
        }
        else
        {
            Console.WriteLine("Invalid format. Use dd/mm/yyyy.");
        }
    }

    int deliveryHour = 0;
    int deliveryMinute = 0;

    while (true)
    {
        Console.Write("Enter Delivery Time (hh:mm): ");
        string time = Console.ReadLine();

        if (time == null) time = "";
        time = time.Trim();

        string[] parts = time.Split(",");

        if (parts.Length == 2)
        {
            try
            {
                deliveryHour = Convert.ToInt32(parts[0]);
                deliveryMinute = Convert.ToInt32(parts[1]);

                if (deliveryHour >= 0 && deliveryHour <= 23 &&
                    deliveryMinute >= 0 && deliveryMinute <= 59)
                {
                    break; //valid time, no errors
                }
                else
                {
                    Console.WriteLine("Hour must be in the range 0-23 and minute in 0-59.");

                }
            }
            catch
            {
                Console.WriteLine("Invalid time. Please try again.");
            }
        }
        else
        {
            Console.WriteLine("Invalid format. Use hh:mm.");
        }

        string deliveryAddress = "";
        while (deliveryAddress.Trim() == "")
        {
            Console.Write("Enter Delivery Address: ");
            deliveryAddress = Console.ReadLine();
            if (deliveryAddress == null) deliveryAddress = "";
            deliveryAddress = deliveryAddress.Trim();
            if (deliveryAddress == "")
                Console.WriteLine("Delivery Address cannot be empty.");
        }

        DateTime deliveryDateTime = new DateTime(deliveryDate.Year, deliveryDate.Month, deliveryDate.Day, deliveryDateTime.Hours, deliveryDateTime.Minutes, 0);

        // find customer in system
        Customer cust = null;
        foreach (Customer c in customers)
        {
            if (c.EmailAddress.ToLower() == custEmail.ToLower())
            {
                cust = c;
                break;
            }
        }
        if (cust == null)
        {
            Console.WriteLine("Invalid customer email. Customer not found.");
            return;
        }

        // find restaurant

        Restaurant rest = null;
        foreach (Restaurant r in restaurants)
        {
            if (r.RestaurantId.ToLower() == restId.ToLower())
            {
                rest = r;
                break;
            }
        }
        if (rest == null)
        {
            Console.WriteLine("Invalid restaurant ID. Restaurant not found.");
            return;
        }

        // display food item
        if (fooditem == null || fooditem.Count == 0)
        {
            Console.WriteLine("No food items available for this restaurant.");
            return;
        }

        Console.WriteLine("Available Food Items: ");
        for (int i = 0; i < fooditem.Count; i++)
        {
            Console.WriteLine($"{i + 1}.{fooditem[i].ItemName} - ${fooditem[i].ItemPrice:F2}");
        }

        int itemNo = 0;
        while (true)
        {
            Console.Write("Enter item number (0 to finish): ");
            string item = Console.ReadLine();

            if (item == null) item = "";
            item = item.Trim();

            try
            {
                itemNo = Convert.ToInt32(item);
                break;
            }
            catch
            {
                Console.WriteLine("Invalid number.Try again.");
            }
            if (itemNo == 0)
                break;
            if (itemNo < 1 || itemNo > menu.Count)
            {
                Console.WriteLine("Invalid item number. Try again.");
                return;
            }

            int qty;
            while (true)
            {
                Console.Write("Enter quantity: ");
                string qtyStr = Console.ReadLine();
                if (qtyStr == null) qtyStr = "";
                qtyStr = qtyStr.Trim();

                if (int.TryParse(qtyStr, out qty) && qty > 0)
                    break;
                Console.WriteLine("Invalid quantity. Must be a number > 0.");
            }
            FoodItem selected = menu[itemNo - 1];
            bool found = false;
            for (int i = 0; i < orderedFoodItems.Count; i++)
            {
                if (orderedFoodItems[i].ItemName.ToLower() == selected.ItemName.ToLower())
                {
                    orderedFoodItems[i].QtyOrdered += qty;
                    orderedFoodItems[i].SubTotal = orderedFoodItems[i].QtyOrdered * orderedFoodItems[i].ItemPrice;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                OrderedFoodItem ofi = new OrderedFoodItem(
                    selected.ItemName,
                    selected.ItemDesc,
                    selected.ItemPrice,
                    "", // special customisation
                    qty);
                orderedFoodItems.Add(ofi);
            }
        }
        if (orderedFoodItems.Count == 0)
        {
            Console.WriteLine("No items selected. Exiting feature.");
            return;
        }
        string specialRequest = "";
        Console.Write("Add special request? [Y/N]: ");
        string srChoice = Console.ReadLine();
        if (srChoice == null) srChoice = "";
        srChoice = srChoice.Trim().ToUpper();

        if (srChoice == "Y")
        {
            while (specialRequest.Trim() == "")
            {
                Console.Write("Enter special request: ");
                specialRequest = Console.ReadLine();
                if (specialRequest == null) specialRequest = "";
                specialRequest = specialRequest.Trim();
                if (specialRequest == "")
                    Console.WriteLine("Special request cannot be empty.");
            }
        }

        double itemsTotal = 0;
        foreach (OrderedFoodItem it in orderedFoodItems)
        {
            // Make sure SubTotal is up to date
            it.SubTotal = it.QtyOrdered * it.ItemPrice;
            itemsTotal += it.SubTotal;
        }

        double deliveryFee = 5.00; // based on sample
        double total = itemsTotal + deliveryFee;

        Console.WriteLine($"Order Total: ${itemsTotal:0.00} + ${deliveryFee:0.00} (delivery) = ${total:0.00}");
    }

}
