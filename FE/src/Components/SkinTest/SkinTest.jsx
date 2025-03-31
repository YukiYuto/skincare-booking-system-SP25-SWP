import React, { useState } from "react";
import { Card, Radio, Button, Typography } from "antd";
import { LeftOutlined, RightOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import styles from "./SkinTest.module.css";
import Footer from "../Footer/Footer";
import Header from "../Common/Header";

const { Title } = Typography;

const questions = [
  { question: "Da bạn có bị khô sau khi rửa mặt không?", options: [{ text: "Rất khô", score: 3 }, { text: "Hơi khô", score: 2 }, { text: "Bình thường", score: 1 }, { text: "Bị dầu", score: 0 }] },
  { question: "Lỗ chân lông của bạn như thế nào?", options: [{ text: "Rất to", score: 3 }, { text: "Hơi to", score: 2 }, { text: "Bình thường", score: 1 }, { text: "Nhỏ", score: 0 }] },
  { question: "Bạn có dễ bị mụn không?", options: [{ text: "Rất dễ", score: 3 }, { text: "Thỉnh thoảng", score: 2 }, { text: "Hiếm khi", score: 1 }, { text: "Không bao giờ", score: 0 }] },
  { question: "Vùng chữ T (trán, mũi) của bạn có bị dầu không?", options: [{ text: "Rất nhiều dầu", score: 3 }, { text: "Hơi dầu", score: 2 }, { text: "Không dầu", score: 1 }, { text: "Khô", score: 0 }] },
  { question: "Bạn có dễ bị kích ứng không?", options: [{ text: "Rất dễ", score: 3 }, { text: "Thỉnh thoảng", score: 2 }, { text: "Hiếm khi", score: 1 }, { text: "Không bao giờ", score: 0 }] },
  { question: "Bạn cảm thấy da mình như thế nào vào cuối ngày?", options: [{ text: "Bóng dầu", score: 3 }, { text: "Hơi nhờn", score: 2 }, { text: "Bình thường", score: 1 }, { text: "Khô căng", score: 0 }] },
  { question: "Bạn có hay bị bong tróc da không?", options: [{ text: "Thường xuyên", score: 3 }, { text: "Thỉnh thoảng", score: 2 }, { text: "Hiếm khi", score: 1 }, { text: "Không bao giờ", score: 0 }] },
  { question: "Da bạn có sáng bóng tự nhiên không?", options: [{ text: "Rất sáng", score: 3 }, { text: "Hơi sáng", score: 2 }, { text: "Bình thường", score: 1 }, { text: "Xỉn màu", score: 0 }] },
  { question: "Bạn có thường xuyên dùng kem dưỡng ẩm không?", options: [{ text: "Mỗi ngày", score: 3 }, { text: "Thỉnh thoảng", score: 2 }, { text: "Rất ít", score: 1 }, { text: "Không bao giờ", score: 0 }] },
  { question: "Bạn có thấy da mình thay đổi theo thời tiết không?", options: [{ text: "Rất nhiều", score: 3 }, { text: "Hơi nhiều", score: 2 }, { text: "Bình thường", score: 1 }, { text: "Không thay đổi", score: 0 }] },
];

const SkinTest = () => {
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [answers, setAnswers] = useState(Array(questions.length).fill(null));
  const navigate = useNavigate();

  const handleAnswer = (score) => {
    const newAnswers = [...answers];
    newAnswers[currentQuestion] = score;
    setAnswers(newAnswers);
  };

  const handleNext = () => {
    if (answers[currentQuestion] !== null) {
      setCurrentQuestion((prev) => prev + 1);
    } else {
      alert("Vui lòng chọn một đáp án!");
    }
  };

  const handlePrev = () => {
    setCurrentQuestion((prev) => prev - 1);
  };

  const handleSubmit = () => {
    if (answers.includes(null)) {
      alert("Vui lòng trả lời tất cả các câu hỏi!");
      return;
    }
    const totalScore = answers.reduce((sum, score) => sum + (score || 0), 0);
    navigate(`/result?score=${totalScore}`);
  };

  return (
    <>
    <Header />
    <div className={styles.container}>
      <Card className={styles.card}>
        <Title level={4}>{questions[currentQuestion].question}</Title>
        <Radio.Group onChange={(e) => handleAnswer(e.target.value)} value={answers[currentQuestion]} className={styles.radioGroup}>
          {questions[currentQuestion].options.map((option, index) => (
            <Radio key={index} value={option.score} className={styles.radio}>
              {option.text}
            </Radio>
          ))}
        </Radio.Group>

        <div className={styles.buttonGroup}>
          {currentQuestion > 0 && (
            <Button icon={<LeftOutlined />} onClick={handlePrev}>
              Quay lại
            </Button>
          )}
          {currentQuestion < questions.length - 1 ? (
            <Button type="primary" icon={<RightOutlined />} onClick={handleNext}>
              Tiếp tục
            </Button>
          ) : (
            <Button type="primary" className={styles.submitBtn} onClick={handleSubmit}>
              Xem kết quả
            </Button>
          )}
        </div>
      </Card>
    </div>
    <Footer />
    </>
  );
};

export default SkinTest;
