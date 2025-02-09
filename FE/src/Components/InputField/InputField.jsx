/* eslint-disable react/prop-types */
import { useState } from "react";
import styles from "./InputField.module.css";
import eyeOpen from "./eyeOpen.svg";
import eyeClose from "./eyeClose.svg";

export function InputField({
  label,
  type,
  id,
  name,
  placeholder,
  minLength,
  maxLength,
  showPasswordToggle = false,
  value,
  onChange,
}) {
  const [showPassword, setShowPassword] = useState(false);

  const handleTogglePassword = () => {
    setShowPassword(!showPassword);
  };

  return (
    <div className={styles.inputContainer}>
      <label htmlFor={id} className={styles.label}>
        {label}
      </label>
      <div className={styles.inputWrapper}>
        <input
          type={showPasswordToggle && showPassword ? "text" : type}
          id={id}
          name={name}
          placeholder={placeholder}
          className={styles.input}
          aria-label={label}
          minLength={minLength}
          maxLength={maxLength}
          value={value}
          onChange={onChange}
        />
        {showPasswordToggle && (
          <button
            type="button"
            onClick={handleTogglePassword}
            className={styles.togglePassword}
            aria-label={showPassword ? "Hide password" : "Show password"}
          >
            <img
              src={showPassword ? eyeClose : eyeOpen}
              alt={showPassword ? "Hide password" : "Show Password"}
              className={styles.eyeIcon}
              aria-hidden="true"
            />
          </button>
        )}
      </div>
      <p className={styles.error}>
        {value.length > 0 &&
        (value.length < minLength || value.length > maxLength)
          ? `Nhập từ ${minLength} đến ${maxLength} ký tự`
          : ""}
      </p>
    </div>
  );
}
