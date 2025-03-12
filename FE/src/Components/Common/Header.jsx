/* eslint-disable no-unused-vars */
import React from "react";
import { Link } from "react-router-dom";
import styles from "./Header.module.css"; // Import custom CSS file
import Brand from "../Brand/Brand";
import AuthButton from "../Authentication/AuthButtons";

const Header = () => {
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
              <li><Link to="/about" className={styles.navLink}>About</Link></li>
              <li><Link to="/skin-test" className={styles.navLink}>Skin Test</Link></li>
              <li><Link to="/promotion" className={styles.navLink}>Promotions</Link></li>
              <li><Link to="/contact" className={styles.navLink}>Contact</Link></li>
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
