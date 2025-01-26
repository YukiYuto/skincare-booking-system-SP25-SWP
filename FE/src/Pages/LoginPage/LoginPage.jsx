import React from "react";
import styles from "./LoginPage.module.css";
import { LoginForm } from "../../Components/LoginForm/LoginForm";
import BrandLogo from "../../Components/BrandLogo/BrandLogo";

export function LoginPage() {
  return (
    <main className={styles.loginPage}>
      <div className={styles.container}>
        <div className={styles.brandColumn}>
          <BrandLogo />
        </div>
        <div className={styles.formColumn}>
          <LoginForm />
        </div>
      </div>
    </main>
  );
}
