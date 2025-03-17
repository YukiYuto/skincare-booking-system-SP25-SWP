import { HTTP_METHODS, POST_CONFIRM_PAYMENT_API, POST_PAYMENT_API } from "../config/apiConfig";
import { apiCall } from "../utils/apiUtils";

export const createPaymentLink = async (orderNumber, cancelUrl, returnUrl) => {
  return await apiCall(HTTP_METHODS.POST, POST_PAYMENT_API, {
    orderNumber,
    cancelUrl,
    returnUrl,
  });
};

export const confirmPayment = async (orderNumber) => {
  return await apiCall(HTTP_METHODS.POST, POST_CONFIRM_PAYMENT_API, { orderNumber });
}