import unittest
from unittest.mock import patch
from modules.auth import login, register

class TestAuth(unittest.TestCase):

    @patch("builtins.input")
    @patch("modules.auth.pause")
    def test_login_inputs(self, mock_input, mock_pause):
        mock_input.side_effect = [
            "test@gmail.com",
            "password"]
        login()
    
    @patch("builtins.input")
    @patch("modules.auth.pause")
    def test_invalid_email(self, mock_input, mock_pause):
        mock_input.side_effect = [
            "Bhagya",  
            "bhagya123",  
            "invalid-email", 
            "password123", 
            "9876543210",
            "Buyer",         
            "Chennai" ]
        register()

if __name__ == "__main__":
    unittest.main()