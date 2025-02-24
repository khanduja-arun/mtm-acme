namespace AcmeApp;

public class VoucherService : IVoucherService
{
    private readonly Basket _basket;
    private readonly RepositoryService _repositoryService;

    public VoucherService(Basket basket, RepositoryService repositoryService)
    {
        _basket = basket;
        _repositoryService = repositoryService;
    }

    public void ShowAvailableVouchers()
    {
        Console.WriteLine("\n🎟️ Available Vouchers:");
        int index = 1;

        foreach (var gv in _repositoryService.GiftVouchers)
            Console.WriteLine($"{index++}. £{gv.Value} Gift Voucher (Code: {gv.Code})");

        foreach (var ov in _repositoryService.OfferVouchers)
            Console.WriteLine($"{index++}. £{ov.Value} Off {ov.CategoryRestriction} (Spend £{ov.Threshold}) (Code: {ov.Code})");
    }

    public string ApplyGiftVoucher(string code)
    {
        var voucher = _repositoryService.GiftVouchers.Find(v => v.Code == code);
        return voucher == null ? "❌ Invalid gift voucher code." : _basket.ApplyGiftVoucher(voucher);
    }

    public string ApplyOfferVoucher(string code)
    {
        var voucher = _repositoryService.OfferVouchers.Find(v => v.Code == code);
        return voucher == null ? "❌ Invalid offer voucher code." : _basket.ApplyOfferVoucher(voucher);
    }

    public string RemoveGiftVoucher(string code)
    {
        return _basket.RemoveGiftVoucher(code);
    }

    public string RemoveOfferVoucher()
    {
        return _basket.RemoveOfferVoucher();
    }

}
