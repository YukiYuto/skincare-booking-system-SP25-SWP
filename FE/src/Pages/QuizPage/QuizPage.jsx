import { useNavigate } from "react-router-dom";
import styles from "./QuizPage.module.css";
import Header from "../../Components/Common/Header";
import Footer from "../../Components/Footer/Footer";
import { useSelector } from "react-redux";

const QuizPage = () => {
  const navigate = useNavigate();

  const { user } = useSelector((state) => state.auth);
    const isAuthenticated = user?.accessToken; 
  
    const handleClick = (e) => {
      if (!isAuthenticated) {
        e.preventDefault(); 
        navigate("/login"); 
      } else {
        navigate("/skin-test");
      }
    };

  return (
    <>
    <Header />
    <div className={styles.container}>
      <div className={styles.textSection}>
        <h3>QUICK QUIZ</h3>
        <h1>
          INSTANT <span className={styles.highlight}>ROUTINE</span>
        </h1>
        <p>
          Skin is complicated—this quiz isn’t. In just<br/> a few questions, we’ll
          determine your skin<br/> type & find the right collection for you.
        </p>
        <button className={styles.quizButton} onClick={handleClick}>
          TAKE THE QUIZ
        </button>
      </div>
      <div className={styles.imageSection}></div>
    </div>
    <div style={{marginTop:"30px"}}>
    <Footer />
    </div>
    </>
  );
};

export default QuizPage;
