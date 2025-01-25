import styles from "./LoginPage.module.css";
import { LoginForm } from "../../Components/LoginForm/LoginForm";
import BrandLogo from "../../Components/LoginForm/BrandLogo";

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
