import Header from "../Common/Header";
import Footer from "../Footer/Footer";
import styles from "./PrivacyPolicy.module.css";

const PrivacyPolicy = () => {
  return (
    <>
    <Header />
    <div className={styles.container}>
      <h1 className={styles.title}>Privacy Policy</h1>
      <p className={styles.updated}>Last Updated: April 1, 2025</p>

      <section className={styles.section}>
        <h2>1. Introduction</h2>
        <p>
          Welcome to **LUMICONNECT**. Your privacy is important to us. This Privacy Policy explains how we collect, use, and protect your personal information.
        </p>
      </section>

      <section className={styles.section}>
        <h2>2. Information We Collect</h2>
        <p>
          - **Personal Information**: Name, email, phone number, and booking details.  
          - **Payment Information**: Securely processed via third-party payment providers.  
          - **Usage Data**: Information about how you interact with our website.  
        </p>
      </section>

      <section className={styles.section}>
        <h2>3. How We Use Your Information</h2>
        <p>
          - To process bookings and provide skincare services.  
          - To improve our website and customer experience.  
          - To send promotions or updates (you can opt out anytime).  
        </p>
      </section>

      <section className={styles.section}>
        <h2>4. Data Security</h2>
        <p>
          - We use encryption and secure servers to protect your data.  
          - We do not sell or share your information with third parties, except as required by law.  
        </p>
      </section>

      <section className={styles.section}>
        <h2>5. Cookies and Tracking</h2>
        <p>
          - We use cookies to enhance your experience.  
          - You can manage cookie settings in your browser.  
        </p>
      </section>

      <section className={styles.section}>
        <h2>6. Your Rights</h2>
        <p>
          - You have the right to access, update, or delete your personal data.  
          - Contact us if you wish to exercise these rights.  
        </p>
      </section>

      <section className={styles.section}>
        <h2>7. Policy Changes</h2>
        <p>
          - We may update this Privacy Policy periodically.  
          - Please check this page for any changes.  
        </p>
      </section>

      <p className={styles.contact}>
        If you have any questions, contact us via email: <a href="mailto:lumiconnect.business@gmail.com">lumiconnect.business@gmail.com</a>
      </p>
    </div>
    <Footer />
    </>
  );
};

export default PrivacyPolicy;
