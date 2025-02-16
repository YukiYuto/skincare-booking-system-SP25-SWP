import { apiCall } from '../utils/apiUtils';
import { LOGIN_API, REGISTER_CUSTOMER_API, HTTP_METHODS } from '../config/apiConfig';

/**
 * Login API call
 * @param {Object} credentials - User credentials (email and password)
 * @returns {Promise} - Resolves with response data (user data) or rejects with an error
 */
export const login = async (credentials) => {
  return await apiCall(HTTP_METHODS.POST, LOGIN_API, credentials);
};

/**
 * Register API call
 * @param {Object} userData - User registration data
 * @returns {Promise} - Resolves with response data (user data) or rejects with an error
 */
export const register = async (userData) => {
  return await apiCall(HTTP_METHODS.POST, REGISTER_CUSTOMER_API, userData);
};
