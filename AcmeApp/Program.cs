namespace AcmeApp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            IShoppingService shoppingService = new ShoppingService();
            shoppingService.StartShopping();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
