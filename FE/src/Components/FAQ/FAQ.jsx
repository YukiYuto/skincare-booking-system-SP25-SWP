import { useState } from "react";
import styles from "./FAQ.module.css";

const faqs = [
    {
      question: "What is hydration therapy?",
      answer:
        "Hydration therapy is a specialized skincare treatment designed to restore moisture to your skin, enhance elasticity, and leave it glowing and refreshed.",
    },
    {
      question: "Is hydration therapy suitable for all skin types?",
      answer:
        "Yes, hydration therapy is safe and effective for all skin types, helping to maintain balance and hydration.",
    },
    {
      question: "How long does the treatment take?",
      answer:
        "A typical hydration therapy session lasts between 30 to 60 minutes, depending on your skin's needs.",
    },
    {
      question: "What results can I expect?",
      answer:
        "You can expect instantly refreshed, plumper, and deeply hydrated skin with a radiant glow.",
    },
  ];
  
const FAQ = () => {
  const [openIndex, setOpenIndex] = useState(0);

  const toggleFAQ = (index) => {
    setOpenIndex(index === openIndex ? null : index);
  };

  return (
    <section className={styles.faqSection}>
      <h2 className={styles.title}>Frequently asked questions</h2>
      <div className={styles.faqContainer}>
        {faqs.map((faq, index) => (
          <div
            key={index}
            className={`${styles.faqItem} ${openIndex === index ? styles.active : ""}`}
            onClick={() => toggleFAQ(index)}
          >
            <div className={styles.faqHeader}>
              <span className={styles.icon}>{openIndex === index ? "−" : "+"}</span>
              <span className={styles.question}>{faq.question}</span>
            </div>
            {openIndex === index && <p className={styles.answer}>{faq.answer}</p>}
          </div>
        ))}
      </div>
    </section>
  );
};

export default FAQ;
