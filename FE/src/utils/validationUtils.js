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

  if (
    !validator.isMobilePhone(phoneNumber, ["en-US", "es-US", "fr-FR", "vi-VN"])
  ) {
    return "Please enter a valid phone number";
  }
  return "";
};

export const validatePassword = (password) => {
  if (!password) {
    return "Password cannot be empty";
  }
  if (!validator.isLength(password, { min: 8, max: 32 })) {
    return "Password must be between 8-32 characters long";
  }
  return validatePasswordFormat(password);
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

export const validatenewPasswordLength = (newPassword) => {
  if (!newPassword) {
    return "New password cannot be empty";
  }
  if (!validator.isLength(newPassword, { min: 8, max: 32 })) {
    return "New password must be between 8-32 characters long";
  }
  return "";
};

export const validatenewPasswordFormat = (newPassword) => {
  if (!validator.isStrongPassword(newPassword)) {
    return "New password must be at least 8 character long, contain at least 1 lowercase, 1 uppercase, 1 number, and 1 special character";
  }
  return "";
};
export const validateConfirmNewPassword = (newPassword, confirmNewPassword) => {
  if (!validator.equals(newPassword, confirmNewPassword)) {
    return "Passwords do not match";
  }
  return "";
};

export const validateAge = (age) => {
  if (!validator.isInt(age, { min: 16, max: 100 })) {
    return "Age must be between 16 and 100.";
  }
  return "";
};

export const validateGender = (gender) => {
  const validGenders = ["male", "female"];

  if (!validGenders.includes(gender.toLowerCase())) {
    return "Gender must be either 'Male' or 'Female'.";
  }

  return "";
};

export const validateAddress = (address) => {
  if (!address) {
    return "Address is required";
  }
  if (!validator.isLength(address, { min: 5, max: 50 })) {
    return "Address must be between 5-50 characters long";
  }
  return "";
};

export const isAccountUnverified = (error) => {
  return error.message == "You need to confirm email!";
};
