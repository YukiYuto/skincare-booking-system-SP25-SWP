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

export async function resetPassword(email) {
  const response = await fetch("/api/auth/reset-password", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email }),
  });

  if (!response.ok) {
    throw new Error("Could not send request reset password.");
  }
}

export async function updatePassword(email, token, newPassword) {
  const response = await fetch("/api/auth/update-password", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, token, newPassword }),
  });

  if (!response.ok) {
    throw new Error("Unable to update password.");
  }
}
