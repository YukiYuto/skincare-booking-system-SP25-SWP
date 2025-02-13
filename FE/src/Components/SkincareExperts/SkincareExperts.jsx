import styles from "./SkincareExperts.module.css";
import doc1 from "../../assets/images/doc1.png";
import doc2 from "../../assets/images/doc2.png";
import doc5 from "../../assets/images/doc5.png";
import doc9 from "../../assets/images/doc9.png";

// Dữ liệu chuyên gia
const experts = [
    { id: 1, name: "Ava Martinez", role: "Anti-Aging Specialist", image: doc1 },
    { id: 2, name: "Michael Kim", role: "Senior Esthetician", image: doc2 },
    { id: 3, name: "Sophia Chen", role: "Skin Health Consultant", image: doc5 },
    { id: 4, name: "Ava Martinez", role: "Anti-Aging Specialist", image: doc9 },
  ];

const SkincareExperts = () => {
  return (
    <section className={styles.expertsSection}>
      <div className={styles.header}>
        <h2>Meet our skincare experts</h2>
        <button className={styles.viewAllButton}>View all experts ➝</button>
      </div>

      <div className={styles.expertsList}>
        {experts.map((expert) => (
          <div key={expert.id} className={styles.expertCard}>
            <img src={expert.image} alt={expert.name} className={styles.expertImage} />
            <h3>{expert.name}</h3>
            <p>{expert.role}</p>
          </div>
        ))}
      </div>
    </section>
  );
};

export default SkincareExperts;
