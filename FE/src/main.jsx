import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.jsx";
import './styles/index.css'
import "react-toastify/dist/ReactToastify.css";
import { ToastContainer } from "react-toastify";
ReactDOM.createRoot(document.getElementById("root")).render(
  <>
    <App />
    <ToastContainer autoClose={1500} pauseOnFocusLoss={false}/>
  </>
);
