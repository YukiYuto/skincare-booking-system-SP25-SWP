import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  therapistId: null,
  slotId: null,
  customerId: null,
  appointmentDate: null,
  appointmentTime: null,
  note: null,
  orderNumber: null,
};

const bookingSlice = createSlice({
  name: "booking",
  initialState,
  reducers: {
    setBookingDetails: (state, action) => {
      return { ...state, ...action.payload };
    },

    clearBookingDetails: () => initialState,
  },
});

export const { setBookingDetails, clearBookingDetails } = bookingSlice.actions;
export default bookingSlice.reducer;
