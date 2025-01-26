import React from "react";
import styles from "./registerForm.module.css";
import { InputField } from "../InputField/InputField";

function RegisterForm() {
  return (
    <form className={styles.registerForm} aria-labelledby="register-title">
      <h1 id="register-title" className={styles.registerTitle}>
        Register
      </h1>

      <InputField
        label="Username"
        type="text"
        id="username"
        placeholder="Username"
      />

      <InputField
        label="Phone Number"
        type="number"
        id="phone"
        placeholder="Phone Number"
      />

      <InputField
        label="Email Address"
        type="text"
        id="email"
        placeholder="Email Address"
      />

      <InputField
        label="Password"
        type="password"
        id="password"
        placeholder="6+ characters"
        showPasswordToggle
      />

      <div className={styles.termsContainer}>
        <span>By registering you agree to </span>
        <a href="/terms" className={styles.termsLink}>
          terms and conditions
        </a>
        <span> of our center.</span>
      </div>

      <button type="submit" className={styles.registerButton}>
        Register
      </button>

      <button
        type="button"
        className={styles.loginButton}
        onClick={() => (window.location.href = "/login")}
      >
        Login
      </button>
    </form>
  );
}
export default RegisterForm;
