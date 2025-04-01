import styles from "./AboutPage.module.css";
import teamImage from "../../assets/images/team.png"; // Đảm bảo đường dẫn đúng
import AboutSection from "../../Components/AboutSection/AboutSection";
import SkincareExperts from "../../Components/SkincareExperts/SkincareExperts";
import Header from "../../Components/Common/Header";
import Footer from "../../Components/Footer/Footer";

const AboutPage = () => {
  return (
    <div>
    <Header/>
    <section className={styles.aboutSection}>
      <div className={styles.container}>
        <div className={styles.imageContainer}>
          <img src={teamImage} alt="Skincare Experts" className={styles.teamImage} />
        </div>

        <div className={styles.contentWrapper}>
         
          <div className={styles.textContainer}>
            <h2>Your journey to radiant<br/> skin starts here</h2>
            <p>At Glisten, we blend science, care, and expertise to bring out<br/> the best in your skin.</p>
          </div>

          
          <div className={styles.experience}>
            <h3>10+</h3>
            <p>Years providing expert<br/> skincare solutions.</p>
          </div>
        </div>

        <AboutSection/><br/>

        <SkincareExperts/>

      </div>

    </section>
    <Footer />
    </div>
  );
};

export default AboutPage;
