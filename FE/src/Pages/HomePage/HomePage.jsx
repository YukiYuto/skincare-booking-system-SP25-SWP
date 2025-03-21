import styles from "./HomePage.module.css";
import spa from "../../assets/images/spa.jpg";
import phone from "../../assets/images/telephone.png";
import { Button } from "antd";
import Header from "../../Components/Common/Header";
import FeaturesSection from "../../Components/FeaturesSection/FeaturesSection";
import CTASection from "../../Components/CTASection/CTASection";
import StatsSection from "../../Components/StatsSection/StatsSection";
import AppointmentSection from "../../Components/AppointmentSection/AppointmentSection";
import SkincareTips from "../../Components/SkincareTips/SkincareTips";
import TrustSection from "../../Components/TrustSection/TrustSection";
import SkincareExperts from "../../Components/SkincareExperts/SkincareExperts";
import { useEffect, useState } from "react";
import BookingModal from "../../Components/BookingModal/BookingModal";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";

function HomePage() {
  const [visible, setVisible] = useState(false);
  const navigate = useNavigate(); // Hook để điều hướng
  const isAuthenticated = useSelector((state) => state.auth.isAuthenticated); // Lấy trạng thái đăng nhập

  const handleBookingClick = () => {
    if (!isAuthenticated) {
      navigate("/login");
    } else {
      setVisible(true);
    }
  };

  // Theo dõi thay đổi trạng thái đăng nhập, nếu logout thì đóng modal
  useEffect(() => {
    if (!isAuthenticated) {
      setVisible(false);
    }
  }, [isAuthenticated]);
  
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
            {isAuthenticated && (
              <BookingModal visible={visible} onClose={() => setVisible(false)} />
            )}
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

        <AppointmentSection />
        <br />
      </div>
    </div>
  );
}

export default HomePage;
