import { combineReducers } from "redux";
import authReducer from "./auth/slice";
import customerReducer from "./Customer/CustomerSlice";
import serviceReducer from "./Services/ServiceSlice";
import serviceTypeReducer from "./ServiceType/ServiceTypeSlice";

const rootReducer = combineReducers({
  auth: authReducer,
  customer: customerReducer,
  service: serviceReducer,
  serviceType: serviceTypeReducer,
});

export default rootReducer;
