import React from "react";
import { Link } from "react-router-dom"; // Thêm dòng này nếu dùng React Router
import styles from "./ServiceCard.module.css";
import Go from "./go.svg";

const SkincareServiceCard = ({ service, Name }) => {
  return (
    <div className={styles.card}>
      <Link to={`/services/${service.ID}`}>
      <img
        className={styles.image}
        src={service.imgUrl}
        alt={service.ServiceName}
      />
      <div className={styles.header}>
        <h2 className={styles.title}>{service.ServiceName}</h2>
      </div>
      <p className={styles.serviceType}>{Name}</p>
      <div className={styles.content}>
        <p className={styles.description}>{service.Description}</p>
        <p className={styles.price}>{service.Price}$</p>
        <div className={styles.popularity}>
          <p>Booked {service.Popularity} times</p>
          
            <img src={Go} alt="Go to service" />
          
        </div>
      </div>
      </Link>
    </div>
  );
};

export default SkincareServiceCard;
