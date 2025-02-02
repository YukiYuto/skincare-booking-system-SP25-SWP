import React, { useState } from "react";
import styles from "./LoginForm.module.css";
import { InputField } from "../InputField/InputField";

export function LoginForm() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [isPasswordVisible, setIsPasswordVisible] = useState(false);

  const handleUsernameChange = (event) => {
    setUsername(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handlePasswordVisibility = () => {
    setIsPasswordVisible(!isPasswordVisible);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    // Add your login logic here
    console.log("Login form submitted with username:", username, "and password:", password);
  };

  return (
    <form className={styles.loginForm} aria-labelledby="login-title" onSubmit={handleSubmit}>
      <h1 id="login-title" className={styles.loginTitle}>
        Login Account
      </h1>

      <InputField
        label="Username"
        type="text"
        id="username"
        placeholder="Username"
        value={username}
        onChange={handleUsernameChange}
      />

      <InputField
        label="Password"
        type={isPasswordVisible ? "text" : "password"}
        id="password"
        placeholder="6+ characters"
        value={password}
        onChange={handlePasswordChange}
        showPasswordToggle
        onPasswordVisibilityChange={handlePasswordVisibility}
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