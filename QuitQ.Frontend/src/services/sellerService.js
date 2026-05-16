import api from "../api/axios";

// Seller endpoints

/**
 * Get seller dashboard statistics
 * @returns {Promise} API response
 */
export const getSellerStats = async () => {
  return api.get("/seller/stats");
};

/**
 * Get seller products
 * @param {object} params - Query parameters (page, limit, status, sort)
 * @returns {Promise} API response
 */
export const getSellerProducts = async (params = {}) => {
  return api.get("/seller/products", { params });
};

/**
 * Get seller orders
 * @param {object} params - Query parameters (page, limit, status, sort)
 * @returns {Promise} API response
 */
export const getSellerOrders = async (params = {}) => {
  return api.get("/seller/orders", { params });
};

/**
 * Get seller sales report
 * @param {object} params - Query parameters (startDate, endDate, groupBy)
 * @returns {Promise} API response
 */
export const getSellerReport = async (params = {}) => {
  return api.get("/seller/report", { params });
};

/**
 * Update seller profile
 * @param {object} data - Seller profile data
 * @returns {Promise} API response
 */
export const updateSellerProfile = async (data) => {
  return api.put("/seller/profile", data);
};

/**
 * Get seller profile
 * @returns {Promise} API response
 */
export const getSellerProfile = async () => {
  return api.get("/seller/profile");
};

/**
 * Get seller earnings
 * @param {object} params - Query parameters (startDate, endDate)
 * @returns {Promise} API response
 */
export const getSellerEarnings = async (params = {}) => {
  return api.get("/seller/earnings", { params });
};

/**
 * Request payout
 * @param {object} data - Payout request data
 * @returns {Promise} API response
 */
export const requestPayout = async (data) => {
  return api.post("/seller/payout-request", data);
};
