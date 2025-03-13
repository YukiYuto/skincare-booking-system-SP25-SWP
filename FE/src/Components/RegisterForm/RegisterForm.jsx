import { useState } from "react";
import { register, sendVerificationEmail } from "../../services/authService";
import styles from "./RegisterForm.module.css";
import { toast } from "react-toastify";
import {
  validateEmail,
  validatePasswordLength,
  validatePhoneNumber,
  validateAge,
  validateConfirmPassword,
  validateGender,
} from "../../utils/validationUtils";
import { InputField } from "../InputField/InputField";

function RegisterForm() {
  const [isLoading, setIsLoading] = useState(false);

  const [registerData, setRegisterData] = useState({
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: "",
    fullName: "",
    address: "",
    age: "",
    gender: "",
  });

  const [errors, setErrors] = useState({
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: "",
    fullName: "",
    address: "",
    age: "",
    gender: "",
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
    const genderError = validateGender(registerData.gender);

    setErrors({
      email: emailError,
      password: passwordError,
      confirmPassword: confirmPasswordError,
      phoneNumber: phoneError,
      fullName: fullNameError,
      address: addressError,
      age: ageError,
      gender: genderError,
    });

    return !(
      emailError ||
      passwordError ||
      confirmPasswordError ||
      phoneError ||
      fullNameError ||
      addressError ||
      ageError ||
      genderError
    );
  };

  const handleRegisterSubmit = async (e) => {
    e.preventDefault();
    setErrors((errors) => {
      for (const key in errors) {
        errors[key] = "";
      }
      return errors;
    });
  
    if (!validateRegisterForm()) return;

    setIsLoading(true);
  
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
        gender: registerData.gender,
      });

      await sendVerificationEmail(registerData.email);

      toast.success("Registration successful! Please check your email to verify your account.");
    } catch (error) {
      console.error("Registration error:", error.message);
      toast.error(error.message || "Registration failed. Please try again.");
    } finally {
      setIsLoading(false); 
    }
  };

  return (
    <form
      className={`${styles.registerForm} ${isLoading ? styles.disabledForm : ""}`}
      aria-labelledby="register-title"
      onSubmit={handleRegisterSubmit}
    >
      <h1 id="register-title" className={styles.registerTitle}>
        Register Account
      </h1>
      <div className={styles.flexContainer1}>
        <InputField
          label="Full Name"
          type="text"
          name="fullName"
          placeholder="Full Name"
          value={registerData.fullName}
          onChange={handleRegisterChange}
          error={errors.fullName}
        />

        <div className={styles.genderContainer}>
          <label>
            <input
              type="radio"
              name="gender"
              value="Male"
              checked={registerData.gender === "Male"}
              onChange={handleRegisterChange}
            />
            Male
          </label>
          <label>
            <input
              type="radio"
              name="gender"
              value="Female"
              checked={registerData.gender === "Female"}
              onChange={handleRegisterChange}
            />
            Female
          </label>
        </div>
        {errors.gender && <span className={styles.error}>{errors.gender}</span>}
      </div>
      

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

      <div>
                <p>Do you have an account? <a 
                className={styles.loginLink}
                href="/login" 
                onClick={() => {
                    if (!isLoading) {
                      window.location.href = "/login";
                    }
                  }}
                  disabled={isLoading} 
                >Login
                </a>
              </p>
            </div>

      <div className={styles.termsContainer}>
        <span>By registering you agree to </span>
        <a 
        style={{ pointerEvents: isLoading ? "none" : "auto", opacity: isLoading ? 0.5 : 1 }}
        href="/terms" className={styles.termsLink}>
          terms and conditions
        </a>
        <span> of our center.</span>
      </div>

      <button type="submit" className={styles.registerButton} disabled={isLoading}>
        {isLoading ? "Registering..." : "Register"}
      </button>
    </form>
  );
}

export default RegisterForm;
