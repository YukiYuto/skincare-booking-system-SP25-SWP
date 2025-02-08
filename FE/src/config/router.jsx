import { createBrowserRouter } from "react-router-dom";
import { HomePage } from "../Pages/HomePage/HomePage";
import { LoginPage } from "../Pages/LoginPage/LoginPage";
import RegisterPage from "../Pages/Register/RegisterPage";
import AllService from "../Pages/ServiceAll/AllService";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <HomePage />,
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
]);
