import axios from "axios";

/**
 * Generic API call function
 * @param {string} method - HTTP method (e.g., 'GET', 'POST', 'PUT', 'DELETE')
 * @param {string} url - API endpoint URL
 * @param {Object} data - Request payload (optional)
 * @returns {Promise} - Resolves with response data or rejects with an error
 */

export const apiCall = async (method, url, data = null) => {
  try {
    const response = await axios({
      method,
      url,
      data,
    });

    return response.data;
  } catch (error) {
    console.error("API call error:", error.message);
    // Handle error response
    if (error.response) {
      const { status, data } = error.response;

      throw {
        status,
        message:
          data?.message ||
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
