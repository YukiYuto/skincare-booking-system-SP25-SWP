import { useLocation, useNavigate } from "react-router-dom";
import styles from "./Result.module.css";
import Header from "../Common/Header";
import Footer from "../Footer/Footer";

const Result = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { totalScore, skinName, description } = location.state || {};

  return (
    <>
      <Header />
      <div className={styles.resultContainer}>
        <h2>Your Skin Analysis Result</h2>
        <p className={styles.skinName}><strong>Skin Type:</strong> {skinName}</p>
        <p><strong>Description:</strong> {description}</p>
        <p className={styles.score}><strong>Total Score:</strong> {totalScore}</p>

        <button className={styles.backButton} onClick={() => navigate("/quiz")}>
          Take the Test Again
        </button>
      </div>
      <div style={{marginTop: "30px"}}>
        <Footer />
      </div>
      
    </>
  );
};

export default Result;
