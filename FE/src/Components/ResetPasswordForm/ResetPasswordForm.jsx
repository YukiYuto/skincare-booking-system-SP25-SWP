import { useState } from "react";
import { toast } from "react-toastify";
import { useLocation, useNavigate } from "react-router-dom";
import { updatePassword } from "../../services/authService";
import styles from "./ResetPasswordForm.module.css";
import PasswordInputField from "../InputField/Password/PasswordInputField";
import { validatePasswordLength } from "../../utils/validationUtils";

export function ResetPasswordForm() {
  const [passwordData, setPasswordData] = useState({ newPassword: "", confirmPassword: "" });
  const [errors, setErrors] = useState({ newPassword: "", confirmPassword: "" });
  const navigate = useNavigate();
  const location = useLocation();
  const email = new URLSearchParams(location.search).get("email");

  if (!email) {
    toast.error("Missing email to reset password.");
    navigate("/forgot-password");
  }

  const handleChange = (e) => {
    const { name, value } = e.target;
    setPasswordData({ ...passwordData, [name]: value });
    setErrors({ ...errors, [name]: "" });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    const passwordError = validatePasswordLength(passwordData.newPassword);
    if (passwordError) {
      setErrors({ ...errors, newPassword: passwordError });
      return;
    }

    if (passwordData.newPassword !== passwordData.confirmPassword) {
      setErrors({ ...errors, confirmPassword: "Confirmation password does not match." });
      return;
    }

    try {
      await updatePassword(email, passwordData.newPassword);
      toast.success("Password reset successful!");
      navigate("/login");
    } catch (error) {
      toast.error(error.message || "Password could not be reset.");
    }
  };

  return (
    <form className={styles.resetPasswordForm} onSubmit={handleSubmit}>
      <h1 className={styles.resetPasswordTitle}>Reset Password</h1>

      <PasswordInputField
        label="New password"
        placeholder="8-32 characters"
        name="newPassword"
        value={passwordData.newPassword}
        onChange={handleChange}
        error={errors.newPassword}
      />

      <PasswordInputField
        label="Confirm password"
        placeholder="Confirm password"
        name="confirmPassword"
        value={passwordData.confirmPassword}
        onChange={handleChange}
        error={errors.confirmPassword}
      />

      <button type="submit" className={styles.submitButton}>Submit</button>
    </form>
  );
}
