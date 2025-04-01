import { createSlice } from "@reduxjs/toolkit";
import { 
  fetchServices, 
  fetchServiceById, 
  createService, 
  updateService, 
  deleteService 
} from "./ServiceThunk";

const initialState = {
  services: {
    result: [],
    total: 0
  },
  serviceDetail: null,
  loading: false,
  error: null
};

const serviceSlice = createSlice({
  name: "service",
  initialState,
  reducers: {
    setServices: (state, action) => {
      state.services.result = action.payload || [];
      state.services.total = action.payload?.length || 0;
      state.loading = false;
      state.error = null;
    },
    setServiceDetail: (state, action) => {
      state.serviceDetail = action.payload;
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
    }
  },
  extraReducers: (builder) => {
    // Fetch Services
    builder.addCase(fetchServices.pending, (state) => {
      state.loading = true;
      state.error = null;
    }).addCase(fetchServices.fulfilled, (state, action) => {
      state.services.result = action.payload || [];
      state.services.total = action.payload?.length || 0;
      state.loading = false;
    }).addCase(fetchServices.rejected, (state, action) => {
      state.services.result = [];
      state.services.total = 0;
      state.loading = false;
      state.error = {
        message: action.error.message || 'Failed to fetch services',
        details: action.error
      };
    })
    
    // Fetch Service By ID
    .addCase(fetchServiceById.pending, (state) => {
      state.loading = true;
      state.error = null;
    }).addCase(fetchServiceById.fulfilled, (state, action) => {
      state.serviceDetail = action.payload;
      state.loading = false;
    }).addCase(fetchServiceById.rejected, (state, action) => {
      state.serviceDetail = null;
      state.loading = false;
      state.error = {
        message: action.error.message || 'Failed to fetch service details',
        details: action.error
      };
    })
    
    // Create Service
    .addCase(createService.pending, (state) => {
      state.loading = true;
      state.error = null;
    }).addCase(createService.fulfilled, (state, action) => {
      state.services.result.push(action.payload);
      state.services.total += 1;
      state.loading = false;
      state.serviceDetail = action.payload;
    }).addCase(createService.rejected, (state, action) => {
      state.loading = false;
      state.error = {
        message: action.error.message || 'Failed to create service',
        details: action.error
      };
    })
    
    // Update Service
    .addCase(updateService.pending, (state) => {
      state.loading = true;
      state.error = null;
    }).addCase(updateService.fulfilled, (state, action) => {
      const index = state.services.result.findIndex(
        service => service.id === action.payload.id
      );
      if (index !== -1) {
        state.services.result[index] = action.payload;
      }
      state.serviceDetail = action.payload;
      state.loading = false;
    }).addCase(updateService.rejected, (state, action) => {
      state.loading = false;
      state.error = {
        message: action.error.message || 'Failed to update service',
        details: action.error
      };
    })
    
    // Delete Service
    .addCase(deleteService.pending, (state) => {
      state.loading = true;
      state.error = null;
    }).addCase(deleteService.fulfilled, (state, action) => {
      state.services.result = state.services.result.filter(
        service => service.id !== action.payload
      );
      state.services.total -= 1;
      state.serviceDetail = null;
      state.loading = false;
    }).addCase(deleteService.rejected, (state, action) => {
      state.loading = false;
      state.error = {
        message: action.error.message || 'Failed to delete service',
        details: action.error
      };
    });
  }
});

export const { 
  setServices, 
  setServiceDetail, 
  setLoading, 
  setError, 
  clearError 
} = serviceSlice.actions;

export default serviceSlice.reducer;