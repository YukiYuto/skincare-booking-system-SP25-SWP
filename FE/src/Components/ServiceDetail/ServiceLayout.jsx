import React from "react";
import styles from "./ServiceLayout.module.css";
import placeholder from "../../assets/images/trust-image.jpg";
import { useState, useEffect } from "react";
import { GET_ALL_SERVICE_TYPES_API } from "../../config/apiConfig";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

const ServiceLayout = ({ service, serviceType, onBookButtonClick }) => {
  const [serviceTypes, setServiceTypes] = useState([]);
  const [loading, setLoading] = useState(false);
  const { accessToken } = useSelector((state) => state.auth);
  const navigate = useNavigate();

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

    if (!Array.isArray(serviceTypes) || serviceTypes.length === 0) {
      return `${service.serviceTypeIds.length} service type(s)`;
    }

    const typeNames = service.serviceTypeIds
      .map((id) => {
        const foundType = serviceTypes.find((t) => t && t.serviceTypeId === id);
        return foundType?.serviceTypeName || "Unknown";
      })
      .slice(0, 3);

    return typeNames.length < service.serviceTypeIds.length
      ? `${typeNames.join(", ")} +${service.serviceTypeIds.length - 3}`
      : typeNames.join(", ");
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

  const handleBookNow = () => {
    if (!accessToken) {
      navigate("/login");
      toast.warn("Please Login!");
      return;
    }
    onBookButtonClick(); 
  };

  return (
    <div className={styles.layout}>
      <div className={styles.imageContainer}>
        <img
          src={
            !service.imageUrl || service.imageUrl === "imageUrl"
              ? placeholder
              : service.imageUrl
          }
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
          <button className={styles.order} onClick={handleBookNow}>
            BOOK NOW
          </button>
        </div>
      </div>
    </div>
  );
};

export default ServiceLayout;
