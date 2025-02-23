import React, { useEffect, useState, useCallback } from "react";
import api from "../../../../config/axios";
import styles from "./Services.module.css";
import ServiceCreateModal from "./ServiceCreateModal";
import ServiceEditModal from "./ServiceEditModal";
import editIcon from "../../../../assets/icon/editIcon.svg";

const Services = () => {
  const [services, setServices] = useState([]);
  const [serviceTypes, setServiceTypes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState({ type: null, data: null });

  const fetchData = useCallback(async () => {
    setLoading(true);
    try {
      const [servicesRes, typesRes] = await Promise.all([
        api.get("Services/all"),
        api.get("ServiceType/all"),
      ]);
      setServices(servicesRes.data.result);
      setServiceTypes(typesRes.data.result);
    } catch (error) {
      console.error("Error fetching data:", error);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const getServiceTypeName = (serviceTypeId) =>
    serviceTypes.find((type) => type.serviceTypeId === serviceTypeId)
      ?.serviceTypeName || "Unknown";

  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Services</h2>
      <button onClick={() => setModal({ type: "create", data: null })}>
        Add New Service
      </button>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <table className={styles.serviceTable}>
          <thead>
            <tr>
              <th>Name</th>
              <th>Price</th>
              <th>Service Type</th>
              <th>Created Time</th>
              <th>Updated Time</th>
              <th>Edit</th>
            </tr>
          </thead>
          <tbody>
            {services.map((service) => (
              <tr key={service.serviceId}>
                <td>{service.serviceName}</td>
                <td>${service.price.toLocaleString()}</td>
                <td>{getServiceTypeName(service.serviceTypeId)}</td>
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
                <td>
                  <button
                    className={styles.editButton}
                    onClick={() => setModal({ type: "edit", data: service })}
                  >
                    <img src={editIcon} alt="Edit" />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {modal.type === "create" && (
        <ServiceCreateModal
          onClose={() => setModal({ type: null, data: null })}
          refresh={fetchData}
        />
      )}
      {modal.type === "edit" && (
        <ServiceEditModal
          service={modal.data}
          onClose={() => setModal({ type: null, data: null })}
          refresh={fetchData}
        />
      )}
    </div>
  );
};

export default Services;
