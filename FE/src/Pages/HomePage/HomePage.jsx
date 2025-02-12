import styles from "./HomePage.module.css";
import spa from "../../assets/images/spa.jpg";
import phone from "../../assets/images/telephone.png";
import { Button } from "antd";
import FeaturesSection from "../../Components/FeaturesSection/FeaturesSection";
import CTASection from "../../Components/CTASection/CTASection";
import StatsSection from "../../Components/StatsSection/StatsSection";
import AppointmentSection from "../../Components/AppointmentSection/AppointmentSection";
import SkincareTips from "../../Components/SkincareTips/SkincareTips";
import TrustSection from "../../Components/TrustSection/TrustSection";
import SkincareExperts from "../../Components/SkincareExperts/SkincareExperts";

function HomePage() {
  return (
    <div className={styles.pageContainer}>
      
      <div className={styles.container}>
        
        <div className={styles.textContainer}>
          <h1 className={styles.title}>
            Glow naturally with <br /> skinn care
          </h1>
          <p className={styles.description}>
            Our skincare line is crafted with pure, high-quality <br /> ingredients for visible results.
          </p>
          <Button type="primary" size="large" className={styles.button}>
            <img src={phone} alt="phone icon" className={styles.phoneIcon} /> 
            Book an appointment
          </Button>
        </div>

        <div className={styles.imageContainer}>
          <img src={spa} alt="Relaxing Skincare" className={styles.image} />
        </div>
      </div>

      <div className={styles.featuresWrapper}>
        <FeaturesSection />
      </div><br/>

      <CTASection/><br/>

      <StatsSection/><br/>

      <TrustSection/><br/>

      <SkincareExperts/><br/>

      <SkincareTips/><br/>

      <AppointmentSection/><br/>

    </div>
  );
}

export default HomePage;
