from modules.auth import login, register

def start_application():
    while True:
        print("\n===== QUITQ =====")
        print("1. Login")
        print("2. Register")
        print("3. Exit")
        choice = input("Enter Choice: ")
        if choice == "1":
            user = login()
            if user:
                if user.role == "Buyer":
                    from menus.buyer_menu import (buyer_dashboard)
                    buyer_dashboard(user)
                elif user.role == "Seller":
                    from menus.seller_menu import (seller_dashboard)
                    seller_dashboard(user)
                elif user.role == "Admin":
                    from menus.admin_menu import (admin_dashboard)
                    admin_dashboard(user)
        elif choice == "2":
            register()
        elif choice == "3":
            print(("Thank You of Visiting QuitQ :)").center(50,"-"))
            break
        else:
            print("Invalid Choice") 