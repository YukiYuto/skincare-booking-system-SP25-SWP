import { GET_CUSTOMER_USER_API, HTTP_METHODS } from "../config/apiConfig"
import { apiCall } from "../utils/apiUtils"

export const getCustomerDetails = async () => {
    return await apiCall(HTTP_METHODS.GET, GET_CUSTOMER_USER_API);
}