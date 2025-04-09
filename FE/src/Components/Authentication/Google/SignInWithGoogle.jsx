import React from "react";
import styles from "./SignInWithGoogle.module.css";

const SignInWithGoogle = ({ handleGoogleSignIn }) => {
  return (
    <div className="mx-auto px-6 sm:px-0 max-w-sm gg-button">
      <button
        onClick={handleGoogleSignIn}
        type="button"
        className={styles.loginWithGoogleButton}
      ></button>
    </div>
  );
};

export default SignInWithGoogle;
