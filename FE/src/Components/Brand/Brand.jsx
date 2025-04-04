/* eslint-disable react/prop-types */
import styles from "./Brand.module.css";
import { Link } from "react-router-dom";

const Brand = ({ override }) => {
  return (
    <Link to="/" className={`${styles.brand} ${override?.value || ""}`}>
      <span className={styles.brandHighlight}>Lumi</span>
      <span className={styles.brandText}>Connect</span>
    </Link>
  );
};

export default Brand;
