import { Button } from "antd";
import styles from "./FeaturesSection.module.css";

// Danh sách dữ liệu cho 3 thẻ
const features = [
  {
    id: 1,
    icon: "🧪", // Hoặc có thể dùng hình ảnh SVG
    title: "Clean ingredients",
    description: "We prioritize high-quality, natural ingredients that are safe and effective for your skin.",
    bgColor: "#FEF1E3",
  },
  {
    id: 2,
    icon: "🌿",
    title: "Sustainable beauty",
    description: "Our eco-conscious approach ensures that our products are kind to the environment and your skin.",
    bgColor: "#FEE4E0",
  },
  {
    id: 3,
    icon: "✔️",
    title: "Dermatologist-approved",
    description: "Each formula is rigorously tested for purity and efficacy, so you can trust what goes on your skin.",
    bgColor: "#FAF1EF",
  },
];

const FeaturesSection = () => {
  return (
    <div className={styles.container}>
      {/* Tiêu đề chính */}
      <div className={styles.header}>
        <div className={styles.icon}>✨</div>
        <h2>The ultimate guide to radiant beauty</h2>
        <p>
          At Skinn Care, we believe that healthy skin starts with pure, effective ingredients.
          Our journey began with a simple mission.
        </p>
        <Button type="primary" className={styles.button}>Learn more →</Button>
      </div>

      {/* 3 Thẻ Nội Dung */}
      <div className={styles.featuresGrid}>
        {features.map((feature) => (
          <div key={feature.id} className={styles.featureItem} style={{ backgroundColor: feature.bgColor }}>
            <div className={styles.featureIcon}>{feature.icon}</div>
            <h3>{feature.title}</h3>
            <p>{feature.description}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default FeaturesSection;
