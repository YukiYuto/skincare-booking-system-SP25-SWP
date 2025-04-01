import axios from "axios";
import { AUTH_HEADERS } from "./apiConfig";
import { refreshAccessToken } from "../redux/auth/thunks";

export const apiClient = axios.create();

let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach((p) => {
    if (error) {
      p.reject(error);
    } else {
      p.resolve(token);
    }
  });
  failedQueue = [];
};

const isUnauthorized = (error) => {
  return (
    error.response &&
    error.response.status === 404 &&
    error.response.data.message == "User not found"
  );
};

// Set up request interceptor with access to Redux store
export const setupInterceptors = (store) => {
  // * Request interceptor: Add authorization token to request headers
  apiClient.interceptors.request.use((config) => {
    const state = store.getState();
    const accessToken = state.auth.accessToken;
    if (accessToken) {
      config.headers = AUTH_HEADERS(accessToken);
    }
    return config;
  });

  apiClient.interceptors.response.use(
    (response) => response,
    async (error) => {
      const originalRequest = error.config;

      if (
        isUnauthorized(error) &&
        originalRequest.headers["Authorization"] &&
        !originalRequest._retry
      ) {
        if (isRefreshing) {
          // If already refreshing, queue the request
          return new Promise((resolve, reject) => {
            failedQueue.push({ resolve, reject });
          })
            .then((token) => {
              originalRequest.headers = AUTH_HEADERS(token);
              return apiClient(originalRequest);
            })
            .catch((err) => Promise.reject(err));
        }

        originalRequest._retry = true;
        isRefreshing = true;

        try {
          // Refresh token and unwrap the result
          const newTokens = await store.dispatch(refreshAccessToken()).unwrap();
          const newAccessToken = newTokens.accessToken;
          // Update default headers for future requests
          apiClient.defaults.headers.common[
            "Authorization"
          ] = `Bearer ${newAccessToken}`;
          originalRequest.headers["Authorization"] = `Bearer ${newAccessToken}`;
          // Resolve queued requests with new token
          processQueue(null, newAccessToken);
          return apiClient(originalRequest);
        } catch (refreshError) {
          // If refresh fails, reject all queued requests
          processQueue(refreshError, null);
          return Promise.reject(refreshError);
        } finally {
          isRefreshing = false;
        }
      }
      return Promise.reject(error);
    }
  );
};
