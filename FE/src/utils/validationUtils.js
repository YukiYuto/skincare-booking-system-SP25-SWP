import validator from "validator";

export const validateEmail = (email) => {
  if (!email) {
    return "Email is required";
  }

  if (!validator.isEmail(email)) {
    return "Please enter a valid email address";
  }
  return "";
};

export const validatePhoneNumber = (phoneNumber) => {
  if (!phoneNumber) {
    return "Phone number is required";
  }

  if (!validator.isMobilePhone(phoneNumber, ["en-US", "es-US", "fr-FR", "vi-VN"])) {
    return "Please enter a valid phone number";
  }
  return "";
};

export const validatePasswordLength = (password) => {
  if (!password) {
    return "Password cannot be empty";
  }
  if (!validator.isLength(password, { min: 8, max: 32 })) {
    return "Password must be between 8-32 characters long";
  }
  return "";
};

export const validatePasswordFormat = (password) => {
  if (!validator.isStrongPassword(password)) {
    return "Password must be at least 8 character long, contain at least 1 lowercase, 1 uppercase, 1 number, and 1 special character";
  }
  return "";
};
export const validateConfirmPassword = (password, confirmPassword) => {
  if (!validator.equals(password, confirmPassword)) {
    return "Passwords do not match";
  }
  return "";
};

export const validateAge = (age) => {
  if (!age.isLength(age, { min: 16, max: 120 })) {
    return "Your age must be at least 16 to do registration";
  }
  return "";
};
