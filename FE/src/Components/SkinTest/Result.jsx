import { useLocation, useNavigate } from "react-router-dom";
import styles from "./Result.module.css";
import Header from "../Common/Header";
import Footer from "../Footer/Footer";
import { useEffect, useState } from "react";
import { getRecommendations } from "../../services/customerService";
import { toast } from "react-toastify";

const Result = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { totalScore, skinName, description, skinProfileId } =
    location.state || {};
  const [recommendations, setRecommendations] = useState([]);

  useEffect(() => {
    const fetchRecommendations = async () => {
      try {
        const response = await getRecommendations(skinProfileId);
        console.log("Recommendations response:", response);
        setRecommendations(response?.result);
      } catch (error) {
        toast.error("Error fetching recommendations:", error);
        setRecommendations([]);
      }
    };
    fetchRecommendations();
  }, [skinProfileId]);
  return (
    <>
      <Header />
      <div className={styles.resultContainer}>
        <h2>Your Skin Analysis Result</h2>
        <p className={styles.skinName}>
          <strong>Skin Type:</strong> {skinName}
        </p>
        <p>
          <strong>Description:</strong> {description}
        </p>
        <p className={styles.score}>
          <strong>Total Score:</strong> {totalScore}
        </p>

        {/* Section to display recommended services */}
        <div className={styles.recommendations}>
          <h3>Recommended Services</h3>
          <ul className={styles.recommendationList}>
            {recommendations.length > 0 ? (
              recommendations.map((item) => (
                <li key={item.serviceId} className={styles.recommendationItem}>
                  <div className={styles.recommendationContent}>
                    <h4>{item.serviceName}</h4>
                    <p>{item.description}</p>
                    <button
                      className={styles.bookButton}
                      onClick={() => navigate(`/service/${item.serviceId}`)}
                    >
                      Book Now
                    </button>
                  </div>
                </li>
              ))
            ) : (
              <li className={styles.noRecommendations}>
                No recommendations available for your skin type.
              </li>
            )}
          </ul>
        </div>
      </div>
      <div style={{ marginTop: "30px" }}>
        <Footer />
      </div>
    </>
  );
};

export default Result;
