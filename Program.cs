
using PRG2Assignment;
using S10273555_PRG2Assignment;
using System.ComponentModel.Design;
using System.Globalization;

// main option code
LoadRestaurant();
LoadFoodItem();
LoadCustomers();
LoadOrders();

Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count} restaurants loaded!");  // - Tiara
Console.WriteLine($"{fooditem.Count} food items loaded!"); // - Tiara

while (true)
{
    Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");

    string input = Console.ReadLine();

    if (input == "1")
    {
        ListRestaurantAndMenuItems();
    }
    else if (input == "2")
    {
        ListAllOrders();
    }

    else if (input == "3")
    {
        CreateNewOrder();
    }

    else if (input == "4")
    {
        ProcessOrder();
    }

    else if (input == "5")
    {
        ModifyOrder();
    }

    else if (input == "6")
    {
        DeleteOrder();
    }

    else if (input == "0")//else if so code breaks only when user presses 0
    {
        break;
    }
}


//load restaurants
// Nur Tiara Nasha - Feature 1
List<Restaurant> restaurants = new List<Restaurant>();
List<OrderedFoodItem> orderedFoodItems = new List<OrderedFoodItem>();
void LoadRestaurant()
{
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
            //creating restaurants objects
            Restaurant r = new Restaurant(id, name, email);
            restaurants.Add(r);
        }

    }
}


//load fooditems.csv
//Nur Tiara Nasha - Feature 1

List<FoodItem> fooditem = new List<FoodItem>();
void LoadFoodItem()
{
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

            // create FoodItem object
            FoodItem foodItem = new FoodItem(itemName, itemDesc, itemPrice, "");

            // find the correct restaurant
            Restaurant r = restaurants.Find(res => res.RestaurantId == restaurantId);

            //  assign the food item to that restaurant's menu
            if (r != null)
            {
                // If restaurant has no menu yet, create one
                if (r.Menus.Count == 0)
                {
                    r.Menus.Add(new Menu("Main Menu"));
                }

                r.Menus[0].AddFoodItem(foodItem);
                fooditem.Add(foodItem); // track total food items
            }
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
// Nur Tiara Nasha - Feature 4
void ListAllOrders()
{
    Console.WriteLine("\nAll Orders");
    Console.WriteLine("==========");
    Console.WriteLine("Order ID Customer       Restaurant       Delivery Date/Time     Amount   Status");
    Console.WriteLine("-------- ------------- ---------------- ---------------------- -------- --------");

    int orderCount = 0;

    foreach (Customer cust in customersList)
    {
        foreach (Order order in cust.Orders)
        {
            orderCount++;

            string restaurantName = "Unknown";
            foreach (Restaurant r in restaurants)
            {
                if (r.Orders.Contains(order))
                {
                    restaurantName = r.RestaurantName;
                    break;
                }
            }

            Console.WriteLine(
                $"{order.OrderId,-8}{cust.CustomerName,-13}{restaurantName,-16}{order.DeliveryDateTime:dd/MM/yyyy HH:mm,-22}${order.OrderTotal:0.00,-8}{order.OrderStatus,-10}"
            );
        }
    }

    if (orderCount == 0)
    {
        Console.WriteLine("No orders found.");
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

        Console.Write("Proceed to payment? [Y/N]: ");
        string payChoice = Console.ReadLine();
        if (payChoice == null) payChoice = "";
        payChoice = payChoice.Trim().ToUpper();

        if (payChoice != "Y")
        {
            Console.WriteLine("Payment not made. Exiting feature.");
            return;
        }

        string paymentMethod = "";
        while (true)
        {
            Console.WriteLine("Payment method:");
            Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
            paymentMethod = Console.ReadLine();
            if (paymentMethod == null) paymentMethod = "";
            paymentMethod = paymentMethod.Trim().ToUpper();

            if (paymentMethod == "CC" || paymentMethod == "PP" || paymentMethod == "CD")
                break;

            Console.WriteLine("Invalid payment method. Try again.");
        }

        int maxId = 1000;
        foreach (Customer c in customers)
        {
            foreach (Order o in c.Orders)
            {
                if (o.OrderId > maxId)
                    maxId = o.OrderId;
            }
        }
        int newOrderId = maxId + 1;

        Order newOrder = new Order(newOrderId);

        newOrder.CustomerEmail = cust.EmailAddress; 
        newOrder.RestaurantId = rest.RestaurantId;
        newOrder.DeliveryDateTime = deliveryDateTime;
        newOrder.DeliveryAddress = deliveryAddress;
        newOrder.OrderedFoodItems = orderedItems;
        newOrder.SpecialRequest = specialRequest;  
        newOrder.OrderPaymentMethod = paymentMethod;
        newOrder.OrderStatus = "Pending";
        newOrder.TotalAmount = total;

        rest.OrderQueue.Enqueue(newOrder);
        cust.Orders.Add(newOrder);

        string itemsPart = "";
        for (int i = 0; i < orderedItems.Count; i++)
        {
            itemsPart += orderedItems[i].ItemName + "|" + orderedItems[i].QtyOrdered;
            if (i < orderedItems.Count - 1) itemsPart += ";";
        }

        // Escape commas by quoting if needed
        string safeAddress = deliveryAddress;
        if (safeAddress.Contains(",") || safeAddress.Contains("\""))
        {
            safeAddress = safeAddress.Replace("\"", "\"\"");
            safeAddress = "\"" + safeAddress + "\"";
        }

        string safeRequest = specialRequest;
        if (safeRequest.Contains(",") || safeRequest.Contains("\""))
        {
            safeRequest = safeRequest.Replace("\"", "\"\"");
            safeRequest = "\"" + safeRequest + "\"";
        }

        string line =
            newOrderId + "," +
            cust.EmailAddress + "," +
            rest.RestaurantId + "," +
            deliveryDateTime.ToString("dd/MM/yyyy HH:mm") + "," +
            safeAddress + "," +
            itemsTotal.ToString("0.00") + "," +
            deliveryFee.ToString("0.00") + "," +
            total.ToString("0.00") + "," +
            paymentMethod + "," +
            "Pending" + "," +
            safeRequest + "," +
            itemsPart;

        using (StreamWriter sw = new StreamWriter(ordersCsvPath, true))
        {
            sw.WriteLine(line);
        }

        // 12) Confirmation
        Console.WriteLine($"Order {newOrderId} created successfully! Status: Pending");
    }
}


// Nur Tiara Nasha - Feature 6

void ProcessOrder()
{
    Console.WriteLine("Process Order");
    Console.WriteLine("=============");
    Console.Write("Enter Restaurant ID: ");
    string restId = Console.ReadLine();

    foreach (Restaurant r in restaurants)
    {
        if (r.RestaurantId == restId)
        {
            foreach (Order order in r.Orders)
            {
                Console.WriteLine();
                Console.WriteLine($"Order {order.OrderId}:");
                Console.WriteLine($"Customer: {order.Customer.CustomerName}");
                Console.WriteLine("Ordered Items:");

                int count = 1;
                foreach (OrderedFoodItem item in order.OrderedFoodItems)
                {
                    Console.WriteLine($"{count}. {item.FoodItem.Name} - {item.Quantity}");
                    count++;
                }

                Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Total Amount: ${order.OrderTotal:0.00}");
                Console.WriteLine($"Order Status: {order.OrderStatus}");

                Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
                string choice = Console.ReadLine().ToUpper();

                if (choice == "C")
                {
                    if (order.OrderStatus == "Pending")
                    {
                        order.OrderStatus = "Preparing";
                        Console.WriteLine($"Order {order.OrderId} confirmed. Status: Preparing");
                    }
                    else
                    {
                        Console.WriteLine("Order cannot be confirmed.");
                    }
                }
                else if (choice == "R")
                {
                    if (order.OrderStatus == "Pending")
                    {
                        order.OrderStatus = "Rejected";
                        refundStack.Push(order);
                        Console.WriteLine($"Order {order.OrderId} rejected. Refund initiated.");
                    }
                    else
                    {
                        Console.WriteLine("Order cannot be rejected.");
                    }
                }
                else if (choice == "S")
                {
                    if (order.OrderStatus == "Cancelled")
                    {
                        Console.WriteLine("Order skipped.");
                    }
                    else
                    {
                        Console.WriteLine("Order cannot be skipped.");
                    }
                }
                else if (choice == "D")
                {
                    if (order.OrderStatus == "Preparing")
                    {
                        order.OrderStatus = "Delivered";
                        Console.WriteLine($"Order {order.OrderId} delivered.");
                    }
                    else
                    {
                        Console.WriteLine("Order cannot be delivered.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }
            }

            return; // stop once restaurant is processed
        }
    }

    Console.WriteLine("Restaurant not found.");
}

//Nur Tiara Nasha - Feature 8
void DeleteOrder()
{
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");
    Console.Write("Enter Customer Email: ");
    string email = Console.ReadLine();

    // find customer
    foreach (Customer cust in customersList)
    {
        if (cust.EmailAddress == email)
        {
            Console.WriteLine("Pending Orders:");

            // show pending orders
            foreach (Order o in cust.Orders)
            {
                if (o.OrderStatus == "Pending")
                {
                    Console.WriteLine(o.OrderId);
                }
            }

            Console.Write("Enter Order ID: ");
            int orderId = Convert.ToInt32(Console.ReadLine());

            // find the order
            foreach (Order order in cust.Orders)
            {
                if (order.OrderId == orderId)
                {
                    Console.WriteLine($"Customer: {cust.CustomerName}");
                    Console.WriteLine("Ordered Items:");

                    int count = 1;
                    foreach (OrderedFoodItem item in order.OrderedFoodItems)
                    {
                        Console.WriteLine($"{count}. {item.ItemName} - {item.QtyOrdered}");
                        count++;
                    }

                    Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Total Amount: ${order.OrderTotal:0.00}");
                    Console.WriteLine($"Order Status: {order.OrderStatus}");

                    Console.Write("Confirm deletion? [Y/N]: ");
                    string confirm = Console.ReadLine().ToUpper();

                    if (confirm == "Y")
                    {
                        order.OrderStatus = "Cancelled";
                        refundStack.Push(order);

                        Console.WriteLine(
                            $"Order {order.OrderId} cancelled. Refund of ${order.OrderTotal:0.00} processed."
                        );
                    }
                    else
                    {
                        Console.WriteLine("Deletion cancelled.");
                    }

                    return; // stop after processing one order
                }
            }

            Console.WriteLine("Order not found.");
            return;
        }
    }

    Console.WriteLine("Customer not found.");
}