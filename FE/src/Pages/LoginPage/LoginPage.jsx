import styles from "./LoginPage.module.css";
import { LoginForm } from "../../Components/LoginForm/LoginForm";
import BrandLogo from "../../Components/BrandLogo/BrandLogo";
import Header from "../../Components/Common/Header";

export function LoginPage() {
  return (
    <>
      <Header />
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
    </>
  );
}
