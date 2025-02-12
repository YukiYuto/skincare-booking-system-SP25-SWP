import styles from "./AboutPage.module.css";
import teamImage from "../../assets/images/team.png"; // Đảm bảo đường dẫn đúng
import AboutSection from "../../Components/AboutSection/AboutSection";
import SkincareExperts from "../../Components/SkincareExperts/SkincareExperts";

const AboutPage = () => {
  return (
    <section className={styles.aboutSection}>
      <div className={styles.container}>
        {/* Hình ảnh đội ngũ chuyên gia */}
        <div className={styles.imageContainer}>
          <img src={teamImage} alt="Skincare Experts" className={styles.teamImage} />
        </div>

        {/* Bố cục hàng ngang: text + experience */}
        <div className={styles.contentWrapper}>
          {/* Nội dung text */}
          <div className={styles.textContainer}>
            <h2>Your journey to radiant<br/> skin starts here</h2>
            <p>At Glisten, we blend science, care, and expertise to bring out<br/> the best in your skin.</p>
          </div>

          {/* Experience */}
          <div className={styles.experience}>
            <h3>10+</h3>
            <p>Years providing expert<br/> skincare solutions.</p>
          </div>
        </div>

        <AboutSection/><br/>

        <SkincareExperts/>

      </div>

    </section>
  );
};

export default AboutPage;
