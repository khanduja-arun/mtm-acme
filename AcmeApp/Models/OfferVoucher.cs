
namespace AcmeApp;
public class OfferVoucher : Voucher
{
    public decimal Threshold { get; private set; }
    public string? CategoryRestriction { get; private set; }

    public OfferVoucher(string code, decimal value, decimal threshold, string? categoryRestriction = null)
        : base(code, value)
    {
        Threshold = threshold;
        CategoryRestriction = categoryRestriction;
    }

    public override string Apply(Basket basket)
    {
        if (basket.Items.Count == 0)
        {
            return "❌ Cannot apply: Basket is empty.";
        }
        
        decimal totalWithoutGiftVouchers = basket.Items
            .Where(item => item.Key.Category != "Gift Voucher")
            .Sum(item => item.Key.Price * item.Value);

        if (totalWithoutGiftVouchers < Threshold)
        {
            decimal needed = Threshold - totalWithoutGiftVouchers;
            return $"❌ Cannot apply {Code}: Spend another £{needed:F2} to use this voucher.";
        }

        basket.ApplyOfferVoucher(this);
        return $"✅ Offer Voucher {Code} applied. Discount will update dynamically!";
    }

    public decimal CalculateDiscount(Basket basket)
    {
        if (!string.IsNullOrEmpty(CategoryRestriction))
        {
            decimal categoryTotal = basket.Items
                .Where(item => item.Key.Category == CategoryRestriction)
                .Sum(item => item.Key.Price * item.Value);

            return Math.Min(Value, categoryTotal);
        }
        return Value;
    }
}
