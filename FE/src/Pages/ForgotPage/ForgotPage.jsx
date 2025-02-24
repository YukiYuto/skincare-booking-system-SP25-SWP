import styles from "./ForgotPage.module.css";
import { ForgotPasswordForm } from "../../Components/ForgotPasswordForm/ForgotPasswordForm";
import BrandLogo from "../../Components/BrandLogo/BrandLogo";
import Header from "../../Components/Common/Header";
export function ForgotPage() {
  return (
    <>
      <Header />
      <main className={styles.forgotPage}>
        <div className={styles.forgotcontainer}>
          <div className={styles.forgotbrandColumn}>
            <BrandLogo />
          </div>
          <div className={styles.forgotformColumn}>
            <ForgotPasswordForm />
          </div>
        </div>
      </main>
    </>
  );
}
