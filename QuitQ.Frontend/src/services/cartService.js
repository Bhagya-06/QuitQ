import api from "../api/axios";

// Cart endpoints

/**
 * Get user's cart
 * @returns {Promise} API response
 */
export const getCart = async () => {
  return api.get("/cart");
};

/**
 * Add product to cart
 * @param {number} productId - Product ID
 * @param {number} quantity - Quantity to add
 * @returns {Promise} API response
 */
export const addToCart = async (productId, quantity = 1) => {
  return api.post("/cart", { productId, quantity });
};

/**
 * Update cart item quantity
 * @param {number} productId - Product ID
 * @param {number} quantity - New quantity
 * @returns {Promise} API response
 */
export const updateCartQuantity = async (productId, quantity) => {
  return api.put("/cart", null, {
    params: { productId, quantity },
  });
};

/**
 * Remove product from cart
 * @param {number} productId - Product ID
 * @returns {Promise} API response
 */
export const removeFromCart = async (productId) => {
  return api.delete(`/cart/${productId}`);
};

/**
 * Clear entire cart
 * @returns {Promise} API response
 */
export const clearCart = async () => {
  return api.delete("/cart/clear");
};

/**
 * Get cart totals
 * @returns {Promise} API response
 */
export const getCartTotal = async () => {
  return api.get("/cart/total");
};

/**
 * Apply coupon/discount to cart
 * @param {string} couponCode - Coupon code
 * @returns {Promise} API response
 */
export const applyCoupon = async (couponCode) => {
  return api.post("/cart/coupon", { couponCode });
};
