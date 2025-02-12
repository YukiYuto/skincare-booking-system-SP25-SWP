import { FaFacebookF, FaInstagram, FaPinterestP, FaTimes } from "react-icons/fa";
import styles from "./Contact.module.css";
import ContactForm from "../../Components/ContactForm/ContactForm";
import FAQ from "../../Components/FAQ/FAQ";

const Contact = () => {
  return (
    <div>
    <section className={styles.contactSection}>
      <h2 className={styles.title}>Contact us</h2>
      <p className={styles.inquiryText}>
        For press related inquiries, please get in touch via <strong>example@domain.com</strong>
      </p>
      <p className={styles.phone}>Call us: <strong>+2 654 785 1245</strong></p>
      <p className={styles.address}>Visit store: Chicago HQ Estica Cop. Macomb, MI 48042</p>

      <div className={styles.socialIcons}>
        <FaFacebookF />
        <FaInstagram />
        <FaPinterestP />
        <FaTimes />
      </div>
    </section><br/>

    <ContactForm/><br/>
    
    <FAQ/><br/>

    </div>
  );
};

export default Contact;
