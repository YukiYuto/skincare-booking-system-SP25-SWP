import React, { useEffect, useState, useCallback } from "react";
import api from "../../../../config/axios";
import styles from "./Services.module.css";
import ServiceCreateModal from "./ServiceCreateModal";
import ServiceEditModal from "./ServiceEditModal";
import ServiceTypeCreateModal from "./ServiceType/ServiceTypeCreateModal";
import editIcon from "../../../../assets/icon/editIcon.svg";
import addIcon from "../../../../assets/icon/addIcon.svg";

const Services = () => {
  const [services, setServices] = useState([]);
  const [serviceTypes, setServiceTypes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState({ type: null, data: null });
  const [sortConfig, setSortConfig] = useState({
    key: null,
    direction: "ascending",
  });
  const [selectedServiceType, setSelectedServiceType] = useState("all");

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

  const handleSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  const sortedServices = [...services]
    .filter(
      (service) =>
        selectedServiceType === "all" ||
        service.serviceTypeId.toString() === selectedServiceType
    )
    .sort((a, b) => {
      if (!sortConfig.key) return 0;

      let valueA =
        typeof a[sortConfig.key] === "string"
          ? a[sortConfig.key].toLowerCase()
          : a[sortConfig.key];
      let valueB =
        typeof b[sortConfig.key] === "string"
          ? b[sortConfig.key].toLowerCase()
          : b[sortConfig.key];

      if (valueA < valueB) return sortConfig.direction === "ascending" ? -1 : 1;
      if (valueA > valueB) return sortConfig.direction === "ascending" ? 1 : -1;
      return 0;
    });

  const handleFilterChange = (e) => {
    setSelectedServiceType(e.target.value);
  };

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <div className={styles.tabTitleContainer}>
          <h2 className={styles.tabTitle}>Services</h2>
          <button onClick={() => setModal({ type: "create", data: null })}>
            <img src={addIcon} alt="Add Service" />
          </button>
        </div>
        <div className={styles.filterContainer}>
          <div className={styles.filterGroup}>
            <select
              id="serviceTypeFilter"
              value={selectedServiceType}
              onChange={handleFilterChange}
              className={styles.filterSelect}
            >
              <option value="all">All Service Types</option>
              {serviceTypes.map((type) => (
                <option
                  key={type.serviceTypeId}
                  value={type.serviceTypeId.toString()}
                >
                  {type.serviceTypeName}
                </option>
              ))}
            </select>
          </div>
          <button
            onClick={() => setModal({ type: "createServiceType", data: null })}
          >
            <img src={addIcon} alt="Add Service Type" />
          </button>
        </div>
      </div>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <table className={styles.serviceTable}>
          <thead>
            <tr>
              <th onClick={() => handleSort("serviceName")}>Name</th>
              <th onClick={() => handleSort("price")}>Price</th>
              <th onClick={() => handleSort("serviceTypeId")}>Service Type</th>
              <th onClick={() => handleSort("createdTime")}>Created Time</th>
              <th onClick={() => handleSort("updatedTime")}>Updated Time</th>
              <th>Edit</th>
            </tr>
          </thead>
          <tbody>
            {sortedServices.map((service) => (
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
      {modal.type === "createServiceType" && (
        <ServiceTypeCreateModal
          onClose={() => setModal({ type: null, data: null })}
          refresh={fetchData}
        />
      )}
    </div>
  );
};

export default Services;
