using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopSystem
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Price:C}";
        }
    }

    public class Order
    {
        private static int nextOrderId = 1;
        
        public int OrderId { get; set; }
        public Dictionary<Product, int> Products { get; set; } // Товар і кількість
        public DateTime CreatedDate { get; set; }

        public Order()
        {
            OrderId = nextOrderId++;
            Products = new Dictionary<Product, int>();
            CreatedDate = DateTime.Now;
        }

        public void AddProduct(Product product, int quantity)
        {
            if (Products.ContainsKey(product))
            {
                Products[product] += quantity;
            }
            else
            {
                Products.Add(product, quantity);
            }
        }

        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }

        public void UpdateProductQuantity(Product product, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                Products.Remove(product);
            }
            else if (Products.ContainsKey(product))
            {
                Products[product] = newQuantity;
            }
        }

        public decimal GetTotalAmount()
        {
            decimal total = 0;
            foreach (var item in Products)
            {
                total += item.Key.Price * item.Value;
            }
            return total;
        }

        public void ShowOrderDetails()
        {
            Console.WriteLine($"\n");
            Console.WriteLine($"     ЗАМОВЛЕННЯ #{OrderId}");
            Console.WriteLine($"     Дата: {CreatedDate:dd.MM.yyyy HH:mm}");

            if (Products.Count == 0)
            {
                Console.WriteLine("  Замовлення порожнє");
            }
            else
            {
                Console.WriteLine("\n  Товари:");
                foreach (var item in Products)
                {
                    Console.WriteLine($"  • {item.Key.Name} x{item.Value} - {item.Key.Price * item.Value:C}");
                }
                Console.WriteLine($"\n");
                Console.WriteLine($"  ЗАГАЛЬНА СУМА: {GetTotalAmount():C}");
            }
        }
    }

    public class Shop
    {
        public List<Product> AvailableProducts { get; set; }
        public List<Order> Orders { get; set; }

        public Shop()
        {
            AvailableProducts = new List<Product>();
            Orders = new List<Order>();
            InitializeProducts();
        }

        private void InitializeProducts()
        {
            AvailableProducts.Add(new Product(1, "Ноутбук Lenovo", 25000m));
            AvailableProducts.Add(new Product(2, "Мишка Logitech", 500m));
            AvailableProducts.Add(new Product(3, "Клавіатура", 1200m));
            AvailableProducts.Add(new Product(4, "Монітор Samsung", 8000m));
            AvailableProducts.Add(new Product(5, "Навушники Sony", 3000m));
            AvailableProducts.Add(new Product(6, "Веб-камера", 1500m));
            AvailableProducts.Add(new Product(7, "USB-флешка 64GB", 300m));
        }

        public void ShowAvailableProducts()
        {
            Console.WriteLine("\n");
            Console.WriteLine("     ДОСТУПНІ ТОВАРИ");
            
            foreach (var product in AvailableProducts)
            {
                Console.WriteLine($"  {product}");
            }
        }

        public Product GetProductById(int id)
        {
            return AvailableProducts.FirstOrDefault(p => p.Id == id);
        }

        public Order CreateOrder()
        {
            Order order = new Order();
            Orders.Add(order);
            return order;
        }

        public Order GetOrderById(int orderId)
        {
            return Orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public void DeleteOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                Orders.Remove(order);
                Console.WriteLine($"\nЗамовлення #{orderId} видалено успішно!");
            }
            else
            {
                Console.WriteLine($"\n Замовлення #{orderId} не знайдено!");
            }
        }

        public void ShowAllOrders()
        {
            if (Orders.Count == 0)
            {
                Console.WriteLine("\n Немає жодного замовлення!");
                return;
            }

            Console.WriteLine("\n");
            Console.WriteLine("     ВСІ ЗАМОВЛЕННЯ");

            decimal grandTotal = 0;

            foreach (var order in Orders)
            {
                Console.WriteLine($"\n Замовлення #{order.OrderId} | {order.CreatedDate:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"     Товарів: {order.Products.Count} | Сума: {order.GetTotalAmount():C}");
                grandTotal += order.GetTotalAmount();
            }

            Console.WriteLine($"\n");
            Console.WriteLine($"    ЗАГАЛЬНА СУМА ВСІХ ЗАМОВЛЕНЬ: {grandTotal:C}");
        }
    }

    // Клас Menu (Меню)
    public class Menu
    {
        private Shop shop;

        public Menu()
        {
            shop = new Shop();
        }

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            bool exit = false;

            while (!exit)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateOrder();
                        break;
                    case "2":
                        EditOrder();
                        break;
                    case "3":
                        ViewOrder();
                        break;
                    case "4":
                        DeleteOrder();
                        break;
                    case "5":
                        ViewAllOrders();
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("\nдо побачення!");
                        break;
                    default:
                        Console.WriteLine("\n Невірний вибір!");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНатисніть будь-яку клавішу...");
                    Console.ReadKey();
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("              MENU");
            Console.WriteLine("\n");
            Console.WriteLine("  1.  Створити замовлення");
            Console.WriteLine("  2. ️ Редагувати замовлення");
            Console.WriteLine("  3.  Переглянути замовлення");
            Console.WriteLine("  4.  Видалити замовлення");
            Console.WriteLine("  5.  Переглянути усі замовлення");
            Console.WriteLine("  0.  Вихід");
            Console.Write("\nВаш вибір: ");
        }

        private void CreateOrder()
        {
            Console.Clear();
            Console.WriteLine("    СТВОРЕННЯ НОВОГО ЗАМОВЛЕННЯ");

            Order order = shop.CreateOrder();
            Console.WriteLine($"\nСтворено замовлення #{order.OrderId}");

            AddProductsToOrder(order);

            Console.WriteLine($"\nЗамовлення #{order.OrderId} успішно створено!");
            order.ShowOrderDetails();
        }

        private void AddProductsToOrder(Order order)
        {
            bool continueAdding = true;

            while (continueAdding)
            {
                shop.ShowAvailableProducts();

                Console.Write("\nВведіть ID товару (0 - завершити): ");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    if (productId == 0)
                    {
                        continueAdding = false;
                        continue;
                    }

                    Product product = shop.GetProductById(productId);
                    if (product != null)
                    {
                        Console.Write("Введіть кількість: ");
                        if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                        {
                            order.AddProduct(product, quantity);
                            Console.WriteLine($"\nДодано: {product.Name} x{quantity}");
                        }
                        else
                        {
                            Console.WriteLine("\nНевірна кількість!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nТовар не знайдено!");
                    }
                }
                else
                {
                    Console.WriteLine("\nНевірний ввід!");
                }

                if (continueAdding)
                {
                    Console.Write("\nДодати ще товар? (y/n): ");
                    string answer = Console.ReadLine()?.ToLower();
                    if (answer != "y" && answer != "н" && answer != "так")
                    {
                        continueAdding = false;
                    }
                }
            }
        }

        private void EditOrder()
        {
            Console.Clear();
            Console.WriteLine("      РЕДАГУВАННЯ ЗАМОВЛЕННЯ");

            shop.ShowAllOrders();

            Console.Write("\nВведіть номер замовлення для редагування: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                Order order = shop.GetOrderById(orderId);
                if (order != null)
                {
                    order.ShowOrderDetails();

                    bool continueEditing = true;
                    while (continueEditing)
                    {
                        Console.WriteLine("\n1. Додати товар");
                        Console.WriteLine("2. Змінити кількість товару");
                        Console.WriteLine("3. Видалити товар");
                        Console.WriteLine("0. Завершити редагування");
                        Console.Write("\nВаш вибір: ");

                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                AddProductsToOrder(order);
                                break;
                            case "2":
                                ChangeProductQuantity(order);
                                break;
                            case "3":
                                RemoveProductFromOrder(order);
                                break;
                            case "0":
                                continueEditing = false;
                                break;
                            default:
                                Console.WriteLine("\nНевірний вибір!");
                                break;
                        }

                        if (continueEditing)
                        {
                            order.ShowOrderDetails();
                        }
                    }

                    Console.WriteLine("\nЗамовлення оновлено!");
                }
                else
                {
                    Console.WriteLine("\nЗамовлення не знайдено!");
                }
            }
            else
            {
                Console.WriteLine("\nНевірний ввід!");
            }
        }

        private void ChangeProductQuantity(Order order)
        {
            shop.ShowAvailableProducts();
            Console.Write("\nВведіть ID товару: ");
            
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                Product product = shop.GetProductById(productId);
                if (product != null && order.Products.ContainsKey(product))
                {
                    Console.Write("Введіть нову кількість (0 - видалити): ");
                    if (int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        order.UpdateProductQuantity(product, quantity);
                        Console.WriteLine("\nКількість оновлено!");
                    }
                }
                else
                {
                    Console.WriteLine("\nТовар не знайдено в замовленні!");
                }
            }
        }

        private void RemoveProductFromOrder(Order order)
        {
            shop.ShowAvailableProducts();
            Console.Write("\nВведіть ID товару для видалення: ");
            
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                Product product = shop.GetProductById(productId);
                if (product != null)
                {
                    order.RemoveProduct(product);
                    Console.WriteLine("\nТовар видалено з замовлення!");
                }
                else
                {
                    Console.WriteLine("\nТовар не знайдено!");
                }
            }
        }

        private void ViewOrder()
        {
            Console.Clear();
            Console.WriteLine("     ПЕРЕГЛЯД ЗАМОВЛЕННЯ");

            shop.ShowAllOrders();

            Console.Write("\nВведіть номер замовлення: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                Order order = shop.GetOrderById(orderId);
                if (order != null)
                {
                    order.ShowOrderDetails();
                }
                else
                {
                    Console.WriteLine("\nЗамовлення не знайдено!");
                }
            }
            else
            {
                Console.WriteLine("\nНевірний ввід!");
            }
        }

        private void DeleteOrder()
        {
            Console.Clear();
            Console.WriteLine("      ВИДАЛЕННЯ ЗАМОВЛЕННЯ");

            shop.ShowAllOrders();

            Console.Write("\nВведіть номер замовлення для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                Order order = shop.GetOrderById(orderId);
                if (order != null)
                {
                    order.ShowOrderDetails();
                    Console.Write("\nВи впевнені? (y/n): ");
                    string confirm = Console.ReadLine()?.ToLower();
                    
                    if (confirm == "y" || confirm == "н" || confirm == "так")
                    {
                        shop.DeleteOrder(orderId);
                    }
                    else
                    {
                        Console.WriteLine("\nВидалення скасовано!");
                    }
                }
                else
                {
                    Console.WriteLine("\nЗамовлення не знайдено!");
                }
            }
            else
            {
                Console.WriteLine("\nНевірний ввід!");
            }
        }

        private void ViewAllOrders()
        {
            Console.Clear();
            shop.ShowAllOrders();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.Run();
        }
    }
}