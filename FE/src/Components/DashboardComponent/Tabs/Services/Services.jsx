import React, { useEffect, useState } from "react";
import api from "../../../../config/axios";
import styles from "./Services.module.css";
import ServiceCreateModal from "./ServiceCreateModal"; // Import the modal component
import ServiceEditModal from "./ServiceEditModal"; // Import the edit modal component

const Services = () => {
  const [services, setServices] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showCreateModal, setShowCreateModal] = useState(false); // State to control create modal visibility
  const [showEditModal, setShowEditModal] = useState(false); // State to control edit modal visibility
  const [selectedService, setSelectedService] = useState(null); // State to store the selected service for editing

  const fetchAllServices = async () => {
    try {
      const response = await api.get("Services/all");
      return response.data.result;
    } catch (error) {
      console.error("Error fetching services:", error);
      throw error;
    }
  };

  const refreshServices = async () => {
    setLoading(true);
    try {
      const data = await fetchAllServices();
      setServices(data);
    } catch (error) {
      console.error("Failed to load services.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    refreshServices();
  }, []);

  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Services</h2>
      <button onClick={() => setShowCreateModal(true)}>
        Add New Service
      </button>{" "}
      {/* Button to open create modal */}
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
              <th>Actions</th> {/* Add Actions column */}
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
                <td>
                  <button
                    onClick={() => {
                      setSelectedService(service);
                      setShowEditModal(true);
                    }}
                  >
                    Edit
                  </button>{" "}
                  {/* Button to open edit modal */}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {showCreateModal && (
        <ServiceCreateModal
          onClose={() => {
            setShowCreateModal(false);
            refreshServices(); // Refresh services after closing the create modal
          }}
        />
      )}
      {showEditModal && (
        <ServiceEditModal
          service={selectedService}
          onClose={() => {
            setShowEditModal(false);
            refreshServices(); // Refresh services after closing the edit modal
          }}
        />
      )}
    </div>
  );
};

export default Services;
