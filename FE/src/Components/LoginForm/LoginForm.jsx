import { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { login as loginAction } from "../../redux/auth/thunks";
import EmailInputField from "../InputField/Email/EmailInputField";
import PasswordInputField from "../InputField/Password/PasswordInputField";
import { isAccountUnverified, validateEmail, validatePassword } from "../../utils/validationUtils";
import styles from "./LoginForm.module.css";
import { sendVerificationEmail } from "../../services/authService";

export function LoginForm() {
  const [loginData, setLoginData] = useState({
    email: "",
    password: "",
  });

  const [errors, setErrors] = useState({
    email: "",
    password: "",
  });

  const [isAccountVerified, setIsAccountVerified] = useState(true);

  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { loading, error: reduxError } = useSelector((state) => state.auth);

  useEffect(() => {
    if (reduxError) {
      toast.error(reduxError.message || "An unexpected error occurred.");
    }
  }, [reduxError]);

  const handleLoginDataChange = (e) => {
    const { name, value } = e.target;
    setLoginData((prevData) => ({ ...prevData, [name]: value }));
    // Reset errors for the field being updated
    setErrors((prevErrors) => ({ ...prevErrors, [name]: "" }));
  };

  const validateLoginForm = () => {
    const emailError = validateEmail(loginData.email.trim());
    const passwordFormatError = validatePassword(loginData.password);

    setErrors((prevErrors) => ({
      ...prevErrors,
      email: emailError,
      password: passwordFormatError,
    }));

    return !emailError && !passwordFormatError;
  };

  const handleVerifyEmail = async () => {
    reduxError && toast.dismiss(); // Dismiss any previous error to show new error
    try {
      await sendVerificationEmail(loginData.email);
      toast.success(
        "Verification email sent successfully! Please check your email inbox."
      );
    } catch (error) {
      console.error("Send verification email error", error.message);
      toast.error("Failed to send verification email. Please try again.");
    }
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const trimmedData = {
      ...loginData,
      email: loginData.email.trim(),
      password: loginData.password.trim(),
    };
    setLoginData(trimmedData);
    setErrors({ email: "", password: "" }); // Reset errors

    // If form is invalid, return early
    if (!validateLoginForm()) return;
    // If form is valid, call the auth service to login
    try {
      const userData = await dispatch(loginAction(trimmedData)).unwrap();
      setIsAccountVerified(true);
      toast.success(`Welcome, ${userData.fullName}!`);
      if (userData.roles.includes("CUSTOMER")) {
        navigate("/");
      } else if (userData.roles.includes("ADMIN")) {
        navigate("/dashboard");
      } else if (userData.roles.includes("STAFF")) {
        navigate("/staff-management");
      } else if (userData.roles.includes("SKINTHERAPIST")) {
        navigate("/therapist-management");
      } else if (userData.roles.includes("MANAGER")) {
        navigate("/dashboard");
      }
    } catch (error) {
      if (isAccountUnverified(error)) {
        setIsAccountVerified(false);
      }
    }
  };

  return (
    <form
      className={`${styles.loginForm} ${loading ? styles.disabledForm : ""}`}
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

      <div className={styles.forgotContainer}>
        <a
          href="forgot-password"
          className={`${styles.forgotLink} ${
            loading ? styles.disabledLink : ""
          }`}
        >
          <span>Forgot Password</span>
        </a>
      </div>
      {!isAccountVerified && (
        <div className={styles.unverifiedContainer}>
          <span>
            Your account is not verified.{" "}
            <span
              type="text"
              className={styles.unverifiedText}
              onClick={handleVerifyEmail}
            >
              Click here
            </span>{" "}
            to verify your email.
          </span>
        </div>
      )}
      <div className={styles.termsContainer}>
        <span>By signing in you agree to </span>
        <a
          href="/terms"
          className={`${styles.termsLink} ${
            loading ? styles.disabledLink : ""
          }`}
        >
          terms and conditions
        </a>
        <span> of our center.</span>
      </div>

      <button type="submit" className={styles.loginButton} disabled={loading}>
        {loading ? "Logging in..." : "Login"}
      </button>

      <button
        type="button"
        className={styles.createAccountButton}
        onClick={() => {
          if (!loading) {
            window.location.href = "/register";
          }
        }}
        disabled={loading}
      >
        Create Account
      </button>
      
    </form>
  );
}
