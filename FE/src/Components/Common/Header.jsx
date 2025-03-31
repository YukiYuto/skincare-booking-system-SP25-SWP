/* eslint-disable no-unused-vars */
import React from "react";
import { Link, useNavigate } from "react-router-dom";
import styles from "./Header.module.css"; // Import custom CSS file
import Brand from "../Brand/Brand";
import AuthButton from "../Authentication/AuthButtons";
import { useSelector } from "react-redux";

const Header = () => {
  const navigate = useNavigate();
  const { user } = useSelector((state) => state.auth);
  const isAuthenticated = user?.accessToken; // Kiểm tra accessToken

  const handleFeedbackClick = (e) => {
    if (!isAuthenticated) {
      e.preventDefault(); // Ngăn chặn điều hướng mặc định
      navigate("/login"); // Chuyển hướng đến trang đăng nhập
    }
  };
  return (
    <header className={styles.stickyHeaderContainer}>
      <div className={styles.navHeaderWrapper}>
        <div className={styles.navHeader}>
          {/* Logo - Sát trái */}
          <div className={styles.navLogoSection}>
            <Link to="/" className={styles.navLogo} aria-label="Home">
              <Brand />
            </Link>
          </div>

          {/* Navigation Menu - Căn giữa */}
          <nav className={styles.navModern}>
            <ul className={styles.navList}>
              <li><Link to="/services" className={styles.navLink}>Services</Link></li>
              <li><Link to="/therapist" className={styles.navLink}>Therapists</Link></li>
              <li><Link to="/about" className={styles.navLink}>About</Link></li>
              <li><Link to="/quiz" className={styles.navLink}>Skin Test</Link></li>
              <li><Link to="/contact" className={styles.navLink}>Contact</Link></li>
              <li><Link to="/blogs" className={styles.navLink}>Blog</Link></li>
              <li>
                <Link 
                  to={isAuthenticated ? "/feedback-page" : "#"} 
                  className={styles.navLink}
                  onClick={handleFeedbackClick}
                >
                  Feedback
                </Link>
              </li>
            </ul>
          </nav>

          <div className={styles.navActions}>
            <AuthButton />
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;
