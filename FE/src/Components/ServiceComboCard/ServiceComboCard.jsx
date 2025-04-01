import React from "react";
import { Link } from "react-router-dom";
import styles from "./ServiceComboCard.module.css";

const ServiceComboCard = ({ combo }) => {
  const formatPrice = (price) => {
    if (!price && price !== 0) return "Contact for price";
    return `${price.toLocaleString()}₫`;
  };

  return (
    <div className={styles.card}>
      <Link to={`/service-combo/${combo.serviceComboId}`}>
        <img
          className={styles.image}
          src={
            combo.imageUrl ||
            "https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png"
          }
          alt={combo.comboName || "Unnamed Combo"}
        />
        <div className={styles.header}>
          <h2 className={styles.title}>{combo.comboName || "No Name"}</h2>
        </div>
        <div className={styles.content}>
          <p className={styles.description}>
            {combo.description || "No description available."}
          </p>
          <p className={styles.price}>{formatPrice(combo.price)}</p>
          <p className={styles.numberOfServices}>
            {combo.numberOfService} service(s) included
          </p>
        </div>
      </Link>
    </div>
  );
};

export default ServiceComboCard;
