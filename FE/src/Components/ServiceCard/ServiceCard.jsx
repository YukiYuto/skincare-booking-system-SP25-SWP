import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import styles from "./ServiceCard.module.css";
import { GET_ALL_SERVICE_TYPES_API } from "../../config/apiConfig";
import trust_image from "../../assets/images/trust-image.jpg";

const SkincareServiceCard = ({ service }) => {
  const [serviceTypes, setServiceTypes] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchServiceTypes = async () => {
      setLoading(true);
      try {
        const response = await fetch(GET_ALL_SERVICE_TYPES_API);
        if (!response.ok) {
          throw new Error("Failed to fetch service types");
        }
        const data = await response.json();
        if (data.isSuccess && Array.isArray(data.result)) {
          setServiceTypes(data.result);
        }
      } catch (error) {
        console.error("Error fetching service types:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchServiceTypes();
  }, []);

  const getServiceTypeDisplay = () => {
    if (
      !service.serviceTypeIds ||
      !Array.isArray(service.serviceTypeIds) ||
      service.serviceTypeIds.length === 0
    ) {
      return "Unknown Type";
    }

    const firstTypeId = service.serviceTypeIds[0];

    let firstTypeName = "Unknown Type";
    if (Array.isArray(serviceTypes)) {
      const firstType = serviceTypes.find(
        (type) => type && type.serviceTypeId === firstTypeId
      );
      if (firstType && firstType.serviceTypeName) {
        firstTypeName = firstType.serviceTypeName;
      }
    }

    if (service.serviceTypeIds.length > 1) {
      return `${firstTypeName} +${service.serviceTypeIds.length - 1}`;
    }

    return firstTypeName;
  };

  const getAllServiceTypeNames = () => {
    if (
      !service.serviceTypeIds ||
      !Array.isArray(service.serviceTypeIds) ||
      service.serviceTypeIds.length === 0
    ) {
      return "No service types";
    }

    if (!Array.isArray(serviceTypes) || serviceTypes.length === 0) {
      return `${service.serviceTypeIds.length} service type(s)`;
    }

    const typeNames = service.serviceTypeIds.map((id) => {
      const foundType = serviceTypes.find((t) => t && t.serviceTypeId === id);
      return foundType && foundType.serviceTypeName
        ? foundType.serviceTypeName
        : "Unknown";
    });

    return typeNames.join(", ");
  };

  const formatPrice = (price) => {
    if (!price && price !== 0) return "Contact for price";
    return `${price.toLocaleString()}₫`;
  };

  return (
    <div className={styles.card}>
      <Link to={`/services/${service.serviceId}`}>
        <img
          className={styles.image}
          src={
            !service.imageUrl || service.imageUrl === "imageUrl"
              ? trust_image
              : service.imageUrl
          }
          alt={service.serviceName || "Unnamed Service"}
        />
        <div className={styles.header}>
          <h2 className={styles.title}>{service.serviceName || "No Name"}</h2>
        </div>
        <p className={styles.serviceType} title={getAllServiceTypeNames()}>
          {loading ? "Loading..." : getServiceTypeDisplay()}
        </p>
        <div className={styles.content}>
          <p className={styles.description}>
            {service.description || "No description available."}
          </p>
          <p className={styles.price}>{formatPrice(service.price)}</p>
        </div>
      </Link>
    </div>
  );
};

export default SkincareServiceCard;
