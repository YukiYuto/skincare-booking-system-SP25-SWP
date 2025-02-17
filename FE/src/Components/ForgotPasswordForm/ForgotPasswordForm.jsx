import { useState } from "react";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { resetPassword } from "../../services/authService";
import styles from "./ForgotPasswordForm.module.css";
import EmailInputField from "../InputField/Email/EmailInputField";
import { validateEmail } from "../../utils/validationUtils";

export function ForgotPasswordForm() {
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate(); // Điều hướng trang

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
    setError("");
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    const emailError = validateEmail(email);
    if (emailError) {
      setError(emailError);
      return;
    }

    try {
      await resetPassword(email);
      toast.success("Password reset email has been sent!");
      navigate(`/reset-password?email=${encodeURIComponent(email)}`); // Điều hướng đến ResetPasswordForm
    } catch (error) {
      toast.error(error.message || "Request sent failed. Please try again.");
    }
  };

  return (
    <form className={styles.forgotPasswordForm} onSubmit={handleSubmit}>
      <h1 className={styles.forgotPasswordTitle}>Forgot password?</h1>
      <p className={styles.description}>Enter your email to receive a password reset link.</p>

      <EmailInputField
        label="Email"
        placeholder="Your email"
        value={email}
        onChange={handleEmailChange}
        error={error}
      />

      <button type="submit" className={styles.submitButton}>Submit request</button>
    </form>
  );
}
