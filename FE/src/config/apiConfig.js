export const HTTP_METHODS = {
  GET: "GET",
  POST: "POST",
  PUT: "PUT",
  DELETE: "DELETE",
};

const BASE_URL = "https://lumiconnect.azurewebsites.net/api";
// const BASE_URL = "https://localhost:7037/api";

export const BASE_API_URL = BASE_URL;
// !IMPORTANT: Update the API endpoint exports here to match the backend routes

//Create user
export const REGISTER_CUSTOMER_API = `${BASE_URL}/auth/customers`;
export const POST_THERAPIST_API = `${BASE_URL}/auth/skin-therapists`;
export const POST_STAFF_API = `${BASE_URL}/auth/staff`;
export const LOGIN_API = `${BASE_URL}/auth/signin`;
export const USER_PROFILE_API = `${BASE_URL}/auth/user`;
export const REFRESH_TOKEN_API = `${BASE_URL}/auth/refresh-token`;
export const CHANGE_PASSWORD_API = `${BASE_URL}/auth/password/change`;
export const FORGOT_PASSWORD_API = `${BASE_URL}/auth/password/forgot`;
export const RESET_PASSWORD_API = `${BASE_URL}/auth/password/reset`;
export const GET_CUSTOMER_PROFILE_API = `${BASE_URL}/auth/profile`;
export const VERIFY_EMAIL_API = `${BASE_URL}/auth/email/verification/send`;
export const CONFIRM_EMAIL_API = `${BASE_URL}/auth/email/verification/confirm`;
export const SIGNIN_BY_GOOGLE_API = `${BASE_URL}/auth/signin-by-google`;

// Thêm API cho Customer
export const POST_CUSTOMER_AVATAR_API = `${BASE_URL}/UserManagement/avatar`;

// Thêm API cho Appointment
export const POST_APPOINTMENT_API = `${BASE_URL}/appointment`;
export const GET_ALL_APPOINTMENTS_API = `${BASE_URL}/appointment`;
export const PUT_APPOINTMENT_API = `${BASE_URL}/appointment`;
export const GET_APPOINTMENT_BY_CUSTOMER_API = `${BASE_URL}/appointment/customer/appointments`;
export const GET_APPOINTMENT_BY_ID_API = `${BASE_URL}/appointment/{appointmentId}`;
export const DELETE_APPOINTMENT_API = `${BASE_URL}/appointment`;
export const GET_TODAY_APPOINTMENTS_API = `${BASE_URL}/staff/today-appointments`;
export const CHECK_IN_APPOINTMENT_API = `${BASE_URL}/staff/check-in`;
export const CHECK_OUT_APPOINTMENT_API = `${BASE_URL}/staff/check-out`;
export const CANCEL_APPOINTMENT_API = `${BASE_URL}/bookings/cancellation`;

// Thêm API cho Booking
export const POST_BOOKING_API = `${BASE_URL}/bookings/orders-bundles`;
export const POST_APPOINTMENT_SCHEDULE_API = `${BASE_URL}/bookings/appointment-schedule`;
export const PUT_RESCHEDULING_API = `${BASE_URL}/bookings/rescheduling`;
export const DELETE_BOOKING_API = `${BASE_URL}/bookings/cancellation/{appointmentId}`;
export const GET_BOOKING_THERAPIST_API = `${BASE_URL}/bookings/therapists`;
export const GET_BOOKING_SLOT_API = `${BASE_URL}/bookings/occupied-slots`;
export const POST_BOOKING_AUTO_ASSIGN_API = `${BASE_URL}/bookings/auto-assign`;

// Thêm API cho Customer
export const GET_ALL_CUSTOMERS_API = `${BASE_URL}/customer`;
export const GET_CUSTOMER_BY_ID_API = `${BASE_URL}/customer/{customerId}`;
export const GET_CUSTOMER_USER_API = `${BASE_URL}/customer/user`;
export const GET_CUSTOMER_TIMETABLE_API = `${BASE_URL}/customer/timetable`;

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

// Thêm API cho Service Combo
export const GET_SERVICE_COMBO_API = `${BASE_URL}/servicecombo`;
export const POST_SERVICE_COMBO_API = `${BASE_URL}/servicecombo`;

// Thêm API cho Service Duration
export const GET_SERVICE_DURATION_API = `${BASE_URL}/serviceDuration`;
export const POST_SERVICE_DURATION_API = `${BASE_URL}/serviceDuration`;

// Thêm API cho Service
export const POST_SERVICE_API = `${BASE_URL}/services`;
export const GET_ALL_SERVICES_API = `${BASE_URL}/services`;
export const PUT_SERVICE_API = `${BASE_URL}/services`;
export const GET_SERVICE_BY_ID_API = `${BASE_URL}/services/{id}`;
export const DELETE_SERVICE_API = `${BASE_URL}/services/{id}`;
export const GET_SIMILAR_SERVICES_API = `${BASE_URL}/services/similar`;

// Thêm API cho ServiceType
export const POST_SERVICE_TYPE_API = `${BASE_URL}/service-type`;
export const GET_ALL_SERVICE_TYPES_API = `${BASE_URL}/service-type`;
export const PUT_SERVICE_TYPE_API = `${BASE_URL}/service-type`;
export const GET_SERVICE_TYPE_BY_ID_API = `${BASE_URL}/service-type/{id}`;
export const DELETE_SERVICE_TYPE_API = `${BASE_URL}/service-type/{id}`;

// Thêm API cho SkinTherapist
export const GET_THERAPIST_BY_ID_API = `${BASE_URL}/therapists/details/{therapistId}`;
export const GET_ALL_THERAPISTS_API = `${BASE_URL}/therapists`;
export const GET_THERAPIST_BY_SERVICE_TYPE_API = `${BASE_URL}/therapists/service-type/{serviceTypeId}`;
export const GET_THERAPIST_BY_SERVICE_API = `${BASE_URL}/therapists/service/{serviceId}`;

// Thêm API cho Slot
export const POST_SLOT_API = `${BASE_URL}/slot`;
export const GET_ALL_SLOTS_API = `${BASE_URL}/slot`;
export const PUT_SLOT_API = `${BASE_URL}/slot`;
export const GET_SLOT_BY_ID_API = `${BASE_URL}/slot/{slotId}`;
export const DELETE_SLOT_API = `${BASE_URL}/slot/{slotId}`;

//Thêm API cho TestAnswer
export const POST_TEST_ANSWER_API = `${BASE_URL}/testAnswer/update`;
export const PUT_TEST_ANSWER_API = `${BASE_URL}/testAnswer/update`;
export const GET_ALL_TEST_ANSWER_API = `${BASE_URL}/testAnswer/all`;
export const GET_TEST_ANSWER_BY_ID_API = `${BASE_URL}/testAnswer/get/{testAnswerId}`;
export const GET_TEST_QUESTIONS_API = `${BASE_URL}/testAnswer/TestQuestion/{testQuestionId}`;
export const DELETE_TEST_ANSWER_API = `${BASE_URL}/testAnswer/delete/{testAnswerId}`;

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

// Thêm API cho TypeItem
export const GET_TYPE_ITEMS_API = `${BASE_URL}/type-item`;
export const POST_TYPE_ITEM_API = `${BASE_URL}/type-item`; 

//Thêm API cho Blog Category
export const POST_BLOG_CATEGORY = `${BASE_URL}/BlogCategory/create`;
export const GET_BLOG_CATEGORY = `${BASE_URL}/BlogCategory/all`;
export const UPDATE_BLOG_CATEGORY = `${BASE_URL}/BlogCategory/update`;

//Thêm API cho Blog
export const GET_BLOG = `${BASE_URL}/blogs/all`;
export const POST_BLOG = `${BASE_URL}/blogs/create`
export const UPDATE_BLOG = `${BASE_URL}/blogs/update`;
export const DELETE_BLOG = `${BASE_URL}/blogs/delete`;
  
// // Thêm hàm hỗ trợ headers chứa token (nếu cần đăng nhập)
export const AUTH_HEADERS = (token) => ({
  "Content-Type": "application/json",
  Authorization: `Bearer ${token}`,
});
