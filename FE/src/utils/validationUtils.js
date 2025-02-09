import validator from "validator";

export const validateEmail = (email) => { 
    if (!validator.isEmail(email)) {
        return 'Please enter a valid email address';
    }
    return '';
}

export const validatePasswordLength = (password) => {
    if (!validator.isLength(password, { min: 8, max: 32 })) {
        return 'Password must be between 8-32 characters long';
    }
    return '';
}

export const validatePasswordFormat = (password) => {
    if (!validator.isStrongPassword(password)) {
        return 'Password must be at least 8 character long, contain at least 1 lowercase, 1 uppercase, 1 number, and 1 special character';
    }
    return '';
}