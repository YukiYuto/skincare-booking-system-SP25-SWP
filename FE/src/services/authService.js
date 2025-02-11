import { apiCall } from '../utils/apiUtils';
import { LOGIN_API, HTTP_METHODS } from '../config/apiConfig';

/**
 * Login API call
 * @param {Object} credentials - User credentials (email and password)
 * @returns {Promise} - Resolves with response data or rejects with an error
 */
export const login = async (credentials) => {
  return await apiCall(HTTP_METHODS.POST, LOGIN_API, credentials);
};