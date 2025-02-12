import styles from "./registerForm.module.css";
import { InputField } from "../InputField/InputField";
import { useState } from "react";

function RegisterForm() {
  const [name, setName] = useState("");
  const [username, setUsername] = useState("");
  const [phone, setPhone] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (event) => {
    event.preventDefault();
    // Add your registration logic here
    console.log("Registration submitted:", {
      name,
      username,
      phone,
      email,
      password,
    });
  };

  return (
    <form
      className={styles.registerForm}
      aria-labelledby="register-title"
      onSubmit={handleSubmit}
    >
      <h1 id="register-title" className={styles.registerTitle}>
        Register
      </h1>

      <InputField
        label="Name"
        type="text"
        id="name"
        placeholder="Name"
        value={name}
        onChange={(event) => setName(event.target.value)}
      />

      <div className={styles.flexContainer}>
        <InputField
          label="Username"
          type="text"
          id="username"
          placeholder="Username"
          value={username}
          onChange={(event) => setUsername(event.target.value)}
        />

        <InputField
          label="Phone"
          type="number"
          id="phone"
          placeholder="Phone Number"
          value={phone}
          onChange={(event) => setPhone(event.target.value)}
        />
      </div>

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
