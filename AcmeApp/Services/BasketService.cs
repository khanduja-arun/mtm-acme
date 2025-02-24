

namespace AcmeApp;
public class BasketService : IBasketService
{
    private readonly Basket _basket;

    public BasketService(Basket basket)
    {
        _basket = basket;
    }

    public void AddProductToBasket(Product product, int quantity)
    {
        _basket.AddProduct(product, quantity);
        Console.WriteLine($"✅ Added {quantity} x {product.Name} to the basket.");
    }

    public void RemoveProductFromBasket(Product product, int quantity)
    {
        _basket.RemoveProduct(product, quantity);
        Console.WriteLine($"❌ Removed {quantity} x {product.Name} from the basket.");
    }

    public void ShowBasket()
    {
        _basket.DisplayBasket();
    }

    public decimal GetBasketTotal()
    {
        return _basket.GetTotalPrice();
    }

     public Basket GetBasket()
    {
        return _basket;
    }
        public bool IsBasketEmpty()
        {
            return _basket.Items.Count == 0;
        }

        public void ClearBasket()
        {
            _basket.Items.Clear();
            _basket.ApplyDiscount(0);
            Console.WriteLine("✅ Basket has been cleared.");
        }
}
