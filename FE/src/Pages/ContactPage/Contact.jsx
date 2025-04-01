import { FaFacebookF, FaInstagram, FaTimes } from "react-icons/fa";
import styles from "./Contact.module.css";
import FAQ from "../../Components/FAQ/FAQ";
import Header from "../../Components/Common/Header";
import Footer from "../../Components/Footer/Footer";

const Contact = () => {
  return (
    <div>
    <Header/>
    <section className={styles.contactSection}>
      <h2 className={styles.title}>Contact us</h2>
      <p className={styles.inquiryText}>
        For press related inquiries, please get in touch via <strong>lumiconnect.business@gmail.com</strong>
      </p>
      <p className={styles.phone}>Call us: <strong>+2 654 785 1245</strong></p>
      <p className={styles.address}>Visit store: Chicago HQ Estica Cop. Macomb, MI 48042</p>

      <div className={styles.socialIcons}>
        <FaFacebookF />
        <FaInstagram />
        <FaTimes />
      </div>
    </section>
    <iframe
        src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3918.163889733434!2d106.79814837451843!3d10.875136457366308!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3174d8a6b19d6763%3A0x143c54525028b2e!2sVNUHCM%20Student%20Cultural%20House!5e0!3m2!1sen!2s!4v1743048684798!5m2!1sen!2s"
        width="100%"
        height="500"
        allowFullScreen={true}
        loading="lazy"
        className="border-4 border-gray-700 rounded-lg shadow-2xl"
        title="Google Map"
      ></iframe>
    
    <FAQ/><br/>
    <Footer />
    </div>
  );
};

export default Contact;
