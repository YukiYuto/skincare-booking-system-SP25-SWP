import { useState } from "react";
import { toast } from "react-toastify";
import { resetPassword } from "../../services/authService";
import styles from "./ForgotPasswordForm.module.css";
import EmailInputField from "../InputField/Email/EmailInputField";
import { validateEmail } from "../../utils/validationUtils";

export function ForgotPasswordForm() {
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
    setError(""); // Xóa lỗi khi người dùng nhập lại
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    // Kiểm tra email hợp lệ
    const emailError = validateEmail(email);
    if (emailError) {
      setError(emailError);
      return;
    }

    try {
      await resetPassword(email);
      toast.success("Password reset email has been sent!");
      setEmail(""); // Xóa input sau khi gửi thành công
    } catch (error) {
      console.error("Password reset error:", error.message);
      toast.error(error.message || "Request sent failed. Please try again.");
    }
  };

  return (
    <form
      className={styles.forgotPasswordForm}
      aria-labelledby="forgot-password-title"
      onSubmit={handleSubmit}
    >
      <h1 id="forgot-password-title" className={styles.forgotPasswordTitle}>
        Forgot Password?
      </h1>

      <p className={styles.description}>
      Enter your email to receive a password reset link.
      </p>

      <EmailInputField
        label="Email"
        placeholder="Your email"
        value={email}
        onChange={handleEmailChange}
        error={error}
      />

      <button type="submit" className={styles.submitButton}>
        Submit request
      </button>

      <button
        type="button"
        className={styles.backButton}
        onClick={() => (window.location.href = "/login")}
      >
        Back to login
      </button>
    </form>
  );
}
