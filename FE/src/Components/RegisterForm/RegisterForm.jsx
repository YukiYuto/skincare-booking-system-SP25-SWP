import { useState } from "react";
import { register, sendVerificationEmail } from "../../services/authService";
import styles from "./RegisterForm.module.css";
import { toast } from "react-toastify";
import {
  validateEmail,
  validatePhoneNumber,
  validateAge,
  validateConfirmPassword,
  validateGender,
  validatePassword,
} from "../../utils/validationUtils";
import { InputField } from "../InputField/InputField";
import { useSelector } from "react-redux";
import SignInWithGoogle from "../Authentication/Google/SignInWithGoogle";
import { GoogleAuthProvider, signInWithPopup } from "firebase/auth";
import { auth } from "../../config/firebase";

function RegisterForm() {
  const [isLoading, setIsLoading] = useState(false);
  const { loading, error: reduxError } = useSelector((state) => state.auth);
  const [registerData, setRegisterData] = useState({
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: "",
    fullName: "",
    address: "",
    birthDate: "",
    gender: "",
  });

  const [errors, setErrors] = useState({
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: "",
    fullName: "",
    address: "",
    birthDate: "",
    gender: "",
  });

  const handleGoogleSignIn = async () => {
    try {
      const provider = new GoogleAuthProvider();
      const result = await signInWithPopup(auth, provider);
      const idTokenResult = await result.user?.getIdTokenResult();
      const idToken = idTokenResult?.token;
      const user = await dispatch(loginByGoogle(idToken)).unwrap();
      setIsAccountVerified(true);
      toast.success(`Welcome, ${user.fullName}!`);
      navigate("/");
    } catch (error) {
      console.error("Google sign-in error", error.message);
      toast.error("Google sign-in failed. Please try again.");
    }
  };

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
    const passwordError = validatePassword(registerData.password);
    const confirmPasswordError = validateConfirmPassword(
      registerData.password,
      registerData.confirmPassword
    );
    const phoneError = validatePhoneNumber(registerData.phoneNumber);
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
      gender: genderError,
    });

    return !(
      emailError ||
      passwordError ||
      confirmPasswordError ||
      phoneError ||
      fullNameError ||
      addressError ||
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
    const formattedDate = new Date(registerData.birthDate).toISOString();
    console.log("Formatted Date:", formattedDate);

    try {
      await register({
        email: registerData.email.trim(),
        password: registerData.password.trim(),
        confirmPassword: registerData.confirmPassword,
        phoneNumber: registerData.phoneNumber,
        fullName: formattedFullname,
        address: registerData.address,
        // send the birthdate for DateTime format in C#
        birthDate: formattedDate,
        gender: registerData.gender,
      });

      await sendVerificationEmail(registerData.email);

      toast.success(
        "Registration successful! Please check your email to verify your account."
      );
    } catch (error) {
      console.error("Registration error: ", error.message);
      toast.error(error.message || "Registration failed. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form
      className={`${styles.registerForm} ${
        isLoading ? styles.disabledForm : ""
      }`}
      aria-labelledby="register-title"
      onSubmit={handleRegisterSubmit}
    >
      <h1 id="register-title" className={styles.registerTitle}>
        Welcome Newcomer
      </h1>
      <div className={styles.flexContainer}>
        <InputField
          label="Full Name"
          type="text"
          name="fullName"
          placeholder="Full Name"
          value={registerData.fullName}
          onChange={handleRegisterChange}
          error={errors.fullName}
        />

        <InputField
          label="Phone"
          type="text"
          name="phoneNumber"
          placeholder="Phone Number"
          value={registerData.phoneNumber}
          onChange={handleRegisterChange}
          error={errors.phoneNumber}
        />
      </div>

      <div className={styles.flexContainer}>
        <InputField
          label="Email"
          type="email"
          name="email"
          placeholder="Email Address"
          value={registerData.email}
          onChange={handleRegisterChange}
          error={errors.email}
        />

        <InputField
          label="Address"
          type="text"
          name="address"
          placeholder="Address"
          value={registerData.address}
          onChange={handleRegisterChange}
          error={errors.address}
        />
      </div>

      <div className={styles.flexContainer}>
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
          label="Confirm"
          type="password"
          name="confirmPassword"
          placeholder="Confirm Password"
          value={registerData.confirmPassword}
          onChange={handleRegisterChange}
          showPasswordToggle
          error={errors.confirmPassword}
        />
      </div>

      <div className={styles.flexContainer}>
        <InputField
          label="Birthdate"
          type="date"
          name="birthDate"
          placeholder="Birthdate"
          min="1900-01-01"
          max={new Date().toISOString().split("T")[0]}
          value={registerData.birthDate}
          onChange={handleRegisterChange}
          error={errors.age}
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

      <div className={styles.termsContainer}>
        <span>By signing in you agree to </span>
        <a
          href="/terms-conditions"
          className={`${styles.termsLink} ${
            loading ? styles.disabledLink : ""
          }`}
        >
          terms and conditions
        </a>
        <span> of our center.</span>
      </div>

      <button
        type="submit"
        className={styles.registerButton}
        disabled={isLoading}
      >
        {isLoading ? "Registering..." : "Register"}
      </button>

      <div className={styles.loginAccountContainer}>
        <span
          className={styles.loginAccountButton}
          onClick={() => {
            if (!loading) {
              window.location.href = "/login";
            }
          }}
          disabled={loading}
        >
          Have an account?
        </span>
        <SignInWithGoogle handleGoogleSignIn={handleGoogleSignIn} />
      </div>
    </form>
  );
}

export default RegisterForm;
