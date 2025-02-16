import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "../../services/authService";
import styles from "./registerForm.module.css";
import { toast } from "react-toastify";
import {
  validateEmail,
  validatePasswordLength,
  validatePhoneNumber,
  validateAge,
  validateConfirmPassword,
} from "../../utils/validationUtils";
import { InputField } from "../InputField/InputField";

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
  });

  const [errors, setErrors] = useState({
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: "",
    fullName: "",
    address: "",
    age: "",
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

    setErrors((prevErrors) => ({
      ...prevErrors,
      [name]: "",
    }));
  };

  const validateRegisterForm = () => {
    const emailError = validateEmail(registerData.email);
    const passwordError = validatePasswordLength(registerData.password);
    const confirmPasswordError = validateConfirmPassword(
      registerData.password,
      registerData.confirmPassword
    );
    const phoneError = validatePhoneNumber(registerData.phoneNumber);
    const ageError = validateAge(registerData.age);
    const fullNameError = registerData.fullName ? "" : "Full name is required.";
    const addressError = registerData.address ? "" : "Address is required.";

    setErrors({
      email: emailError,
      password: passwordError,
      confirmPassword: confirmPasswordError,
      phoneNumber: phoneError,
      fullName: fullNameError,
      address: addressError,
      age: ageError,
    });

    return !(
      emailError ||
      passwordError ||
      confirmPasswordError ||
      phoneError ||
      fullNameError ||
      addressError ||
      ageError
    );
  };

  const handleRegisterSubmit = async (e) => {
    e.preventDefault();
    // Reset errors without setting empty strings for all fields manually
    setErrors((errors) => {
      for (const key in errors) {
        errors[key] = "";
      }
      return errors;
    });
    console.log(errors);
    if (!validateRegisterForm()) return;

    const formattedFullname = formatFullname(registerData.fullName);
    
    try {
      await register({
        email: registerData.email,
        password: registerData.password,
        confirmPassword: registerData.confirmPassword,
        phoneNumber: registerData.phoneNumber,
        fullName: formattedFullname,
        address: registerData.address,
        age: Number(registerData.age),
      });

      toast.success(`Registration successful! Welcome, ${formattedFullname}`);
      navigate("/login");
    } catch (error) {
      console.error("Registration error:", error.message);
      toast.error(error.message || "Registration failed. Please try again.");
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
        name="fullName"
        placeholder="Full Name"
        value={registerData.fullName}
        onChange={handleRegisterChange}
        error={errors.fullName}
      />

      <div className={styles.flexContainer1}>
        <InputField
          label="Phone Number"
          type="text"
          name="phoneNumber"
          placeholder="Phone Number"
          value={registerData.phoneNumber}
          onChange={handleRegisterChange}
          error={errors.phoneNumber}
        />

        <InputField
          label="Email Address"
          type="email"
          name="email"
          placeholder="Email Address"
          value={registerData.email}
          onChange={handleRegisterChange}
          error={errors.email}
        />
      </div>

      <div className={styles.flexContainer2}>
        <InputField
          label="Password"
          type="password"
          name="password"
          placeholder="Enter Password"
          value={registerData.password}
          onChange={handleRegisterChange}
          showPasswordToggle
          error={errors.password}
        />

        <InputField
          label="Confirm Password"
          type="password"
          name="confirmPassword"
          placeholder="Confirm Password"
          value={registerData.confirmPassword}
          onChange={handleRegisterChange}
          showPasswordToggle
          error={errors.confirmPassword}
        />
      </div>

      <div className={styles.flexContainer3}>
        <InputField
          label="Address"
          type="text"
          name="address"
          placeholder="Address"
          value={registerData.address}
          onChange={handleRegisterChange}
          error={errors.address}
        />

        <InputField
          label="Age"
          type="number"
          name="age"
          placeholder="Age"
          value={registerData.age}
          onChange={handleRegisterChange}
          error={errors.age}
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
