import api from "../api/axios";

// Order endpoints

/**
 * Get user's orders
 * @param {object} params - Query parameters (page, limit, status)
 * @returns {Promise} API response
 */
export const getUserOrders = async (params = {}) => {
  return api.get("/order", { params });
};

/**
 * Get order by ID
 * @param {number} orderId - Order ID
 * @returns {Promise} API response
 */
export const getOrderById = async (orderId) => {
  return api.get(`/order/${orderId}`);
};

/**
 * Create new order
 * @param {object} orderData - Order data
 * @returns {Promise} API response
 */
export const createOrder = async (orderData) => {
  return api.post("/order", orderData);
};

/**
 * Update order status
 * @param {number} orderId - Order ID
 * @param {string} status - New status
 * @returns {Promise} API response
 */
export const updateOrderStatus = async (orderId, status) => {
  return api.put(`/order/${orderId}/status`, { status });
};

/**
 * Cancel order
 * @param {number} orderId - Order ID
 * @returns {Promise} API response
 */
export const cancelOrder = async (orderId) => {
  return api.post(`/order/${orderId}/cancel`);
};

/**
 * Get order summary
 * @returns {Promise} API response
 */
export const getOrderSummary = async () => {
  return api.get("/order/summary");
};
