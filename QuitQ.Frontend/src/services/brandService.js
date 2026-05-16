import api from "../api/axios";

// Brand endpoints

/**
 * Get all brands
 * @param {object} params - Query parameters (page, limit, sort)
 * @returns {Promise} API response
 */
export const getAllBrands = async (params = {}) => {
  return api.get("/brand", { params });
};

/**
 * Get brand by ID
 * @param {number} brandId - Brand ID
 * @returns {Promise} API response
 */
export const getBrandById = async (brandId) => {
  return api.get(`/brand/${brandId}`);
};

/**
 * Create brand (admin only)
 * @param {object} data - Brand data
 * @returns {Promise} API response
 */
export const createBrand = async (data) => {
  return api.post("/brand", data, {
    headers: { "Content-Type": "multipart/form-data" },
  });
};

/**
 * Update brand (admin only)
 * @param {number} brandId - Brand ID
 * @param {object} data - Updated brand data
 * @returns {Promise} API response
 */
export const updateBrand = async (brandId, data) => {
  return api.put(`/brand/${brandId}`, data, {
    headers: { "Content-Type": "multipart/form-data" },
  });
};

/**
 * Delete brand (admin only)
 * @param {number} brandId - Brand ID
 * @returns {Promise} API response
 */
export const deleteBrand = async (brandId) => {
  return api.delete(`/brand/${brandId}`);
};
