import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { Provider, useSelector } from "react-redux";
import { store } from "./redux/store";

// Import các page
import Home from "./Pages/HomePage/HomePage";
import { LoginPage } from "./Pages/LoginPage/LoginPage";
import Contact from "./Pages/ContactPage/Contact";
import AboutPage from "./Pages/AboutPage/AboutPage";
import { RegisterPage } from "./Pages/Register/RegisterPage";
import AllService from "./Pages/ServiceAll/AllService";
import ServiceDetail from "./Pages/ServiceDetail/ServiceDetail";
import { ForgotPage } from "./Pages/ForgotPage/ForgotPage";
import { ResetPage } from "./Pages/ResetPage/ResetPage";
import CustomerProfile from "./Pages/CustomerProfile/CustomerProfile";
import VerifyEmail from "./Components/VerifyEmail/VerifyEmail";
import Dashboard from "./Pages/DashBoard/Dashboard";
import Revenue from "./Components/DashboardComponent/Tabs/Revenue/Revenue";
import Customers from "./Components/DashboardComponent/Tabs/Customers/Customers";
import SkinTherapists from "./Components/DashboardComponent/Tabs/Therapists/SkinTherapists";
import Services from "./Components/DashboardComponent/Tabs/Services/Services";
import Orders from "./Components/DashboardComponent/Tabs/Orders/Orders";
import Schedule from "./Components/DashboardComponent/Tabs/Schedule/Schedule";
import ErrorPage from "./Pages/ErrorPage/ErrorPage";
import PaymentConfirmationPage from "./Pages/Payment/PaymentConfirmationPage";
import TherapistCard from "./Components/TherapistCard/TherapistCard";
import TherapistDetail from "./Components/TherapistDetail/TherapistDetail";
import TherapistManagement from "./Pages/Therapist/TherapistManagement/TherapistManagement";
import AppointmentPage from "./Pages/Appointment/AppointmentPage";
import BlogForCus from "./Components/BlogForCus/BlogForCus";
import BlogForCusList from "./Components/BlogForCus/BlogForCusList/BlogForCusList";
import StaffBlogManagement from "./Pages/Staff/StaffBlogManagement/StaffBlogManagement";
import StaffBlogDetail from "./Pages/Staff/StaffBlogDetail/StaffBlogDetail";
import ViewBlogCategory from "./Components/DashboardComponent/Tabs/BlogCategory/ViewBlogCategory/ViewBlogCategory";
import ViewDetail from "./Components/DashboardComponent/Tabs/BlogCategory/ViewDetail/ViewDetail";
import DashboardTherapist from "./Pages/Therapist/DashboardTherapist/DashboardTherapist";

const AppRoutes = () => {
  const { user, accessToken } = useSelector((state) => state.auth);
  const roles = user?.roles || [];

  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="contact" element={<Contact />} />
      <Route path="about" element={<AboutPage />} />
      <Route path="services" element={<AllService />} />
      <Route path="services/:id" element={<ServiceDetail />} />
      <Route path="therapist" element={<TherapistCard />} />
      <Route path="therapist/:therapistId" element={<TherapistDetail />} />
      <Route path="blogs" element={<BlogForCus />} />
      <Route path="blogs/:categoryName?" element={<BlogForCus />} />
      <Route path="blogs-detail/:title" element={<BlogForCusList />} /> 
      <Route path="error" element={<ErrorPage />} />
      {!accessToken && (
        <>
          <Route path="login" element={<LoginPage />} />
          <Route path="register" element={<RegisterPage />} />
          <Route path="forgot-password" element={<ForgotPage />} />
          <Route path="reset-password" element={<ResetPage />} />
          <Route path="verify-email" element={<VerifyEmail />} />
        </>
      )}
      {accessToken && (
        <>
          {roles.includes("CUSTOMER") && (
            <>
              <Route path="profile" element={<CustomerProfile />} />
              <Route path="payment-confirmation" element={<PaymentConfirmationPage />} />
              <Route path="appointments" element={<AppointmentPage />} />
            </>
          )}
          {roles.includes("ADMIN") && (
            <>
              <Route path="profile" element={<CustomerProfile />} />
              <Route path="dashboard" element={<Dashboard />}>
                <Route index element={<Revenue />} />
                <Route path="revenue" element={<Revenue />} />
                <Route path="customers" element={<Customers />} />
                <Route path="therapists" element={<SkinTherapists />} />
                <Route path="services" element={<Services />} />
                <Route path="orders" element={<Orders />} />
                <Route path="schedule" element={<Schedule />} />
                <Route path="view-blogcategory" element={<ViewBlogCategory />} />
                <Route path="view-blogcategory/:categoryName?" element={<ViewBlogCategory />} />
                <Route path="view-detail/:title" element={<ViewDetail />} /> 
              </Route>
            </>
          )}
          {roles.includes("STAFF") && (
            <>
              <Route path="profile" element={<CustomerProfile />} />
              <Route path="staff-blogs" element={<StaffBlogManagement />} />
              <Route path="staff-blogs/:categoryName?" element={<StaffBlogManagement />} />
              <Route path="detail/:title" element={<StaffBlogDetail />} /> 
            </>
          )}
          {roles.includes("SKINTHERAPIST") && (
            <>
              <Route path="profile" element={<CustomerProfile />} />
              <Route path="therapist-management" element={<TherapistManagement />} />
              <Route path="therapist-dashboard" element={<DashboardTherapist />} />
            </>
          )}
          {roles.includes("MANAGER") && (
            <>
              <Route path="profile" element={<CustomerProfile />} />
              <Route path="dashboard" element={<Dashboard />}>
                <Route index element={<Revenue />} />
                <Route path="revenue" element={<Revenue />} />
                <Route path="customers" element={<Customers />} />
                <Route path="therapists" element={<SkinTherapists />} />
                <Route path="services" element={<Services />} />
                <Route path="orders" element={<Orders />} />
                <Route path="schedule" element={<Schedule />} />
                <Route path="view-blogcategory" element={<ViewBlogCategory />} />
                <Route path="view-blogcategory/:categoryName?" element={<ViewBlogCategory />} />
                <Route path="view-detail/:title" element={<ViewDetail />} /> 
              </Route>
              
            </>
          )}
        </>
      )}
      <Route path="*" element={<Navigate to="/error" replace />} />
    </Routes>
  );
};

const App = () => {
  return (
    <Provider store={store}>
      <Router>
        <AppRoutes />
      </Router>
    </Provider>
  );
};

export default App;
