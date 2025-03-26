import styles from "./Brand.module.css";
import { Link } from "react-router-dom";

const Brand = () => {
  return (
    <Link to="/" className={(override?.value || "") + " " + styles.brand}>
      <span className={styles.brandHighlight}>lumi</span>
      <span className={styles.brandText}>connect</span>
    </Link>
  );
};

export default Brand;
