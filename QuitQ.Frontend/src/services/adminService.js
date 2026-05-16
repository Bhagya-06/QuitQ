import api from "../api/axios";

// Admin endpoints

/**
 * Get admin dashboard statistics
 * @returns {Promise} API response
 */
export const getAdminStats = async () => {
  return api.get("/admin/stats");
};

/**
 * Get all users (admin only)
 * @param {object} params - Query parameters (page, limit, role, sort)
 * @returns {Promise} API response
 */
export const getAllUsers = async (params = {}) => {
  return api.get("/admin/users", { params });
};

/**
 * Get user details (admin only)
 * @param {number} userId - User ID
 * @returns {Promise} API response
 */
export const getUserDetails = async (userId) => {
  return api.get(`/admin/users/${userId}`);
};

/**
 * Update user status (admin only)
 * @param {number} userId - User ID
 * @param {string} status - New status
 * @returns {Promise} API response
 */
export const updateUserStatus = async (userId, status) => {
  return api.put(`/admin/users/${userId}/status`, { status });
};

/**
 * Get all sellers (admin only)
 * @param {object} params - Query parameters (page, limit, status, sort)
 * @returns {Promise} API response
 */
export const getAllSellers = async (params = {}) => {
  return api.get("/admin/sellers", { params });
};

/**
 * Update seller verification status (admin only)
 * @param {number} sellerId - Seller ID
 * @param {boolean} verified - Verification status
 * @returns {Promise} API response
 */
export const updateSellerVerification = async (sellerId, verified) => {
  return api.put(`/admin/sellers/${sellerId}/verify`, { verified });
};

/**
 * Get sales report (admin only)
 * @param {object} params - Query parameters (startDate, endDate, groupBy)
 * @returns {Promise} API response
 */
export const getSalesReport = async (params = {}) => {
  return api.get("/admin/report/sales", { params });
};

/**
 * Get revenue report (admin only)
 * @param {object} params - Query parameters (startDate, endDate, groupBy)
 * @returns {Promise} API response
 */
export const getRevenueReport = async (params = {}) => {
  return api.get("/admin/report/revenue", { params });
};

/**
 * Get order report (admin only)
 * @param {object} params - Query parameters (startDate, endDate, status)
 * @returns {Promise} API response
 */
export const getOrderReport = async (params = {}) => {
  return api.get("/admin/report/orders", { params });
};
