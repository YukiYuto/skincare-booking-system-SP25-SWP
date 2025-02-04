  import { RouterProvider } from "react-router-dom";
  import { router } from "./config/router";
  import { Provider } from "react-redux";
  import { persistor, store } from "./redux/store";
  import { PersistGate } from "redux-persist/integration/react";
  import { StateProvider } from "./Context/StateProvider";
  import { UserProvider } from "./Context/UserContext";
  function App() {

    return (
      <>
        <UserProvider>
          <Provider store={store}>
            <PersistGate loading={null} persistor={persistor}>
              <StateProvider>
                <RouterProvider router={router} />
              </StateProvider>
            </PersistGate>
          </Provider>
        </UserProvider>
      </>
    );
  }

  export default App;
