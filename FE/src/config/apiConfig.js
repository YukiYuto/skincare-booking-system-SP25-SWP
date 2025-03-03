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
export const LOGIN_API = `${BASE_URL}/Auth/signin`;
export const REGISTER_CUSTOMER_API = `${BASE_URL}/Auth/customers`;
export const USER_PROFILE_API = `${BASE_URL}/Auth/user`;
export const VERIFY_EMAIL_API = `${BASE_URL}/Auth/email/verification/send`;
export const CONFIRM_EMAIL_API = `${BASE_URL}/Auth/email/verification/confirm`; 

// Thêm API cho Forgot Password & Reset Password
export const FORGOT_PASSWORD_API = `${BASE_URL}/Auth/password/forgot`;
export const RESET_PASSWORD_API = `${BASE_URL}/Auth/password/reset`;

// Thêm API cho Customer
export const GET_ALL_CUSTOMERS_API = `${BASE_URL}/Customer/list`;
export const GET_CUSTOMER_BY_ID_API = `${BASE_URL}/Customer/{customerId}`;

// Thêm API cho Order
export const POST_ORDER_API = `${BASE_URL}/Order/create`;
export const GET_ALL_ORDERS_API = `${BASE_URL}/Order/all`;
export const GET_ORDER_BY_ID_API = `${BASE_URL}/Order/get/{id}`;
export const PUT_ORDER_API = `${BASE_URL}/Order/update/`;
export const DELETE_ORDER_API = `${BASE_URL}/Order/delete/{id}`;

// Thêm API cho OrderDetail
export const POST_ORDER_DETAILS_API = `${BASE_URL}/OrderDetail/create`;
export const GET_ORDER_DETAILS_API = `${BASE_URL}/OrderDetail/all`;
export const GET_ORDER_DETAILS_API_BY_ID = `${BASE_URL}/OrderDetail/get/{id}`;
export const PUT_ORDER_DETAILS_API = `${BASE_URL}/OrderDetail/update/`;
export const DELETE_ORDER_DETAILS_API = `${BASE_URL}/OrderDetail/delete/{id}`;

// Thêm API cho Service
export const GET_ALL_SERVICES_API = `${BASE_URL}/Services/all`;
export const GET_SERVICE_BY_ID_API = `${BASE_URL}/Services/get/{serviceId}`;

// Thêm API cho ServiceType
export const POST_SERVICE_TYPE_API = `${BASE_URL}/ServiceType/create`;
export const GET_ALL_SERVICE_TYPES_API = `${BASE_URL}/ServiceType/all`;
export const GET_SERVICE_TYPE_BY_ID_API = `${BASE_URL}/ServiceType/get/{id}`;
export const PUT_SERVICE_TYPE_API = `${BASE_URL}/ServiceType/update/`;
export const DELETE_SERVICE_TYPE_API = `${BASE_URL}/ServiceType/delete/{id}`;

// Thêm API cho SkinTherapist
export const GET_SKIN_THERAPIST_BY_ID_API = `${BASE_URL}/SkinTherapist/skin-therapist/{therapistId}`;
export const GET_SKIN_THERAPIST_API = `${BASE_URL}/SkinTherapist/skin-therapist`;

// Thêm API cho Slot
export const POST_SLOT_API = `${BASE_URL}/Slot/create`;
export const GET_ALL_SLOTS_API = `${BASE_URL}/Slot/{SlotId}`;
export const DELETE_SLOT_API = `${BASE_URL}/Slot/{SlotId}`;
export const GET_SLOT_BY_ID_API = `${BASE_URL}/Slot/list`;
export const PUT_SLOT_API = `${BASE_URL}/Slot/update/`;

// // Thêm hàm hỗ trợ headers chứa token (nếu cần đăng nhập)
export const AUTH_HEADERS = (token) => ({
  "Content-Type": "application/json",
  Authorization: `Bearer ${token}`,
});