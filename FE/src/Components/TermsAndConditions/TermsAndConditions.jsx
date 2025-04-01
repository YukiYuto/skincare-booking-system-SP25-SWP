import Header from "../Common/Header";
import Footer from "../Footer/Footer";
import styles from "./TermsAndConditions.module.css";

const TermsAndConditions = () => {
  return (
    <>
    <Header />
    <div className={styles.container}>
      <h1 className={styles.title}>Terms & Conditions</h1>
      <p className={styles.updated}>Last Updated: April 1, 2025</p>

      <section className={styles.section}>
        <h2>1. Introduction</h2>
        <p>
          Welcome to **LUMICONNECT**. By using our skincare booking services, you agree to these terms.
        </p>
      </section>

      <section className={styles.section}>
        <h2>2. Appointment Booking</h2>
        <p>
          - You must provide accurate information when booking.  
          - Cancellations must be made **at least 24 hours** in advance to avoid cancellation fees.  
        </p>
      </section>

      <section className={styles.section}>
        <h2>3. Payment Policy</h2>
        <p>
          - Payments can be made via **PayOs or cash**.  
          - All transactions are securely processed following international security standards.  
        </p>
      </section>

      <section className={styles.section}>
        <h2>4. Privacy Policy</h2>
        <p>
          - We are committed to protecting your personal data.  
          - See our full <a className={styles.privacy} href="/privacy-policy">Privacy Policy</a> for more details.  
        </p>
      </section>

      <section className={styles.section}>
        <h2>5. Changes and Updates</h2>
        <p>
          - We may update these terms at any time.  
          - Please check this page periodically for the latest updates.  
        </p>
      </section>

      <p className={styles.contact}>
        If you have any questions, please contact us via email: <a href="mailto:lumiconnect.business@gmail.com">lumiconnect.business@gmail.com</a>
      </p>
    </div>
    <Footer />
    </>
  );
};

export default TermsAndConditions;
