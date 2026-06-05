from tabulate import tabulate
from core.db import DatabaseConnection
from core.utils import pause

def view_users():
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id, Username, Email, Phone, Role, IsActive, CreatedDate FROM Users"""
        cursor.execute(query)
        rows = cursor.fetchall()
        if not rows:
            print("No Users Found")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.Username, row.Email, row.Phone, row.Role, "Active" if row.IsActive else "Inactive", row.CreatedDate])
        print(tabulate(table,
            headers=["ID", "Username", "Email", "Phone", "Role", "Status", "Created Date"],
            tablefmt="grid"))
        pause()

def view_sellers():
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT s.Id, u.Username, s.StoreName, s.GSTIN, s.City, s.VerificationStatus
        FROM Sellers s
        JOIN Users u ON s.UserId = u.Id"""
        cursor.execute(query)
        rows = cursor.fetchall()
        if not rows:
            print("No Sellers Found")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.Username, row.StoreName, row.GSTIN, row.City, row.VerificationStatus])
        print(tabulate(table,
            headers=["Seller ID", "Username", "Store Name", "GSTIN", "City", "Verification Status"],
            tablefmt="grid"))
        pause()

def verify_sellers():
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT s.Id, u.Username, s.StoreName, s.GSTIN, s.City, s.VerificationStatus
        FROM Sellers s
        JOIN Users u ON s.UserId = u.Id
        WHERE s.VerificationStatus = 'Pending'"""
        cursor.execute(query)
        rows = cursor.fetchall()
        if not rows:
            print("No Sellers Pending Verification")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.Username, row.StoreName, row.GSTIN, row.City, row.VerificationStatus])
        print(tabulate(table,
            headers=["Seller ID", "Username", "Store Name", "GSTIN", "City", "Verification Status"],
            tablefmt="grid"))
        seller_id = input("Enter Seller ID To Verify: ")
        if not seller_id.isdigit():
            print("Invalid Seller ID")
            pause()
            return
        cursor.execute("SELECT * FROM Sellers WHERE Id = ?", (seller_id,))
        seller = cursor.fetchone()
        if not seller:
            print("Seller Not Found")
            pause()
            return
        if seller.VerificationStatus != "Pending":
            print("Seller Already Verified/Rejected")
            pause()
            return
        decision = input("Enter 'A' to Approve or 'R' to Reject: ").upper()
        if decision == "A":
            cursor.execute("UPDATE Sellers SET VerificationStatus = 'Approved' WHERE Id = ?", (seller_id,))
            conn.commit()
            print("Seller Approved Successfully")
        elif decision == "R":
            cursor.execute("UPDATE Sellers SET VerificationStatus = 'Rejected' WHERE Id = ?", (seller_id,))
            conn.commit()
            print("Seller Rejected Successfully")
        else:
            print("Invalid Choice")
        pause()

def add_category_brand():
    print("\n===== ADD CATEGORY/BRAND =====")
    choice = input("Enter 'C' to Add Category or 'B' to Add Brand: ").upper()
    if choice == "C":
        name = input("Enter Category Name: ")
        with DatabaseConnection() as conn:
            cursor = conn.cursor()
            query = "SELECT * FROM Categories WHERE Name = ?"
            cursor.execute(query, (name,))
            if cursor.fetchone():
                print("Category Already Exists")
                pause()
                return
            query = "INSERT INTO Categories (Name) VALUES (?)"
            cursor.execute(query, (name,))
            conn.commit()
            print("Category Added Successfully")
    elif choice == "B":
        name = input("Enter Brand Name: ")
        with DatabaseConnection() as conn:
            cursor = conn.cursor()
            query = "SELECT * FROM Brands WHERE Name = ?"
            cursor.execute(query, (name,))
            if cursor.fetchone():
                print("Brand Already Exists")
                pause()
                return
            query = "INSERT INTO Brands (Name) VALUES (?)"
            cursor.execute(query, (name,))
            conn.commit()
            print("Brand Added Successfully")
    else:
        print("Invalid Choice")
    pause()

def remove_category_brand():
    print("\n===== REMOVE CATEGORY/BRAND =====")
    choice = input("Enter 'C' to Remove Category or 'B' to Remove Brand: ").upper()
    if choice == "C":
        name = input("Enter Category Name To Remove: ")
        with DatabaseConnection() as conn:
            cursor = conn.cursor()
            query = "SELECT * FROM Categories WHERE Name = ?"
            cursor.execute(query, (name,))
            if not cursor.fetchone():
                print("Category Not Found")
                pause()
                return
            query = "DELETE FROM Categories WHERE Name = ?"
            cursor.execute(query, (name,))
            conn.commit()
            print("Category Removed Successfully")
    elif choice == "B":
        name = input("Enter Brand Name To Remove: ")
        with DatabaseConnection() as conn:
            cursor = conn.cursor()
            query = "SELECT * FROM Brands WHERE Name = ?"
            cursor.execute(query, (name,))
            if not cursor.fetchone():
                print("Brand Not Found")
                pause()
                return
            query = "DELETE FROM Brands WHERE Name = ?"
            cursor.execute(query, (name,))
            conn.commit()
            print("Brand Removed Successfully")
    else:
        print("Invalid Choice")
    pause()

def view_analytics(user):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        cursor.execute("SELECT COUNT(*) FROM Users")
        total_users = cursor.fetchone()[0]
        cursor.execute("SELECT COUNT(*) FROM Users WHERE Role='Buyer'")
        buyers = cursor.fetchone()[0]
        cursor.execute("SELECT COUNT(*) FROM Users WHERE Role='Seller'")
        sellers = cursor.fetchone()[0]
        cursor.execute("SELECT COUNT(*) FROM Products")
        products = cursor.fetchone()[0]
        cursor.execute("SELECT COUNT(*) FROM Orders")
        orders = cursor.fetchone()[0]
        cursor.execute("SELECT ISNULL(SUM(Total),0) FROM Orders")
        revenue = cursor.fetchone()[0]
        cursor.execute("SELECT TOP 5 p.Name, SUM(oi.Quantity) AS Sold FROM OrderItems oi JOIN Products p ON oi.ProductId = p.Id GROUP BY p.Name ORDER BY Sold DESC")
        top_products = cursor.fetchall()
        cursor.execute("SELECT TOP 5 s.StoreName, SUM(oi.Quantity * oi.Price) AS Sales FROM OrderItems oi JOIN Products p ON oi.ProductId = p.Id JOIN Sellers s ON p.SellerId = s.Id GROUP BY s.StoreName ORDER BY Sales DESC")
        top_sellers = cursor.fetchall()
        table = [
            ["Total Users", total_users],
            ["Total Buyers", buyers],
            ["Total Sellers", sellers],
            ["Total Products", products],
            ["Total Orders", orders],
            ["Total Revenue", revenue]
        ]
        print("\n===== ANALYTICS DASHBOARD =====\n")
        print(tabulate(table,
            headers=["Metric", "Value"],
            tablefmt="grid"))
        print("\n===== TOP PRODUCTS =====")
        print(tabulate(top_products,
            headers=["Product Name", "Quantity Sold"],
            tablefmt="grid"))
        print("\n===== TOP SELLERS =====")
        print(tabulate(top_sellers,
            headers=["Store Name", "Sales"],
            tablefmt="grid"))
        pause()