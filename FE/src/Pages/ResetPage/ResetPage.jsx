import styles from "./ResetPage.module.css";
import BrandLogo from "../../Components/BrandLogo/BrandLogo";
import Header from "../../Components/Common/Header";
import { ResetPasswordForm } from "../../Components/ResetPasswordForm/ResetPasswordForm";
export function ResetPage() {
  return (
    <>
      <Header />
      <main className={styles.resetPage}>
        <div className={styles.resetcontainer}>
          <div className={styles.resetbrandColumn}>
            <BrandLogo />
          </div>
          <div className={styles.resetformColumn}>
            <ResetPasswordForm />
          </div>
        </div>
      </main>
    </>
  );
}
