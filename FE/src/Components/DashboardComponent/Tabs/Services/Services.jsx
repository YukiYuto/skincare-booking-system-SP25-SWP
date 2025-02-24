import React, { useEffect, useState } from "react";
import api from "../../../../config/axios";
import styles from "./Services.module.css";

const Services = () => {
  const [services, setServices] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchAllServices = async () => {
    try {
      const response = await api.get("Services/all");
      return response.data.result;
    } catch (error) {
      console.error("Error fetching services:", error);
      throw error;
    }
  };

  useEffect(() => {
    const getServices = async () => {
      try {
        const data = await fetchAllServices();
        setServices(data);
      } catch (error) {
        console.error("Failed to load services.");
      } finally {
        setLoading(false);
      }
    };

    getServices();
  }, []);

  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Services</h2>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <table className={styles.serviceTable}>
          <thead>
            <tr>
              <th>Name</th>
              <th>Description</th>
              <th>Price</th>
              <th>Created Time</th>
              <th>Updated Time</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {services.map((service) => (
              <tr key={service.serviceId}>
                <td>{service.serviceName}</td>
                <td>{service.description}</td>
                <td>${service.price.toLocaleString()}</td>
                <td>
                  {service.createdTime
                    ? new Date(service.createdTime).toLocaleString()
                    : "N/A"}
                </td>
                <td>
                  {service.updatedTime
                    ? new Date(service.updatedTime).toLocaleString()
                    : "N/A"}
                </td>
                <td>{service.status === "0" ? "Active" : "Inactive"}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default Services;
