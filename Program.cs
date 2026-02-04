using PRG2Assignment;
using S10273555_PRG2Assignment;

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
    using (StreamReader sr = new StreamReader("orders - Copy.csv"))
    {
        string header = sr.ReadLine();
        string? line;

        while ((line = sr.ReadLine()) != null)
        {
            string[] parts = line.Split(',');
            string orderid = parts[0];
            string custem = parts[1];
            string restid = parts[2];
            string dd = parts[3];
            string dt = parts[4];
            string da = parts[5];
            string cdt = parts[6];
            double totalamt = double.Parse(parts[7]);
            string status = parts[8];
            string items = parts[9];

            Order order = new Order(orderid, custem, restid, dd, dt, da, cdt, totalamt, status, items);
            ordersList.Add(order);
        }
    }
}