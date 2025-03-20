import { apiCall } from "../utils/apiUtils";
import {
  LOGIN_API,
  REGISTER_CUSTOMER_API,
  HTTP_METHODS,
  VERIFY_EMAIL_API,
  CONFIRM_EMAIL_API,
  FORGOT_PASSWORD_API,
  RESET_PASSWORD_API,
  USER_PROFILE_API,
  REFRESH_TOKEN_API,
  AUTH_HEADERS,
  CHANGE_PASSWORD_API,
} from "../config/apiConfig";

/**
 * Login API call
 * @param {Object} credentials - User credentials (email and password)
 * @returns {Promise} - Resolves with response data (tokens) or rejects with an error
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
  try {
    return await apiCall(HTTP_METHODS.POST, REGISTER_CUSTOMER_API, userData);
  } catch (error) {
    throw new Error(error.message || "Failed to register user");
  }
};
/**
 * Fetch user profile API from token
 * @param {string} token
 * @returns {Promise} - Resolves with response data (user data) or rejects with an error
 */
export const fetchUserProfile = async (token) => {
  return await apiCall(
    HTTP_METHODS.GET,
    USER_PROFILE_API,
    null,
    { token },
    AUTH_HEADERS(token)
  );
};

/**
 * Refresh access token API call, using the refresh token
 * @param {string} refreshToken - Refresh token
 * @returns {Promise} - Resolves with response data (tokens) or rejects with an error
 * @throws {error} - Error message
 */
export const refreshTokens = async (refreshToken) => {
  try {
    const response = await apiCall(HTTP_METHODS.POST, REFRESH_TOKEN_API, {
      refreshToken,
    });
    return response;
  } catch (error) {
    throw new Error("Failed to refresh token" + error.message);
  }
};

export const changePassword = async (changePasswordData) => {
  try {
    return await apiCall(HTTP_METHODS.POST, CHANGE_PASSWORD_API, changePasswordData);
  } catch (error) {
    throw new Error(error.message || "Failed to change password");
  }
}

export async function forgotPassword(email) {
  const trimmedEmail = email.trim();
  try {
    const response = await apiCall(HTTP_METHODS.POST, FORGOT_PASSWORD_API, {
      email: trimmedEmail,
    });
    return response;
  } catch (error) {
    throw new Error(
      error.message || "Unexpected error occurred. Please try again."
    );
  }
}

export async function resetPassword(
  email,
  token,
  newPassword,
  confirmPassword
) {
  // const encodedToken = encodeURIComponent(token);

  const response = await fetch(RESET_PASSWORD_API, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      email,
      token,
      newPassword,
      confirmPassword,
    }),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(
      errorData?.message || "Could not send request reset password."
    );
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

/**
 * Get customer ID by user ID
 * @returns {Promise} - Resolves with response data or rejects with an error
 */
