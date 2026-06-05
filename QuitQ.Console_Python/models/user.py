class User:

    def __init__(self,id,username,role):
        self.id = id
        self.username = username
        self.role = role
        
    def __str__(self):
        return (
            f"User(Id={self.id}, "
            f"Username='{self.username}', "
            f"Role='{self.role}')"
            )