import { apiCall } from '../utils/apiUtils';
import { USER_API, HTTP_METHODS } from '../config/apiConfig';


/**
 * Get user data by ID
 *  @param {string} userId - User ID
 * @returns {Promise} - Resolves with response data or rejects with an error
 */

// TODO: In the components that need to use this function, use Redux to get the user ID from the store
export const getUserDataById = async (userId) => {
    return await apiCall(HTTP_METHODS.GET, `${USER_API}/${userId}`);
}
