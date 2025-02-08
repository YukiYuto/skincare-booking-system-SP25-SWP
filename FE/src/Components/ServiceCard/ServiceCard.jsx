import React from "react";
import styles from "./ServiceCard.module.css";
import Go from "./go.svg";

const SkincareServiceCard = ({ service }) => {
  return (
    <div className={styles.card}>
      <img
        className={styles.image}
        src={service.imgUrl}
        alt={service.ServiceName}
      />
      <div className={styles.header}>
        <h2 className={styles.title}>{service.ServiceName}</h2>
      </div>
      <div className={styles.content}>
        <p className={styles.description}>{service.Description}</p>
        <p className={styles.price}>{service.Price}$</p>
        <div className={styles.popularity}>
          <p>Booked {service.Popularity} times</p>
          <img src={Go} alt={service.ServiceName} />
        </div>
      </div>
    </div>
  );
};

export default SkincareServiceCard;
