import api from "../api/axios";

// Offer endpoints

/**
 * Get all offers
 * @param {object} params - Query parameters (page, limit, status, sort)
 * @returns {Promise} API response
 */
export const getAllOffers = async (params = {}) => {
  return api.get("/offer", { params });
};

/**
 * Get offer by ID
 * @param {number} offerId - Offer ID
 * @returns {Promise} API response
 */
export const getOfferById = async (offerId) => {
  return api.get(`/offer/${offerId}`);
};

/**
 * Get active offers
 * @param {object} params - Query parameters (page, limit)
 * @returns {Promise} API response
 */
export const getActiveOffers = async (params = {}) => {
  return api.get("/offer/active", { params });
};

/**
 * Create offer (admin only)
 * @param {object} data - Offer data
 * @returns {Promise} API response
 */
export const createOffer = async (data) => {
  return api.post("/offer", data);
};

/**
 * Update offer (admin only)
 * @param {number} offerId - Offer ID
 * @param {object} data - Updated offer data
 * @returns {Promise} API response
 */
export const updateOffer = async (offerId, data) => {
  return api.put(`/offer/${offerId}`, data);
};

/**
 * Delete offer (admin only)
 * @param {number} offerId - Offer ID
 * @returns {Promise} API response
 */
export const deleteOffer = async (offerId) => {
  return api.delete(`/offer/${offerId}`);
};

/**
 * Apply offer to product
 * @param {number} offerId - Offer ID
 * @param {number} productId - Product ID
 * @returns {Promise} API response
 */
export const applyOfferToProduct = async (offerId, productId) => {
  return api.post(`/offer/${offerId}/apply`, { productId });
};
