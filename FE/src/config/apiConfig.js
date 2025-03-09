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

// Thêm API cho Appointment
export const POST_APPOINTMENT_API = `${BASE_URL}/appointment`;
export const GET_ALL_APPOINTMENTS_API = `${BASE_URL}/appointment`;
export const PUT_APPOINTMENT_API = `${BASE_URL}/appointment`;
export const GET_APPOINTMENT_BY_CUSTOMER_API = `${BASE_URL}/appointment/{customerId}/appointments`;
export const GET_APPOINTMENT_BY_ID_API = `${BASE_URL}/appointment/{appointmentId}`;
export const DELETE_APPOINTMENT_API = `${BASE_URL}/appointment`;

// Thêm API cho Booking
export const POST_BOOKING_API = `${BASE_URL}/bookings/orders-bundles`;
export const GET_BOOKING_THERAPIST_API = `${BASE_URL}/bookings/therapists`;
export const GET_BOOKING_SLOT_API = `${BASE_URL}/bookings/occupied-slots`;

// Thêm API cho Customer
export const GET_ALL_CUSTOMERS_API = `${BASE_URL}/customer`;
export const GET_CUSTOMER_BY_ID_API = `${BASE_URL}/customer/{customerId}`;
export const GET_CUSTOMER_USER_API = `${BASE_URL}/customer/user`;

// Thêm API cho FileStorage
export const POST_FILE_SERVICE_API = `${BASE_URL}/files/service`;
export const POST_FILE_SERVICE_COMBO_API = `${BASE_URL}/files/service-combo`;

// Thêm API cho Order
export const GET_ALL_ORDERS_API = `${BASE_URL}/order`;
export const PUT_ORDER_API = `${BASE_URL}/order`;
export const GET_ORDER_BY_ID_API = `${BASE_URL}/order/{id}`;
export const DELETE_ORDER_API = `${BASE_URL}/order/{id}`;

// Thêm API cho OrderDetail
export const GET_ORDER_DETAILS_API = `${BASE_URL}/order-detail`;
export const PUT_ORDER_DETAILS_API = `${BASE_URL}/order-detail`;
export const DELETE_ORDER_DETAILS_API = `${BASE_URL}/order-detail`;
export const GET_ORDER_DETAILS_API_BY_ID = `${BASE_URL}/order-detail/{id}`;

// Thêm API cho Payment
export const POST_PAYMENT_API = `${BASE_URL}/payment/create-link`;
export const POST_CONFIRM_PAYMENT_API = `${BASE_URL}/payment/confirm-transaction`;

// Thêm API cho Service
export const POST_SERVICE_API = `${BASE_URL}/services`;
export const GET_ALL_SERVICES_API = `${BASE_URL}/services`;
export const PUT_SERVICE_API = `${BASE_URL}/services`;
export const GET_SERVICE_BY_ID_API = `${BASE_URL}/services/{id}`;
export const DELETE_SERVICE_API = `${BASE_URL}/services/{id}`;

// Thêm API cho ServiceType
export const POST_SERVICE_TYPE_API = `${BASE_URL}/service-type`;
export const GET_ALL_SERVICE_TYPES_API = `${BASE_URL}/service-type`;
export const PUT_SERVICE_TYPE_API = `${BASE_URL}/service-type`;
export const GET_SERVICE_TYPE_BY_ID_API = `${BASE_URL}/service-type/{id}`;
export const DELETE_SERVICE_TYPE_API = `${BASE_URL}/service-type/{id}`;

// Thêm API cho SkinTherapist
export const GET_THERAPIST_BY_ID_API = `${BASE_URL}/therapists/details/{therapistId}`;
export const GET_ALL_THERAPISTS_API = `${BASE_URL}/therapists`;
export const GET_THERAPIST_BY_SERVICE_API = `${BASE_URL}/therapists/service/{serviceTypeId}`;

// Thêm API cho Slot
export const POST_SLOT_API = `${BASE_URL}/slot`;
export const GET_ALL_SLOTS_API = `${BASE_URL}/slot`;
export const PUT_SLOT_API = `${BASE_URL}/slot`;
export const GET_SLOT_BY_ID_API = `${BASE_URL}/slot/{slotId}`;
export const DELETE_SLOT_API = `${BASE_URL}/slot/{slotId}`;

// Thêm API cho TherapistSchedule
export const POST_THERAPIST_SCHEDULE_API = `${BASE_URL}/therapist-schedules`;
export const GET_ALL_THERAPIST_SCHEDULE_API = `${BASE_URL}/therapist-schedules`;
export const PUT_THERAPIST_SCHEDULE_API = `${BASE_URL}/therapist-schedules`;
export const GET_THERAPIST_SCHEDULE_BY_ID_API = `${BASE_URL}/therapist-schedules/{scheduleId}`;
export const DELETE_THERAPIST_SCHEDULE = `${BASE_URL}/therapist-schedules/{scheduleId}`;
export const GET_THERAPIST_SCHEDULE_BY_THERAPIST_API = `${BASE_URL}/therapist-schedules/therapist/{therapistId}`;

// Thêm API cho TherapistServiceType
export const POST_THERAPIST_SERVICE_TYPE_API = `${BASE_URL}/therapist-service-type`;
export const DELETE_THERAPIST_SERVICE_TYPES_API = `${BASE_URL}/therapist-service-type`;

// // Thêm hàm hỗ trợ headers chứa token (nếu cần đăng nhập)
export const AUTH_HEADERS = (token) => ({
  "Content-Type": "application/json",
  Authorization: `Bearer ${token}`,
});