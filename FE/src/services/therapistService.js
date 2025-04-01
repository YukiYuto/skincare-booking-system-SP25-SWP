import { COMPLETE_SERVICE_API, HTTP_METHODS } from "../config/apiConfig";
import { apiCall } from "../utils/apiUtils";

export const completeService = async (appointmentId) => {
  return await apiCall(HTTP_METHODS.PUT, COMPLETE_SERVICE_API, {
    appointmentId
  });
};
