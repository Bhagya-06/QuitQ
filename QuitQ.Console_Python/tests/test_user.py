import unittest
from models.user import User

class TestUser(unittest.TestCase):

    def test_user_creation(self):

        user = User(
            1,
            "Bhagya",
            "Buyer",
        )

        self.assertEqual(user.id, 1)
        self.assertEqual(user.username, "Bhagya")
        self.assertEqual(user.role, "Buyer")

if __name__ == "__main__":
    unittest.main()