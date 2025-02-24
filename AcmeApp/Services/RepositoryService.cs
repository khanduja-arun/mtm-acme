using System.Text.Json;
namespace AcmeApp;

public class RepositoryService
{
    // private const string ProductFilePath = "/Users/arunkhanduja/Desktop/mtm-acme/AcmeApp/Repository/products.json";
    // private const string VoucherFilePath = "/Users/arunkhanduja/Desktop/mtm-acme/AcmeApp/Repository/vouchers.json";

    public List<Product> Products { get; private set; }
    public List<GiftVoucher> GiftVouchers { get; private set; }
    public List<OfferVoucher> OfferVouchers { get; private set; }

        private class VoucherData
    {
        public List<GiftVoucher> GiftVouchers { get; set; } = new();
        public List<OfferVoucher> OfferVouchers { get; set; } = new();
    }

    public RepositoryService()
    {
        Products = LoadProducts();
        var vouchers = LoadVouchers();
        GiftVouchers = vouchers.giftVouchers;
        OfferVouchers = vouchers.offerVouchers;
    }

    private List<Product> LoadProducts()
    {
        try
        {
            System.Console.WriteLine(Config.ProductsJsonPath);
        


            if (!File.Exists(Config.ProductsJsonPath))
            {
                Console.WriteLine("❌ Product file not found!");
                return new List<Product>();
            }

            string jsonData = File.ReadAllText(Config.ProductsJsonPath);
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var products = JsonSerializer.Deserialize<List<Product>>(jsonData, options);
            if (products == null)
            {
                Console.WriteLine("❌ Failed to load products. JSON file is empty or corrupted.");
                return new List<Product>();
            }

            Console.WriteLine("✅ Products loaded successfully.");
            return products;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error loading products: {ex.Message}");
            return new List<Product>();
        }
    }

    private (List<GiftVoucher> giftVouchers, List<OfferVoucher> offerVouchers) LoadVouchers()
    {
        try
        {
            if (!File.Exists(Config.VouchersJsonPath))
            {
                Console.WriteLine("❌ Voucher file not found!");
                return (new List<GiftVoucher>(), new List<OfferVoucher>());
            }

            string jsonData = File.ReadAllText(Config.VouchersJsonPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var vouchersData = JsonSerializer.Deserialize<VoucherData>(jsonData, options);
            if (vouchersData == null)
            {
                Console.WriteLine("❌ Failed to load vouchers. JSON file is empty or corrupted.");
                return (new List<GiftVoucher>(), new List<OfferVoucher>());
            }

            Console.WriteLine("✅ Vouchers loaded successfully.");
            return (vouchersData.GiftVouchers, vouchersData.OfferVouchers);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error loading vouchers: {ex.Message}");
            return (new List<GiftVoucher>(), new List<OfferVoucher>());
        }
    }
}