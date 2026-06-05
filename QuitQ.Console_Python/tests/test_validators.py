import unittest
from core.validators import *

class TestValidators(unittest.TestCase):

    def test_valid_email(self):
        self.assertTrue(
            validate_email("test@gmail.com")
        )

    def test_invalid_email(self):
        self.assertFalse(
            validate_email("testgmail.com")
        )

    def test_valid_phone(self):
        self.assertTrue(
            validate_phone("9876543210")
        )

    def test_invalid_phone(self):
        self.assertFalse(
            validate_phone("123")
        )

    def test_valid_password(self):
        self.assertTrue(
            validate_password("abcdef")
        )

    def test_invalid_password(self):
        self.assertFalse(
            validate_password("abc")
        )

    def test_valid_role(self):
        self.assertTrue(
            validate_role("Buyer")
        )

    def test_invalid_role(self):
        self.assertFalse(
            validate_role("Manager")
        )

if __name__ == "__main__":
    unittest.main()