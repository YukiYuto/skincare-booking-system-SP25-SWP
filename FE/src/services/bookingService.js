import { GET_ALL_SLOTS_API, GET_BOOKING_SLOT_API, GET_THERAPIST_BY_SERVICE_API, HTTP_METHODS, POST_APPOINTMENT_SCHEDULE_API, POST_BOOKING_API } from "../config/apiConfig";
import { apiCall } from "../utils/apiUtils";

export const getTherapistsByService = async (serviceId) => {
    return await apiCall(HTTP_METHODS.GET, GET_THERAPIST_BY_SERVICE_API.replace("{serviceId}", serviceId));
}

export const getAllSlots = async () => {
    return await apiCall(HTTP_METHODS.GET, GET_ALL_SLOTS_API);
}

export const getOccupiedSlots = async (therapistId, date) => {
    return await apiCall(HTTP_METHODS.GET, GET_BOOKING_SLOT_API, null, { therapistId, date });
}

export const bundleOrder = async (orderData) => {
    return await apiCall(HTTP_METHODS.POST, POST_BOOKING_API, orderData);
}

export const finalizeAppointment = async (appointmentData) => {
    return await apiCall(HTTP_METHODS.POST, POST_APPOINTMENT_SCHEDULE_API, appointmentData);
}