using System;
using System.IO;
using System.Linq;
using System.Globalization;

// 1. Data Model (Equivalent to a dictionary row structure)
public record SalesItem(string ProductName, decimal Price, int Quantity);

public class CsvProcessor
{
    // Identifies the top-selling product by total revenue (price * quantity)
    public static (string ProductName, decimal Revenue) GetTopSellingProduct(string filename)
    {
        string topProduct = null;
        decimal maxRevenue = 0.0m;
        try
        {
            var dataLines = File.ReadLines(filename).Skip(1);
            foreach (string line in dataLines)
            {
                string[] columns = line.Split(',');
                if (columns.Length != 3)
                    continue;
                if (!decimal.TryParse(columns[1], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                    continue;
                if (!int.TryParse(columns[2], out int quantity))
                    continue;
                decimal revenue = price * quantity;
                if (revenue > maxRevenue || topProduct == null)
                {
                    topProduct = columns[0];
                    maxRevenue = revenue;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finding top-selling product: {ex.Message}");
        }
        return (topProduct, maxRevenue);
    }

    // C# method equivalent to the Python 'calculate_total_sales' function
    public static decimal CalculateTotalSales(string filename)
    {
        decimal total = 0.0m;

        try
        {
            var dataLines = File.ReadLines(filename).Skip(1);

            foreach (string line in dataLines)
            {
                string[] columns = line.Split(',');

                if (columns.Length != 3)
                {
                    Console.WriteLine($"Error: Malformed row (expected 3 columns): {line}");
                    continue;
                }

                if (!decimal.TryParse(columns[1], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                {
                    Console.WriteLine($"Error: Invalid price '{columns[1]}' in row: {line}");
                    continue;
                }

                if (!int.TryParse(columns[2], out int quantity))
                {
                    Console.WriteLine($"Error: Invalid quantity '{columns[2]}' in row: {line}");
                    continue;
                }

                total += price * quantity;
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: The file '{filename}' was not found.");
            return 0.0m;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        return total;
    }

    // C# Main method equivalent to Python's 'if __name__ == "__main__":' block
    public static void Main(string[] args)
    {
        // Note: For this to run, a file named 'sales_data.csv' must exist
        // in the output directory (e.g., bin/Debug/net8.0/).
        string salesDataFile = "sales_data.csv";

        // Example: Create a dummy file if it doesn't exist for testing
        if (!File.Exists(salesDataFile))
        {
            // You should replace this with a real file in a live environment
            File.WriteAllText(salesDataFile, "product_name,price,quantity\nLaptop,1200.00,5\nMouse,25.50,10\n");
        }

        decimal totalSales = CalculateTotalSales(salesDataFile);
        Console.WriteLine($"Total sales from {salesDataFile}: {totalSales.ToString("C", CultureInfo.CurrentCulture)}");

        var (topProduct, revenue) = GetTopSellingProduct(salesDataFile);
        if (topProduct != null)
        {
            Console.WriteLine($"Top-selling product: {topProduct}\n  Total revenue: {revenue.ToString("C", CultureInfo.CurrentCulture)}");
        }
        else
        {
            Console.WriteLine("No valid sales data to determine top-selling product.");
        }
    }
}
