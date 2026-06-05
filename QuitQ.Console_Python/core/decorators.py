def login_required(func):
    def wrapper(*args, **kwargs):
        user = kwargs.get("user")
        if not user:
            print("Login Required")
            return
        return func(*args, **kwargs)
    return wrapper