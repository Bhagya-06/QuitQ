from tabulate import tabulate
from core.db import DatabaseConnection
from core.utils import pause
from modules.cart import add_to_cart
from modules.orders import buy_now

def view_wishlist(user):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT w.Id,p.Id AS ProductId, p.Name, p.Price
        FROM Wishlist w
        JOIN Products p ON w.ProductId = p.Id
        WHERE w.UserId = ?"""
        cursor.execute(query,(user.id,))
        rows = cursor.fetchall()
        if not rows:
            print("Your Wishlist is Empty")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.ProductId, row.Name, row.Price])
        print(tabulate(table,
            headers=["ID", "Product ID", "Product Name", "Price"],
            tablefmt="grid"))
        
        print("\n1. Add To Cart")
        print("2. Buy Now")
        print("3. Remove From Wishlist")
        print("4. Back")
        choice = input("Choice: ")
        if choice == "1":
            product_id = input("Enter Product ID To Add To Cart: ")
            if not product_id.isdigit():
                print("Invalid Product ID")
                pause()
                return
            add_to_cart(user, product_id)
        elif choice == "2":
            product_id = input("Enter Product ID To Buy Now: ")
            if not product_id.isdigit():
                print("Invalid Product ID")
                pause()
                return
            buy_now(user, product_id)
        elif choice == "3":
            product_id = input("Enter Product ID To Remove From Wishlist: ")
            if not product_id.isdigit():
                print("Invalid Product ID")
                pause()
                return
            remove_from_wishlist(user, product_id)
            print("Product Removed From Wishlist")
        elif choice == "4":
            return

def add_to_wishlist(user, product_id):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT * FROM Products WHERE Id = ?"""
        cursor.execute(query,(product_id,))
        row = cursor.fetchone()
        if not row:
            print("Product Not Found")
            pause()
            return
        query = """SELECT * FROM Wishlist
        WHERE UserId = ? AND ProductId = ?"""
        cursor.execute(query,(user.id, product_id))
        if cursor.fetchone():
            print("Product Already In Wishlist")
            pause()
            return
        query = """INSERT INTO Wishlist (UserId, ProductId)
        VALUES (?, ?)"""
        cursor.execute(query,(user.id, product_id))
        conn.commit()
        print("Product Added To Wishlist")
        pause()

def remove_from_wishlist(user, product_id):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """DELETE FROM Wishlist
        WHERE UserId = ? AND ProductId = ?"""
        cursor.execute(query,(user.id, product_id))
        conn.commit()
        print("Product Removed From Wishlist")
        pause()