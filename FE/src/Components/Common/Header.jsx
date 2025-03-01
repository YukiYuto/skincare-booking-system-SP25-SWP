/* eslint-disable no-unused-vars */
import React from "react";
import { Link } from "react-router-dom";
import styles from "./Header.module.css"; // Import custom CSS file
import Brand from "../Brand/Brand";

const Header = () => {
  return (
    <header className={styles.stickyHeaderContainer}>
      <div className={styles.navHeaderWrapper}>
        <div className={styles.navHeader}>
          {/* Logo */}
          <div className={styles.navLogoSection}>
            <Link to="/" className={styles.navLogo} aria-label="Home">
              <Brand />
            </Link>
          </div>

          {/* Navigation Menu */}
          <nav className={styles.navHeaderInlineMenu}>
            <ul className={styles.listMenu}>
              <li className={styles.listMenuItem}>
                <Link to="/services" className={styles.listMenuLink} aria-label="Services">
                  Services
                </Link>
              </li>
              <li className={styles.listMenuItem}>
                <Link to="/contact" className={styles.listMenuLink} aria-label="Contact">
                  Contact
                </Link>
              </li>
              <li className={styles.listMenuItem}>
                <Link to="/about" className={styles.listMenuLink} aria-label="About">
                  About
                </Link>
              </li>
              <li className={styles.listMenuItem}>
                <Link to="/skin-test" className={styles.listMenuLink} aria-label="Skin Mapper">
                  Skin Mapper
                </Link>
              </li>
              <li className={styles.listMenuItem}>
                <Link to="/promotion" className={styles.listMenuLink} aria-label="Promotion">
                  Promotion
                </Link>
              </li>
            </ul>
          </nav>

          {/* Login & Register Buttons */}
          <div className={styles.navHeaderAuth}>
            <Link to="/login" className={styles.btnPrimary} aria-label="Login">
              Login
            </Link>
            <Link to="/register" className={styles.btnSecondary} aria-label="Register">
              Register
            </Link>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;