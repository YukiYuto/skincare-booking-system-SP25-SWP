// Used for API configuration
// HTTP methods
export const HTTP_METHODS = {
  GET: "GET",
  POST: "POST",
  PUT: "PUT",
  DELETE: "DELETE",
};

// Base URL for backend API calls
// TODO: Replace this with the deployed backend URL (if applicable)
const BASE_URL = 'https://localhost:7037/api';

// Base URL for customized API calls
export const BASE_API_URL = BASE_URL;
// API endpoints
// !IMPORTANT: Update the API endpoint exports here to match the backend routes
export const LOGIN_API = `${BASE_URL}/Auth/sign-in`;
export const REGISTER_CUSTOMER_API = `${BASE_URL}/Auth/customers`;
export const USER_PROFILE_API = `${BASE_URL}/User`;

// Thêm API cho Forgot Password & Reset Password
export const FORGOT_PASSWORD_API = `${BASE_URL}/Auth/forgot-password`;
export const RESET_PASSWORD_API = `${BASE_URL}/Auth/reset-password`;

// Thêm hàm hỗ trợ headers chứa token (nếu cần đăng nhập)
export const AUTH_HEADERS = (token) => ({
  "Content-Type": "application/json",
  Authorization: `Bearer ${token}`,
});