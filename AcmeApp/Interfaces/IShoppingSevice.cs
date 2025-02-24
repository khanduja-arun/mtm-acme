namespace AcmeApp;

public interface IShoppingService
{
    void ShowProducts();
    void AddProduct(int productNumber);
    void RemoveProduct(int productNumber);
    void ShowBasket();
    void ApplyVoucher();
    void RemoveVoucher();
    void StartShopping();
    void Checkout() ;
}