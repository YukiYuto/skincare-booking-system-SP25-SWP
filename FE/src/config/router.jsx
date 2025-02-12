import { createBrowserRouter } from "react-router-dom";
import Home from "../Pages/HomePage/HomePage";
import { LoginPage } from "../Pages/LoginPage/LoginPage";
import { RegisterPage } from "../Pages/Register/RegisterPage";
import AllService from "../Pages/ServiceAll/AllService";
import ServiceDetail from "../Pages/ServiceDetail/ServiceDetail";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Home />,
  },
  {
    path: "login",
    element: <LoginPage />,
  },
  {
    path: "register",
    element: <RegisterPage />,
  },
  {
    path: "services",
    element: <AllService />,
  },
  {
    path: "services/:id",
    element: <ServiceDetail />,
  },
]);
