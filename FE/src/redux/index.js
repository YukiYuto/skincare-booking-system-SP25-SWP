import { combineReducers } from "redux";
import authReducer from "./auth/slice";
import bookingReducer from "./booking/slice";
import customerReducer from "./Customer/CustomerSlice";
import serviceReducer from "./Services/ServiceSlice";
import serviceTypeReducer from "./ServiceType/ServiceTypeSlice";
import orderReducer from "./Order/OrderSlice";

const rootReducer = combineReducers({
  auth: authReducer,
  booking: bookingReducer,
  customer: customerReducer,
  service: serviceReducer,
  serviceType: serviceTypeReducer,
  order: orderReducer,
});

export default rootReducer;
