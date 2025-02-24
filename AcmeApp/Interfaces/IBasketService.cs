using AcmeApp;

public interface IBasketService
{
    void AddProductToBasket(Product product, int quantity);
    void RemoveProductFromBasket(Product product, int quantity);
    void ShowBasket();
    decimal GetBasketTotal();
    void ClearBasket();
    bool IsBasketEmpty();
}