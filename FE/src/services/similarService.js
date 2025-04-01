import { HTTP_METHODS } from "../config/apiConfig";
import { apiCall } from "../utils/apiUtils";
import { GET_SIMILAR_SERVICES_API } from "../config/apiConfig";

export const getSimilarServices = async (serviceId, batch, itemPerBatch) => {
  return await apiCall(HTTP_METHODS.GET, GET_SIMILAR_SERVICES_API, null, {
    serviceId,
    batch,
    itemPerBatch,
  });
};
