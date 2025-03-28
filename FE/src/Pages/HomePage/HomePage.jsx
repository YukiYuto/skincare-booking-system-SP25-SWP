import styles from "./HomePage.module.css";
import spa from "../../assets/images/spa.jpg";
import phone from "../../assets/images/telephone.png";
import { Button } from "antd";
import Header from "../../Components/Common/Header";
import FeaturesSection from "../../Components/FeaturesSection/FeaturesSection";
import CTASection from "../../Components/CTASection/CTASection";
import StatsSection from "../../Components/StatsSection/StatsSection";
import SkincareTips from "../../Components/SkincareTips/SkincareTips";
import TrustSection from "../../Components/TrustSection/TrustSection";
import SkincareExperts from "../../Components/SkincareExperts/SkincareExperts";
import { useNavigate } from "react-router-dom";
import Footer from "../../Components/Footer/Footer";

function HomePage() {
  const navigate = useNavigate(); // Hook để điều hướng

  const handleBookingClick = () => {
      navigate("/services");
  };
  
  return (
    <div>
      <Header />
      <div className={styles.pageContainer}>
        {/* Hero Section */}
        <div className={styles.container}>
          {/* Left Side - Text & Button */}
          <div className={styles.textContainer}>
            <h1 className={styles.title}>
              Glow naturally with <br /> skincare therapy
            </h1>
            <p className={styles.description}>
              Our skincare line is crafted with pure, high-quality <br />{" "}
              ingredients, and professional techniques to deliver visible results.
            </p>
            <Button type="primary" size="large" className={styles.button} onClick={handleBookingClick}>
              <img src={phone} alt="phone icon" className={styles.phoneIcon} />
              Book an appointment
            </Button>
          </div>

          {/* Right Side - Image */}
          <div className={styles.imageContainer}>
            <img src={spa} alt="Relaxing Skincare" className={styles.image} />
          </div>
        </div>

        {/* Features Section - Nằm dưới nhưng vẫn có background */}
        <div className={styles.featuresWrapper}>
          <FeaturesSection />
        </div>
        <br />

        <CTASection />
        <br />

        <StatsSection />
        <br />

        <TrustSection />
        <br />

        <SkincareExperts />
        <br />

        <SkincareTips />
        <br />
      </div>
      <Footer />
    </div>
  );
}

export default HomePage;
