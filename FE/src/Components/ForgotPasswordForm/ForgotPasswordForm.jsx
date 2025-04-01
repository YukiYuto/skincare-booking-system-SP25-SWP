import { useState } from "react";
import { toast } from "react-toastify";
import styles from "./ForgotPasswordForm.module.css";
import EmailInputField from "../InputField/Email/EmailInputField";
import { validateEmail } from "../../utils/validationUtils";
import { forgotPassword } from "../../services/authService";

export function ForgotPasswordForm() {
  const [ForgotData, setForgotData] = useState({ email: "" });
  const [error, setError] = useState({ email: "" });
  const [isLoading, setIsLoading] = useState(false);

  const handleEmailChange = (e) => {
    const { name, value } = e.target;
    setForgotData((prevData) => ({
      ...prevData,
      [name]: value,
    }));

    setError((prevErrors) => ({
      ...prevErrors,
      [name]: "",
    }));
  };

  const validateRegisterForm = () => {
    const emailError = validateEmail(ForgotData.email);
    setError({ email: emailError });
    return !emailError;
  };

  const handleForgotSubmit = async (e) => {
    e.preventDefault();

    // Kiểm tra email hợp lệ trước khi gửi
    if (!validateRegisterForm()) {
      toast.error("Please enter a valid email address.");
      return;
    }

    setIsLoading(true);

    try {
      await forgotPassword(ForgotData.email);
      toast.success("Password reset link has been sent to your email.");
    } catch (error) {
      console.error("Forgot password error:", error);
      const errorMessage = error?.response?.data?.message || error.message || "Failed to send reset link. Please try again.";
      toast.error(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form className={styles.forgotPasswordForm} onSubmit={handleForgotSubmit}>
      <h1 className={styles.forgotPasswordTitle}>Forgot password?</h1>
      <p className={styles.description}>Enter your email to receive a password reset link.</p>

      <EmailInputField
        label="Email Address"
        type="email"
        name="email"
        placeholder="Email Address"
        value={ForgotData.email}
        error={error.email}
        onChange={handleEmailChange}
      />

      <button type="submit" className={styles.submitButton} disabled={isLoading}>
        {isLoading ? "Sending..." : "Send email"}
      </button>
    </form>
  );
}
