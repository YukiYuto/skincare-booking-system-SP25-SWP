import React, { useState } from "react";
import styles from "./registerForm.module.css";
import { InputField } from "../InputField/InputField";

function RegisterForm() {
  const [name, setName] = useState("");
  const [username, setUsername] = useState("");
  const [phone, setPhone] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [termsAgreed, setTermsAgreed] = useState(false);

  const handleSubmit = (event) => {
    event.preventDefault();
    // Add your registration logic here
    console.log("Registration submitted:", { name, username, phone, email, password });
  };

  return (
    <form className={styles.registerForm} aria-labelledby="register-title" onSubmit={handleSubmit}>
      <h1 id="register-title" className={styles.registerTitle}>
        Register
      </h1>

      <InputField
        label="Name"
        type="text"
        id="name"
        placeholder="name"
        value={name}
        onChange={(event) => setName(event.target.value)}
      />

      <InputField
        label="Username"
        type="text"
        id="username"
        placeholder="Username"
        value={username}
        onChange={(event) => setUsername(event.target.value)}
      />

      <InputField
        label="Phone Number"
        type="number"
        id="phone"
        placeholder="Phone Number"
        value={phone}
        onChange={(event) => setPhone(event.target.value)}
      />

      <InputField
        label="Email Address"
        type="text"
        id="email"
        placeholder="Email Address"
        value={email}
        onChange={(event) => setEmail(event.target.value)}
      />

      <InputField
        label="Password"
        type="password"
        id="password"
        placeholder="6+ characters"
        value={password}
        onChange={(event) => setPassword(event.target.value)}
        showPasswordToggle
      />

      <div className={styles.termsContainer}>
        <span>By registering you agree to </span>
        <a href="/terms" className={styles.termsLink}>
          terms and conditions
        </a>
        <span> of our center.</span>
        <input
          type="checkbox"
          id="terms-agreed"
          checked={termsAgreed}
          onChange={(event) => setTermsAgreed(event.target.checked)}
        />
        <label htmlFor="terms-agreed">I agree to the terms and conditions</label>
      </div>

      <button type="submit" className={styles.registerButton} disabled={!termsAgreed}>
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