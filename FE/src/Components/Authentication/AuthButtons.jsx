/* eslint-disable no-unused-vars */
import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";
import { logout as logoutAction } from "../../redux/auth/thunks";
import styles from "./AuthButtons.module.css";

const AuthButtons = () => {
  const { isAuthenticated, user, loading } = useSelector((state) => state.auth);
  const dispatch = useDispatch();

  const handleLogout = () => {
    dispatch(logoutAction());
  };

  return isAuthenticated ? (
    <div className={styles.profileDropdown}>
      <span>{user?.name || "Profile"}</span>
      <ul>
        <li>
          <Link to="/profile">Profile</Link>
        </li>
        <li>
          <Link to="/orders">Orders</Link>
        </li>
        <li>
          <button onClick={handleLogout} disabled={loading}>
            {loading ? "Logging out..." : "Logout"}
          </button>
        </li>
      </ul>
    </div>
  ) : (
    <>
      <Link to="/register" className={styles.registerButton}>
        Register
      </Link>
      <Link to="/login" className={styles.loginButton + " " + "text-white"}>
        Login
      </Link>
    </>
  );
};

export default AuthButtons;
