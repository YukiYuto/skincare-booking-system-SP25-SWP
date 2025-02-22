import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { useNavigate, useSearchParams } from "react-router-dom";
import styles from "./ResetPasswordForm.module.css";
import { InputField } from "../InputField/InputField";
import { resetPassword } from "../../services/authService";
import { validateConfirmNewPassword, validatenewPasswordLength } from "../../utils/validationUtils";

export function ResetPasswordForm() {
  const [passwordData, setPasswordData] = useState({ newPassword: "", confirmNewPassword: "" });
  const [errors, setErrors] = useState({ newPassword: "", confirmNewPassword: "" });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  // ✅ Lấy email & token từ URL
  const email = searchParams.get("email");
  const token = searchParams.get("token");

  // ✅ Kiểm tra nếu thiếu email hoặc token
  useEffect(() => {
    if (!email || !token) {
      toast.error("Invalid or missing reset link.");
      navigate("/forgot-password");
    }
  }, [email, token, navigate]);

  // ✅ Cập nhật giá trị khi người dùng nhập
  const handleChange = (e) => {
    const { name, value } = e.target;
  
    setPasswordData((prevData) => ({
      ...prevData,
      [name === "confirmPassword" ? "confirmNewPassword" : name]: value,  // ✅ Chuyển confirmPassword thành confirmNewPassword
    }));
  
    setErrors((prevErrors) => ({
      ...prevErrors,
      [name === "confirmPassword" ? "confirmNewPassword" : name]: "", // ✅ Reset lỗi theo đúng state
    }));
  };
  
  
  const validateResetForm = () => {
    const newPasswordError = validatenewPasswordLength(passwordData.newPassword);
    const confirmNewPasswordError = validateConfirmNewPassword(
      passwordData.newPassword,
      passwordData.confirmNewPassword
    );
  
    setErrors({
      newPassword: newPasswordError, // ✅ Khớp với state ban đầu
      confirmNewPassword: confirmNewPasswordError, // ✅ Khớp với state ban đầu
    });
  
    return !(newPasswordError || confirmNewPasswordError);
  };
  
  // ✅ Xử lý khi nhấn nút submit
  const handleSubmit = async (event) => {
    event.preventDefault();
    setErrors({
      newPassword: "",
      confirmNewPassword: "",
    });
  
    if (!validateResetForm()) return;   

    console.log("Sending data:", {
      email,
      token,
      newPassword: passwordData.newPassword,
      confirmPassword: passwordData.confirmNewPassword
    });

    setIsLoading(true);
    try {
      // ✅ Gửi cả 4 dữ liệu đến API backend
      await resetPassword(email, token, passwordData.newPassword, passwordData.confirmNewPassword);

      toast.success("Password reset successful!");
      navigate("/login");
    } catch (error) {
      toast.error(error?.response?.data?.message || "Password could not be reset.");
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
        name="confirmPassword"
        type="password"
        value={passwordData.confirmNewPassword}
        onChange={handleChange}
        error={errors.confirmNewPassword}
      />

      <button type="submit" className={styles.submitButton} disabled={isLoading}>
        {isLoading ? "Changing..." : "Submit Change"}
      </button>
    </form>
  );
}
