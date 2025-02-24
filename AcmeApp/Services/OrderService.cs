namespace AcmeApp;
public class OrderService
{
    // private const string OrderFilePath = "/Users/arunkhanduja/Desktop/mtm-acme/AcmeApp/Repository/orders.txt"; 
    public static void SaveOrder(Basket basket)
    {
        try
        {
            if (basket.Items.Count == 0)
            {
                Console.WriteLine("❌ Your basket is empty. Cannot place an order.");
                return;
            }

            using (StreamWriter writer = new StreamWriter(Config.OrdersFilePath, append: true))
            {
                writer.WriteLine("========================================");
                writer.WriteLine($"🛒 Order Placed on: {DateTime.Now}");
                writer.WriteLine("Items:");

                foreach (var item in basket.Items)
                    writer.WriteLine($"{item.Value} x {item.Key.Name} - £{item.Key.Price} each");

                writer.WriteLine("------------");
                writer.WriteLine($"Total Discount: £{basket.TotalDiscount}");
                writer.WriteLine($"Final Price: £{basket.GetTotalPrice()}");
                writer.WriteLine("========================================\n");
            }

            Console.WriteLine("\n✅ Order has been successfully placed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error saving order: {ex.Message}");
        }
    }

            public static void ShowPastOrders()
        {
            try
            {
                if (!File.Exists(Config.OrdersFilePath) || new FileInfo(Config.OrdersFilePath).Length == 0)
                {
                    Console.WriteLine("❌ No past orders found.");
                    return;
                }

                Console.WriteLine("\n📜 Past Orders:");
                string[] orderHistory = File.ReadAllLines(Config.OrdersFilePath);
                foreach (string line in orderHistory)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error reading past orders: {ex.Message}");
            }
        }
}

