from modules.products import (add_product, view_seller_products, update_product, delete_product)
from modules.orders import (view_seller_orders, update_order_status)
from modules.auth import (logout, view_seller_profile)

def seller_dashboard(user):
    while True:
        print("\n===== SELLER =====")
        print("1. Add Product")
        print("2. View My Products")
        print("3. Update Product")
        print("4. Delete Product")
        print("5. View Orders")
        print("6. Update Order Status")
        print("7. My Profile")
        print("8. Logout")
        choice = input("Enter Choice: ")
        if choice == "1":
            add_product(user)
        elif choice == "2":
            view_seller_products(user)
        elif choice == "3":
            update_product(user)
        elif choice == "4":
            delete_product(user)
        elif choice == "5":
            view_seller_orders(user)
        elif choice == "6":
            update_order_status(user)
        elif choice == "7":
            view_seller_profile(user)
        elif choice == "8":
            logout()
            break
        else:
            print("Invalid Choice")