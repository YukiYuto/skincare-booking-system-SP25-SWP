/* eslint-disable no-unused-vars */
/* eslint-disable react/prop-types */
import React, { useState } from "react";
import { InputField } from "../InputField";

// EmailInputField component: A wrapper for InputField component to handle email input fields
const EmailInputField = ({ label, placeholder, value, onChange, error }) => {

  return (
    <InputField
      label={label}
      type="text"
      id="email"
      name="email"
      value={value}
      placeholder={placeholder}
      onChange={onChange}
      error={error}
    />
  );
};

export default EmailInputField;
