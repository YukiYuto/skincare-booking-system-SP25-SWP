/* eslint-disable react/prop-types */
import styles from "./Brand.module.css";
import { Link } from "react-router-dom";

// Allows overriding the inline styles of the Brand component
const Brand = ({ override }) => {
  return (
    <Link to="/" className={(override?.value || "") + " " + styles.brand}>
      <span className={styles.brandHighlight}>lumi</span>
      <span className={styles.brandText}>connect</span>
    </Link>
  );
};

export default Brand;
