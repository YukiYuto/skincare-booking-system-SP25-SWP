import React from "react";
import { RouterProvider } from "react-router-dom";
import { router } from "./config/router";
import { Provider } from "react-redux";
import { store } from "./redux/store";

function App() {
  return (
    <React.StrictMode>
       <Provider store={store}>
          <RouterProvider router={router} />
       </Provider>
    </React.StrictMode>
  );
}

export default App;
