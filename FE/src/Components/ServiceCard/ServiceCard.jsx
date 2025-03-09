import React from "react";
import { Link } from "react-router-dom";
import styles from "./ServiceCard.module.css";
import Go from "./go.svg";
import { GET_SERVICE_BY_ID_API } from "../../config/apiConfig";

const SkincareServiceCard = ({ service, Name }) => {
  return (
    <div className={styles.card}>
      <Link to={`/services/${service.serviceId}`}>
        <img
          className={styles.image}
          src={service.imageUrl || "https://via.placeholder.com/300"}
          alt={service.serviceName || "Unnamed Service"}
        />
        <div className={styles.header}>
          <h2 className={styles.title}>{service.serviceName || "No Name"}</h2>
        </div>
        <p className={styles.serviceType}>{Name || "Unknown Type"}</p>
        <div className={styles.content}>
          <p className={styles.description}>{service.description || "No description available."}</p>
          <p className={styles.price}>
            {service.price ? `${service.price.toLocaleString()}₫` : "Contact for price"}
          </p>
          <div className={styles.popularity}>
            <img src={Go} alt="Go to service" />
          </div>
        </div>
      </Link>
    </div>
  );
};

export default SkincareServiceCard;
