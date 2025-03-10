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
export const LOGIN_API = `${BASE_URL}/auth/signin`;
export const REGISTER_CUSTOMER_API = `${BASE_URL}/auth/customers`;
export const USER_PROFILE_API = `${BASE_URL}/auth/user`;
export const VERIFY_EMAIL_API = `${BASE_URL}/auth/email/verification/send`; // Gửi email xác thực
export const CONFIRM_EMAIL_API = `${BASE_URL}/auth/email/verification/confirm`; // Xác thực email
export const FORGOT_PASSWORD_API = `${BASE_URL}/auth/password/forgot`;
export const RESET_PASSWORD_API = `${BASE_URL}/auth/password/reset`;
