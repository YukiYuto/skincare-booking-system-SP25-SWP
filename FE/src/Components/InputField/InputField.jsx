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
  error,
  onChange,
  onBlur,
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
          onBlur={onBlur}
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
      {error && <p className={styles.error}>{error}</p>}
    </div>
  );
}
