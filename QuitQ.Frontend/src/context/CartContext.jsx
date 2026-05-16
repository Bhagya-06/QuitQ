import React, { createContext, useState, useCallback, useEffect } from "react";
import { getCart, addToCart as addToCartAPI, removeFromCart as removeFromCartAPI, updateCartQuantity as updateCartQuantityAPI } from "../services/cartService";
import { showError } from "../utils/toast";

export const CartContext = createContext();

export const CartProvider = ({ children }) => {
  const [cart, setCart] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [cartTotal, setCartTotal] = useState(0);

    const fetchCart = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await getCart();
      setCart(response.data?.data || []);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, []);
  
  // Fetch cart on mount if authenticated
  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      fetchCart();
    }
  }, [fetchCart]);

  // Calculate total whenever cart changes
  useEffect(() => {
    const total = cart.reduce(
      (sum, item) => sum + (item.unitPrice || item.price || 0) * item.quantity,
      0
    );
    setCartTotal(total);
  }, [cart]);

  const addToCart = useCallback(
    async (productId, quantity = 1) => {
      setLoading(true);
      setError(null);
      try {
        await addToCartAPI(productId, quantity);
        await fetchCart();
        return true;
      } catch (err) {
        const errorMessage = err.response?.data?.message || "Failed to add to cart";
        setError(errorMessage);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    [fetchCart]
  );

  const removeFromCart = useCallback(
    async (productId) => {
      setLoading(true);
      setError(null);
      try {
        await removeFromCartAPI(productId);
        setCart((prev) => prev.filter((item) => item.productId !== productId));
      } catch (err) {
        const errorMessage = err.response?.data?.message || "Failed to remove from cart";
        setError(errorMessage);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    []
  );

  const updateQuantity = useCallback(
    async (productId, quantity) => {
      setLoading(true);
      setError(null);
      try {
        await updateCartQuantityAPI(productId, quantity);
        setCart((prev) =>
          prev.map((item) =>
            item.productId === productId ? { ...item, quantity } : item
          )
        );
      } catch (err) {
        const errorMessage = err.response?.data?.message || "Failed to update quantity";
        setError(errorMessage);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    []
  );

  const clearCart = useCallback(() => {
    setCart([]);
    setCartTotal(0);
    setError(null);
  }, []);

  const value = {
    cart,
    loading,
    error,
    cartTotal,
    cartCount: cart.length,
    fetchCart,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
  };

  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
};
