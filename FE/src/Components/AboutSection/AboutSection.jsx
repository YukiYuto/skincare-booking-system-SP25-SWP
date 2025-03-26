import styles from "./AboutSection.module.css";
import skincareImage from "../../assets/images/skincare-treatment.jpg"; // Đảm bảo đường dẫn đúng
import massage from "../../assets/images/massage.jpg";

const AboutSection = () => {
  return (
    <section className={styles.aboutSection}>
      <div className={styles.container}>
        {/* Phần "Who we are" */}
        <div className={styles.textBlock}>
          <h2>Who we are</h2>
          <p>
            At Glisten, we are passionate about helping you achieve your skin goals. Our team of skincare experts combines
            cutting-edge treatments with a personalized approach to deliver visible, lasting results. Whether you are looking for
            hydration, rejuvenation, or a tailored solution to specific concerns, we are here to guide you every step of the way.
          </p>
        </div>

        {/* Phần "Our mission" */}
        <div className={styles.missionBlock}>
          <div className={styles.missionText}>
            <h3>Our mission: your confidence</h3>
            <p>
              We believe that great skin is the foundation of<br/> self-confidence.
            </p>
            <p>
              Our mission is to empower you with the knowledge,<br/> tools, and treatments you need to feel 
              your best inside<br/> and out.
            </p>
          </div>
          <div className={styles.imageContainer}>
            <img src={skincareImage} alt="Skincare Treatment" className={styles.missionImage} />
          </div>
        </div>

        <div className={styles.missionBlock1}>
          <div className={styles.imageContainer1}>
            <img src={massage} alt="Massage" className={styles.missionImage1} />
          </div>
          <div className={styles.missionText1}>
            <h3>What sets us apart</h3>
            <p>
            With years of experience, a client-first philosophy, and a commitment to excellence.
            </p>
            <ul>
            <li>
              Experienced Specialists: Certified professionals<br/> dedicated to your care.
            </li>
            <li>
              Personalized Plans: Treatments tailored to your<br/> unique skin needs.
            </li>
            <li>
              Safe & Effective Products: Only the best for your skin.
            </li>
          </ul>
          </div>
          
        </div>
      </div>
    </section>
  );
};

export default AboutSection;
