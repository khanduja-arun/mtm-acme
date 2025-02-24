namespace AcmeApp;

public static class Config
{
    private static readonly string BasePath = GetProjectRoot();
    public static readonly string OrdersFilePath = Path.Combine(BasePath, "Repository", "orders.txt");
    public static readonly string ProductsJsonPath = Path.Combine(BasePath, "Repository", "products.json");
    public static readonly string VouchersJsonPath = Path.Combine(BasePath, "Repository", "vouchers.json");

    private static string GetProjectRoot()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        
        DirectoryInfo? directory = new DirectoryInfo(currentDirectory);
        while (directory != null && directory.Name != "AcmeApp")
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? currentDirectory; 
    }
}

