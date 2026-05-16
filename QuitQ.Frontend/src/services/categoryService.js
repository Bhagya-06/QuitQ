import api from "../api/axios";

// Category endpoints

/**
 * Get all categories
 * @param {object} params - Query parameters (page, limit, sort)
 * @returns {Promise} API response
 */
export const getAllCategories = async (params = {}) => {
  return api.get("/category", { params });
};

/**
 * Get category by ID
 * @param {number} categoryId - Category ID
 * @returns {Promise} API response
 */
export const getCategoryById = async (categoryId) => {
  return api.get(`/category/${categoryId}`);
};

/**
 * Create category (admin only)
 * @param {object} data - Category data
 * @returns {Promise} API response
 */
export const createCategory = async (data) => {
  return api.post("/category", data);
};

/**
 * Update category (admin only)
 * @param {number} categoryId - Category ID
 * @param {object} data - Updated category data
 * @returns {Promise} API response
 */
export const updateCategory = async (categoryId, data) => {
  return api.put(`/category/${categoryId}`, data);
};

/**
 * Delete category (admin only)
 * @param {number} categoryId - Category ID
 * @returns {Promise} API response
 */
export const deleteCategory = async (categoryId) => {
  return api.delete(`/category/${categoryId}`);
};
