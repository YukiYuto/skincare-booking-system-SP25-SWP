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

  const { accessToken, user } = useSelector((state) => state.auth);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchQuestions = async () => {
      try {
        const res = await axios.get(
          "https://lumiconnect.azurewebsites.net/api/TestQuestion/all",
          {
            headers: { Authorization: `Bearer ${accessToken}` },
          }
        );

        const questionData = res.data.result;
        setQuestions(questionData);

        const answersData = {};
        await Promise.all(
          questionData.map(async (question) => {
            const resAnswers = await axios.get(
              `https://lumiconnect.azurewebsites.net/api/TestAnswer/TestQuestion/${question.testQuestionId}`,
              {
                headers: { Authorization: `Bearer ${accessToken}` },
              }
            );
            answersData[question.testQuestionId] = resAnswers.data.result;
          })
        );

        setAnswers(answersData);
        setStep(1);
        setLoading(false);
      } catch (error) {
        console.error("Lỗi khi load câu hỏi:", error);
        setLoading(false);
      }
    };

    fetchQuestions();
  }, []);

  const currentQuestion = questions[step - 1];

  const handleSelectOption = (testAnswerId) => {
    if (!currentQuestion) return;

    setSelectedAnswers((prev) => {
      const allowMultipleSelection = false;
      const prevAnswers = prev[currentQuestion.testQuestionId] || [];

      const updatedAnswers = allowMultipleSelection
        ? prevAnswers.includes(testAnswerId)
          ? prevAnswers.filter((id) => id !== testAnswerId)
          : [...prevAnswers, testAnswerId]
        : [testAnswerId];

      return {
        ...prev,
        [currentQuestion.testQuestionId]: updatedAnswers,
      };
    });
  };

  const calculateTotalScore = () => {
    let totalScore = 0;

    Object.entries(selectedAnswers).forEach(([questionId, selectedOptionIds]) => {
      selectedOptionIds.forEach((testAnswerId) => {
        const answerOption = answers[questionId]?.find(
          (opt) => opt.testAnswerId === testAnswerId
        );
        if (answerOption) {
          totalScore += parseInt(answerOption.score, 10);
        }
      });
    });

    return totalScore;
  };

  const handleFinishTest = async () => {
    const totalScore = calculateTotalScore();

    try {
      // 🔍 Lấy customerId theo phone number
      const customerRes = await axios.get("https://lumiconnect.azurewebsites.net/api/customer", {
        headers: { Authorization: `Bearer ${accessToken}` },
      });

      const matchedCustomer = customerRes.data.result.find(
        (c) => c.phoneNumber === user.phoneNumber
      );
      const customerId = matchedCustomer?.customerId;
      if (!customerId) throw new Error("Không tìm thấy customerId");

      // 🔍 Lấy skinTestId
      const skinTestRes = await axios.get("https://lumiconnect.azurewebsites.net/api/skintest/all", {
        headers: { Authorization: `Bearer ${accessToken}` },
      });

      const skinTestId = skinTestRes.data.result[0]?.skinTestId;
      if (!skinTestId) throw new Error("Không tìm thấy skinTestId");

      // 🔄 Format dữ liệu câu trả lời
      const formattedAnswers = Object.entries(selectedAnswers).flatMap(([questionId, answerIds]) =>
        answerIds.map((answerId) => ({
          testQuestionId: questionId,
          testAnswerId: answerId,
        }))
      );

      // 📤 Gửi dữ liệu lên customer-skin-test
      await axios.post(
        "https://lumiconnect.azurewebsites.net/api/customer-skin-test",
        {
          customerId,
          skinTestId,
          answers: formattedAnswers,
        },
        {
          headers: { Authorization: `Bearer ${accessToken}` },
        }
      );

      // 🔍 Lấy kết quả phân tích da từ điểm số
      const skinProfileRes = await axios.get(
        "https://lumiconnect.azurewebsites.net/api/skin-profile/all",
        {
          headers: { Authorization: `Bearer ${accessToken}` },
        }
      );

      const skinProfiles = skinProfileRes.data.result;
      const matchedSkin = skinProfiles.find(
        (skin) => totalScore >= skin.scoreMin && totalScore <= skin.scoreMax
      );

      // 👉 Chuyển đến trang kết quả
      navigate("/result", {
        state: {
          totalScore,
          skinName: matchedSkin?.skinName || "Unknown",
          description: matchedSkin?.description || "No description available.",
        },
      });
    } catch (err) {
      console.error("Lỗi khi gửi dữ liệu hoặc lấy kết quả:", err);
      navigate("/result", {
        state: {
          totalScore,
          skinName: "Error",
          description: "Không thể xác định loại da.",
        },
      });
    }
  };

  const handleNext = () => {
    if (step < questions.length) {
      setStep(step + 1);
      setProgress(((step + 1) / questions.length) * 100);
    } else {
      handleFinishTest(); // ✅ Gửi kết quả & chuyển trang
    }
  };

  const handleBack = () => {
    if (step > 1) {
      setStep(step - 1);
      setProgress(((step - 1) / questions.length) * 100);
    }
  };

  if (loading) return <div className={styles.loading}>Loading...</div>;

  return (
    <>
      <Header />
      <div className={styles.container}>
        <div className={styles.leftPanel}>
          <div className={styles.progressBar}>
            <div className={styles.progress} style={{ width: `${progress}%` }} />
          </div>
          <h2>STEP {step}: {currentQuestion?.content}</h2>
          <p>{currentQuestion?.type}</p>
        </div>

        <div className={styles.rightPanel}>
          <div className={styles.options}>
            {answers[currentQuestion?.testQuestionId]?.map((option) => (
              <div
                key={option.testAnswerId}
                className={`${styles.option} ${
                  selectedAnswers[currentQuestion.testQuestionId]?.includes(option.testAnswerId)
                    ? styles.selected
                    : ""
                }`}
                onClick={() => handleSelectOption(option.testAnswerId)}
              >
                <p>
                  {option.content} <br /> (Score: {option.score})
                </p>
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
              disabled={
                !selectedAnswers[currentQuestion?.testQuestionId] ||
                selectedAnswers[currentQuestion?.testQuestionId].length === 0
              }
              onClick={handleNext}
            >
              {step === questions.length ? "See Result" : "NEXT"}
            </button>
          </div>
        </div>
      </div>

      <div style={{ marginTop: "80px" }}>
        <Footer />
      </div>
    </>
  );
};

export default SkinTest;
