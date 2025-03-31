import React from "react";
import { Card, Button, Typography } from "antd";
import { ReloadOutlined, LeftOutlined, SmileOutlined } from "@ant-design/icons";
import { useNavigate, useLocation } from "react-router-dom";
import styles from "./SkinTestResult.module.css";
import Footer from "../Footer/Footer";
import Header from "../Common/Header";

const { Title, Paragraph } = Typography;

const getResult = (score) => {
  if (score >= 20) return { type: "Da dầu", advice: "Sử dụng sản phẩm kiềm dầu, làm sạch sâu mỗi ngày.", service: "Liệu trình làm sạch da chuyên sâu & giảm dầu." };
  if (score >= 10) return { type: "Da hỗn hợp", advice: "Chăm sóc kỹ cả vùng dầu và vùng khô, dưỡng ẩm đúng cách.", service: "Liệu trình dưỡng ẩm & detox da." };
  return { type: "Da khô", advice: "Dưỡng ẩm thường xuyên, tránh sản phẩm chứa cồn.", service: "Liệu trình cấp ẩm chuyên sâu & phục hồi da." };
};

const SkinTestResult = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const params = new URLSearchParams(location.search);
  const score = parseInt(params.get("score"), 10) || 0;
  const result = getResult(score);

  return (
    <>
    <Header />
    <div className={styles.container}>
      <Card className={styles.card}>
        <SmileOutlined className={styles.icon} />
        <Title level={3}>Kết quả kiểm tra da</Title>
        <Title level={4} className={styles.skinType}>Loại da của bạn: <span>{result.type}</span></Title>
        <Paragraph className={styles.advice}>{result.advice}</Paragraph>
        <Paragraph><strong>Dịch vụ spa khuyên dùng:</strong> {result.service}</Paragraph>
        
        <div className={styles.buttonGroup}>
          <Button icon={<LeftOutlined />} onClick={() => navigate("/")}>
            Quay lại bài test
          </Button>
          <Button type="primary" icon={<ReloadOutlined />} onClick={() => navigate("/skin-test")}>
            Làm lại bài test
          </Button>
        </div>
      </Card>
    </div>
    <Footer />
    </>
  );
};

export default SkinTestResult;
