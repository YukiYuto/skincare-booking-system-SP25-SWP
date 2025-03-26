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

export function LoginForm() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [isPasswordVisible, setIsPasswordVisible] = useState(false);

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

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    setErrors({ email: "", password: "" }); // Reset errors

    // If form is invalid, return early
    if (!validateLoginForm()) return;

    // If form is valid, call the auth service to login
    try {
      await dispatch(loginAction(loginData)).unwrap();
      toast.success(`Login successful! Welcome, ${loginData.email}!`);
      navigate("/");
    } catch (error) {
      console.error("Login error", error.message);
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

      <InputField
        label="Username"
        type="text"
        id="username"
        placeholder="3-15 characters"
        value={username}
        onChange={handleUsernameChange}
        minLength={3}
        maxLength={15}
      />

      <InputField
        label="Password"
        type={isPasswordVisible ? "text" : "password"}
        id="password"
        placeholder="6-32 characters"
        value={password}
        onChange={handlePasswordChange}
        showPasswordToggle
        minLength={6}
        maxLength={30}
      />
      
      <div>
      <a 
      style={{
            fontSize: "20px",
            pointerEvents: loading ? "none" : "auto", // Ngăn nhấn khi loading
            opacity: loading ? 0.5 : 1, // Làm mờ khi loading
          }}
      href="forgot-password" 
      className={styles.forgotLink}>
          Forgot Password
        </a>
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
