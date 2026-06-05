from modules.products import (view_products, search_products, view_product_details)
from modules.cart import (view_cart)
from modules.wishlist import (view_wishlist)
from modules.orders import (view_orders)
from modules.auth import (logout, view_profile)

def buyer_dashboard(user):
    while True:
        print("\n===== BUYER =====")
        print("1. View Products")
        print("2. Search Products")
        print("3. View Product Details")
        print("4. My Cart")
        print("5. My Wishlist")
        print("6. My Orders")
        print("7. My Profile")
        print("8. Logout")
        choice = input("Enter Choice: ")
        if choice == "1":
            view_products()
        elif choice == "2":
            search_products()
        elif choice == "3":
            view_product_details(user)
        elif choice == "4":
            view_cart(user)
        elif choice == "5":
            view_wishlist(user)
        elif choice == "6":
            view_orders(user)
        elif choice == "7":
            view_profile(user)
        elif choice == "8":
            logout()
            break
        else:
            print("Invalid Choice")