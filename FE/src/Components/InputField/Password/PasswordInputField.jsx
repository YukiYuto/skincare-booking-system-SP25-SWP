/* eslint-disable no-unused-vars */
/* eslint-disable react/prop-types */
import React, { useState } from "react";
import { InputField } from "../InputField";

// PasswordInputField component: A wrapper for InputField component to handle password input fields
const PasswordInputField = ({ label, placeholder, value, onChange, error }) => {
  const [isPasswordVisible, setIsPasswordVisible] = useState(false);

  return (
    <InputField
      label={label}
      type={isPasswordVisible ? "text" : "password"}
      id="password"
      name="password"
      value={value}
      placeholder={placeholder}
      showPasswordToggle
      onChange={onChange}
      error={error}
    />
  );
};

export default PasswordInputField;
