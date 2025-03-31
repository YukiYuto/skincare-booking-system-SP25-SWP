import { useState } from "react";
import styles from "./SkinTest.module.css";
import { useNavigate } from "react-router-dom";
import Header from "../Common/Header";
import Footer from "../Footer/Footer";

const SkinTest = () => {
  const [step, setStep] = useState(1);
  const [progress, setProgress] = useState(50);
  const [selectedAnswers, setSelectedAnswers] = useState({ step1: null, step2: null });
  const navigate = useNavigate();

  const steps = [
    {
      id: 1,
      title: "STEP 1",
      question: "Wash your face with a gentle cleanser and wait 15 - 30 minutes. Click the icon that matches what you see.",
      options: [
        { id: "dry", label: "Feels tight & dehydrated. Some flaky areas.", image: "https://www.paulaschoice.com/on/demandware.static/-/Library-Sites-paulachoice/default/dw0fae9daf/images/quiz/1_Dry.jpg" },
        { id: "oily", label: "Looks shiny & feels slick to the touch.", image: "https://www.paulaschoice.com/on/demandware.static/-/Library-Sites-paulachoice/default/dw7ee69eb0/images/quiz/1_Oily.jpg" },
        { id: "combo", label: "Looks dry in some areas, but shiny in others.", image: "https://www.paulaschoice.com/on/demandware.static/-/Library-Sites-paulachoice/default/dw8925484f/images/quiz/1_Combo.jpg" },
        { id: "normal", label: "Feels smooth, balanced & healthy.", image: "https://www.paulaschoice.com/on/demandware.static/-/Library-Sites-paulachoice/default/dw5f514fbe/images/quiz/1_Normal.jpg" },
      ],
      key: "step1",
    },
    {
      id: 2,
      title: "STEP 2",
      question: "What’s your main concern? If none of these apply, skip this part.",
      options: [
        { id: "breakouts", label: "Breakouts, blackheads or clogged pores", image: "https://www.paulaschoice.com/on/demandware.static/-/Library-Sites-paulachoice/default/dwc7c0dd51/images/quiz/acne.jpg" },
        { id: "aging", label: "Aging, loss of firmness or wrinkles", image: "https://www.paulaschoice.com/on/demandware.static/-/Library-Sites-paulachoice/default/dw4066c95e/images/quiz/aging.jpg" },
        { id: "uneven", label: "Uneven skin tone or discoloration", image: "https://www.paulaschoice.com/on/demandware.static/-/Library-Sites-paulachoice/default/dwd249b290/images/quiz/uneven.jpg" },
        { id: "sensitivity", label: "Redness or sensitivity", image: "https://www.paulaschoice.com/on/demandware.static/-/Library-Sites-paulachoice/default/dwa22c01d7/images/quiz/sensitive.jpg" },
      ],
      key: "step2",
    },
  ];

  const currentStep = steps.find((s) => s.id === step);

  const handleSelectOption = (optionId) => {
    setSelectedAnswers((prev) => ({
      ...prev,
      [currentStep.key]: prev[currentStep.key] === optionId ? null : optionId, // Nếu đã chọn thì hủy chọn
    }));
  };

  const handleNext = () => {
    if (step < steps.length) {
      setStep(step + 1);
      setProgress(progress + 50);
    } else {
      navigate("/result", { state: selectedAnswers });
    }
  };

  const handleBack = () => {
    if (step > 1) {
      setStep(step - 1);
      setProgress(progress - 50);
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
        <h2>{steps[step - 1].title}</h2>
        <p>{steps[step - 1].question}</p>
      </div>

      <div className={styles.rightPanel}>
        <div className={styles.options}>
          {steps[step - 1].options.map((option) => (
            <div
              key={option.id}
              className={`${styles.option} ${
                selectedAnswers[steps[step - 1].key] === option.id ? styles.selected : ""
              }`}
              onClick={() => handleSelectOption(option.id)}
            >
              <img src={option.image} alt={option.id} className={styles.optionImage} />
              <p>{option.label}</p>
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
            disabled={!selectedAnswers[steps[step - 1].key]}
            onClick={handleNext}
          >
            {step === steps.length ? "See Result" : "NEXT"}
          </button>
        </div>
      </div>
    </div>
    <Footer />
    </>
  );
};
export default SkinTest;
