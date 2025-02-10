import { Button } from "antd";
import styles from "./CTASection.module.css";
import phoneIcon from "../../assets/images/telephone.png";

const CTASection = () => {
  return (
    <div className={styles.ctaContainer}>
      <div className={styles.text}>
        <h2>Ready for radiant skin?</h2>
        <p>
          Book your personalized skincare session today and discover your natural glow.
        </p>
      </div>
      <Button type="primary" size="large" className={styles.ctaButton}>
        <img src={phoneIcon} alt="Phone" className={styles.phoneIcon} />
        Schedule appointment
      </Button>
    </div>
  );
};

export default CTASection;
