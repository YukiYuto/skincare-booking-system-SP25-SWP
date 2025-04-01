import {
  GET_CUSTOMER_ORDERS_API,
  GET_CUSTOMER_USER_API,
  GET_ORDER_DETAILS_BY_ORDER_ID,
  GET_SERVICE_RECOMMENDATIONS_API,
  HTTP_METHODS,
} from "../config/apiConfig";
import { apiCall } from "../utils/apiUtils";

export const getCustomerDetails = async () => {
  return await apiCall(HTTP_METHODS.GET, GET_CUSTOMER_USER_API);
};

export const getCustomerOrders = async () => {
  return await apiCall(HTTP_METHODS.GET, GET_CUSTOMER_ORDERS_API);
};

export const getOrderDetailsByOrderId = async (orderId) => {
  return await apiCall(HTTP_METHODS.GET, GET_ORDER_DETAILS_BY_ORDER_ID, null, {
    orderId,
  });
};

export const getRecommendations = async (skinProfileId) => {
  return await apiCall(
    HTTP_METHODS.POST,
    `${GET_SERVICE_RECOMMENDATIONS_API}`,
    { skinProfileId }
  );
};
