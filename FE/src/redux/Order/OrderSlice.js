import { createSlice } from "@reduxjs/toolkit";
import {
  fetchOrders,
  fetchOrderById,
  updateOrder,
  deleteOrder,
} from "./OrderThunk";

const initialState = {
  orders: {
    result: [],
    total: 0,
  },
  orderDetail: null,
  loading: false,
  error: null,
};

const orderSlice = createSlice({
  name: "order",
  initialState,
  reducers: {
    setOrders: (state, action) => {
      state.orders.result = action.payload || [];
      state.orders.total = action.payload?.length || 0;
      state.loading = false;
      state.error = null;
    },
    setOrderDetail: (state, action) => {
      state.orderDetail = action.payload;
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
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    // Fetch Orders
    builder
      .addCase(fetchOrders.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchOrders.fulfilled, (state, action) => {
        state.orders.result = action.payload || [];
        state.orders.total = action.payload?.length || 0;
        state.loading = false;
      })
      .addCase(fetchOrders.rejected, (state, action) => {
        state.orders.result = [];
        state.orders.total = 0;
        state.loading = false;
        state.error = {
          message: action.error.message || "Failed to fetch orders",
          details: action.error,
        };
      })

      // Fetch Order By ID
      .addCase(fetchOrderById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchOrderById.fulfilled, (state, action) => {
        state.orderDetail = action.payload;
        state.loading = false;
      })
      .addCase(fetchOrderById.rejected, (state, action) => {
        state.orderDetail = null;
        state.loading = false;
        state.error = {
          message: action.error.message || "Failed to fetch order details",
          details: action.error,
        };
      })

      // Create Order
      .addCase(updateOrder.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateOrder.fulfilled, (state, action) => {
        state.orders.result.push(action.payload);
        state.orders.total += 1;
        state.loading = false;
        state.orderDetail = action.payload;
      })
      .addCase(updateOrder.rejected, (state, action) => {
        state.loading = false;
        state.error = {
          message: action.error.message || "Failed to create order",
          details: action.error,
        };
      })

      // Delete Order
      .addCase(deleteOrder.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteOrder.fulfilled, (state, action) => {
        state.orders.result = state.orders.result.filter(
          (order) => order.id !== action.payload
        );
        state.orders.total -= 1;
        state.orderDetail = null;
        state.loading = false;
      })
      .addCase(deleteOrder.rejected, (state, action) => {
        state.loading = false;
        state.error = {
          message: action.error.message || "Failed to delete order",
          details: action.error,
        };
      });
  },
});

export const { setOrders, setOrderDetail, setLoading, setError, clearError } =
  orderSlice.actions;

export default orderSlice.reducer;
