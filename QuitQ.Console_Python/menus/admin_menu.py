from modules.admin import ( view_users, view_sellers, verify_sellers, view_analytics, add_category_brand, remove_category_brand)
from modules.products import (view_products)
from modules.auth import (logout, view_profile)


def admin_dashboard(user):
    while True:
        print("\n===== ADMIN =====")
        print("1. View Users")
        print("2. View Sellers")
        print("3. Verify Sellers")
        print("4. View Products")
        print("5. View Analytics")
        print("6. Add Category/Brand")
        print("7. Remove Category/Brand")
        print("8. My Profile")
        print("9. Logout")
        choice = input("Enter Choice: ")
        if choice == "1":
            view_users()
        elif choice == "2":
            view_sellers()
        elif choice == "3":
            verify_sellers()
        elif choice == "4":
            view_products()
        elif choice == "5":
            view_analytics(user)
        elif choice == "6":
            add_category_brand()
        elif choice == "7":
            remove_category_brand()
        elif choice == "8":
            view_profile(user)
        elif choice == "9":
            logout()
            break
        else:
            print("Invalid Choice")