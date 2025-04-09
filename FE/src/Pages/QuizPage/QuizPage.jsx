import { useNavigate } from "react-router-dom";
import styles from "./QuizPage.module.css";
import Header from "../../Components/Common/Header";
import Footer from "../../Components/Footer/Footer";
import { useSelector } from "react-redux";
import { toast } from "react-toastify";

const QuizPage = () => {
  const navigate = useNavigate();
  const { accessToken } = useSelector((state) => state.auth);

  const handleTakeQuiz = () => {
      if (!accessToken) {
        navigate("/login");
        toast.warn("Please Login!");
        return;
      }
      navigate("/skin-test"); 
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
        <button className={styles.quizButton} onClick={handleTakeQuiz}>
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
