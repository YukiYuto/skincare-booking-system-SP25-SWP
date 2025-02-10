import styles from "./SkincareTips.module.css";

// Import ảnh từ thư mục assets
import skincare1 from "../../assets/images/skincare1.png";
import skincare2 from "../../assets/images/skincare2.png";
import skincare3 from "../../assets/images/skincare3.png";

const SkincareTips = () => {
  const articles = [
    {
      img: skincare1,
      title: "The ultimate guide to winter skincare",
      date: "February 8, 2025",
    },
    {
      img: skincare2,
      title: "Top 5 anti-aging ingredients you need to know",
      date: "February 9, 2025",
    },
    {
      img: skincare3,
      title: "Common skincare myths - debunked!",
      date: "February 10, 2025",
    },
  ];

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Skincare tips & insights</h2>

      <div className={styles.articles}>
        {articles.map((article, index) => (
          <div key={index} className={styles.article}>
            <img src={article.img} alt={article.title} className={styles.image} />
            <h3 className={styles.articleTitle}>{article.title}</h3>
            <p className={styles.date}>📅 {article.date}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default SkincareTips;
