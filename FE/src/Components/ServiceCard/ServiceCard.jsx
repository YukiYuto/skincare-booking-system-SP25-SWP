import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import styles from "./ServiceCard.module.css";
import Go from "./go.svg";
import { GET_ALL_SERVICE_TYPES_API } from "../../config/apiConfig";

const SkincareServiceCard = ({ service }) => {
  const [serviceTypes, setServiceTypes] = useState([]);
  const [loading, setLoading] = useState(false);

  // Fetch service types when component mounts
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

  // Format the service type display
  const getServiceTypeDisplay = () => {
    // Check if serviceTypeIds exists and is an array
    if (
      !service.serviceTypeIds ||
      !Array.isArray(service.serviceTypeIds) ||
      service.serviceTypeIds.length === 0
    ) {
      return "Unknown Type";
    }

    // Get the first service type
    const firstTypeId = service.serviceTypeIds[0];

    // Find the service type name
    let firstTypeName = "Unknown Type";
    if (Array.isArray(serviceTypes)) {
      const firstType = serviceTypes.find(
        (type) => type && type.serviceTypeId === firstTypeId
      );
      if (firstType && firstType.serviceTypeName) {
        firstTypeName = firstType.serviceTypeName;
      }
    }

    // If there are additional types, add +1, +2, etc.
    if (service.serviceTypeIds.length > 1) {
      return `${firstTypeName} +${service.serviceTypeIds.length - 1}`;
    }

    return firstTypeName;
  };

  // Get all service type names for the tooltip
  const getAllServiceTypeNames = () => {
    // Check if serviceTypeIds exists and is an array
    if (
      !service.serviceTypeIds ||
      !Array.isArray(service.serviceTypeIds) ||
      service.serviceTypeIds.length === 0
    ) {
      return "No service types";
    }

    // If serviceTypes is not loaded yet, return a simple message
    if (!Array.isArray(serviceTypes) || serviceTypes.length === 0) {
      return `${service.serviceTypeIds.length} service type(s)`;
    }

    // Map service type IDs to names
    const typeNames = service.serviceTypeIds.map((id) => {
      const foundType = serviceTypes.find((t) => t && t.serviceTypeId === id);
      return foundType && foundType.serviceTypeName
        ? foundType.serviceTypeName
        : "Unknown";
    });

    return typeNames.join(", ");
  };

  // Function to format price with thousands separator
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
            service.imageUrl ||
            "https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png"
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
          <div className={styles.popularity}>
            <img src={Go} alt="Go to service" />
          </div>
        </div>
      </Link>
    </div>
  );
};

export default SkincareServiceCard;
