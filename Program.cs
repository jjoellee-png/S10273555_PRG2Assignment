
using PRG2Assignment;
using S10273555_PRG2Assignment;
using System.ComponentModel.Design;
using System.Globalization;

List<Restaurant> restaurants = new List<Restaurant>();
List<OrderedFoodItem> orderedFoodItems = new List<OrderedFoodItem>();
List<FoodItem> fooditem = new List<FoodItem>();
List<Customer> customersList = new List<Customer>();
List<Order> ordersList = new List<Order>();
Stack<Order> refundStack = new Stack<Order>();

// main option code
LoadRestaurant();
LoadFoodItem();
LoadCustomers();
LoadOrders();

Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count} restaurants loaded!");  // - Tiara
Console.WriteLine($"{fooditem.Count} food items loaded!"); // - Tiara
Console.WriteLine($"{customersList.Count} restaurants loaded!");  // - Joelle
Console.WriteLine($"{ordersList.Count} food items loaded!"); // - Joelle

while (true)
{
    Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("7. Bulk processing of unprocessed orders for a current day");
    Console.WriteLine("8. Display total order amount");
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
        CreateNewOrder(customersList, restaurants);
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

    else if (input == "7")
    {
        BulkProcessTodayOrders();
    }

    else if (input == "8")
    {
        DisplayTotalOrderAmount();
    }

    else if (input == "0")//else if so code breaks only when user presses 0
    {
        break;
    }
}


//load restaurants
// Nur Tiara Nasha - Feature 1
void LoadRestaurant()
{
    using (StreamReader sr = new StreamReader("C:\\Users\\nurti\\OneDrive\\Documents\\Assignment(PRG)\\PRG2 Assignment\\restaurants.csv"))
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
void LoadFoodItem()
{
    using (StreamReader sr = new StreamReader("C:\\Users\\nurti\\OneDrive\\Documents\\Assignment(PRG)\\PRG2 Assignment\\fooditems - Copy.csv"))
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
                    r.Menus.Add(new Menu("M001", "Main Menu"));
                }

                r.Menus[0].AddFoodItem(foodItem);
                fooditem.Add(foodItem); // track total food items
            }
        }
    }
}



// Joelle Heng - Feature 2
void LoadCustomers()
{
    using (StreamReader sr = new StreamReader("C:\\Users\\nurti\\OneDrive\\Documents\\Assignment(PRG)\\PRG2 Assignment\\customers.csv"))
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
    using (StreamReader sr = new StreamReader("C:\\Users\\nurti\\OneDrive\\Documents\\Assignment(PRG)\\PRG2 Assignment\\orders - Copy.csv"))
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
                    restaurant.Orders.Enqueue(order);
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
        string[] parts = dd.Split('/');
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

        string[] parts = time.Split(":");

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
            Console.WriteLine("Invalid format. Use hh,mm.");
        }
    }

    string deliveryAddress = "";
    while (deliveryAddress.Trim() == "")
    {
        Console.Write("Enter Delivery Address: ");
        deliveryAddress = Console.ReadLine() ?? "";
        deliveryAddress = deliveryAddress.Trim();
        if (deliveryAddress == "")
            Console.WriteLine("Delivery Address cannot be empty.");
    }

    DateTime deliveryDateTime = new DateTime(
        deliveryDate.Year, deliveryDate.Month, deliveryDate.Day,
        deliveryHour, deliveryMinute, 0
    );

    // find customer object
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

    // find restaurant object
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

    if (fooditem == null || fooditem.Count == 0)
    {
        Console.WriteLine("No food items available.");
        return;
    }

    Console.WriteLine("Available Food Items: ");
    for (int i = 0; i < fooditem.Count; i++)
    {
        Console.WriteLine($"{i + 1}.{fooditem[i].ItemName} - ${fooditem[i].ItemPrice:F2}");
    }

    List<OrderedFoodItem> newOrderItems = new List<OrderedFoodItem>();
    while (true)
    {
        Console.Write("Enter item number (0 to finish): ");
        string item = Console.ReadLine() ?? "";
        int itemNo;
        try
        {
            itemNo = Convert.ToInt32(item);
        }
        catch
        {
            Console.WriteLine("Invalid number. Try again.");
            continue;
        }

        if (itemNo == 0) break;

        if (itemNo < 1 || itemNo > fooditem.Count)
        {
            Console.WriteLine("Invalid item number. Try again.");
            continue;
        }

        int qty;
        while (true)
        {
            Console.Write("Enter quantity: ");
            string qtyStr = Console.ReadLine() ?? "";
            try
            {
                qty = Convert.ToInt32(qtyStr);
                if (qty > 0) break;
                Console.WriteLine("Quantity must be > 0.");
            }
            catch
            {
                Console.WriteLine("Invalid quantity. Must be a number > 0.");
            }
        }

        FoodItem selected = fooditem[itemNo - 1];
        bool found = false;
        for (int i = 0; i < newOrderItems.Count; i++)
        {
            if (newOrderItems[i].ItemName.ToLower() == selected.ItemName.ToLower())
            {
                newOrderItems[i].QtyOrdered += qty;
                newOrderItems[i].CalculateSubtotal();
                found = true;
                break;
            }
        }
        if (!found)
        {
            OrderedFoodItem ofi = new OrderedFoodItem(selected.ItemName, selected.ItemDesc, selected.ItemPrice, "", qty);
            newOrderItems.Add(ofi);
        }
    }

    if (newOrderItems.Count == 0)
    {
        Console.WriteLine("No items selected. Exiting feature.");
        return;
    }

    string specialRequest = "";
    Console.Write("Add special request? [Y/N]: ");
    string srChoice = Console.ReadLine() ?? "";
    srChoice = srChoice.Trim().ToUpper();
    if (srChoice == "Y")
    {
        while (specialRequest.Trim() == "")
        {
            Console.Write("Enter special request: ");
            specialRequest = Console.ReadLine() ?? "";
            specialRequest = specialRequest.Trim();
            if (specialRequest == "")
                Console.WriteLine("Special request cannot be empty.");
        }
    }

    double itemsTotal = 0;
    foreach (OrderedFoodItem it in newOrderItems)
    {
        it.SubTotal = it.QtyOrdered * it.ItemPrice;
        itemsTotal += it.SubTotal;
    }

    double deliveryFee = 5.00;
    double total = itemsTotal + deliveryFee;

    Console.WriteLine($"Order Total: ${itemsTotal:0.00} + ${deliveryFee:0.00} (delivery) = ${total:0.00}");

    Console.Write("Proceed to payment? [Y/N]: ");
    string payChoice = Console.ReadLine() ?? "";
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
        paymentMethod = Console.ReadLine() ?? "";
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

    // set object links and properties
    newOrder.Customer = cust;
    newOrder.Restaurant = rest;
    newOrder.DeliveryDateTime = deliveryDateTime;
    newOrder.DeliveryAddress = deliveryAddress;
    newOrder.OrderedFoodItems = newOrderItems;
    newOrder.OrderPaymentMethod = paymentMethod;
    newOrder.OrderStatus = "Pending";
    newOrder.OrderTotal = total;
    newOrder.OrderPaid = (paymentMethod == "CD") ? false : true;

    rest.Orders.Enqueue(newOrder);
    cust.AddOrder(newOrder);

    // build csv items part
    string itemsPart = "";
    for (int i = 0; i < newOrderItems.Count; i++)
    {
        itemsPart += newOrderItems[i].ItemName + "," + newOrderItems[i].QtyOrdered;
        if (i < newOrderItems.Count - 1) itemsPart += "|";
    }

    // escape address/request
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

    using (StreamWriter sw = new StreamWriter("orders.csv", true))
    {
        sw.WriteLine(line);
    }

    Console.WriteLine($"Order {newOrderId} created successfully! Status: Pending");
}

// Process order
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
                    Console.WriteLine($"{count}. {item.ItemName} - {item.QtyOrdered}");
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

            return;
        }
    }

    Console.WriteLine("Restaurant not found.");
}

// Joelle Heng - Advanced (a)
void BulkProcessTodayOrders()
{
    Console.WriteLine("\nBulk processing pending orders for today");
    Console.WriteLine("========================================");

    DateTime today = DateTime.Today;
    DateTime now = DateTime.Now;

    // collect pending orders in queues for today
    List<Order> pendingToday = new List<Order>();
    foreach (Restaurant r in restaurants)
    {
        if (r.Orders == null) continue;
        foreach (Order order in r.Orders)
        {
            if (order == null) continue;
            if (order.OrderStatus != null && order.OrderStatus.ToLower() == "pending" && order.DeliveryDateTime.Date == today)
            {
                pendingToday.Add(order);
            }
        }
    }

    Console.WriteLine($"Total pending orders in queues for today: {pendingToday.Count}");
    if (pendingToday.Count == 0)
    {
        Console.WriteLine("No pending orders to process for today.");
        return;
    }

    int processedCount = 0;
    int preparingCount = 0;
    int rejectedCount = 0;

    foreach (Order order in pendingToday)
    {
        TimeSpan timeUntilDelivery = order.DeliveryDateTime - now;

        if (timeUntilDelivery.TotalHours < 1.0)
        {
            // auto-reject
            if (order.OrderStatus != null && order.OrderStatus.ToLower() == "pending")
            {
                order.OrderStatus = "Rejected";
                // keep same pattern as other parts of code (push to refundStack)
                try
                {
                    refundStack.Push(order);
                }
                catch
                {
                    // if refundStack not available, continue without stopping bulk processing
                }
                rejectedCount++;
                processedCount++;
                Console.WriteLine($"Order {order.OrderId} => Rejected (delivery in {timeUntilDelivery.TotalMinutes:0} minutes)");
            }
        }
        else
        {
            // auto-prepare
            if (order.OrderStatus != null && order.OrderStatus.ToLower() == "pending")
            {
                order.OrderStatus = "Preparing";
                preparingCount++;
                processedCount++;
                Console.WriteLine($"Order {order.OrderId} => Preparing (delivery at {order.DeliveryDateTime:HH:mm})");
            }
        }
    }

    double percentProcessed = (pendingToday.Count > 0) ? (processedCount / (double)pendingToday.Count * 100.0) : 0.0;

    Console.WriteLine("\nBulk processing summary:");
    Console.WriteLine($" - Total pending today: {pendingToday.Count}");
    Console.WriteLine($" - Orders processed: {processedCount}");
    Console.WriteLine($"   - Preparing: {preparingCount}");
    Console.WriteLine($"   - Rejected: {rejectedCount}");
    Console.WriteLine($" - Percentage auto-processed: {percentProcessed:0.00}%");
}

// Joelle Heng - Feature 7
void ModifyOrder()
{
    Console.WriteLine("Modify Order");
    Console.WriteLine("============");

    Console.Write("Enter Customer Email: ");
    string email = Console.ReadLine() ?? "";
    email = email.Trim();
    if (email == "")
    {
        Console.WriteLine("Customer email cannot be empty.");
        return;
    }

    // find customer using a simple loop and ToLower()
    Customer cust = null;
    for (int i = 0; i < customersList.Count; i++)
    {
        if (customersList[i].EmailAddress.ToLower() == email.ToLower())
        {
            cust = customersList[i];
            break;
        }
    }

    if (cust == null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    // collect pending orders
    List<Order> pending = new List<Order>();
    for (int i = 0; i < cust.Orders.Count; i++)
    {
        if (cust.Orders[i].OrderStatus == "Pending")
        {
            pending.Add(cust.Orders[i]);
        }
    }

    if (pending.Count == 0)
    {
        Console.WriteLine("No pending orders.");
        return;
    }

    Console.WriteLine("Pending Orders:");
    for (int i = 0; i < pending.Count; i++)
    {
        Console.WriteLine(pending[i].OrderId);
    }

    Console.Write("Enter Order ID: ");
    string idInput = Console.ReadLine() ?? "";
    int orderId;
    try
    {
        orderId = Convert.ToInt32(idInput);
    }
    catch
    {
        Console.WriteLine("Invalid order id.");
        return;
    }

    // find the chosen order by loop
    Order order = null;
    for (int i = 0; i < pending.Count; i++)
    {
        if (pending[i].OrderId == orderId)
        {
            order = pending[i];
            break;
        }
    }

    if (order == null)
    {
        Console.WriteLine("Order not found or not pending.");
        return;
    }

    // show current details
    Console.WriteLine("Order Items:");
    for (int i = 0; i < order.OrderedFoodItems.Count; i++)
    {
        OrderedFoodItem it = order.OrderedFoodItems[i];
        Console.WriteLine((i + 1) + ". " + it.ItemName + " - " + it.QtyOrdered);
    }
    Console.WriteLine("Address:");
    Console.WriteLine(order.DeliveryAddress);
    Console.WriteLine("Delivery Date/Time:");
    Console.WriteLine(order.DeliveryDateTime.ToString("d/M/yyyy, HH:mm"));

    Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
    string choice = (Console.ReadLine() ?? "").Trim();

    // backup items and old total
    List<OrderedFoodItem> backupItems = new List<OrderedFoodItem>();
    for (int i = 0; i < order.OrderedFoodItems.Count; i++)
    {
        OrderedFoodItem oi = order.OrderedFoodItems[i];
        backupItems.Add(new OrderedFoodItem(oi.ItemName, oi.ItemDesc, oi.ItemPrice, "", oi.QtyOrdered));
    }
    double oldTotal = order.OrderTotal;
    const double deliveryFee = 5.00;

    if (choice == "1")
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Current items:");
            for (int i = 0; i < order.OrderedFoodItems.Count; i++)
            {
                OrderedFoodItem it = order.OrderedFoodItems[i];
                Console.WriteLine((i + 1) + ". " + it.ItemName + " x " + it.QtyOrdered + " = $" + it.CalculateSubtotal().ToString("0.00"));
            }
            Console.WriteLine("[A]dd  [R]emove  [U]pdate qty  [D]one");
            Console.Write("Choice: ");
            string op = (Console.ReadLine() ?? "").Trim().ToUpper();
            if (op == "D") break;

            if (op == "A")
            {
                Console.WriteLine("Available Food Items:");
                for (int i = 0; i < fooditem.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + fooditem[i].ItemName + " - $" + fooditem[i].ItemPrice.ToString("0.00"));
                }

                Console.Write("Enter item number to add: ");
                string idxStr = Console.ReadLine();
                if (idxStr == null)
                {
                    idxStr = "";
                }
                int idx;
                bool idxValid = true;
                try
                {
                    idx = Convert.ToInt32(idxStr);
                }
                catch
                {
                    Console.WriteLine("Invalid selection.");
                    idxValid = false;
                    idx = -1;
                }

                if (idxValid)
                {
                    if (idx < 1 || idx > fooditem.Count)
                    {
                        Console.WriteLine("Invalid selection.");
                        idxValid = false;
                    }
                }

                if (idxValid)
                {
                    FoodItem sel = fooditem[idx - 1];

                    Console.Write("Enter quantity: ");
                    string qStr = Console.ReadLine() ?? "";
                    int q;
                    bool qValid = true;
                    try
                    {
                        q = Convert.ToInt32(qStr);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid quantity.");
                        qValid = false;
                        q = 0;
                    }

                    if (qValid)
                    {
                        if (q <= 0)
                        {
                            Console.WriteLine("Quantity must be > 0.");
                            qValid = false;
                        }
                    }

                    if (qValid)
                    {
                        // find existing item with loop and ToLower()
                        bool found = false;
                        for (int i = 0; i < order.OrderedFoodItems.Count; i++)
                        {
                            if (order.OrderedFoodItems[i].ItemName.ToLower() == sel.ItemName.ToLower())
                            {
                                order.OrderedFoodItems[i].QtyOrdered += q;
                                order.OrderedFoodItems[i].CalculateSubtotal();
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            order.OrderedFoodItems.Add(new OrderedFoodItem(sel.ItemName, sel.ItemDesc, sel.ItemPrice, "", q));
                        }
                    }
                }
            }
            else if (op == "R")
            {
                Console.Write("Enter item number to remove: ");
                string remStr = Console.ReadLine() ?? "";
                int rem;
                bool remValid = true;
                try
                {
                    rem = Convert.ToInt32(remStr);
                }
                catch
                {
                    Console.WriteLine("Invalid selection.");
                    remValid = false;
                    rem = -1;
                }

                if (remValid)
                {
                    if (rem < 1 || rem > order.OrderedFoodItems.Count)
                    {
                        Console.WriteLine("Invalid selection.");
                        remValid = false;
                    }
                }

                if (remValid)
                {
                    order.OrderedFoodItems.RemoveAt(rem - 1);
                }
            }
            else if (op == "U")
            {
                Console.Write("Enter item number to update: ");
                string uStr = Console.ReadLine() ?? "";
                int uidx;
                bool uidxValid = true;
                try
                {
                    uidx = Convert.ToInt32(uStr);
                }
                catch
                {
                    Console.WriteLine("Invalid selection.");
                    uidxValid = false;
                    uidx = -1;
                }

                if (uidxValid)
                {
                    if (uidx < 1 || uidx > order.OrderedFoodItems.Count)
                    {
                        Console.WriteLine("Invalid selection.");
                        uidxValid = false;
                    }
                }

                if (uidxValid)
                {
                    Console.Write("Enter new quantity: ");
                    string newQStr = Console.ReadLine() ?? "";
                    int newQty;
                    bool newQtyValid = true;
                    try
                    {
                        newQty = Convert.ToInt32(newQStr);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid quantity.");
                        newQtyValid = false;
                        newQty = 0;
                    }

                    if (newQtyValid)
                    {
                        if (newQty <= 0)
                        {
                            Console.WriteLine("Quantity must be > 0.");
                            newQtyValid = false;
                        }
                    }

                    if (newQtyValid)
                    {
                        order.OrderedFoodItems[uidx - 1].QtyOrdered = newQty;
                        order.OrderedFoodItems[uidx - 1].CalculateSubtotal();
                    }
                }
            }
            else
            {
                Console.WriteLine("Unknown operation.");
            }
        }
    }
    else if (choice == "2")
    {
        Console.Write("Enter new Delivery Address: ");
        string addr = Console.ReadLine() ?? "";
        if (addr.Trim() != "")
        {
            order.DeliveryAddress = addr.Trim();
        }
        else
        {
            Console.WriteLine("Address not changed.");
        }
    }
    else if (choice == "3")
    {
        Console.Write("Enter new Delivery Time (hh:mm): ");
        string time = Console.ReadLine() ?? "";
        time = time.Trim();
        string[] parts = time.Split(':');
        if (parts.Length == 2)
        {
            int hh;
            int mm;
            try
            {
                hh = Convert.ToInt32(parts[0]);
                mm = Convert.ToInt32(parts[1]);
            }
            catch
            {
                Console.WriteLine("Invalid numbers in time.");
                return;
            }
            if (hh >= 0 && hh <= 23 && mm >= 0 && mm <= 59)
            {
                DateTime date = order.DeliveryDateTime.Date;
                order.DeliveryDateTime = new DateTime(date.Year, date.Month, date.Day, hh, mm, 0);
            }
            else
            {
                Console.WriteLine("Hour must be 0-23 and minute 0-59.");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid time format. Use hh:mm.");
            return;
        }
    }
    else
    {
        Console.WriteLine("Invalid modification option.");
        return;
    }

    // Recalculate totals
    double itemsTotal = 0.0;
    for (int i = 0; i < order.OrderedFoodItems.Count; i++)
    {
        itemsTotal += order.OrderedFoodItems[i].CalculateSubtotal();
    }

    if (order.OrderedFoodItems.Count == 0)
    {
        Console.WriteLine("Order must contain at least one item. Reverting changes.");
        order.OrderedFoodItems = backupItems;
        order.OrderTotal = oldTotal;
        return;
    }

    double newTotal = itemsTotal + deliveryFee;

    if (newTotal > oldTotal)
    {
        double diff = newTotal - oldTotal;
        Console.WriteLine("Order total increased by $" + diff.ToString("0.00") + ". Pay the difference to confirm change.");
        Console.Write("Proceed to pay extra amount? [Y/N]: ");
        string pay = (Console.ReadLine() ?? "").Trim().ToUpper();
        if (pay != "Y")
        {
            // revert
            order.OrderedFoodItems = backupItems;
            order.OrderTotal = oldTotal;
            Console.WriteLine("Change cancelled.");
            return;
        }

        // payment method
        while (true)
        {
            Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
            string pm = (Console.ReadLine() ?? "").Trim().ToUpper();
            if (pm == "CC" || pm == "PP" || pm == "CD")
            {
                order.OrderPaymentMethod = pm;
                order.OrderPaid = true;
                break;
            }
            Console.WriteLine("Invalid payment method. Try again.");
        }
    }

    // apply new total
    order.OrderTotal = newTotal;

    // display updated order
    Console.WriteLine("Order " + order.OrderId + " updated.");
    Console.WriteLine("Updated items:");
    for (int i = 0; i < order.OrderedFoodItems.Count; i++)
    {
        OrderedFoodItem it = order.OrderedFoodItems[i];
        Console.WriteLine((i + 1) + ". " + it.ItemName + " - " + it.QtyOrdered + " = $" + it.CalculateSubtotal().ToString("0.00"));
    }
    Console.WriteLine("Address:");
    Console.WriteLine(order.DeliveryAddress);
    Console.WriteLine("Delivery Date/Time:");
    Console.WriteLine(order.DeliveryDateTime.ToString("d/M/yyyy, HH:mm"));
    Console.WriteLine("Order Total: $" + order.OrderTotal.ToString("0.00"));
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

// Nur Tiara Nasha - Advanced (b)
void DisplayTotalOrderAmount()
{
    Console.WriteLine("\n===== Total Revenue =====");

    double totalOrdersAmount = 0.0;
    double totalRefunds = 0.0;

    foreach (Restaurant r in restaurants)
    {
        double restaurantDeliveredTotal = 0.0;
        double restaurantRefundTotal = 0.0;

        //for successful delivered orders
        foreach (Order order in r.Orders)
        {
            if (order.OrderStatus == "Delivered")
            {
                //subtract delivery fee
                double orderNet = order.OrderTotal - 5.00; // assuming the delivery fee
                restaurantDeliveredTotal += orderNet;
            }
        }
        //refunded orders
        foreach (Order refundedOrder in refundStack)
        {
            if (refundedOrder.RestaurantId == r.RestaurantId)
            {
                restaurantRefundTotal += refundedOrder.OrderTotal;
            }
        }

        //display for each rest

        Console.WriteLine($"\nRestaurant: {r.RestaurantName} ({r.RestaurantId})");
        Console.WriteLine($"  Total Delivered Orders (less delivery fee): ${restaurantDeliveredTotal:0.00}");
        Console.WriteLine($"  Total Refunds: ${restaurantRefundTotal:0.00}");

        totalOrdersAmount += restaurantDeliveredTotal;
        totalRefunds += restaurantRefundTotal;
    }

    double totalEarnings = totalOrdersAmount - totalRefunds;
    //print
    Console.WriteLine("\n===== Overall Totals =====");
    Console.WriteLine($"Total Orders Amount: ${totalOrdersAmount:0.00}");
    Console.WriteLine($"Total Refunds: ${totalRefunds:0.00}");
    Console.WriteLine($"Final Amount Gruberoo Earns: ${totalEarnings:0.00}");
}
