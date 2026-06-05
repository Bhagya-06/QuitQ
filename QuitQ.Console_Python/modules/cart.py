from tabulate import tabulate
from core.db import DatabaseConnection
from core.utils import pause
from modules.orders import (place_order)

def view_cart(user):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT c.Id,c.ProductId, p.Name, c.Quantity, p.Price, (c.Quantity * p.Price) AS Total
        FROM ShoppingCart c
        JOIN Products p ON c.ProductId = p.Id
        WHERE c.UserId = ?"""
        cursor.execute(query,(user.id,))
        rows = cursor.fetchall()
        if not rows:
            print("Your Cart is Empty")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.ProductId, row.Name, row.Quantity, row.Price, row.Total])
        print(tabulate(table,
            headers=["ID", "Product ID", "Product Name", "Quantity", "Price", "Total"],
            tablefmt="grid"))
        total = sum(row.Total for row in rows)
        print(f"Total: {total}")

        print("\n1. Place Order")
        print("2. Remove Item")
        print("3. Clear Cart")
        print("4. Back")
        choice = input("Choice: ")
        if choice == "1":
            items = []
            for row in rows:
                items.append({"product_id": row.ProductId, "quantity": row.Quantity})
            success = place_order(user, items)
            if success:
                query = """DELETE FROM ShoppingCart WHERE UserId = ?"""
                cursor.execute(query,(user.id,))
                conn.commit()
        elif choice == "2":
            remove_cart_item(user)
        elif choice == "3":
            clear_cart(user)
        elif choice == "4":            
            return

def add_to_cart(user, product_id):
    quantity = input("Enter Quantity: ")
    if not quantity.isdigit() or int(quantity) <= 0:
        print("Invalid Quantity")
        pause()
        return
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Stock FROM Products WHERE Id = ?"""
        cursor.execute(query,(product_id,))
        row = cursor.fetchone()
        if not row:
            print("Product Not Found")
            pause()
            return
        if row.Stock < int(quantity):
            print("Insufficient Stock Available")
            pause()
            return
        query = """SELECT * FROM ShoppingCart
        WHERE UserId = ? AND ProductId = ?"""
        cursor.execute(query,(user.id, product_id))
        if cursor.fetchone():
            query = """UPDATE ShoppingCart
            SET Quantity = Quantity + ?
            WHERE UserId = ? AND ProductId = ?"""
            cursor.execute(query,(int(quantity), user.id, product_id))
            conn.commit()
            print("Product Quantity Updated In Cart")
            pause()
            return
        query = """INSERT INTO ShoppingCart (UserId, ProductId, Quantity)
        VALUES (?, ?, ?)"""
        cursor.execute(query,(user.id, product_id, quantity))
        conn.commit()
        print("Product Added To Cart")
        pause()

def remove_cart_item(user):
    cart_id = input("Enter Cart Item ID To Remove: ")
    if not cart_id.isdigit():
        print("Invalid Cart Item ID")
        pause()
        return
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """DELETE FROM ShoppingCart WHERE Id = ? AND UserId = ?"""
        cursor.execute(query,(cart_id, user.id))
        if cursor.rowcount == 0:
            print("Cart Item Not Found")
            pause()
            return
        conn.commit()
        print("Cart Item Removed")
        pause()

def clear_cart(user):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """DELETE FROM ShoppingCart WHERE UserId = ?"""
        cursor.execute(query,(user.id,))
        conn.commit()
        print("Cart Cleared")
        pause()