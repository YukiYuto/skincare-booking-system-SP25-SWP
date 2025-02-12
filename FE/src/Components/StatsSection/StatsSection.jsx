import styles from "./StatsSection.module.css";

const stats = [
  { number: "5,000+", text: "Satisfied clients who trust us with their skincare." },
  { number: "10+", text: "Years providing expert skincare solutions." },
  { number: "20,000+", text: "Transformative treatments completed with care." },
];

const StatsSection = () => {
  return (
    <div className={styles.statsContainer}>
      {stats.map((stat, index) => (
        <div key={index} className={styles.statItem}>
          <h2>{stat.number}</h2>
          <p>{stat.text}</p>
        </div>
      ))}
    </div>
  );
};

export default StatsSection;
