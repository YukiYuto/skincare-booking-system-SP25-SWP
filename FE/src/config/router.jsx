import { createBrowserRouter, Navigate } from "react-router-dom";
import Home from "../Pages/HomePage/HomePage";
import { LoginPage } from "../Pages/LoginPage/LoginPage";
import Contact from "../Pages/ContactPage/Contact";
import AboutPage from "../Pages/AboutPage/AboutPage";
import { RegisterPage } from "../Pages/Register/RegisterPage";
import AllService from "../Pages/ServiceAll/AllService";
import ServiceDetail from "../Pages/ServiceDetail/ServiceDetail";
import { ForgotPage } from "../Pages/ForgotPage/ForgotPage";
import { ResetPage } from "../Pages/ResetPage/ResetPage";
import CustomerProfile from "../Pages/CustomerProfile/CustomerProfile";
import VerifyEmail from "../Components/VerifyEmail/VerifyEmail";
import Dashboard from "../Pages/DashBoard/Dashboard";
import Revenue from "../Components/DashboardComponent/Tabs/Revenue/Revenue";
import Customers from "../Components/DashboardComponent/Tabs/Customers/Customers";
import SkinTherapists from "../Components/DashboardComponent/Tabs/Therapists/SkinTherapists";
import Services from '../Components/DashboardComponent/Tabs/Services/Services';
import Orders from "../Components/DashboardComponent/Tabs/Orders/Orders";
import Schedule from "../Components/DashboardComponent/Tabs/Schedule/Schedule";
import ErrorPage from "../Pages/ErrorPage/ErrorPage";
import TableCustomer from "../Components/TableCustomer/TableCustomer";
import PaymentPage from "../Pages/PaymentPage/PaymentPage";


export const router = createBrowserRouter([
  { path: "/", element: <Home /> },
  { path: "login", element: <LoginPage /> },
  { path: "register", element: <RegisterPage /> },
  { path: "contact", element: <Contact /> },
  { path: "about", element: <AboutPage /> },
  { path: "services", element: <AllService /> },
  { path: "services/:id", element: <ServiceDetail /> },

  // Manager Dashboard with Nested Routes
  {
    path: "dashboard",
    element: <Dashboard />,
    children: [
      { path: "revenue", element: <Revenue /> },
      { path: "customers", element: <Customers /> },
      { path: "therapists", element: <SkinTherapists /> },
      { path: "services", element: <Services /> },
      { path: "orders", element: <Orders /> },
      { path: "schedule", element: <Schedule /> },
      { index: true, element: <Revenue /> }, // Default dashboard page
    ],
  },
  {
    path: "profile",
    element: <CustomerProfile />,
  },
  {
    path: "forgot-password",
    element: <ForgotPage />,
  },
  {
    path: "reset-password",
    element: <ResetPage />,
  },
  {
    path: "verify-email",
    element: <VerifyEmail />,
  },
  {
    path: "error",
    element: <ErrorPage />
  },
  {
    path: "*",
    element: <Navigate to="/error" replace />,
  },
  {
    path: "table-customer",
    element: <TableCustomer />,
  },
  {
    path: "/payment",
    element: <PaymentPage />,
  }
]);
