import {
  CANCEL_APPOINTMENT_API,
  CHECK_IN_APPOINTMENT_API,
  CHECK_OUT_APPOINTMENT_API,
  GET_APPOINTMENT_BY_ID_API,
  GET_TODAY_APPOINTMENTS_API,
  HTTP_METHODS,
} from "../config/apiConfig";
import { apiCall } from "../utils/apiUtils";

export const getTodayAppointments = async () => {
  return await apiCall(HTTP_METHODS.GET, GET_TODAY_APPOINTMENTS_API);
};

export const getAppointmentById = async (id) => {
  return await apiCall(
    HTTP_METHODS.GET,
    GET_APPOINTMENT_BY_ID_API.replace("{appointmentId}", id),
    null
  );
};

export const checkInAppointment = async (customerId, appointmentId) => {
  return await apiCall(HTTP_METHODS.PUT, CHECK_IN_APPOINTMENT_API, {
    customerId,
    appointmentId,
  });
};

export const checkOutAppointment = async (customerId, appointmentId) => {
  return await apiCall(HTTP_METHODS.PUT, CHECK_OUT_APPOINTMENT_API, {
    customerId,
    appointmentId,
  });
};

export const cancelAppointment = async (appointmentId, reason) => {
  return await apiCall(
    HTTP_METHODS.DELETE,
    `${CANCEL_APPOINTMENT_API}`,
    { appointmentId, reason }
  );
};
