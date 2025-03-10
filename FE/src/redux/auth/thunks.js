import { createAsyncThunk } from "@reduxjs/toolkit";
import * as authService from "../../services/authService";
import { setUser, setTokens, clearUser, setLoading, setError } from "./slice";

/**
 * Login Thunk for authenticating user
 * @param {Object} credentials - User credentials
 * @param {Object} { dispatch } - Redux Thunk API
 * @returns {Promise} - Promise object representing the user
 * @throws {error} - Error message
 */
export const login = createAsyncThunk("auth/login", async (credentials, { dispatch }) => {
    try {
        dispatch(setLoading(true));
        const tokens = await authService.login(credentials);
        const response = await authService.fetchUserProfile(tokens.result.accessToken);
        // const user = response.result;
            //! Now the user object will contain both the tokens and the user profile data
        const user = { ...response.result };
        dispatch(setUser(user));
        dispatch(setTokens(tokens.result));
        return user;
    } catch (error) {
      dispatch(setError(error || "Login failed. Please try again."));
      throw error;
    }
  });
  
/**
 * Register Thunk for registering user
 * @param {Object} userData - User registration data
 * @param {Object} { dispatch } - Redux Thunk API
 * @returns {Promise} - Promise object representing the user
 * @throws {error} - Error message
 */
export const register = createAsyncThunk('auth/register', async (userData, { dispatch }) => {
    try {
        dispatch(setLoading(true));
        const user = await authService.register(userData);
        dispatch(setUser(user));
        return user;
    } catch (error) {
        dispatch(setError(error || "Registration failed."));
        throw error;
    }
});

// Logout Thunk
export const logout = () => (dispatch) => {
    dispatch(clearUser());
};
