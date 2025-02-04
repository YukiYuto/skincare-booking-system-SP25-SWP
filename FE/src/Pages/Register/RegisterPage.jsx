import React from "react";
import styles from "./RegisterPage.module.css";
import RegisterForm from "../../Components/RegisterForm/RegisterForm";
import BrandLogo from "../../Components/BrandLogo/BrandLogo";

function RegisterPage() {
  return (
    <main className={styles.registerPage}>
      <div className={styles.container}>
        <div className={styles.brandColumn}>
          <BrandLogo />
        </div>
        <div className={styles.formColumn}>
          <RegisterForm />
        </div>
      </div>
    </main>
  );
}
export default RegisterPage;
