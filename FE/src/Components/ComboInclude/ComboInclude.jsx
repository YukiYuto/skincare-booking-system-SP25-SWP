import React, { useState, useEffect } from "react";
import ServiceCard from "../ServiceCard/ServiceCard";
import { GET_SERVICE_BY_ID_API } from "../../config/apiConfig";
import style from "./ComboInclude.module.css";

const ComboInclude = ({ serviceIds = [] }) => {
  const [services, setServices] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchServices = async () => {
      setLoading(true);
      try {
        if (!serviceIds || serviceIds.length === 0) {
          setServices([]);
          return;
        }
        const servicePromises = serviceIds.map(async (id) => {
          const serviceEndpoint = GET_SERVICE_BY_ID_API.replace("{id}", id);
          const response = await fetch(serviceEndpoint);
          if (!response.ok) {
            throw new Error(`Failed to fetch service with ID: ${id}`);
          }
          const data = await response.json();
          return data.isSuccess && data.result ? data.result : null;
        });

        const results = await Promise.all(servicePromises);
        const validServices = results.filter(service => service !== null);
        setServices(validServices);
      } catch (error) {
        console.error("Error fetching services:", error);
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchServices();
  }, [serviceIds]);

  if (loading) {
    return <div className={style.container}>Loading included services...</div>;
  }

  if (error) {
    return <div className={style.container}>Error loading services: {error}</div>;
  }

  return (
    <div className={style.container}>
      {services.length > 0 ? (
        services.map((service) => (
          <ServiceCard key={service.serviceId} service={service} />
        ))
      ) : (
        <p>No services included in this combo</p>
      )}
      
      {services.length > 0 && (
        <>
          <div className={style.viewMore}>View More</div>
          <div className={style.viewAll}>View All</div>
        </>
      )}
    </div>
  );
};

export default ComboInclude;