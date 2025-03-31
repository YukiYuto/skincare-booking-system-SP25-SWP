import { useLocation, useNavigate } from "react-router-dom";
import styles from "./Result.module.css";
import Footer from "../Footer/Footer";
import Header from "../Common/Header";

const resultData = {
  dry: {
    title: "Your Skin Type: Dry",
    description: "Your skin tends to feel tight and dehydrated. It may be prone to flakiness and irritation.",
    recommendations: [
      "Use a hydrating cleanser that doesn’t strip moisture.",
      "Incorporate a rich moisturizer with ceramides and hyaluronic acid.",
      "Avoid alcohol-based products that can dry out your skin.",
    ],
  },
  oily: {
    title: "Your Skin Type: Oily",
    description: "Your skin produces excess oil, making it prone to shine and clogged pores.",
    recommendations: [
      "Use a gentle foaming cleanser to remove excess oil.",
      "Look for oil-free and non-comedogenic moisturizers.",
      "Incorporate a BHA exfoliant (salicylic acid) to unclog pores.",
    ],
  },
  combo: {
    title: "Your Skin Type: Combination",
    description: "Your skin is dry in some areas and oily in others, usually in the T-zone.",
    recommendations: [
      "Use a gentle, balanced cleanser suitable for combination skin.",
      "Apply lightweight moisturizers to oily areas and richer creams to dry spots.",
      "Use a mix of hydrating and oil-controlling products based on skin needs.",
    ],

  },
  normal: {
    title: "Your Skin Type: Normal",
    description: "Your skin feels balanced, smooth, and healthy with minimal concerns.",
    recommendations: [
      "Maintain a simple routine with a gentle cleanser and moisturizer.",
      "Use sunscreen daily to protect your skin.",
      "Consider occasional exfoliation to keep skin fresh and radiant.",
    ],
  },
};

const concernData = {
  breakouts: "Consider using a BHA exfoliant and oil-free skincare products.",
  aging: "Use anti-aging products with retinol, peptides, and antioxidants.",
  uneven: "Incorporate vitamin C serums and exfoliants to brighten skin.",
  sensitivity: "Choose fragrance-free, gentle skincare products with calming ingredients.",
};

const Result = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { step1, step2 } = location.state || {};

  const skinResult = resultData[step1] || {};
  const concernResult = step2 ? concernData[step2] : null;

  return (
    <>
    <Header />
    <div className={styles.container}>
      <h2>Skin Test Result</h2>
      
      <div className={styles.resultBox}>
        <h3>{skinResult.title}</h3>
        <p>{skinResult.description}</p>
        
        <h4>Recommended Care:</h4>
        <ul>
          {skinResult.recommendations.map((tip, index) => (
            <li key={index}>{tip}</li>
          ))}
        </ul>

        {concernResult && (
          <div className={styles.concernBox}>
            <h4>Main Concern:</h4>
            <p>{concernResult}</p>
          </div>
        )}
      </div>
      <div>
        <button className={styles.retakeButton} onClick={() => navigate("/skin-test")}>
          Retake Test
        </button>
      </div>
      <div>
        <button className={styles.retakeButton} onClick={() => navigate("/services")}>
          Go to service
        </button>
      </div>
    </div>
    <Footer />
    </>
  );
};

export default Result;
