import { useState, useEffect } from "react";
import styles from "./ServiceLayout.module.css";
import { GET_ALL_SERVICE_TYPES_API } from "../../config/apiConfig";

const ServiceLayout = ({ service, serviceType, onBookButtonClick }) => {
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

  return (
    <div className={styles.layout}>
      <div className={styles.imageContainer}>
        <img
          src={service.imageUrl}
          alt={service.serviceName}
          className={styles.image}
        />
      </div>

      <div className={styles.infoContainer}>
        <h1 className={styles.title}>{service.serviceName}</h1>
        <p className={styles.description}>{service.description}</p>

        <div className={styles.detailsSection}>
          <span className={styles.serviceType} title={getAllServiceTypeNames()}>
            {loading ? "Loading..." : getServiceTypeDisplay()}
          </span>
          <br />
          <span className={styles.price}>
            {service.price.toLocaleString()}₫
          </span>
        </div>
        <br />
        <div className={styles.purchaseSection}>
          <button className={styles.order} onClick={onBookButtonClick}>
            BOOK NOW
          </button>
        </div>
      </div>
    </div>
  );
};

export default ServiceLayout;
