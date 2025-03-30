/* eslint-disable no-unused-vars */
/* eslint-disable react/prop-types */
import React from "react";
import styles from "./SignInWithGoogle.module.css";

const SignInWithGoogle = ({ handleGoogleSignIn }) => {
  return (
    <div className="mx-auto px-6 sm:px-0 max-w-sm">
      <button
        onClick={handleGoogleSignIn}
        type="button"
        className={styles.loginWithGoogleButton}
      >
        Sign in with Google
      </button>
    </div>
  );
};

export default SignInWithGoogle;
