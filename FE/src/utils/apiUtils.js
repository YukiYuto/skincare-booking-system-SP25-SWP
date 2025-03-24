import { apiClient } from "../config/axios";
/**
 * Generic API call function
 * @param {string} method - HTTP method (e.g., 'GET', 'POST', 'PUT', 'DELETE')
 * @param {string} url - API endpoint URL
 * @param {Object} data - Request payload (optional)
 * @returns {Promise} - Resolves with response data or rejects with an error
 */

export const apiCall = async (method, url, data = null, query = null, headers = null) => {
  try {
    const response = await apiClient({
      method,
      url,
      data,
      params: query,
      headers: headers || {},
    });

    return response.data;
  } catch (error) {
    // Handle error response
    if (error.response) {
      console.log("Error message:", error.response.data.message);
      const { status, data } = error.response;
      console.log ("Error status:", status);
      throw {
        status,
        message:
          error.response.data?.message ||
          `Error: ${status}: ${data?.error || "Unable to process request."}`,
      };
    } else if (error.request) {
      // Network error
      throw {
        message:
          "No response from the server. Please check your internet connection.",
      };
    } else {
      // Other errors
      throw { message: `An unexpected error occurred. ${error.message}` };
    }
  }
};
