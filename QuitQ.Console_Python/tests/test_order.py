import unittest

class TestCart(unittest.TestCase):

    def test_single_item_order(self):
        items = [{"price": 100, "quantity": 2}]
        total = sum(item["price"] * item["quantity"] for item in items)
        self.assertEqual(total, 200)

    def test_total_calculation(self):

        cart = [
            {"price": 100, "qty": 2},
            {"price": 200, "qty": 1}
        ]

        total = sum(item["price"] * item["qty"] for item in cart)
        self.assertEqual(total, 400)

if __name__ == "__main__":
    unittest.main()