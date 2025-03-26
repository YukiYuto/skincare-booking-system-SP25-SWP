import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  serviceTypes: [],
  serviceTypeDetail: null,
  loading: false,
  error: null,
};

const serviceTypeSlice = createSlice({
  name: "serviceType",
  initialState,
  reducers: {
    setServiceTypes: (state, action) => {
      state.serviceTypes = action.payload;
      state.loading = false;
      state.error = null;
    },
    setServiceTypeDetail: (state, action) => {
      state.serviceTypeDetail = action.payload;
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

export const { setServiceTypes, setServiceTypeDetail, setLoading, setError } =
  serviceTypeSlice.actions;
export default serviceTypeSlice.reducer;
