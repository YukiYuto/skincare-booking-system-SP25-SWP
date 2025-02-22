import { useState } from "react";
import { toast } from "react-toastify";
import { useNavigate, useSearchParams } from "react-router-dom";
import styles from "./ResetPasswordForm.module.css";
import { InputField } from "../InputField/InputField";
import { validateConfirmNewPassword, validatenewPasswordLength } from "../../utils/validationUtils";

export function ResetPasswordForm() {
  const [passwordData, setPasswordData] = useState({ newPassword: "", confirmNewPassword: "" });
  const [errors, setErrors] = useState({ newPassword: "", confirmNewPassword: "" });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  // Lấy email & token từ URL
  const email = searchParams.get("email");
  const token = searchParams.get("token")?.replace(/ /g, "+"); // Thay dấu cách thành "+"

  // Xử lý thay đổi input
  const handleChange = (e) => {
    const { name, value } = e.target;
    setPasswordData({
      ...passwordData,
      [name === "confirmPassword" ? "confirmNewPassword" : name]: value,
    });
  };
  

  // Xử lý gửi form
  const handleSubmit = async (e) => {
    e.preventDefault();

    // Kiểm tra hợp lệ
    let newErrors = {
      newPassword: validatenewPasswordLength(passwordData.newPassword),
      confirmNewPassword: validateConfirmNewPassword(passwordData.newPassword, passwordData.confirmNewPassword),
    };

    setErrors(newErrors);

    if (newErrors.newPassword || newErrors.confirmNewPassword) return;

    setIsLoading(true);
    console.log("Sending data:", {
      email,
      token,
      newPassword: passwordData.newPassword,
      confirmPassword: passwordData.confirmNewPassword,
    });
    try {
      const response = await fetch("https://localhost:7037/api/Auth/password/reset", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Accept": "application/json",
        },
        body: JSON.stringify({
          email,
          token,
          newPassword: passwordData.newPassword,
          confirmPassword: passwordData.confirmNewPassword, // Gửi đúng tên API yêu cầu
        }),
      });
      
      const data = await response.json();

      if (response.ok) {
        toast.success("Password reset successful!");
        navigate("/login");
      } else {
        toast.error(data.message || "Password reset failed.");
      }
    } catch (error) {
      toast.error("Network error. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form className={styles.resetPasswordForm} onSubmit={handleSubmit}>
      <h1 className={styles.resetPasswordTitle}>Reset Password</h1>

      <InputField
        label="New password"
        placeholder="8-32 characters"
        name="newPassword"
        type="password"
        value={passwordData.newPassword}
        onChange={handleChange}
        error={errors.newPassword}
      />

      <InputField
        label="Confirm password"
        placeholder="Confirm password"
        name="confirmPassword" // Đổi name để khớp với backend
        type="password"
        value={passwordData.confirmNewPassword} // Vẫn sử dụng state confirmNewPassword
        onChange={handleChange}
        error={errors.confirmNewPassword}
      />


      <button type="submit" className={styles.submitButton} disabled={isLoading}>
        {isLoading ? "Changing..." : "Submit Change"}
      </button>
    </form>
  );
}
