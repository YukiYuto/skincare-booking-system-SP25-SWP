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
              <li>Home</li>
              <li>About</li>
              <li>Contact</li>
              <li>Blogs</li>
              <li>Blog detail</li>
              <li>Locations</li>
              <li>Services</li>
              <li>FAQs</li>
            </ul>
          </div>

          <div className={styles.column}>
            <h3>Services</h3>
            <ul>
              <li>Custom facials</li>
              <li>Anti-aging treatments</li>
              <li>Acne solutions</li>
              <li>Hydration therapy</li>
              <li>Skin resurfacing</li>
              <li>Specialty treatments</li>
            </ul>
          </div>
          <div className={styles.column}>
            <h3>Blogs</h3>
            <ul>
              <li>Skin Health</li>
              <li>Massage</li>
              <li>Sauna</li>
              <li>Wash Face</li>
              <li>Head Washing</li>
              <li>Acne Removal</li>
              <li>Skin Care</li>
              <li>Facial Care</li>
              <li>Scar Treatment</li>
              <li>Specialty treatments</li>
            </ul>
          </div>

          <div className={styles.column}>
            <h3>Social</h3>
            <ul>
              <li>📷 Instagram</li>
              <li>📘 Facebook</li>
              <li>✖ Twitter</li>
              <li>📌 Pinterest</li>
            </ul>
          </div>

          <div className={styles.column}>
            <h3>Contact us</h3>
            <ul>
              <li>📞 (123) 456-7890</li>
              <li>📧 info@example.com</li>
              <li>📍 123 Skincare Street, Wellness City, NY 10001</li>
            </ul>
          </div>
        </div>
      </div>

      {/* Footer cuối */}
      <div className={styles.bottom}>
        <p>
          Designed by <span className={styles.link}>LumiConnect</span>, Powered by{" "}
          <span className={styles.link}>LumiConnect</span>
        </p>
        <div className={styles.policies}>
          <span>Terms & Conditions</span>
          <span>Privacy Policy</span>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
