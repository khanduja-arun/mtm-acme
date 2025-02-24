
namespace AcmeApp;
public interface IVoucherService
{
         string ApplyGiftVoucher(string code);
        string ApplyOfferVoucher(string code);
        void ShowAvailableVouchers();
}