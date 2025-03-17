import { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { login as loginAction } from "../../redux/auth/thunks";
import EmailInputField from "../InputField/Email/EmailInputField";
import PasswordInputField from "../InputField/Password/PasswordInputField";
import {
  validateEmail,
  validatePasswordLength,
} from "../../utils/validationUtils";
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
    const emailError = validateEmail(loginData.email);
    const passwordLengthError = validatePasswordLength(loginData.password);

    setErrors((prevErrors) => ({
      ...prevErrors,
      email: emailError,
      password: passwordLengthError,
    }));

    return !emailError && !passwordLengthError;
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setErrors({ email: "", password: "" }); // Reset errors
  
    if (!validateLoginForm()) return;
  
    try {
      const userData = await dispatch(loginAction(loginData)).unwrap();
      toast.success(`Login successful! Welcome, ${loginData.email}!`);
      if(userData.roles.includes("CUSTOMER") ){
        navigate("/");
      } else if (userData.roles.includes("ADMIN") ){
        navigate("/dashboard");
      } else if (userData.roles.includes("STAFF") ){
        navigate("/staff-management");
      } else if (userData.roles.includes("THERAPIST") ){
        navigate("/therapist-management");
      } else if (userData.roles.includes("MANAGER") ){
        navigate("/dashboard");
      }  
      
    } catch (error) {
      console.error("Login error", error.message);
  
      if (error.message === "You need to confirm email!") {
        setErrors((prevErrors) => ({
          ...prevErrors,
          email: "You need to confirm your email!",
        }));
      } else {
        toast.error(error.message || "An unexpected error occurred.");
      }
    }
  };
  

  const handleSendVerifyEmail = async () => {
    try {
      await sendVerificationEmail(loginData.email);
      toast.success("Verification email sent. Please check your inbox.");
    } catch (error) {
      toast.error("Failed to send verification email.");
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

      {errors.email && (
        <div>
          <a 
            style={{
              fontSize: "20px",
              color: "red",
              pointerEvents: loading ? "none" : "auto",
              opacity: loading ? 0.5 : 1,
              cursor: "pointer",
            }} 
            onClick={handleSendVerifyEmail}
            className={styles.verifyLink}
          >
            Click here to send verify!
          </a>
        </div>
      )}

      
      <div>
      <a 
      style={{
            fontSize: "20px",
            pointerEvents: loading ? "none" : "auto", // Ngăn nhấn khi loading
            opacity: loading ? 0.5 : 1, // Làm mờ khi loading
          }}
      href="forgot-password" 
      className={styles.forgotLink}>
          Forgot Password?
        </a>
      </div>
      
      <div>
          <p>Do not have an account? <a 
              className={styles.registerLink}
              href="/register" 
              disabled={loading} 
            >
            Sign up
          </a>
        </p>
      </div>

      <div className={styles.termsContainer}>
        <span>By signing in you agree to </span>
        <a 
        style={{ pointerEvents: loading ? "none" : "auto", opacity: loading ? 0.5 : 1 }}
        href="/terms" className={styles.termsLink}>
          terms and conditions
        </a>
        <span> of our center.</span>
      </div>

      <button type="submit" className={styles.loginButton} disabled={loading}>
        {loading ? "Logging in..." : "Login"}
      </button>

      
    </form>
  );
}
