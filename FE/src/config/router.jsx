import { createBrowserRouter } from "react-router-dom";
import Home from "../Pages/HomePage/HomePage";
import { LoginPage } from "../Pages/LoginPage/LoginPage";
import Contact from "../Pages/ContactPage/Contact";
import AboutPage from "../Pages/AboutPage/AboutPage";
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
    path: "contact",
    element: <Contact />,
  },
  {
    path: "about",
    element: <AboutPage />,
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
