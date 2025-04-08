import {
  COMPLETE_SERVICE_API,
  GET_ALL_THERAPIST_SCHEDULE_API,
  GET_THERAPIST_ADVICE,
  HTTP_METHODS,
  POST_THERAPIST_ADVICE,
} from "../config/apiConfig";
import { apiCall } from "../utils/apiUtils";

export const completeService = async (appointmentId) => {
  return await apiCall(HTTP_METHODS.PUT, COMPLETE_SERVICE_API, {
    appointmentId,
  });
};

export const getTherapistScheduleId = async () => {
  return await apiCall(HTTP_METHODS.GET, GET_ALL_THERAPIST_SCHEDULE_API);
};

export const createTherapistAdvice = async (
  therapistScheduleId,
  customerId,
  adviceContent
) => {
  try {
    const response = await apiCall(HTTP_METHODS.POST, POST_THERAPIST_ADVICE, {
      therapistScheduleId,
      customerId,
      adviceContent,
    });
    return response;
  } catch (error) {
    console.error("Error creating therapist advice:", error);
    throw error;
  }
};

export const getTherapistScheduleAdvice = async (appointmentId) => {
  try {
    const response = await apiCall(
      HTTP_METHODS.GET,
      `${GET_THERAPIST_ADVICE}/${appointmentId}`
    );
    return response;
  } catch (error) {
    console.error("Error getting therapist schedule advice:", error);
    throw error;
  }
};
