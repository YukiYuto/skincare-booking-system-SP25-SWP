import React, { useState, useEffect } from "react";
import ServiceCard from "../ServiceCard/ServiceCard";
import styles from "./SimilarService.module.css";
import { GET_SIMILAR_SERVICES_API } from "../../config/apiConfig";

const SimilarService = ({ serviceId }) => {
  const [similarServices, setSimilarServices] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchSimilarServices = async () => {
      setLoading(true);
      try {
        const response = await fetch(
          `${GET_SIMILAR_SERVICES_API}?serviceId=${serviceId}`
        );
        if (!response.ok) {
          throw new Error("Failed to fetch similar services");
        }
        const data = await response.json();
        if (data.isSuccess && Array.isArray(data.result)) {
          setSimilarServices(data.result);
        }
      } catch (error) {
        console.error("Error fetching similar services:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchSimilarServices();
  }, [serviceId]);

  return (
    <div className={styles.container}>
      <h1>Similar Services</h1>
      <div className={styles.itemList}>
        {loading ? (
          <p>Loading...</p>
        ) : similarServices.length === 0 ? (
          <p>None similar service</p>
        ) : (
          similarServices.map((service) => (
            <ServiceCard key={service.serviceId} service={service} />
          ))
        )}
      </div>
    </div>
  );
};

export default SimilarService;
