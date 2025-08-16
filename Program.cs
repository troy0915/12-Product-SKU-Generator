using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Product
{
    public string Brand { get; private set; }
    public string Category { get; private set; }
    public string Color { get; private set; }
    public string Size { get; private set; }
    public int Year { get; private set; }
    public string CustomCode { get; private set; }

    private static readonly List<string> AllowedCategories = new List<string>
    {
        "SHOES", "BAGS", "CLOTHING", "ACCESSORY"
    };

    private static readonly List<string> AllowedColors = new List<string>
    {
        "RED", "BLUE", "BLACK", "WHITE", "GREEN"
    };

    public Product(string brand, string category, string color, string size, int year, string customCode = null)
    {
        Brand = brand.Trim().ToUpper();
        Category = category.Trim().ToUpper();
        Color = color.Trim().ToUpper();
        Size = size.Trim().ToUpper();
        Year = year;
        CustomCode = string.IsNullOrWhiteSpace(customCode) ? null : customCode.Trim().ToUpper();

        ValidateAttributes();
    }

    private void ValidateAttributes()
    {
        if (!AllowedCategories.Contains(Category))
            throw new ArgumentException($"Invalid category: {Category}. Allowed: {string.Join(", ", AllowedCategories)}");

        if (!AllowedColors.Contains(Color))
            throw new ArgumentException($"Invalid color: {Color}. Allowed: {string.Join(", ", AllowedColors)}");

        if (Year < 1900 || Year > DateTime.Now.Year)
            throw new ArgumentException($"Invalid year: {Year}");
    }

    public string GenerateSku()
    {
        if (CustomCode != null)
            return CustomCode;

        string brandPart = Brand.Length >= 3 ? Brand.Substring(0, 3) : Brand.PadRight(3, 'X');
        string catPart = Category.Substring(0, 2);
        string colorPart = Color.Substring(0, 2);
        string sizePart = Size.Replace(" ", "");
        string yearPart = (Year % 100).ToString("D2");

        return $"{brandPart}-{catPart}-{colorPart}-{sizePart}-{yearPart}".ToUpper();
    }
}


namespace _12__Product_SKU_Generator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var existingSkus = new HashSet<string>();

            var productsToAdd = new List<Product>
        {
            new Product("Nike", "Shoes", "Red", "M", 2025),
            new Product("Adidas", "Clothing", "Black", "L", 2024),
            new Product("Puma", "Shoes", "Blue", "42", 2025),
            new Product("Nike", "Shoes", "Red", "M", 2025), // Duplicate SKU
            new Product("Gucci", "Bags", "Green", "OneSize", 2023, "GUCCI-BAG-123")
        };

            foreach (var product in productsToAdd)
            {
                try
                {
                    string sku = product.GenerateSku();
                    if (existingSkus.Contains(sku))
                    {
                        int counter = 1;
                        string newSku;
                        do
                        {
                            newSku = $"{sku}-{counter}";
                            counter++;
                        } while (existingSkus.Contains(newSku));
                        sku = newSku;
                    }

                    existingSkus.Add(sku);
                    Console.WriteLine($"[APPROVED] {sku}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"[REJECTED] {ex.Message}");
                }
            }
        }
    }
}




