import { useEffect } from "react";
import { Link } from "react-router-dom";
import { showError } from "../../utils/toast";
import { validateQuantity } from "../../utils/validations/userValidation";
import { useCart } from "../../hooks/useCart";

function Cart() {
  const {
    cart,
    cartTotal,
    fetchCart,
    removeFromCart,
    updateQuantity,
    clearCart,
  } = useCart();

  useEffect(() => {
    fetchCart();
  }, [fetchCart]);

  const removeItem = async (productId) => {
    try {
      await removeFromCart(productId);
      await fetchCart();
    } catch (err) {
      showError(err.response?.data?.message || "Unable to remove item");
    }
  };

  const clearCartHandler = async () => {
    try {
      await clearCart();
    } catch (err) {
      showError(err.response?.data?.message || "Unable to clear cart");
    }
  };

  const updateItemQuantity = async (productId, quantity) => {
    const errorMessage = validateQuantity(quantity);
    if (errorMessage) {
      showError(errorMessage);
      return;
    }

    try {
      await updateQuantity(productId, quantity);
      await fetchCart();
    } catch (err) {
      showError(err.response?.data?.message || "Unable to update quantity");
    }
  };

  return (
    <div className="container mt-4">

      <div className="d-flex justify-content-between align-items-center mb-4">

        <h2>Shopping Cart</h2>

        <button
          className="btn btn-danger"
          onClick={clearCart}
        >
          Clear Cart
        </button>

      </div>

      {cart.length === 0 ? (
        <div className="alert alert-info">Cart is empty</div>
      ) : (
        <>
          <div className="table-responsive">
            <table className="table table-bordered">
              <thead className="table-dark">
                <tr>
                  <th>Product</th>
                  <th>Price</th>
                  <th>Quantity</th>
                  <th>Total</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                {cart.map((item) => (
                  <tr key={item.productId || item.id}>
                    <td>{item.productName}</td>
                    <td>₹ {item.unitPrice}</td>
                    <td style={{ width: "180px" }}>
                      <div className="d-flex">
                        <button
                          className="btn btn-secondary btn-sm"
                          onClick={() => updateItemQuantity(item.productId, item.quantity - 1)}
                        >
                          -
                        </button>
                        <input
                          className="form-control text-center mx-2"
                          value={item.quantity}
                          readOnly
                        />
                        <button
                          className="btn btn-secondary btn-sm"
                          onClick={() => updateItemQuantity(item.productId, item.quantity + 1)}
                        >
                          +
                        </button>
                      </div>
                    </td>
                    <td>₹ {(item.unitPrice || 0) * item.quantity}</td>
                    <td>
                      <button
                        className="btn btn-danger btn-sm"
                        onClick={() => removeItem(item.productId)}
                      >
                        Remove
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="text-end mt-4">
            <h3>Total: ₹ {cartTotal}</h3>
            <Link to="/checkout" className="btn btn-success mt-3">
              Proceed To Checkout
            </Link>
          </div>
        </>
      )}
    </div>
  );
}

export default Cart;