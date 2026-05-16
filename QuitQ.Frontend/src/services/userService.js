import api from "../api/axios";

// User profile endpoints

/**
 * Get current user profile
 * @returns {Promise} API response
 */
export const getCurrentUser = async () => {
  return api.get("/user/profile");
};

/**
 * Update user profile
 * @param {object} data - Profile data
 * @returns {Promise} API response
 */
export const updateUserProfile = async (data) => {
  return api.put("/user/profile", data);
};

/**
 * Change password
 * @param {string} oldPassword - Current password
 * @param {string} newPassword - New password
 * @returns {Promise} API response
 */
export const changePassword = async (oldPassword, newPassword) => {
  return api.post("/user/change-password", { oldPassword, newPassword });
};

/**
 * Request password reset
 * @param {string} email - Email address
 * @returns {Promise} API response
 */
export const requestPasswordReset = async (email) => {
  return api.post("/user/forgot-password", { email });
};

/**
 * Reset password with token
 * @param {string} token - Reset token
 * @param {string} newPassword - New password
 * @returns {Promise} API response
 */
export const resetPassword = async (token, newPassword) => {
  return api.post("/user/reset-password", { token, newPassword });
};

/**
 * Upload profile picture
 * @param {FormData} formData - File data
 * @returns {Promise} API response
 */
export const uploadProfilePicture = async (formData) => {
  return api.post("/user/profile-picture", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
};

/**
 * Get user addresses
 * @returns {Promise} API response
 */
export const getUserAddresses = async () => {
  return api.get("/user/addresses");
};

/**
 * Add user address
 * @param {object} data - Address data
 * @returns {Promise} API response
 */
export const addUserAddress = async (data) => {
  return api.post("/user/addresses", data);
};

/**
 * Update user address
 * @param {number} addressId - Address ID
 * @param {object} data - Updated address data
 * @returns {Promise} API response
 */
export const updateUserAddress = async (addressId, data) => {
  return api.put(`/user/addresses/${addressId}`, data);
};

/**
 * Delete user address
 * @param {number} addressId - Address ID
 * @returns {Promise} API response
 */
export const deleteUserAddress = async (addressId) => {
  return api.delete(`/user/addresses/${addressId}`);
};
