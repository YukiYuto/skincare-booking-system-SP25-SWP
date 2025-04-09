import styles from "./BrandLogo.module.css";
import background from "../../assets/images/background.jpg";
import Brand from "../Brand/Brand.jsx";

export function BrandLogo() {
  return (
    <div className={styles.brandContainer}>
      <img
        src={background}
        alt="Skincare Background"
        className={styles.backgroundImage}
        loading="lazy"
      />
      <div className={styles.brandOverlay}>
        <Brand />
      </div>
    </div>
  );
}

export default BrandLogo;
