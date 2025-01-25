import React, { useState } from 'react';
import styles from './InputField.module.css';

export function InputField({ label, type, id, placeholder, showPasswordToggle }) {
  const [showPassword, setShowPassword] = useState(false);

  return (
    <div className={styles.inputContainer}>
      <label htmlFor={id} className={styles.label}>
        {label}
      </label>
      <div className={styles.inputWrapper}>
        <input
          type={showPasswordToggle && showPassword ? 'text' : type}
          id={id}
          placeholder={placeholder}
          className={styles.input}
          aria-label={label}
        />
        {showPasswordToggle && (
          <button
            type="button"
            onClick={() => setShowPassword(!showPassword)}
            className={styles.togglePassword}
            aria-label={showPassword ? 'Hide password' : 'Show password'}
          >
            <img
              src="/assets/eye-icon.svg"
              alt=""
              className={styles.eyeIcon}
              aria-hidden="true"
            />
          </button>
        )}
      </div>
    </div>
  );
}