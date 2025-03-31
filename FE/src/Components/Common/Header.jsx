/* eslint-disable no-unused-vars */
import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import styles from "./Header.module.css"; // Import custom CSS file
import Brand from "../Brand/Brand";
import AuthButton from "../Authentication/AuthButtons";
import { useSelector } from "react-redux";

const Header = () => {
  const [menuOpen, setMenuOpen] = useState(false);
  const [isMobile, setIsMobile] = useState(false);

  useEffect(() => {
    const checkScreenSize = () => {
      setIsMobile(window.innerWidth < 1350);
      if (window.innerWidth >= 1350) {
        setMenuOpen(false);
      }
    };

    checkScreenSize();
    window.addEventListener("resize", checkScreenSize);
    return () => window.removeEventListener("resize", checkScreenSize);
  }, []);

  const toggleMenu = () => {
    setMenuOpen(!menuOpen);
  };

  const navigate = useNavigate();
  const { user } = useSelector((state) => state.auth);
  const isAuthenticated = user?.accessToken; // Kiểm tra accessToken

  const handleFeedbackClick = (e) => {
    if (!isAuthenticated) {
      e.preventDefault();
      navigate("/login"); 
    }
  };
  return (
    <header className={styles.stickyHeaderContainer}>
      <div className={styles.navHeaderWrapper}>
        <div className={styles.navHeader}>
          <div className={styles.navLogoSection}>
            <Link to="/" className={styles.navLogo} aria-label="Home">
              <Brand />
            </Link>
          </div>

          {isMobile ? (
            <>
              <button
                className={styles.hamburgerButton}
                onClick={toggleMenu}
                aria-label="Toggle menu"
                aria-expanded={menuOpen}
              >
                <span className={styles.hamburgerIcon}></span>
                <span className={styles.hamburgerIcon}></span>
                <span className={styles.hamburgerIcon}></span>
              </button>
          {/* Navigation Menu - Căn giữa */}
          <nav className={styles.navModern}>
            <ul className={styles.navList}>
              <li><Link to="/services" className={styles.navLink}>Services</Link></li>
              <li><Link to="/therapist" className={styles.navLink}>Therapists</Link></li>
              <li><Link to="/about" className={styles.navLink}>About</Link></li>
              <li><Link to="/skin-test" className={styles.navLink}>Skin Test</Link></li>
              <li><Link to="/promotion" className={styles.navLink}>Promotions</Link></li>
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

              {menuOpen && (
                <nav className={styles.mobileNav}>
                  <ul className={styles.mobileNavList}>
                    <li><Link to="/services" className={styles.mobileNavLink} onClick={toggleMenu}>Services</Link></li>
                    <li><Link to="/service-combo" className={styles.mobileNavLink} onClick={toggleMenu}>Combos</Link></li>
                    <li><Link to="/therapist" className={styles.mobileNavLink} onClick={toggleMenu}>Therapists</Link></li>
                    <li><Link to="/about" className={styles.mobileNavLink} onClick={toggleMenu}>About</Link></li>
                    <li><Link to="/skin-test" className={styles.mobileNavLink} onClick={toggleMenu}>Skin Test</Link></li>
                    <li><Link to="/promotion" className={styles.mobileNavLink} onClick={toggleMenu}>Promotions</Link></li>
                    <li><Link to="/contact" className={styles.mobileNavLink} onClick={toggleMenu}>Contact</Link></li>
                    <li className={styles.mobileAuthButton}><AuthButton /></li>
                  </ul>
                </nav>
              )}
            </>
          ) : (
            <>
              <nav className={styles.navModern}>
                <ul className={styles.navList}>
                  <li><Link to="/services" className={styles.navLink}>Services</Link></li>
                  <li><Link to="/service-combo" className={styles.navLink}>Combos</Link></li>
                  <li><Link to="/therapist" className={styles.navLink}>Therapists</Link></li>
                  <li><Link to="/about" className={styles.navLink}>About</Link></li>
                  <li><Link to="/skin-test" className={styles.navLink}>Skin Test</Link></li>
                  <li><Link to="/promotion" className={styles.navLink}>Promotions</Link></li>
                  <li><Link to="/contact" className={styles.navLink}>Contact</Link></li>
                </ul>
              </nav>

              <div className={styles.navActions}>
                <AuthButton />
              </div>
            </>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;