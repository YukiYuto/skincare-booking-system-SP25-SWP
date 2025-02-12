import styles from "./TrustSection.module.css";
import trustImage from "../../assets/images/trust-image.jpg"; // Đường dẫn ảnh
import checked from "../../assets/images/checked.png";

const TrustSection = () => {
  return (
    <section className={styles.trustSection}>
      {/* Ảnh tròn bên trái */}
      <div className={styles.imageContainer}>
        <img src={trustImage} alt="Why trust Glisten" className={styles.trustImage} />
      </div>

      {/* Nội dung bên phải */}
      <div className={styles.content}>
        <h2 className={styles.title}>Why trust Glisten</h2>
        <p className={styles.description}>
          With years of experience, cutting-edge techniques, and a commitment to quality, we’re dedicated to helping you achieve radiant skin.
        </p>

        {/* Danh sách lợi ích */}
        <ul className={styles.benefits}>
          <li><img className={styles.checked} src={checked} alt=""/> Experienced skincare specialists</li>
          <li><img className={styles.checked} src={checked} alt=""/> Personalized treatment plans</li>
          <li><img className={styles.checked} src={checked} alt=""/> Safe, effective products</li>
          <li><img className={styles.checked} src={checked} alt=""/> Exceptional client care</li>
        </ul>
      </div>
    </section>
  );
};

export default TrustSection;
