import api from "../api/axios";

// Review endpoints

/**
 * Get reviews for a product
 * @param {number} productId - Product ID
 * @param {object} params - Query parameters (page, limit, sort)
 * @returns {Promise} API response
 */
export const getProductReviews = async (productId, params = {}) => {
  return api.get(`/review/product/${productId}`, { params });
};

/**
 * Get single review by ID
 * @param {number} reviewId - Review ID
 * @returns {Promise} API response
 */
export const getReviewById = async (reviewId) => {
  return api.get(`/review/can-review/${reviewId}`);
};

/**
 * Create new review
 * @param {object} reviewData - Review data (rating, comment, productId)
 * @returns {Promise} API response
 */
export const createReview = async (reviewData) => {
  return api.post("/review/product", reviewData);
};

/**
 * Update review
 * @param {number} reviewId - Review ID
 * @param {object} reviewData - Updated review data
 * @returns {Promise} API response
 */
export const updateReview = async (reviewId, reviewData) => {
  return api.put(`/review/${reviewId}`, null,
    {
      params: {
        rating: reviewData.rating,
        comment: reviewData.comment,
      },
    }
  );
};

/**
 * Delete review
 * @param {number} reviewId - Review ID
 * @returns {Promise} API response
 */
export const deleteReview = async (reviewId) => {
  return api.delete(`/review/${reviewId}`);
};

/**
 * Check if user can review product (bought it)
 * @param {number} productId - Product ID
 * @returns {Promise} API response
 */
export const canReviewProduct = async (productId) => {
  return api.get(`/review/can-review/${productId}`);
};
