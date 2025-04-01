import { configureStore } from "@reduxjs/toolkit";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage";
import rootReducer from "./index";
import { setupInterceptors } from "../config/axios";

const persistConfig = {
  key: "root",
  storage,
  whitelist: ["auth", "booking"],
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

const store = configureStore({
  reducer: persistedReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: false, // Disable check for non-serializable values, like axios errors
    }),
});

const persistor = persistStore(store);

setupInterceptors(store);

export { store, persistor };
