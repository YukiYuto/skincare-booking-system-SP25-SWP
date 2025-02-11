/* eslint-disable no-unused-vars */
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import styles from "./registerForm.module.css";
import { InputField } from "../InputField/InputField";
import axios from "axios";

function RegisterForm() {
  const navigate = useNavigate();

  const [registerData, setRegisterData] = useState({
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: "",
    fullName: "",
    address: "",
    age: "",
    skinProfileId: null,
  });

  const formatFullname = (fullName) => {
    return fullName
      .split(" ")
      .map((word) => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
      .join(" ");
  };

  const handleRegisterChange = (e) => {
    const { name, value } = e.target;
    setRegisterData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleRegisterSubmit = async (e) => {
    e.preventDefault();
    const formattedFullname = formatFullname(registerData.fullName);
    const api = "https://localhost:7037/api/Auth/customers";

    try {
      const response = await axios.post(api, {
        email: registerData.email,
        password: registerData.password,
        confirmPassword: registerData.confirmPassword,
        phoneNumber: registerData.phoneNumber,
        fullName: formattedFullname,
        address: registerData.address,
        age: Number(registerData.age),
      });

      console.log("Registration response:", response.data);
      alert(`Registration successful! Welcome, ${response.data.fullName}`);
      navigate("/login");
    } catch (error) {
      console.error("Registration error:", error.message);
      console.log(error);
      alert("Registration failed. Please try again.");
    }
  };

  return (
    <form
      className={styles.registerForm}
      aria-labelledby="register-title"
      onSubmit={handleRegisterSubmit}
    >
      <h1 id="register-title" className={styles.registerTitle}>
        Register
      </h1>

      <InputField
        label="Full Name"
        type="text"
        id="fullName"
        name="fullName"
        placeholder="Full Name"
        value={registerData.fullName}
        onChange={handleRegisterChange}
      />

      <div className={styles.flexContainer1}>
        <InputField
          label="Phone Number"
          type="text"
          id="phoneNumber"
          name="phoneNumber"
          placeholder="Phone Number"
          value={registerData.phoneNumber}
          onChange={handleRegisterChange}
        />

        <InputField
          label="Email Address"
          type="email"
          id="email"
          name="email"
          placeholder="Email Address"
          value={registerData.email}
          onChange={handleRegisterChange}
        />
      </div>

      <div className={styles.flexContainer2}>
        <InputField
          label="Password"
          type="password"
          id="password"
          name="password"
          placeholder="Enter Password"
          value={registerData.password}
          onChange={handleRegisterChange}
          showPasswordToggle
        />
        <InputField
          label="Confirm Password"
          type="password"
          id="confirmPassword"
          name="confirmPassword"
          placeholder="Confirm Password"
          value={registerData.confirmPassword}
          onChange={handleRegisterChange}
          showPasswordToggle
        />
      </div>

      <div className={styles.flexContainer3}>
        <InputField
          label="Address"
          type="text"
          id="address"
          name="address"
          placeholder="Address"
          value={registerData.address}
          onChange={handleRegisterChange}
        />

        <InputField
          label="Age"
          type="number"
          id="age"
          name="age"
          placeholder="Age"
          value={registerData.age}
          onChange={handleRegisterChange}
        />
      </div>

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
        onClick={() => navigate("/login")}
      >
        Login
      </button>
    </form>
  );
}

export default RegisterForm;
