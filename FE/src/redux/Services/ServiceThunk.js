import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  GET_ALL_SERVICES_API,
  GET_SERVICE_BY_ID_API,
  POST_SERVICE_API,
  PUT_SERVICE_API,
  DELETE_SERVICE_API,
} from "../../config/apiConfig";
import {
  setServices,
  setServiceDetail,
  setLoading,
  setError,
} from "./ServiceSlice";

// Enhanced error handling utility
const handleApiError = (error, dispatch, defaultMessage) => {
  console.error('API Error Details:', {
    name: error.name,
    message: error.message,
    status: error.response?.status,
    data: error.response?.data,
  });

  dispatch(setLoading(false));
  dispatch(setError({
    message: defaultMessage,
    details: error.message,
    status: error.response?.status || 500
  }));

  throw error;
};

// Fetch Services with robust error handling
export const fetchServices = createAsyncThunk(
  "service/fetchAll",
  async (_, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(GET_ALL_SERVICES_API);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      dispatch(setServices(data || []));
      dispatch(setLoading(false));
      return data;
    } catch (error) {
      return handleApiError(error, dispatch, "Failed to fetch services");
    }
  }
);

// Fetch Service by ID
export const fetchServiceById = createAsyncThunk(
  "service/fetchById",
  async (serviceId, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(
        GET_SERVICE_BY_ID_API.replace("{serviceId}", serviceId)
      );
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      dispatch(setServiceDetail(data));
      dispatch(setLoading(false));
      return data;
    } catch (error) {
      return handleApiError(error, dispatch, "Failed to fetch service details");
    }
  }
);

// Create Service
export const createService = createAsyncThunk(
  "service/create",
  async (serviceData, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(POST_SERVICE_API, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(serviceData),
      });
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      dispatch(fetchServices()); // Refresh services list
      dispatch(setLoading(false));
      return data;
    } catch (error) {
      return handleApiError(error, dispatch, "Failed to create service");
    }
  }
);

// Update Service
export const updateService = createAsyncThunk(
  "service/update",
  async ({ serviceId, serviceData }, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(PUT_SERVICE_API.replace("{id}", serviceId), {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(serviceData),
      });
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      dispatch(fetchServices()); // Refresh services list
      dispatch(setLoading(false));
      return data;
    } catch (error) {
      return handleApiError(error, dispatch, "Failed to update service");
    }
  }
);

// Delete Service
export const deleteService = createAsyncThunk(
  "service/delete",
  async (serviceId, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(DELETE_SERVICE_API.replace("{id}", serviceId), {
        method: "DELETE",
      });
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      dispatch(fetchServices()); // Refresh services list
      dispatch(setLoading(false));
      return serviceId;
    } catch (error) {
      return handleApiError(error, dispatch, "Failed to delete service");
    }
  }
);