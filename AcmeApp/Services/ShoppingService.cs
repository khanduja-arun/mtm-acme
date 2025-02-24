namespace AcmeApp;

public class ShoppingService : IShoppingService
{
    private readonly BasketService _basketService;
    private readonly VoucherService _voucherService;
    private readonly RepositoryService _repositoryService;

    public ShoppingService()
    {
        var basket = new Basket();
        _basketService = new BasketService(basket);
        _repositoryService = new RepositoryService();
        _voucherService = new VoucherService(basket, _repositoryService);
    }

    public void ShowProducts()
    {
        Console.WriteLine("\n🛍️ Available Products:");
        for (int i = 0; i < _repositoryService.Products.Count; i++)
            Console.WriteLine($"{i + 1}. {_repositoryService.Products[i].Name} - £{_repositoryService.Products[i].Price}");
    }

    public void AddProduct(int productNumber)
    {
        if (productNumber >= 1 && productNumber <= _repositoryService.Products.Count)
        {
            _basketService.AddProductToBasket(_repositoryService.Products[productNumber - 1], 1);
        }
        else
        {
            Console.WriteLine("❌ Invalid selection.");
        }
    }

    public void RemoveProduct(int productNumber)
    {
        if (productNumber >= 1 && productNumber <= _repositoryService.Products.Count)
        {
            _basketService.RemoveProductFromBasket(_repositoryService.Products[productNumber - 1], 1);
            Console.WriteLine($"✅ Removed {_repositoryService.Products[productNumber - 1].Name} from basket.");
        }
        else
        {
            Console.WriteLine("❌ Invalid selection.");
        }
    }

    public void ShowBasket()
    {
        _basketService.ShowBasket();
    }

    public void ApplyVoucher()
    {
        _voucherService.ShowAvailableVouchers();
        Console.Write("\nEnter voucher code to apply: ");
        string code = Console.ReadLine()?.Trim().ToUpper();

        if (string.IsNullOrEmpty(code))
        {
            Console.WriteLine("❌ Invalid input.");
            return;
        }

        string result = code.StartsWith("GV-") ? _voucherService.ApplyGiftVoucher(code)
                    : code.StartsWith("OV-") ? _voucherService.ApplyOfferVoucher(code)
                    : "❌ Invalid voucher selection.";

        Console.WriteLine(result);
    }

    public void RemoveVoucher()
    {
        Console.WriteLine("\n🎟️ Applied Vouchers:");
        if (!_basketService.GetBasket().AppliedGiftVouchers.Any() && _basketService.GetBasket().AppliedOfferVoucher == null)
        {
            Console.WriteLine("❌ No vouchers applied.");
            return;
        }


        int index = 1;
        foreach (var gv in _basketService.GetBasket().AppliedGiftVouchers)
            Console.WriteLine($"{index++}. £{gv.Value} Gift Voucher (Code: {gv.Code})");

        if (_basketService.GetBasket().AppliedOfferVoucher != null)
            Console.WriteLine($"{index}. £{_basketService.GetBasket().AppliedOfferVoucher.Value} Off Offer Voucher (Code: {_basketService.GetBasket().AppliedOfferVoucher.Code})");

        Console.Write("\nEnter voucher code to remove or type 'OFFER' to remove the offer voucher: ");
        string code = Console.ReadLine()?.Trim().ToUpper();

        if (string.IsNullOrEmpty(code))
        {
            Console.WriteLine("❌ Invalid input.");
            return;
        }

        string result = code == "OFFER" ? _voucherService.RemoveOfferVoucher() : _voucherService.RemoveGiftVoucher(code);
        Console.WriteLine(result);
    }


    public void Checkout()
    {
        if (_basketService.IsBasketEmpty())
        {
            Console.WriteLine("❌ Your basket is empty. Add items before checking out.");
            return;
        }

        Console.Write("\n🛒 Confirm checkout? (Y/N): ");
        string confirmation = Console.ReadLine()?.Trim().ToUpper();

        if (confirmation == "Y")
        {
            OrderService.SaveOrder(_basketService.GetBasket());
            _basketService.ClearBasket(); 
            Console.WriteLine("✅ Your basket has been cleared. You can start a new shopping session.");
        }
        else
        {
            Console.WriteLine("❌ Checkout cancelled.");
        }
    }

    public void StartShopping()
    {
        Console.WriteLine("\n\n🛒 Welcome to (MTM)Acme Shopping Basket!\n");

        while (true)
        {
            Console.WriteLine("\n📌 Available Options:");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Add Product to Basket");
            Console.WriteLine("3. Remove Product from Basket");
            Console.WriteLine("4. Show Basket");
            Console.WriteLine("5. Apply Voucher");
            Console.WriteLine("6. Remove Applied Vouchers"); 
            Console.WriteLine("7. Checkout");
            Console.WriteLine("8. View Past Orders");
            Console.WriteLine("9. Exit");
            Console.Write("\nEnter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowProducts();
                    break;
                case "2":
                    Console.Write("Enter product number to add: ");
                    if (int.TryParse(Console.ReadLine(), out int addIndex))
                        AddProduct(addIndex);
                    break;
                case "3":
                    Console.Write("Enter product number to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int removeIndex))
                        RemoveProduct(removeIndex);
                    break;
                case "4":
                    ShowBasket();
                    break;
                case "5":
                    ApplyVoucher();
                    break;
                case "6":
                    RemoveVoucher(); 
                    break;
                case "7":
                    Checkout();
                    break;
                case "8":
                    OrderService.ShowPastOrders();
                    break;
                case "9":
                    Console.WriteLine("\n👋 Thank you for shopping with Acme. Goodbye!\n");
                    return;
                default:
                    Console.WriteLine("❌ Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
