import unittest

class TestCart(unittest.TestCase):

    def test_total_calculation(self):
        cart = [{"price": 100, "qty": 2},
            {"price": 200, "qty": 1}]
        total = sum(item["price"] * item["qty"] for item in cart)
        self.assertEqual(total, 400)

    def test_empty_cart(self):
        cart = []
        total = sum(item["price"] * item["qty"]for item in cart)
        self.assertEqual(total, 0)

if __name__ == "__main__":
    unittest.main()