import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../../services/authService";
import styles from "./LoginForm.module.css";
import EmailInputField from "../InputField/Email/EmailInputField";
import PasswordInputField from "../InputField/Password/PasswordInputField";
import {
  validateEmail,
  validatePasswordLength,
} from "../../utils/validationUtils";


export function LoginForm() {
  const [loginData, setLoginData] = useState({
    email: "",
    password: "",
  });

  const [errors, setErrors] = useState({
    email: "",
    password: "",
  });

  const navigate = useNavigate();

  const handleLoginDataChange = (e) => {
    const { name, value } = e.target;
    setLoginData((prevData) => ({
      ...prevData,
      [name]: value,
    }));

    setErrors((prevErrors) => ({
      // Reset errors for the field being updated
      ...prevErrors,
      [name]: "",
    }));
  };

  const validateLoginForm = () => {
    const emailError = validateEmail(loginData.email);
    const passwordLengthError = validatePasswordLength(loginData.password);

    setErrors((prevErrors) => ({
      ...prevErrors,
      email: validateEmail(loginData.email),
      password: validatePasswordLength(loginData.password),
    }));

    return !emailError && !passwordLengthError;
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setErrors({ email: "", password: "" }); // Reset errors

    // If form is invalid, return early
    if (!validateLoginForm()) return;

    // If form is valid, call the auth service to login
    try {
      await login(loginData);
      // TODO: Replace the alert with a toast notification 
      alert(`Login successful! Welcome, ${loginData.email}!`);
      navigate("/");
    } catch (error) {
      console.error("Login error:", error.message);
      alert(error.message || "Login failed. Please try again.");
    }
  };

  return (
    <form
      className={styles.loginForm}
      aria-labelledby="login-title"
      onSubmit={handleSubmit}
    >
      <h1 id="login-title" className={styles.loginTitle}>
        Login Account
      </h1>

      <EmailInputField
        label="Email"
        placeholder="Email address"
        value={loginData.email}
        onChange={handleLoginDataChange}
        error={errors.email}
      />

      <PasswordInputField
        label="Password"
        placeholder="8-32 characters"
        value={loginData.password}
        onChange={handleLoginDataChange}
        error={errors.password}
      />

      <div className={styles.termsContainer}>
        <span>By signing in you agree to </span>
        <a href="/terms" className={styles.termsLink}>
          terms and conditions
        </a>
        <span> of our center.</span>
      </div>

      <button type="submit" className={styles.loginButton}>
        Login
      </button>

      <button
        type="button"
        className={styles.createAccountButton}
        onClick={() => (window.location.href = "/register")}
      >
        Create Account
      </button>
    </form>
  );
}
