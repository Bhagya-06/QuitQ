from tabulate import tabulate
from core.db import DatabaseConnection
from core.utils import pause

def view_orders(user):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id, Total, Status, CreatedDate FROM Orders WHERE UserId = ?"""
        cursor.execute(query,(user.id,))
        rows = cursor.fetchall()
        if not rows:
            print("You have no orders")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.Total, row.Status, row.CreatedDate])
        print(tabulate(table,
            headers=["ID", "Total", "Status", "Created Date"],
            tablefmt="grid"))
        pause()

def view_order_details(user):
    order_id = input("Enter Order ID To View Details: ")
    if not order_id.isdigit():
        print("Invalid Order ID")
        pause()
        return
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT o.Id, p.Name, oi.Quantity, p.Price, (oi.Quantity * p.Price) AS Total
        FROM OrderItems oi
        JOIN Products p ON oi.ProductId = p.Id
        JOIN Orders o ON oi.OrderId = o.Id
        WHERE o.UserId = ?
        AND o.Id = ?"""
        cursor.execute(query,(user.id, order_id))
        rows = cursor.fetchall()
        if not rows:
            print("Order Not Found")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.Name, row.Quantity, row.Price, row.Total])
        print(tabulate(table,
            headers=["Order ID", "Product Name", "Quantity", "Price", "Total"],
            tablefmt="grid"))
        pause()

def buy_now(user, product_id):
    quantity = input("Enter Quantity: ")
    if not quantity.isdigit() or int(quantity) <= 0:
        print("Invalid Quantity")
        pause()
        return
    place_order(user,[{"product_id": product_id, "quantity": int(quantity)}])

def place_order(user, items):  
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        total = 0
        for item in items:
            query = """SELECT Price,Stock FROM Products WHERE Id = ?"""
            cursor.execute(query,(item["product_id"],))
            row = cursor.fetchone()
            if not row:
                print(f"Product ID {item['product_id']} Not Found")
                pause()
                return False
            if row.Stock < item["quantity"]:
                print(f"Insufficient Stock for Product ID {item['product_id']}")
                pause()
                return False
            total += row.Price * item["quantity"]
        query = """INSERT INTO Orders (UserId, Total, Status, AddressId)
        OUTPUT INSERTED.Id
        VALUES (?, ?, ?, ?)"""
        cursor.execute(query,(user.id, total, "Pending", 1))
        order_id = cursor.fetchone()[0]
        print(f"Order ID: {order_id}, Total: {total}")
        for item in items:
            query = """SELECT Price FROM Products
            WHERE Id = ?"""
            cursor.execute(query,(item["product_id"],))
            product= cursor.fetchone()
            query = """INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
            VALUES (?, ?, ?, ?)"""
            cursor.execute(query,(order_id, item["product_id"], item["quantity"], product.Price))
            query = """UPDATE Products SET Stock = Stock - ?
            WHERE Id = ?"""
            cursor.execute(query,(item["quantity"], item["product_id"]))
        conn.commit()
        print("Order Placed Successfully")
        pause()
        return True
    
def view_seller_orders(user):
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id FROM Sellers WHERE UserId = ?"""
        cursor.execute(query, (user.id,))
        seller_id = cursor.fetchone()[0]
        query = """SELECT o.Id, p.Name, oi.Quantity, oi.Price, oi.Quantity * oi.Price AS Total, o.Status
        FROM Orders o
        JOIN OrderItems oi ON o.Id = oi.OrderId
        JOIN Products p ON oi.ProductId = p.Id
        WHERE p.SellerId = ?"""
        cursor.execute(query,(seller_id,))
        rows = cursor.fetchall()
        if not rows:
            print("No Orders Found")
            pause()
            return
        table = []
        for row in rows:
            table.append([row.Id, row.Name, row.Quantity, row.Price, row.Total, row.Status])
        print(tabulate(table,
            headers=["Order ID", "Product Name", "Quantity", "Price", "Total", "Status"],
            tablefmt="grid"))
        pause()

def update_order_status(user):
    order_id = input("Enter Order ID To Update Status: ")
    if not order_id.isdigit():
        print("Invalid Order ID")
        pause()
        return
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT Id FROM Sellers WHERE UserId = ?"""
        cursor.execute(query, (user.id,))
        seller_id = cursor.fetchone()[0]
        query = """SELECT o.Id, o.Status
        FROM Orders o
        JOIN OrderItems oi ON o.Id = oi.OrderId
        JOIN Products p ON oi.ProductId = p.Id
        WHERE p.SellerId = ?
        AND o.Id = ?"""
        cursor.execute(query,(seller_id, order_id))
        row = cursor.fetchone()
        if not row:
            print("Order Not Found")
            pause()
            return
        print(f"Current Status: {row.Status}")
        new_status = input("Enter New Status (Pending/Shipped/Delivered): ")
        if new_status not in ["Pending", "Shipped", "Delivered"]:
            print("Invalid Status")
            pause()
            return
        query = """UPDATE Orders SET Status = ? WHERE Id = ?"""
        cursor.execute(query,(new_status, order_id))
        conn.commit()
        print("Order Status Updated Successfully")
        pause()