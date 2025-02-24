namespace AcmeApp;
public class GiftVoucher : Voucher
{
    public GiftVoucher(string code, decimal value) : base(code, value) { }

    public override string Apply(Basket basket)
    {
        if (basket.Items.Count == 0)
        {
            return "❌ Cannot apply: Basket is empty.";
        }

        decimal totalExcludingGiftVouchers = basket.Items
            .Where(item => item.Key.Category != "Gift Voucher")
            .Sum(item => item.Key.Price * item.Value);

        if (totalExcludingGiftVouchers == 0)
        {
            return $"❌ Cannot apply {Code}: Gift vouchers cannot be used for purchasing other gift vouchers.";
        }

        decimal discount = Math.Min(Value, totalExcludingGiftVouchers);
        basket.ApplyDiscount(discount);

        return $"✅ Applied Gift Voucher {Code}: £{discount} discount.";
    }
}
