namespace AcmeApp;
public class Basket
{
    public Dictionary<Product, int> Items { get; private set; } = new();
    public decimal TotalDiscount { get; private set; } = 0m;
    private OfferVoucher? _appliedOfferVoucher; 
    private List<GiftVoucher> _appliedGiftVouchers = new(); 
    public IEnumerable<GiftVoucher> AppliedGiftVouchers => _appliedGiftVouchers.AsReadOnly();
    public OfferVoucher? AppliedOfferVoucher => _appliedOfferVoucher;

    public void AddProduct(Product product, int quantity = 1)
    {
        if (Items.ContainsKey(product))
        {
            Items[product] += quantity;
        }
        else
        {
            Items[product] = quantity;
        }

        RecalculateDiscount();
    }

    public void RemoveProduct(Product product, int quantity = 1)
    {
        if (!Items.ContainsKey(product))
        {
            Console.WriteLine($"❌ Cannot remove {product.Name}: It is not in the basket.");
            return;
        }

        if (Items[product] > quantity)
        {
            Items[product] -= quantity;
            Console.WriteLine($"❌ Removed {quantity} x {product.Name} from the basket.");
        }
        else
        {
            Items.Remove(product);
            Console.WriteLine($"❌ Removed {product.Name} from the basket.");
        }

        RecalculateDiscount();
    }





    public decimal GetTotalPrice()
    {
        decimal total = Items.Sum(item => item.Key.Price * item.Value);
        return Math.Max(total - TotalDiscount, 0);
    }

    public void ApplyDiscount(decimal discount)
    {
        TotalDiscount = discount;
    }

    public string ApplyOfferVoucher(OfferVoucher voucher)
    {
        if (Items.Count == 0)
        {
            return "❌ Cannot apply: Basket is empty.";
        }

        decimal totalWithoutGiftVouchers = Items
            .Where(item => item.Key.Category != "Gift Voucher")
            .Sum(item => item.Key.Price * item.Value);

        if (totalWithoutGiftVouchers < voucher.Threshold)
        {
            decimal needed = voucher.Threshold - totalWithoutGiftVouchers;
            return $"❌ Cannot apply {voucher.Code}: Spend another £{needed:F2} to use this voucher.";
        }

        if (_appliedOfferVoucher != null)
        {
            return $"❌ Cannot apply {voucher.Code}: Only one offer voucher can be applied at a time.";
        }
            
        _appliedOfferVoucher = voucher;
        RecalculateDiscount();
        return $"✅ Offer Voucher {voucher.Code} applied!";
    }

    public string ApplyGiftVoucher(GiftVoucher voucher)
    {
        if (Items.Count == 0)
        {
            return "❌ Cannot apply: Basket is empty.";
        }
            

        decimal totalExcludingGiftVouchers = Items
            .Where(item => item.Key.Category != "Gift Voucher")
            .Sum(item => item.Key.Price * item.Value);

        if (totalExcludingGiftVouchers == 0)
        {
            return $"❌ Cannot apply {voucher.Code}: Gift vouchers cannot be used for purchasing other gift vouchers.";
        }

        _appliedGiftVouchers.Add(voucher);
        RecalculateDiscount();
        return $"✅ Applied Gift Voucher {voucher.Code}: £{voucher.Value} discount.";
    }

    public string RemoveGiftVoucher(string code)
    {
        var voucher = _appliedGiftVouchers.FirstOrDefault(v => v.Code == code);
        if (voucher == null)
        {
            return "❌ No such gift voucher applied.";
        }
            
        _appliedGiftVouchers.Remove(voucher);
        RecalculateDiscount();
        return $"✅ Gift Voucher {code} removed.";
    }

    public string RemoveOfferVoucher()
    {
        if (_appliedOfferVoucher == null)
        {
            return "❌ No offer voucher applied.";
        }

        string removedVoucher = _appliedOfferVoucher.Code;
        _appliedOfferVoucher = null;
        RecalculateDiscount();
        return $"✅ Offer Voucher {removedVoucher} removed.";
    }


    private void RecalculateDiscount()
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("❌ All vouchers have been removed as the basket is empty.");
            _appliedGiftVouchers.Clear();
            _appliedOfferVoucher = null;
            ApplyDiscount(0);
            return;
        }

        decimal totalWithoutGiftVouchers = Items
            .Where(item => item.Key.Category != "Gift Voucher")
            .Sum(item => item.Key.Price * item.Value);

        if (totalWithoutGiftVouchers == 0 && _appliedGiftVouchers.Any())
        {
            Console.WriteLine("❌ All Gift Vouchers have been removed as no valid items remain.");
            _appliedGiftVouchers.Clear();
        }

        decimal discount = 0m;

        if (_appliedOfferVoucher != null)
        {
            if (totalWithoutGiftVouchers < _appliedOfferVoucher.Threshold)
            {
                Console.WriteLine($"❌ Offer Voucher {_appliedOfferVoucher.Code} removed: Basket total is below £{_appliedOfferVoucher.Threshold}.");
                _appliedOfferVoucher = null;
            }
            else
            {
                discount += _appliedOfferVoucher.CalculateDiscount(this);
            }
        }

        foreach (var giftVoucher in _appliedGiftVouchers)
        {
            decimal remainingTotal = totalWithoutGiftVouchers - discount; 
            if (remainingTotal > 0)
            {
                discount += Math.Min(giftVoucher.Value, remainingTotal);
            }
        }

        ApplyDiscount(discount);
    }



    public void DisplayBasket()
    {
        Console.WriteLine("\n🛒 Basket Contents:");
        if (Items.Count == 0)
        {
            Console.WriteLine("Your basket is empty.");
            return;
        }

        foreach (var item in Items)
        {
            Console.WriteLine($"{item.Value} x {item.Key.Name} - £{item.Key.Price} each");
        }

        Console.WriteLine("------------");

        if (_appliedGiftVouchers.Any())
        {
            foreach (var gv in _appliedGiftVouchers)
            {
                Console.WriteLine($"1 x £{gv.Value} Gift Voucher {gv.Code} applied");
            }
        }

        if (_appliedOfferVoucher != null)
        {
            Console.WriteLine($"1 x £{_appliedOfferVoucher.Value} off {(_appliedOfferVoucher.CategoryRestriction ?? "baskets over " + _appliedOfferVoucher.Threshold)} Offer Voucher {_appliedOfferVoucher.Code} applied");
        }

        Console.WriteLine("------------");
        Console.WriteLine($"Total Discount: £{TotalDiscount}");
        Console.WriteLine($"Final Price: £{GetTotalPrice()}\n");
    }
}

