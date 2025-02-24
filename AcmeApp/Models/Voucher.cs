namespace AcmeApp;
public abstract class Voucher
{
    public string Code { get; private set; }
    public decimal Value { get; private set; }

    protected Voucher(string code, decimal value)
    {
        Code = code;
        Value = value;
    }
    
    public abstract string Apply(Basket basket);
}