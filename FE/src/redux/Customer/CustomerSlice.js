import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { GET_ALL_CUSTOMERS_API, GET_CUSTOMER_BY_ID_API } from "../../api/apiConfig";

// Async Thunk: Fetch danh sách Customers
export const fetchCustomers = createAsyncThunk(
  "customers/fetchAll",
  async (_, { rejectWithValue }) => {
    try {
      const response = await fetch(GET_ALL_CUSTOMERS_API);
      if (!response.ok) throw new Error("Failed to fetch customers");
      return await response.json();
    } catch (error) {
      return rejectWithValue(error.message);
    }
  }
);

// Async Thunk: Fetch Customer theo ID
export const fetchCustomerById = createAsyncThunk(
  "customers/fetchById",
  async (id, { rejectWithValue }) => {
    try {
      const response = await fetch(GET_CUSTOMER_BY_ID_API.replace("{customerId}", id));
      if (!response.ok) throw new Error("Customer not found");
      return await response.json();
    } catch (error) {
      return rejectWithValue(error.message);
    }
  }
);

const customerSlice = createSlice({
  name: "customers",
  initialState: {
    customers: [],
    selectedCustomer: null,
    loading: false,
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch all customers
      .addCase(fetchCustomers.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchCustomers.fulfilled, (state, action) => {
        state.loading = false;
        state.customers = action.payload;
      })
      .addCase(fetchCustomers.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      // Fetch customer by ID
      .addCase(fetchCustomerById.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchCustomerById.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedCustomer = action.payload;
      })
      .addCase(fetchCustomerById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      });
  },
});

export default customerSlice.reducer;
