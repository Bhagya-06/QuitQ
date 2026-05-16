import api from "../api/axios";

// Wishlist endpoints

/**
 * Get user's wishlist
 * @returns {Promise} API response
 */
export const getWishlist = async () => {
  return api.get("/wishlist");
};

/**
 * Check if product is in wishlist
 * @param {number} productId - Product ID
 * @returns {Promise} API response
 */
export const checkWishlist = async (productId) => {
  return api.get(`/wishlist/check/${productId}`);
};

/**
 * Add product to wishlist
 * @param {number} productId - Product ID
 * @returns {Promise} API response
 */
export const addToWishlist = async (productId) => {
  return api.post(`/wishlist/${productId}`);
};

/**
 * Remove product from wishlist
 * @param {number} productId - Product ID
 * @returns {Promise} API response
 */
export const removeFromWishlist = async (productId) => {
  return api.delete(`/wishlist/${productId}`);
};

