import styles from "./Footer.module.css";

const Footer = () => {
  return (
    <footer className={styles.footer}>
      <div className={styles.container}>
        {/* Logo và giới thiệu */}
        <div className={styles.about}>
          <h2 className={styles.logo}>
            <span className={styles.logoIcon}>LumiConnect</span><br/> skin care
          </h2>
          <p>
            At Glisten, we are dedicated to helping you achieve radiant, healthy
            skin through personalized skincare treatments and expert guidance.
          </p>
        </div>

        {/* Các danh mục */}
        <div className={styles.sections}>
          <div className={styles.column}>
            <h3>Pages</h3>
            <ul>
              <li><a href="/">Home</a></li>
              <li><a href="/about">About</a></li>
              <li><a href="/contact">Contact</a></li>
              <li><a href="/blogs">Blogs</a></li>
              <li><a href="/services">Services</a></li>
              <li><a href="/therapist">Therapists</a></li>
              <li><a href="/quiz">Skin Test</a></li>
            </ul>
          </div>

          <div className={styles.column}>
            <h3>Services</h3>
            <ul>
              <li><a href="/services">Custom facials</a></li>
              <li><a href="/services">Anti-aging treatments</a></li>
              <li><a href="/services">Acne solutions</a></li>
              <li><a href="/services">Hydration therapy</a></li>
              <li><a href="/services">Skin resurfacing</a></li>
              <li><a href="/services">Specialty treatments</a></li>
            </ul>
          </div>
          <div className={styles.column}>
            <h3>Blogs</h3>
            <ul>
              <li><a href="/blogs">Skin Health</a></li>
              <li><a href="/blogs">Massage</a></li>
              <li><a href="/blogs">Sauna</a></li>
              <li><a href="/blogs">Wash Face</a></li>
              <li><a href="/blogs">Head Washing</a></li>
              <li><a href="/blogs">Acne Removal</a></li>
              <li><a href="/blogs">Skin Care</a></li>
              <li><a href="/blogs">Facial Care</a></li>
              <li><a href="/blogs">Scar Treatment</a></li>
              <li><a href="/blogs">Specialty treatments</a></li>
            </ul>
          </div>

          <div className={styles.column}>
            <h3>Social</h3>
            <ul>
              <li>📷 Instagram</li>
              <li>📘 Facebook</li>
              <li>✖ Twitter</li>
            </ul>
          </div>

          <div className={styles.column}>
            <h3>Contact us</h3>
            <ul>
              <li>📞 (123) 456-7890</li>
              <li>📧 lumiconnect.business@gmail.com</li>
              <li>📍 123 Skincare Street, Wellness City, NY 10001</li>
            </ul>
          </div>
        </div>
      </div>

      {/* Footer cuối */}
      <div className={styles.bottom}>
        <p>
          Designed by <span className={styles.link}><a href="/">LumiConnect</a></span>, Powered by{" "}
          <span className={styles.link}><a href="/">LumiConnect</a></span>
        </p>
        <div className={styles.policies}>
          <span><a href="/terms-conditions">Terms & Conditions</a></span>
          <span><a href="/privacy-policy">Privacy Policy</a></span>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
