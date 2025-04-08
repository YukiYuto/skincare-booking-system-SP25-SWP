/* eslint-disable no-unused-vars */
import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import styles from "./Header.module.css";
import Brand from "../Brand/Brand";
import AuthButton from "../Authentication/AuthButtons";
import { useSelector } from "react-redux";

const Header = () => {
  const [menuOpen, setMenuOpen] = useState(false);
  const [isMobile, setIsMobile] = useState(window.innerWidth < 1208);
  const navigate = useNavigate();
  const { user } = useSelector((state) => state.auth);
  const isAuthenticated = Boolean(user?.accessToken);

  useEffect(() => {
    const checkScreenSize = () => {
      const mobile = window.innerWidth < 1208;
      setIsMobile(mobile);
      if (!mobile) setMenuOpen(false);
    };

    window.addEventListener("resize", checkScreenSize);
    return () => window.removeEventListener("resize", checkScreenSize);
  }, []);

  const handleFeedbackClick = (e) => {
    if (!isAuthenticated) {
      e.preventDefault();
      navigate("/login");
    }
  };

  const renderNavLinks = (isMobile) => (
    <ul className={isMobile ? styles.mobileNavList : styles.navList}>
      <li><Link className={styles.navLink} to="/services" onClick={() => isMobile && setMenuOpen(false)}>Services</Link></li>
      <li><Link className={styles.navLink} to="/service-combo" onClick={() => isMobile && setMenuOpen(false)}>Combos</Link></li>
      <li><Link className={styles.navLink} to="/therapist" onClick={() => isMobile && setMenuOpen(false)}>Therapists</Link></li>
      <li><Link className={styles.navLink} to="/about" onClick={() => isMobile && setMenuOpen(false)}>About</Link></li>
      <li><Link className={styles.navLink} to="/quiz" onClick={() => isMobile && setMenuOpen(false)}>Skin Test</Link></li>
      <li><Link className={styles.navLink} to="/blogs" onClick={() => isMobile && setMenuOpen(false)}>Blogs</Link></li>
      <li><Link className={styles.navLink} to="/contact" onClick={() => isMobile && setMenuOpen(false)}>Contact</Link></li>
      <li>
      <Link 
        className={styles.navLink} 
        to={isAuthenticated ? "/feedback-page" : "#"} 
        onClick={(e) => {
          handleFeedbackClick(e);
          if (isMobile) setMenuOpen(false);
        }}
      >
        Feedback
      </Link>
    </li>
      {isMobile && <li className={styles.mobileAuthButton}><AuthButton /></li>}
    </ul>
  );

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
              <button className={styles.hamburgerButton} onClick={() => setMenuOpen(!menuOpen)} aria-label="Toggle menu">
                <span className={styles.hamburgerIcon}></span>
                <span className={styles.hamburgerIcon}></span>
                <span className={styles.hamburgerIcon}></span>
              </button>
              {menuOpen && <nav className={styles.mobileNav}>{renderNavLinks(true)}</nav>}
            </>
          ) : (
            <>
              <nav className={styles.navModern}>{renderNavLinks(false)}</nav>
              <div className={styles.navActions}><AuthButton /></div>
            </>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;