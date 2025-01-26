import React from "react";

import { createBrowserRouter } from "react-router-dom";
import { HomePage } from "../Pages/HomePage/HomePage";
import { LoginPage } from "../Pages/LoginPage/LoginPage";
import RegisterPage from "../Pages/Register/RegisterPage";
//import Member from "../page/member";
//import Dashboard from "../page/admin/dashboard";
//import Inventory from "../page/admin/inventory";
//import Services from "../page/admin/services";
//import Orders from "../page/admin/orders";
//import Users from "../page/admin/users";
//import Admin from "../page/admin";
//import Product from "../page/product";
//import Service from "../page/service";
//import Stylist from "../page/stylist";
//import StylistProfile from "../page/stylist-detail";
//import Contact from "../page/contact";

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
    path: "member",
    //element: <Member />,
  },
  {
    path: "admin",
    //element: <Admin />,
    children: [
      {
        path: "dashboard",
        //      element: <Dashboard />, // Component cho trang Dashboard
      },
      {
        path: "services",
        //     element: <Services />, // Component cho trang Dashboard
      },
      {
        path: "inventory",
        //   element: <Inventory />, // Component cho trang Inventory
      },
      {
        path: "orders",
        //  element: <Orders />, // Component cho trang Orders
      },
      {
        path: "users",
        //element: <Users />, // Component cho trang Users
      },
    ],
  },
  {
    path: "product",
    //    element: <Product />,
  },
  {
    path: "service",
    //    element: <Service />,
  },
  {
    path: "stylist",
    //   element: <Stylist />,
  },
  {
    path: "stylist/:id",
    //    element: <StylistProfile />,
  },
  {
    path: "contact",
    //  element: <Contact />,
  },
]);
