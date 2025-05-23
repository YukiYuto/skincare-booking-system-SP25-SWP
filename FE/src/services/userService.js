import { apiCall } from '../utils/apiUtils';
import { HTTP_METHODS, USER_PROFILE_API, GET_CUSTOMER_PROFILE_API } from '../config/apiConfig';


/**
 * Get user data by ID
 *  @param {string} userId - User ID
 * @returns {Promise} - Resolves with response data or rejects with an error
 */

// TODO: In the components that need to use this function, use Redux to get the user ID from the store
export const getUserDataById = async (userId) => {
    return await apiCall(HTTP_METHODS.GET, `${USER_PROFILE_API}/${userId}`);
}

export const updateUserProfile = async (updateProfileData) => {
    try {
        return await apiCall(HTTP_METHODS.PUT, GET_CUSTOMER_PROFILE_API, updateProfileData);
    } catch (error) {
        throw new Error(error.message || "Failed to update user");
    }
}