import { createSlice } from "@reduxjs/toolkit";
import Cookies from "js-cookie";

const initialState = {
  isAuthenticated: false,
  user: null,
  accessToken: null,
  refreshToken: Cookies.get("refreshToken") || null,
  loading: false,
  error: null,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setUser: (state, action) => {
      state.isAuthenticated = true;
      state.user = action.payload;
      state.loading = false;
      state.error = null;
    },
    updateUser: (state, action) => {
      if (state.user) {
        state.user = { ...state.user, ...action.payload }; // Cập nhật thông tin user
      }
    },
    clearUser: (state) => {
      state.isAuthenticated = false;
      state.user = null;
      state.accessToken = null;
      state.refreshToken = null;
      Cookies.remove("refreshToken");
    },
    setTokens: (state, action) => {
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      Cookies.set("refreshToken", action.payload.refreshToken, { sameSite: "strict" });
    },
    setLoading: (state, action) => {
      state.loading = action.payload;
    },
    setError: (state, action) => {
      state.error = action.payload;
      state.loading = false;
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase("auth/login/pending", (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase("auth/login/fulfilled", (state, action) => {
        state.isAuthenticated = true;
        state.user = action.payload;
        state.loading = false;
      })
      .addCase("auth/login/rejected", (state, action) => {
        state.isAuthenticated = false;
        state.user = null;
        state.loading = false;
        state.error = action.payload;
      });
  },
});

export const { setUser, clearUser, updateUser, setTokens, setLoading, setError } = authSlice.actions;

export default authSlice.reducer;
