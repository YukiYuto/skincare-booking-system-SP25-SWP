import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  GET_ALL_ORDERS_API,
  GET_ORDER_BY_ID_API,
  PUT_ORDER_API,
  DELETE_ORDER_API,
} from "../../config/apiConfig";
import { setOrders, setOrderDetail, setLoading, setError } from "./OrderSlice";

// Enhanced error handling utility with better logging
const handleApiError = (error, dispatch, defaultMessage) => {
  console.error("API Error Details:", {
    name: error.name,
    message: error.message,
    status: error.response?.status,
    data: error.response?.data,
    stack: error.stack
  });

  dispatch(setLoading(false));
  dispatch(
    setError({
      message: defaultMessage,
      details: error.message,
      status: error.response?.status || 500,
    })
  );

  throw error;
};

// Fetch Orders with improved error handling and logging
export const fetchOrders = createAsyncThunk(
  "order/fetchAll",
  async (_, { dispatch }) => {
    try {
      dispatch(setLoading(true));
      console.log("Fetching orders from:", GET_ALL_ORDERS_API);
      
      const response = await fetch(GET_ALL_ORDERS_API);
      console.log("Orders API response status:", response.status);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      console.log("Orders API response data:", data);
      
      // Ensure we always dispatch an array, even if API returns null/undefined
      dispatch(setOrders(Array.isArray(data) ? data : []));
      dispatch(setLoading(false));
      return data;
    } catch (error) {
      console.error("Error fetching orders:", error);
      return handleApiError(error, dispatch, "Failed to fetch orders");
    }
  }
);

// Fetch Order by ID with improved error handling and logging
export const fetchOrderById = createAsyncThunk(
  "order/fetchById",
  async (orderId, { dispatch }) => {
    try {
      if (!orderId) {
        throw new Error("Order ID is required");
      }
      
      dispatch(setLoading(true));
      const url = GET_ORDER_BY_ID_API.replace("{id}", orderId);
      console.log("Fetching order details from:", url);
      
      const response = await fetch(url);
      console.log("Order details API response status:", response.status);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      console.log("Order details API response data:", data);
      
      if (!data) {
        throw new Error(`No data found for order ID: ${orderId}`);
      }
      
      dispatch(setOrderDetail(data));
      dispatch(setLoading(false));
      return data;
    } catch (error) {
      console.error(`Error fetching order ${orderId}:`, error);
      return handleApiError(error, dispatch, `Failed to fetch order details for ID: ${orderId}`);
    }
  }
);

// Update Order with improved error handling, validation and logging
export const updateOrder = createAsyncThunk(
  "order/update",
  async ({ orderId, orderData }, { dispatch }) => {
    try {
      // Validate inputs
      if (!orderId) {
        throw new Error("Order ID is required for updating an order");
      }
      
      if (!orderData || Object.keys(orderData).length === 0) {
        throw new Error("Order data is required for updating an order");
      }
      
      dispatch(setLoading(true));
      const url = PUT_ORDER_API.replace("{id}", orderId);
      console.log("Updating order at:", url);
      console.log("Update payload:", orderData);
      
      const response = await fetch(url, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(orderData),
      });
      
      console.log("Update order API response status:", response.status);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      console.log("Update order API response data:", data);
      
      // Refresh orders list after successful update
      dispatch(fetchOrders());
      dispatch(setLoading(false));
      return data;
    } catch (error) {
      console.error(`Error updating order ${orderId}:`, error);
      return handleApiError(error, dispatch, `Failed to update order ID: ${orderId}`);
    }
  }
);

// Delete Order with improved error handling and logging
export const deleteOrder = createAsyncThunk(
  "order/delete",
  async (orderId, { dispatch }) => {
    try {
      // Validate input
      if (!orderId) {
        throw new Error("Order ID is required for deleting an order");
      }
      
      dispatch(setLoading(true));
      const url = DELETE_ORDER_API.replace("{id}", orderId);
      console.log("Deleting order at:", url);
      
      const response = await fetch(url, {
        method: "DELETE",
      });
      
      console.log("Delete order API response status:", response.status);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      // Some APIs return empty responses for DELETE operations
      let data;
      try {
        const text = await response.text();
        data = text ? JSON.parse(text) : { success: true, deletedId: orderId };
      } catch (e) {
        console.log("Delete operation successful, but no JSON response returned");
        data = { success: true, deletedId: orderId };
      }
      
      console.log("Delete order API response data:", data);
      
      // Refresh orders list after successful deletion
      dispatch(fetchOrders());
      dispatch(setLoading(false));
      return orderId;
    } catch (error) {
      console.error(`Error deleting order ${orderId}:`, error);
      return handleApiError(error, dispatch, `Failed to delete order ID: ${orderId}`);
    }
  }
);

// Create new Order thunk
export const createOrder = createAsyncThunk(
  "order/create",
  async (orderData, { dispatch }) => {
    try {
      // Validate input
      if (!orderData || Object.keys(orderData).length === 0) {
        throw new Error("Order data is required for creating a new order");
      }
      
      dispatch(setLoading(true));
      console.log("Creating new order at:", GET_ALL_ORDERS_API);
      console.log("Create payload:", orderData);
      
      const response = await fetch(GET_ALL_ORDERS_API, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(orderData),
      });
      
      console.log("Create order API response status:", response.status);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      console.log("Create order API response data:", data);
      
      // Refresh orders list after successful creation
      dispatch(fetchOrders());
      dispatch(setLoading(false));
      return data;
    } catch (error) {
      console.error("Error creating new order:", error);
      return handleApiError(error, dispatch, "Failed to create new order");
    }
  }
);