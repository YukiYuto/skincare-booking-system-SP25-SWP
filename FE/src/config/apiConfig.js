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
const BASE_URL = 'https://lumiconnect.azurewebsites.net/api';

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
export const UPLOAD_AVATAR = `${BASE_URL}/UserManagement/avatar`;
export const UPDATE_PROFILE = `${BASE_URL}/auth/profile`;
export const CHANGE_PASSWORD = `${BASE_URL}/auth/password/change`;
export const GET_SERVICES = `${BASE_URL}/services`;
export const GET_THERAPISTS = `${BASE_URL}/bookings/therapists`;
export const GET_SLOT = `${BASE_URL}/slot`;
export const GET_OCCUPIED_SLOTS = `${BASE_URL}/bookings/occupied-slots`;
export const GET_CUSTOMER = `${BASE_URL}/customer/user`;
export const CREATE_ORDERS_BUNDLES = `${BASE_URL}/bookings/orders-bundles`;
export const CREATE_PAYMENT_LINK = `${BASE_URL}/payment/create-link`
