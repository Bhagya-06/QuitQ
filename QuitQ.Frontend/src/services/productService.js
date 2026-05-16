import api from "../api/axios";

/**
 * Search and filter products
 * @param {object} params - Query parameters (search, category, brand, minPrice, maxPrice, page, limit, sort)
 * @returns {Promise} API response
 */
export const getProducts = (params) =>
  api.get("/product/search", {
    params,
  });

/**
 * Get product by ID
 * @param {number} id - Product ID
 * @returns {Promise} API response
 */
export const getProductById = (id) =>
  api.get(`/product/${id}`);

/**
 * Get featured products
 * @param {number} limit - Number of products to fetch
 * @returns {Promise} API response
 */
export const getFeaturedProducts = (limit = 10) =>
  api.get("/product/featured", { params: { limit } });

/**
 * Get trending products
 * @param {number} limit - Number of products to fetch
 * @returns {Promise} API response
 */
export const getTrendingProducts = (limit = 10) =>
  api.get("/product/trending", { params: { limit } });

/**
 * Add new product
 * @param {object} data - Product data with FormData
 * @returns {Promise} API response
 */
export const addProduct = (data) =>
  api.post("/product", data, {
    headers: { "Content-Type": "multipart/form-data" },
  });

/**
 * Update product
 * @param {number} id - Product ID
 * @param {object} data - Updated product data
 * @returns {Promise} API response
 */
export const updateProduct = (id, data) =>
  api.put(`/product/${id}`, data, {
    headers: { "Content-Type": "multipart/form-data" },
  });

/**
 * Delete product
 * @param {number} id - Product ID
 * @returns {Promise} API response
 */
export const deleteProduct = (id) =>
  api.delete(`/product/${id}`);

/**
 * Get seller's products
 * @param {object} params - Query parameters (page, limit, status)
 * @returns {Promise} API response
 */
export const getSellerProducts = (params = {}) =>
  api.get("/product/seller/my-products", { params });

/**
 * Get product statistics
 * @returns {Promise} API response
 */
export const getProductStats = () =>
  api.get("/product/stats");

/**
 * Bulk update product status
 * @param {array} productIds - Array of product IDs
 * @param {string} status - New status
 * @returns {Promise} API response
 */
export const bulkUpdateStatus = (productIds, status) =>
  api.post("/product/bulk-update", { productIds, status });
