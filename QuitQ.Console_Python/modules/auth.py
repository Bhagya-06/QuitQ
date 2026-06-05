from core.validators import *
from core.exceptions import *
from core.utils import pause
from core.db import DatabaseConnection
from models.user import User
import hashlib

def login():
    try:
        print("\n===== LOGIN =====")
        email = input("Enter Email: ").strip().lower()
        password = input("Enter Password: ").strip()
        with DatabaseConnection() as conn:
            cursor = conn.cursor()
            query = """
            SELECT Id, Username, Role FROM Users
            WHERE Email = ? AND PasswordHash = ?"""
            cursor.execute(query,(email, password))
            user = cursor.fetchone()
            if not user:
                raise InvalidCredentialsError("Invalid Email Or Password")

            user = User(user.Id, user.Username, user.Role)
            print(f"\nWelcome {user.username}")
            pause()
        return user

    except Exception as e:
        print(e)
        pause()
        return None

def register():

    print("\n===== REGISTER =====")
    name = input("Name: ").strip()
    username = input("Username: ").strip()
    email = input("Email: ").strip().lower()
    password = input("Password: ").strip()
    phone = input("Phone: ").strip()
    role = input("Role (Buyer/Seller): ").strip().capitalize()
    address = input("Address: ").strip()
    password_hash = hashlib.sha256(password.encode()).hexdigest()
    try:
        if not validate_email(email):
            raise InvalidEmailError("Invalid Email Format")
        if not validate_password(password):
            raise InvalidPasswordError("Password must be at least 6 characters long")
        if not validate_phone(phone):
            raise InvalidPhoneError("Invalid Phone Number Format. It should be 10 digits")
        if not validate_role(role):
            raise InvalidRoleError("Invalid Role. Role must be Buyer or Seller")  

        with DatabaseConnection() as conn:
            cursor = conn.cursor()
            query = """SELECT * FROM Users
            WHERE Username = ?"""
            cursor.execute(query,(username,))
            if cursor.fetchone():
                raise InvalidUsernameError("Username Already Taken")
            query = """
            SELECT * FROM Users
            WHERE Email = ?"""
            cursor.execute(query,(email,))
            result = cursor.fetchone()
            if result:
                raise InvalidEmailError("Email Already Registered")

            query = """INSERT INTO Users (Name, Username, Email, PasswordHash, Phone, Role, Address, IsActive, CreatedDate)
            VALUES (?, ?, ?, ?, ?, ?, ?, 1, CURRENT_TIMESTAMP)"""
            cursor.execute(query,(name, username, email, password_hash, phone, role, address))
            conn.commit()
            print("Registration Successful")
        
        if role == "Seller":
            with DatabaseConnection() as conn:
                cursor = conn.cursor()
                query = """
                SELECT Id FROM Users
                WHERE Email = ?"""
                cursor.execute(query,(email,))
                user_id = cursor.fetchone().Id
                store_name = input("Enter Store Name: ").strip()
                gstin = input("Enter GSTIN: ").strip()
                city = input("Enter City: ").strip()
                query = """INSERT INTO Sellers (UserId, StoreName, GSTIN, City, VerificationStatus)
                VALUES (?, ?, ?, ?, 'Pending')"""
                cursor.execute(query,(user_id, store_name, gstin, city))
                conn.commit()
                print("Seller Profile Created. Awaiting Verification.")
    except Exception as e:
        print(e)
    finally:
        pause()

def logout():
    print(("Logging Out... See You Soon :)").center(50,"-"))
    pause()

def view_profile(user):
    print("\n===== PROFILE =====")
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT * FROM Users WHERE Id = ?"""
        cursor.execute(query, (user.id,))
        result = cursor.fetchone()
        if result:
            print(f"Name: {result.Name}")
            print(f"Username: {result.Username}")
            print(f"Role: {result.Role}")
            print(f"Email: {result.Email}")
            print(f"Phone: {result.Phone}")
            print(f"Address: {result.Address}")
            print(f"Is Active: {'Yes' if result.IsActive else 'No'}")
            print(f"Created Date: {result.CreatedDate}")
        else:
            print("User Not Found")
        pause()

def view_seller_profile(user):
    view_profile(user)
    with DatabaseConnection() as conn:
        cursor = conn.cursor()
        query = """SELECT * FROM Sellers WHERE UserId = ?"""
        cursor.execute(query, (user.id,))
        seller_result = cursor.fetchone()
        if seller_result:
            print(f"Store Name: {seller_result.StoreName}")
            print(f"GST Number: {seller_result.GSTIN}")
            print(f"Verification Status: {seller_result.VerificationStatus}")
            print(f"City: {seller_result.City}")
        else:
            print("Seller Not Found")
        pause()
        