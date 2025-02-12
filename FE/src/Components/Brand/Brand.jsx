/* eslint-disable react/prop-types */
import styles from "./Brand.module.css";

// Allows overriding the inline styles of the Brand component
const Brand = ({ override }) => {
  return (
    <div className={(override?.value || "") + " " + styles.brand}>
      <span className={styles.brandHighlight}>lumi</span>
      <span className={styles.brandText}>connect</span>
    </div>
  );
};

export default Brand;
