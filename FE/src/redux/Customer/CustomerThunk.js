import { createAsyncThunk } from "@reduxjs/toolkit";
import { GET_ALL_CUSTOMERS_API, GET_CUSTOMER_BY_ID_API } from "../../config/apiConfig";
import { setCustomers, setCustomerDetail, setLoading, setError } from "./CustomerSlice";

// API lấy danh sách Customers
export const fetchCustomers = createAsyncThunk("customer/fetchAll", async (_, { dispatch }) => {
  try {
    dispatch(setLoading(true));
    const response = await fetch(GET_ALL_CUSTOMERS_API);
    const data = await response.json();
    dispatch(setCustomers(data));
  } catch (error) {
    dispatch(setError(error.message || "Failed to fetch customers"));
  }
});

// API lấy chi tiết 1 Customer
export const fetchCustomerById = createAsyncThunk("customer/fetchById", async (customerId, { dispatch }) => {
  try {
    dispatch(setLoading(true));
    const response = await fetch(GET_CUSTOMER_BY_ID_API.replace("{customerId}", customerId));
    const data = await response.json();
    dispatch(setCustomerDetail(data));
  } catch (error) {
    console.trace(error);
    dispatch(setError(error.message || "Failed to fetch customer details"));
  }
});
