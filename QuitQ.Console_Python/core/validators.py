import re

def validate_email(email):
    pattern = r'^[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]{2,}$'
    return re.fullmatch(pattern, email)

def validate_phone(phone):
    return re.fullmatch(r'^\d{10}$', phone)

def validate_password(password):
    return len(password) >= 6

def validate_role(role):
    return role in ["Buyer", "Seller"]