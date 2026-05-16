import api from "../api/axios";
import { jwtDecode } from "jwt-decode";

/**
 * Decode JWT token and extract user information
 * @param {string} token - JWT token
 * @returns {object} Decoded token payload
 */
const decodeToken = (token) => {
  try {
    return jwtDecode(token);
  } catch (err) {
    console.error("Failed to decode token", err);
    return null;
  }
};

/**
 * Extract role from decoded token
 * @param {string} token - JWT token
 * @returns {string} User role
 */
const getRoleFromToken = (token) => {
  const decoded = decodeToken(token);
  if (!decoded) return null;
  
  // Handle different role claim formats
  const roleClaim =
    decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
    decoded.role ||
    decoded.Role;
  
  return roleClaim;
};

/**
 * Extract user ID from decoded token
 * @param {string} token - JWT token
 * @returns {string} User ID
 */
const getUserIdFromToken = (token) => {
  const decoded = decodeToken(token);
  if (!decoded) return null;
  
  // Handle different ID claim formats
  const userIdClaim =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] ||
    decoded.userId ||
    decoded.sub ||
    decoded.id;
  
  return userIdClaim;
};

/**
 * Login user with email and password
 * @param {object} credentials - Email and password
 * @returns {Promise} API response
 */
export const loginUser = async (credentials) => {
  const response = await api.post("/auth/login", credentials);
  const payload = response.data?.data || response.data;
  
  if (payload?.token) {
    const role = getRoleFromToken(payload.token);
    const userId = getUserIdFromToken(payload.token);
    
    localStorage.setItem("token", payload.token);
    localStorage.setItem("role", role || "");
    localStorage.setItem("userId", userId || "");
  }
  
  return response;
};

/**
 * Register new user
 * @param {object} credentials - Registration data
 * @returns {Promise} API response
 */
export const registerUser = async (credentials) => {
  const response = await api.post("/auth/register", credentials);
  return response;
};

/**
 * Google OAuth login
 * @param {string} googleToken - Google OAuth token
 * @returns {Promise} API response
 */
export const googleLogin = async (googleToken) => {
  const response = await api.post("/auth/google-login", { token: googleToken });
  const payload = response.data?.data || response.data;
  
  if (payload?.token) {
    const role = getRoleFromToken(payload.token);
    const userId = getUserIdFromToken(payload.token);
    
    localStorage.setItem("token", payload.token);
    localStorage.setItem("role", role || "");
    localStorage.setItem("userId", userId || "");
  }
  
  return response;
};

/**
 * Logout user (client-side)
 */
export const logoutUser = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("role");
  localStorage.removeItem("userId");
};

/**
 * Refresh authentication token
 * @returns {Promise} API response
 */
export const refreshToken = async () => {
  const response = await api.post("/auth/refresh");
  const { data } = response;
  
  if (data.token) {
    localStorage.setItem("token", data.token);
  }
  
  return response;
};

/**
 * Verify if token is valid
 * @returns {boolean} Token validity
 */
export const isTokenValid = () => {
  const token = localStorage.getItem("token");
  if (!token) return false;
  
  try {
    const decoded = decodeToken(token);
    if (!decoded) return false;
    
    // Check if token is expired
    const expiryTime = decoded.exp * 1000; // Convert to milliseconds
    return Date.now() < expiryTime;
  } catch {
    return false;
  }
};

/**
 * Get current user role
 * @returns {string} User role
 */
export const getUserRole = () => {
  return localStorage.getItem("role");
};

/**
 * Get current user ID
 * @returns {string} User ID
 */
export const getUserId = () => {
  return localStorage.getItem("userId");
};
