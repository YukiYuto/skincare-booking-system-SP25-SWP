import { useState, useEffect } from "react";
import axios from "axios";
import styles from "./SkinTest.module.css";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import Header from "../Common/Header";
import Footer from "../Footer/Footer";

const SkinTest = () => {
  const [step, setStep] = useState(0);
  const [progress, setProgress] = useState(0);
  const [questions, setQuestions] = useState([]);
  const [answers, setAnswers] = useState({});
  const [selectedAnswers, setSelectedAnswers] = useState({});
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const { accessToken } = useSelector((state) => state.auth);

  useEffect(() => {
    const fetchQuestions = async () => {
      try {
        const res = await axios.get("https://lumiconnect.azurewebsites.net/api/TestQuestion/all", {
          headers: { Authorization: `Bearer ${accessToken}` },
        });

        const questionData = res.data.result;
        setQuestions(questionData);

        const answersData = {};
        await Promise.all(
          questionData.map(async (question) => {
            const resAnswers = await axios.get(
              `https://lumiconnect.azurewebsites.net/api/TestAnswer/TestQuestion/${question.testQuestionId}`,
              { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            answersData[question.testQuestionId] = resAnswers.data.result;
          })
        );

        setAnswers(answersData);
        setLoading(false);
        setStep(1);
      } catch (error) {
        console.error("Error fetching data:", error);
        setLoading(false);
      }
    };

    fetchQuestions();
  }, []);

  if (loading) {
    return <div className={styles.loading}>Loading...</div>;
  }

  const currentQuestion = questions[step - 1];

  const handleSelectOption = (testAnswerId) => {
    if (!currentQuestion) {
      console.error("❌ Lỗi: currentQuestion bị undefined");
      return;
    }

    setSelectedAnswers((prev) => {
      const prevAnswers = prev[currentQuestion.testQuestionId] || [];
      const allowMultipleSelection = false;

      let updatedAnswers;
      if (allowMultipleSelection) {
        updatedAnswers = prevAnswers.includes(testAnswerId)
          ? prevAnswers.filter((id) => id !== testAnswerId)
          : [...prevAnswers, testAnswerId];
      } else {
        updatedAnswers = [testAnswerId];
      }

      return { ...prev, [currentQuestion.testQuestionId]: updatedAnswers };
    });
  };

  const calculateTotalScore = () => {
    let totalScore = 0;

    Object.entries(selectedAnswers).forEach(([questionId, selectedOptionIds]) => {
      selectedOptionIds.forEach((testAnswerId) => {
        const answerOption = answers[questionId]?.find((opt) => opt.testAnswerId === testAnswerId);
        if (answerOption) {
          totalScore += parseInt(answerOption.score, 10);
        }
      });
    });

    console.log("Total Score Calculated:", totalScore);
    return totalScore;
  };

  const handleFinishTest = async () => {
    const totalScore = calculateTotalScore();

    try {
      const res = await axios.get("https://lumiconnect.azurewebsites.net/api/skin-profile/all", {
        headers: { Authorization: `Bearer ${accessToken}` },
      });

      const skinProfiles = res.data.result;
      const matchedSkin = skinProfiles.find(
        (skin) => totalScore >= skin.scoreMin && totalScore <= skin.scoreMax
      );

      navigate("/result", {
        state: {
          totalScore,
          skinName: matchedSkin?.skinName || "Unknown",
          description: matchedSkin?.description || "No description available.",
        },
      });
    } catch (error) {
      console.error("Error fetching skin profile:", error);
      navigate("/result", {
        state: { totalScore, skinName: "Error", description: "Could not determine skin type." },
      });
    }
  };

  const handleNext = () => {
    if (step < questions.length) {
      setStep(step + 1);
      setProgress(((step + 1) / questions.length) * 100);
    } else {
      handleFinishTest();
    }
  };

  const handleBack = () => {
    if (step > 1) {
      setStep(step - 1);
      setProgress(((step - 1) / questions.length) * 100);
    }
  };

  return (
    <>
      <Header />
      <div className={styles.container}>
        <div className={styles.leftPanel}>
          <div className={styles.progressBar}>
            <div className={styles.progress} style={{ width: `${progress}%` }} />
          </div>
          <h2>STEP {step} : {currentQuestion?.type}</h2>
          <p>{currentQuestion?.content}</p>
        </div>

        <div className={styles.rightPanel}>
          <div className={styles.options}>
            {answers[currentQuestion?.testQuestionId]?.map((option) => (
              <div
                key={option.testAnswerId}
                className={`${styles.option} ${
                  selectedAnswers[currentQuestion.testQuestionId]?.includes(option.testAnswerId) ? styles.selected : ""
                }`}
                onClick={() => handleSelectOption(option.testAnswerId)}
              >
                <p>{option.content}<br /> (Score: {option.score})</p>
              </div>
            ))}
          </div>

          <div className={styles.buttonGroup}>
            {step > 1 && (
              <button className={styles.backButton} onClick={handleBack}>
                BACK
              </button>
            )}
            <button
              className={styles.nextButton}
              disabled={!selectedAnswers[currentQuestion?.testQuestionId] || selectedAnswers[currentQuestion?.testQuestionId].length === 0}
              onClick={handleNext}
            >
              {step === questions.length ? "See Result" : "NEXT"}
            </button>
          </div>
        </div>
      </div>
      <div style={{marginTop: "80px"}}>
        <Footer />
      </div>
    </>
  );
};

export default SkinTest;
