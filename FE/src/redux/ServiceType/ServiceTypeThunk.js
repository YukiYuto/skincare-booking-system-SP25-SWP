import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  GET_ALL_SERVICE_TYPES_API,
  GET_SERVICE_TYPE_BY_ID_API,
  POST_SERVICE_TYPE_API,
  PUT_SERVICE_TYPE_API,
  DELETE_SERVICE_TYPE_API,
} from "../../config/apiConfig";
import {
  setServiceTypes,
  setServiceTypeDetail,
  setLoading,
  setError,
} from "./ServiceTypeSlice";

// API to fetch all service types
export const fetchServiceTypes = createAsyncThunk(
  "serviceType/fetchAll",
  async (_, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(GET_ALL_SERVICE_TYPES_API);
      const data = await response.json();
      dispatch(setServiceTypes(data));
    } catch (error) {
      dispatch(setError(error.message || "Failed to fetch service types"));
    }
  }
);

// API to fetch service type details by ID
export const fetchServiceTypeById = createAsyncThunk(
  "serviceType/fetchById",
  async (serviceTypeId, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(
        GET_SERVICE_TYPE_BY_ID_API.replace("{serviceTypeId}", serviceTypeId)
      );
      const data = await response.json();
      dispatch(setServiceTypeDetail(data));
    } catch (error) {
      console.trace(error);
      dispatch(
        setError(error.message || "Failed to fetch service type details")
      );
    }
  }
);

// API to create a new service type
export const createServiceType = createAsyncThunk(
  "serviceType/create",
  async (serviceTypeData, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(POST_SERVICE_TYPE_API, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(serviceTypeData),
      });
      const data = await response.json();
      dispatch(fetchServiceTypes()); // Refresh the list of service types
    } catch (error) {
      dispatch(setError(error.message || "Failed to create service type"));
    }
  }
);

// API to update an existing service type
export const updateServiceType = createAsyncThunk(
  "serviceType/update",
  async ({ serviceTypeId, serviceTypeData }, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      const response = await fetch(
        PUT_SERVICE_TYPE_API.replace("{id}", serviceTypeId),
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(serviceTypeData),
        }
      );
      const data = await response.json();
      dispatch(fetchServiceTypes()); // Refresh the list of service types
    } catch (error) {
      dispatch(setError(error.message || "Failed to update service type"));
    }
  }
);

// API to delete a service type
export const deleteServiceType = createAsyncThunk(
  "serviceType/delete",
  async (serviceTypeId, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      await fetch(DELETE_SERVICE_TYPE_API.replace("{id}", serviceTypeId), {
        method: "DELETE",
      });
      dispatch(fetchServiceTypes()); // Refresh the list of service types
    } catch (error) {
      dispatch(setError(error.message || "Failed to delete service type"));
    }
  }
);
