import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  customers: [],
  customerDetail: null,
  loading: false,
  error: null,
};

const customerSlice = createSlice({
  name: "customer",
  initialState,
  reducers: {
    setCustomers: (state, action) => {
      state.customers = action.payload;
      state.loading = false;
      state.error = null;
    },
    setCustomerDetail: (state, action) => {
      state.customerDetail = action.payload;
      state.loading = false;
      state.error = null;
    },
    setLoading: (state, action) => {
      state.loading = action.payload;
    },
    setError: (state, action) => {
      state.error = action.payload;
      state.loading = false;
    },
  },
});

export const { setCustomers, setCustomerDetail, setLoading, setError } = customerSlice.actions;
export default customerSlice.reducer;
