using System;
using System.Data;
using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=QuitQ;Trusted_Connection=True;TrustServerCertificate=True;";
    static int loggedInUserId = 0;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n==== QuitQ E-Commerce Console ====");
            Console.WriteLine("1. Login / Signup");
            Console.WriteLine("2. View Products");
            Console.WriteLine("3. Add to Cart");
            Console.WriteLine("4. View Cart");
            Console.WriteLine("5. Place Order");
            Console.WriteLine("6. Logout");
            Console.WriteLine("7. Exit");

            Console.Write("Enter choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("\nDo you have an account? (Y/N): ");
                    char hasAccount = char.ToUpper(Console.ReadKey().KeyChar);
                    Console.WriteLine();

                    if (hasAccount == 'Y') Login();
                    else Signup();
                    break;

                case 2:
                    ViewProducts();
                    break;

                case 3:
                    if (CheckLogin()) AddToCart();
                    break;

                case 4:
                    if (CheckLogin()) ViewCart();
                    break;

                case 5:
                    if (CheckLogin()) PlaceOrder();
                    break;

                case 6:
                    Logout();
                    break;

                case 7:
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static bool CheckLogin()
    {
        if (loggedInUserId == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Please login first!");
            Console.ResetColor();
            return false;
        }
        return true;
    }

    static void Signup()
    {
        Console.Write("\nUsername: ");
        string username = Console.ReadLine();
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        Console.Write("Phone: ");
        string phone = Console.ReadLine();
        Console.Write("Address: ");
        string address = Console.ReadLine();
        Console.Write("Role (Buyer/Seller/Admin): ");
        string role = Console.ReadLine();

        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("SP_RegisterUser", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Username", username);
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@Password", password);
        cmd.Parameters.AddWithValue("@Phone", phone);
        cmd.Parameters.AddWithValue("@Address", address);
        cmd.Parameters.AddWithValue("@Role", role);

        con.Open();
        try
        {
            int userId = Convert.ToInt32(cmd.ExecuteScalar());
            SqlCommand addrCmd = new SqlCommand(@"INSERT INTO Addresses (UserId, AddressName, Address1, City, Pincode, IsDefault) 
            VALUES (@UserId, 'Home', @Address, 'Unknown', '000000', 1)", con);
            addrCmd.Parameters.AddWithValue("@UserId", userId);
            addrCmd.Parameters.AddWithValue("@Address", address);
            addrCmd.ExecuteNonQuery();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Registration successful! Please login.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Oops! An Error occurred: " + ex.Message);
            Console.ResetColor();
        }
    }

    static void Login()
    {
        Console.Write("\nEmail: ");
        string email = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("SP_LoginUser", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@Password", password);

        con.Open();
        try
        {
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                loggedInUserId = (int)reader["Id"];

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Welcome {reader["Name"]}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Invalid credentials");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Login failed: " + ex.Message);
            Console.ResetColor();
        }
    }

    static void Logout()
    {
        loggedInUserId = 0;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("👋 Logged out successfully!");
        Console.ResetColor();
    }

    static void ViewProducts()
    {
        using SqlConnection con = new SqlConnection(connectionString);
        string query = "SELECT Id, Name, Price, Stock FROM Products WHERE IsActive = 1";
        SqlCommand cmd = new SqlCommand(query, con);
        con.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Product List ---");
        while (reader.Read())
        {
            Console.WriteLine($"ID: {reader["Id"]} | {reader["Name"]} | ₹{reader["Price"]} | Stock: {reader["Stock"]}");
        }

        con.Close();
    }

    static void AddToCart()
    {
        Console.Write("\nEnter Product ID: ");
        int productId = int.Parse(Console.ReadLine());
        Console.Write("Enter Quantity: ");
        int quantity = int.Parse(Console.ReadLine());

        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("SP_AddToCart", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserId", loggedInUserId);
        cmd.Parameters.AddWithValue("@ProductId", productId);
        cmd.Parameters.AddWithValue("@Quantity", quantity);

        con.Open();
        cmd.ExecuteNonQuery();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ Product added to cart!");
        Console.ResetColor();
    }

    static void ViewCart()
    {
        using SqlConnection con = new SqlConnection(connectionString);
        string query = @"
            SELECT P.Name, C.Quantity, P.Price, (C.Quantity * P.Price) AS Total
            FROM ShoppingCart C
            JOIN Products P ON C.ProductId = P.Id
            WHERE C.UserId = @UserId";

        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@UserId", loggedInUserId);

        con.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Your Cart ---");
        while (reader.Read())
        {
            Console.WriteLine($"{reader["Name"]} | Qty: {reader["Quantity"]} | ₹{reader["Price"]} | Total: ₹{reader["Total"]}");
        }

        con.Close();
    }

    static void PlaceOrder()
    {
        Console.Write("\n1. Use Existing Address (or) 2. Add New Address ");
        Console.Write("Enter your choice: ");
        int choice = int.Parse(Console.ReadLine());

        switch(choice)
        {
            case 1:
                ShowUserAddresses();
                break;
            case 2:
                AddNewAddress();
                ShowUserAddresses();
                break;
            default:
                Console.WriteLine("Invalid choice! Please try again.");
                return;
        }
        Console.Write("Enter Address ID to use for this order: ");
        int addressId = int.Parse(Console.ReadLine());

        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("SP_PlaceOrder", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UserId", loggedInUserId);
        cmd.Parameters.AddWithValue("@AddressId", addressId);

        con.Open();

        try
        {
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("🎉 Order placed successfully!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Oops! An Error occurred: " + ex.Message);
            Console.ResetColor();
        }
    }

    static void ShowUserAddresses()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        string query = @"
        SELECT Id, AddressName, Address1, City, Pincode
        FROM Addresses
        WHERE UserId = @UserId";

        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@UserId", loggedInUserId);

        con.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Your Addresses ---");

        while (reader.Read())
        {
            Console.WriteLine(
                $"ID: {reader["Id"]} | {reader["AddressName"]} | {reader["Address1"]}, {reader["City"]} - {reader["Pincode"]}"
            );
        }

        con.Close();
    }

    static void AddNewAddress()
    {
        Console.Write("\nAddress Name (e.g. Home, Work): ");
        string addressName = Console.ReadLine();
        Console.Write("Address Line 1: ");
        string address1 = Console.ReadLine();
        Console.Write("City: ");
        string city = Console.ReadLine();
        Console.Write("Pincode: ");
        string pincode = Console.ReadLine();
        Console.Write("Set as default? (Y/N): ");
        bool isDefault = char.ToUpper(Console.ReadKey().KeyChar) == 'Y';
        Console.WriteLine();

        using SqlConnection con = new SqlConnection(connectionString);
        con.Open();

        if (isDefault)
        {
            SqlCommand resetCmd = new SqlCommand(
                "UPDATE Addresses SET IsDefault = 0 WHERE UserId = @UserId", con);

            resetCmd.Parameters.AddWithValue("@UserId", loggedInUserId);
            resetCmd.ExecuteNonQuery();
        }

        string query = @"
        INSERT INTO Addresses (UserId, AddressName, Address1, City, Pincode, IsDefault)
        VALUES (@UserId, @AddressName, @Address1, @City, @Pincode, @IsDefault)";

        SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.AddWithValue("@UserId", loggedInUserId);
        cmd.Parameters.AddWithValue("@AddressName", addressName);
        cmd.Parameters.AddWithValue("@Address1", address1);
        cmd.Parameters.AddWithValue("@City", city);
        cmd.Parameters.AddWithValue("@Pincode", pincode);
        cmd.Parameters.AddWithValue("@IsDefault", isDefault);
        cmd.ExecuteNonQuery();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ Address added successfully!");
        Console.ResetColor();
    }
}