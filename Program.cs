using System;
using System.Collections.Generic;

public class Product
{
    // every product has its own specific id
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    // constructor to asign menu values to the shopping cart menu
    public Product(int id, string name, decimal price, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }

    public override string ToString()
    {
        return $"{Id}. {Name} - ${Price} ({Category})";
    }
}

public class CartItem
{
    // stores the product id and the amount qauntity
    public Product Product { get; set; }
    public int Quantity { get; set; }

    public CartItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public override string ToString()
    {
        return $"{Quantity}x {Product.Name} - ${Product.Price}";
    }

    public decimal GetTotalPrice()
    {
        return Product.Price * Quantity;
    }
}
public class ShoppingCart
{
    private List<CartItem> items;
    private decimal salesTax = 0.08m; // 8% sales tax
    private DateTime cartExpiration;

    public ShoppingCart()
    {
        items = new List<CartItem>();
        cartExpiration = DateTime.Now.AddMinutes(30); // Cart expires in 30 minutes
    }

    public void AddProduct(Product product, int quantity)
    {
        var existingItem = items.Find(i => i.Product.Id == product.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            items.Add(new CartItem(product, quantity));
        }
        Console.WriteLine($"{quantity}x {product.Name} added to the cart.");
    }

    public void RemoveProduct(int productId, int quantity)
    {
        var item = items.Find(i => i.Product.Id == productId);
        if (item != null)
        {
            if (quantity >= item.Quantity)
            {
                items.Remove(item);
                Console.WriteLine($"Removed {item.Product.Name} from the cart.");
            }
            else
            {
                item.Quantity -= quantity;
                Console.WriteLine($"Removed {quantity}x {item.Product.Name} from the cart.");
            }
        }
        else
        {
            Console.WriteLine("Product not found in cart.");
        }
    }

    public void ViewCart()
    {
        Console.WriteLine();
        Console.WriteLine(" ╔══════════════════════════════════╗");
        Console.WriteLine(" ║         Items in your cart       ║");
        Console.WriteLine(" ╚══════════════════════════════════╝\n");
        if (items.Count == 0)
        {
            Console.WriteLine("\nYour cart is empty.");
            return;
        }
        foreach (var item in items)
        {
            Console.WriteLine(item.ToString());
            Console.WriteLine($"{item.Product.Name} x {item.Quantity} - ${item.GetTotalPrice()}");
        }
    }

    public decimal GetSubtotal()
    {
        decimal subtotal = 0;
        foreach (var item in items)
        {
            subtotal += item.GetTotalPrice();
        }
        return subtotal;
    }

    public decimal GetDiscountAmount()
    {
        decimal subtotal = GetSubtotal();
        decimal discount = 0; // Declare the discount variable here

        // Calculate discount based on the subtotal
        if (subtotal > 10000)
        {
            discount = 0.20m; // 20% discount
        }
        else if (subtotal > 5000)
        {
            discount = 0.10m; // 10% discount
        }
        else if (subtotal > 2000)
        {
            discount = 0.05m; // 5% discount
        }

        return subtotal * discount; // Return the calculated discount amount
    }

    public decimal GetTaxAmount()
    {
        return (GetSubtotal() - GetDiscountAmount()) * salesTax;
    }

    public decimal GetTotal()
    {
        decimal subtotal = GetSubtotal();
        decimal totalAfterDiscount = subtotal - GetDiscountAmount();
        return totalAfterDiscount + GetTaxAmount();
    }

    public bool IsCartExpired()
    {
        return DateTime.Now > cartExpiration;
    }

    public void RecommendProducts(List<Product> allProducts)
    {
        var purchasedCategories = new HashSet<string>();
        foreach (var item in items)
        {
            purchasedCategories.Add(item.Product.Category);
        }
        Console.WriteLine("\nBased on your cart, we recommend:");
        foreach (var product in allProducts)
        {
            if (purchasedCategories.Contains(product.Category))
            {
                Console.WriteLine($"{product.Name} - ${product.Price} ({product.Category})");
            }
        }
    }

    public void Checkout(List<Product> allProducts)
    {
        if (IsCartExpired())
        {
            Console.WriteLine("Your cart has expired. Please start a new order.");
        }
        else if (items.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
        }
        else
        {
            ViewCart();
            Console.WriteLine(" ╔══════════════════════════════════╗");
            Console.WriteLine(" ║        Proceeding to checkout    ║");
            Console.WriteLine(" ╚══════════════════════════════════╝");

            Console.WriteLine("\n----------------------------------");
            Console.WriteLine($"Sub_total: ${GetSubtotal()}");
            Console.WriteLine($"After_Discount: ${GetDiscountAmount()}");
            Console.WriteLine($"Tax: ${GetTaxAmount()}");
            Console.WriteLine($"Total: ${GetTotal()}");
            Console.WriteLine("----------------------------------");

            Console.WriteLine($"Checkout Date and Time: {DateTime.Now}");
            Console.WriteLine("Thank you for your purchase!.........");
            RecommendProducts(allProducts);
            items.Clear(); // Clear the cart after checkout

        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var cart = new ShoppingCart();
        var products = new List<Product>
        {
            new Product(1, "Laptop", 999.99m, "Electronics"),
            new Product(2, "Smartphone", 599.99m, "Electronics"),
            new Product(3, "Headphones", 199.99m, "Electronics"),
            new Product(4, "Monitor", 299.99m, "Electronics"),
            new Product(5, "Keyboard", 49.99m, "Accessories"),
            new Product(6, "Mouse", 29.99m, "Accessories"),
            new Product(7, "Laptop Bag", 49.99m, "Accessories"),
            new Product(8, "Smartwatch", 199.99m, "Wearables")
        };

        Console.WriteLine("\t  =================================================================================================");
        Console.WriteLine("\t                                          Welcome to the Shop!                                    ");
        Console.WriteLine("\t  =================================================================================================\n");

        Console.WriteLine("                                _______               _______               _______  ");
        Console.WriteLine("                               /      /,             /      /,             /      /, ");
        Console.WriteLine("                              /      //             /      //             /      //  ");
        Console.WriteLine("                             /______//             /______//             /______//   ");
        Console.WriteLine("                            (______(/             (______(/             (______(/    ");
        Console.WriteLine("                              |  |                  |  |                  |  |       ");
        Console.WriteLine("                              o  o                  o  o                  o  o       ");

        Console.WriteLine("\n\t  =================================================================================================\n");

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n ╔══════════════════════════════════╗");
            Console.WriteLine(" ║       ~:~(Shopping Cart )~:~     ║");
            Console.WriteLine(" ╠══════════════════════════════════╣");
            Console.WriteLine(" ║ 1. View products                 ║");
            Console.WriteLine(" ║ 2. Add product to cart           ║");
            Console.WriteLine(" ║ 3. Remove product from cart      ║");
            Console.WriteLine(" ║ 4. View cart                     ║");
            Console.WriteLine(" ║ 5. Checkout                      ║");
            Console.WriteLine(" ║ 6. Exit                          ║");
            Console.WriteLine(" ╚══════════════════════════════════╝");
            Console.Write("\n ENTER CHOICE (1-6) : ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewProducts(products);
                    break;
                case "2":
                    AddToCart(cart, products);
                    break;
                case "3":
                    RemoveFromCart(cart);
                    break;
                case "4":
                    cart.ViewCart();
                    break;
                case "5":
                    cart.Checkout(products);
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void ViewProducts(List<Product> products)
    {
        Console.WriteLine("\n  ╔══════════════════════════════════╗");
        Console.WriteLine("  ║        Available Products        ║");
        Console.WriteLine("  ╚══════════════════════════════════╝\n");
        foreach (var product in products)
        {
            Console.WriteLine(product.ToString());
        }
    }

    static void AddToCart(ShoppingCart cart, List<Product> products)
    {
        Console.Write("Enter the product ID to add to the cart:");
        int productId;
        if (int.TryParse(Console.ReadLine(), out productId))
        {
            var product = products.Find(p => p.Id == productId);
            if (product != null)
            {
                Console.Write("Enter quantity:");
                int quantity;
                if (int.TryParse(Console.ReadLine(), out quantity))
                {
                    cart.AddProduct(product, quantity);

                    // Ask if the user wants to add another product
                    Console.Write("Do you want to add another product? (yes/no)  : ");
                    string addMore = Console.ReadLine()?.ToLower();
                    if (addMore == "yes")
                    {
                        AddToCart(cart, products); // Recursively call AddToCart to add more items
                    }
                }
                else
                {
                    Console.WriteLine("Invalid quantity. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid product ID.");
        }
    }

    static void RemoveFromCart(ShoppingCart cart)
    {
        Console.Write("Enter the product ID to remove from the cart:");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            Console.Write("Enter the quantity to remove:");
            if (int.TryParse(Console.ReadLine(), out int quantity))
            {
                cart.RemoveProduct(productId, quantity);
            }
            else
            {
                Console.WriteLine("Invalid quantity. Please try again.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid product ID.");
        }

        // Ask if the user wants to remove another product
        Console.Write("\nDo you want to remove another product? (yes/no) : ");
        string removeMore = Console.ReadLine()?.ToLower();
        if (removeMore == "yes")
        {
            RemoveFromCart(cart); // Recursive call for removing more items
        }
    }
}

