import { apiCall } from '../utils/apiUtils';
import { LOGIN_API, REGISTER_CUSTOMER_API, HTTP_METHODS, VERIFY_EMAIL_API, CONFIRM_EMAIL_API, FORGOT_PASSWORD_API } from '../config/apiConfig';

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

export async function forgotPassword(email) {
  const response = await fetch(FORGOT_PASSWORD_API, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email }),
  });
  if (!response.ok) {
    throw new Error("Unable to forogt password.");
  }
  return response.json();
}


export const sendVerificationEmail = async (email) => {
  const response = await fetch(VERIFY_EMAIL_API, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email }),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.message || "Failed to send verification email");
  }

  return response.json();
};

export const confirmEmailVerification = async (userId, token) => {
  const response = await fetch(CONFIRM_EMAIL_API, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ userId, token }),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.message || "Email verification failed");
  }

  return response.json();
};
