from tabulate import tabulate
from core.db import DatabaseConnection
from core.utils import pause
from modules.cart import (add_to_cart)
from modules.wishlist import (add_to_wishlist)
from modules.orders import (buy_now)
import webbrowser

def view_products():
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id, Name, Price, Stock, ImageUrl FROM Products"""
        cursor.execute(query)
        rows = cursor.fetchall()
        table = []
        for row in rows:
            table.append([row.Id, row.Name, row.Price, row.Stock, "View Image"
            ])
        print(tabulate(table,
            headers=["ID", "Name", "Price", "Stock", "Image"],
            tablefmt="grid"))
        choice = input("\nEnter Product ID to View Image (or press Enter to skip): ").strip()
        if choice:
            product = next(
                (row for row in rows if str(row.Id) == choice),
                None
            )
            if product:
                webbrowser.open(product.ImageUrl)
            else:
                print("Product Not Found")
        pause()

def search_products():
    keyword = input("Enter Product Name To Search: ").strip()
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id, Name, Price, Stock, ImageUrl FROM Products WHERE Name LIKE ?"""
        cursor.execute(query, (f"%{keyword}%",))
        rows = cursor.fetchall()
        if not rows:
            print("No Products Found")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.Name, row.Price, row.Stock, "View Image"])
        print(tabulate(table,
            headers=["ID", "Name", "Price", "Stock", "Image"],
            tablefmt="grid"))
        choice = input("\nEnter Product ID to View Image (or press Enter to skip): ").strip()
        if choice:
            product = next((row for row in rows if str(row.Id) == choice), None)
            if product: 
                webbrowser.open(product.ImageUrl)
            else:
                print("Product Not Found")
        pause()

def view_product_details(user):
    product_id = input("Enter Product ID To View Details: ")
    if not product_id.isdigit():
        print("Invalid Product ID")
        pause()
        return
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id, Name, Description, Price, Stock, ImageUrl FROM Products WHERE Id = ?"""
        cursor.execute(query,(product_id,))
        row = cursor.fetchone()
        if not row:
            print("Product Not Found")
            pause()
            return
        print(f"\nID: {row.Id}")
        print(f"Name: {row.Name}")
        print(f"Description: {row.Description}")
        print(f"Price: {row.Price}")
        print(f"Stock: {row.Stock}")
        view_image = input("\nOpen Product Image? (Y/N): ").upper()
        if view_image == "Y":
            webbrowser.open(row.ImageUrl)
        print("\n1. Add To Cart")
        print("2. Add To Wishlist")
        print("3. Buy Now")
        print("4. Back")
        choice = input("Choice: ")
        if choice == "1":
            add_to_cart(user, product_id)
        elif choice == "2":
            add_to_wishlist(user, product_id)
        elif choice == "3":
            buy_now(user, product_id)
        elif choice == "4":
            return

def add_product(user):
    name = input("Enter Product Name: ")
    description = input("Enter Product Description: ")
    price = input("Enter Product Price: ")
    stock = input("Enter Product Stock: ")
    image_url = input("Enter Product Image URL: ")
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id, Name FROM Categories"""
        cursor.execute(query)
        categories = cursor.fetchall()
        print("Available Categories:")
        for cat in categories:
            print(f"  {cat.Id}. {cat.Name}")
        category_id = input("Enter Category ID: ")
        query = """SELECT Id FROM Sellers WHERE UserId = ?"""
        cursor.execute(query, (user.id,))
        seller_id = cursor.fetchone()[0]
        if not seller_id:
            print("Seller Not Found")
            pause()
            return
        query = """SELECT Id, Name FROM Brands"""
        cursor.execute(query)
        brands = cursor.fetchall()
        print("Available Brands:")
        for brand in brands:
            print(f"  {brand.Id}. {brand.Name}")
        brand_id = input("Enter Brand ID: ")
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """INSERT INTO Products (Name, Description, Price, Stock, ImageUrl, CategoryId, SellerId, BrandId)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)"""
        cursor.execute(query,(name, description, price, stock, image_url, category_id, seller_id, brand_id))
        conn.commit()
        print("Product Added Successfully")
        pause()

def view_seller_products(user):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id FROM Sellers
        WHERE UserId = ?"""
        cursor.execute(query, (user.id,))
        seller_id = cursor.fetchone()[0]
        query = """SELECT Id, Name, Description, Price, Stock FROM Products WHERE SellerId = ?"""
        cursor.execute(query,(seller_id,))
        rows = cursor.fetchall()
        if not rows:
            print("No Products Found")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.Name, row.Description, row.Price, row.Stock])
        print(tabulate(table,
            headers=["ID", "Name", "Description", "Price", "Stock"],
            tablefmt="grid"))
        pause()

def update_product(user):
    product_id = input("Enter Product ID To Update: ")
    if not product_id.isdigit():
        print("Invalid Product ID")
        pause()
        return
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id FROM Sellers WHERE UserId = ?"""
        cursor.execute(query, (user.id,))
        seller_id = cursor.fetchone()[0]
        query = """SELECT * FROM Products
        WHERE Id = ? AND SellerId = ?"""
        cursor.execute(query,(product_id, seller_id))
        row = cursor.fetchone()
        if not row:
            print("Product Not Found")
            pause()
            return
        name = input(f"Enter Product Name ({row.Name}): ") or row.Name
        description = input(f"Enter Product Description ({row.Description}): ") or row.Description
        price = input(f"Enter Product Price ({row.Price}): ") or row.Price
        stock = input(f"Enter Product Stock ({row.Stock}): ") or row.Stock
        query = """UPDATE Products
        SET Name = ?, Description = ?, Price = ?, Stock = ?
        WHERE Id = ?"""
        cursor.execute(query,(name, description, price, stock, product_id))
        conn.commit()
        print("Product Updated Successfully")
        pause()

def delete_product(user):
    product_id = input("Enter Product ID To Delete: ")
    if not product_id.isdigit():
        print("Invalid Product ID")
        pause()
        return
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id FROM Sellers WHERE UserId = ?"""
        cursor.execute(query, (user.id,))
        seller_id = cursor.fetchone()[0]
        query = """DELETE FROM Products
        WHERE Id = ? AND SellerId = ?"""
        cursor.execute(query,(product_id, seller_id))
        if cursor.rowcount == 0:
            print("Product Not Found")
            pause()
            return
        conn.commit()
        print("Product Deleted Successfully")
        pause()

