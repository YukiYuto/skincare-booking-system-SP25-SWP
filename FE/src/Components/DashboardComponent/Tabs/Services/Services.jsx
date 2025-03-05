import React, { useEffect, useState, useMemo, useCallback } from "react";
import { useDispatch, useSelector } from "react-redux";
import { createSelector } from "@reduxjs/toolkit";
import { 
  fetchServices, 
  createService, 
  updateService, 
  deleteService 
} from "../../../../redux/Services/ServiceThunk";
import { 
  fetchServiceTypes 
} from "../../../../redux/ServiceType/ServiceTypeThunk";
import styles from "./Services.module.css";
import ServiceCreateModal from "./ServiceCreateModal";
import ServiceEditModal from "./ServiceEditModal";
import ServiceTypeCreateModal from "./ServiceType/ServiceTypeCreateModal";
import editIcon from "../../../../assets/icon/editIcon.svg";
import deleteIcon from "../../../../assets/icon/deleteIcon.svg";
import addIcon from "../../../../assets/icon/addIcon.svg";

const Services = () => {
  const dispatch = useDispatch();

  // Memoized selectors with robust implementation
  const selectServicesResult = (state) => 
    state.service?.services?.result ?? [];
  const selectServiceTypesResult = (state) => 
    state.serviceType?.serviceTypes?.result ?? [];

  const selectServices = useMemo(
    () => createSelector(
      [selectServicesResult],
      (services) => services
    ),
    []
  );

  const selectServiceTypes = useMemo(
    () => createSelector(
      [selectServiceTypesResult],
      (serviceTypes) => serviceTypes
    ),
    []
  );

  const services = useSelector(selectServices);
  const serviceTypes = useSelector(selectServiceTypes);
  const { loading, error } = useSelector((state) => ({
    loading: state.service?.loading,
    error: state.service?.error
  }));

  const [modal, setModal] = useState({ type: null, data: null });
  const [sortConfig, setSortConfig] = useState({ key: null, direction: "asc" });
  const [selectedServiceType, setSelectedServiceType] = useState("all");

  // Improved error handling and retry mechanism
  const fetchData = useCallback(() => {
    dispatch(fetchServices());
    dispatch(fetchServiceTypes());
  }, [dispatch]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const getServiceTypeName = (id) =>
    serviceTypes.find((type) => type.serviceTypeId === id)?.serviceTypeName ||
    "Unknown";

  const handleSort = (key) => {
    setSortConfig((prev) => ({
      key,
      direction: prev.key === key && prev.direction === "asc" ? "desc" : "asc",
    }));
  };

  const handleDeleteService = (serviceId) => {
    if (window.confirm("Are you sure you want to delete this service?")) {
      dispatch(deleteService(serviceId));
    }
  };

  const sortedServices = useMemo(() => {
    return [...services]
      .filter(
        (service) =>
          selectedServiceType === "all" ||
          service.serviceTypeId.toString() === selectedServiceType
      )
      .sort((a, b) => {
        if (!sortConfig.key) return 0;
        let valueA = a[sortConfig.key];
        let valueB = b[sortConfig.key];

        if (typeof valueA === "string") valueA = valueA.toLowerCase();
        if (typeof valueB === "string") valueB = valueB.toLowerCase();

        return sortConfig.direction === "asc"
          ? valueA > valueB
            ? 1
            : -1
          : valueA < valueB
          ? 1
          : -1;
      });
  }, [services, selectedServiceType, sortConfig]);

  // Render error message if fetch fails
  if (error) {
    return (
      <div className={styles.errorContainer}>
        <h2>Error Loading Services</h2>
        <p>{error.message || 'An unexpected error occurred'}</p>
        <button onClick={fetchData} className={styles.retryButton}>
          Retry Loading
        </button>
      </div>
    );
  }

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <h2 className={styles.tabTitle}>Services Management</h2>
        <div className={styles.controls}>
          <select
            value={selectedServiceType}
            onChange={(e) => setSelectedServiceType(e.target.value)}
            className={styles.filterSelect}
          >
            <option value="all">All Service Types</option>
            {serviceTypes.map((type) => (
              <option key={type.serviceTypeId} value={type.serviceTypeId}>
                {type.serviceTypeName}
              </option>
            ))}
          </select>
          <button 
            onClick={() => setModal({ type: "createServiceType" })}
            className={styles.iconButton}
            title="Add Service Type"
          >
            <img src={addIcon} alt="Add Service Type" />
          </button>
          <button 
            onClick={() => setModal({ type: "create" })}
            className={styles.iconButton}
            title="Add Service"
          >
            <img src={addIcon} alt="Add Service" />
          </button>
        </div>
      </div>

      {loading ? (
        <div className={styles.loadingContainer}>
          <p>Loading services...</p>
        </div>
      ) : (
        <div className={styles.serviceTableContainer}>
          <table className={styles.serviceTable}>
            <thead>
              <tr>
                {[
                  "Service Name",
                  "Price",
                  "Service Type",
                  "Created Time",
                  "Updated Time",
                  "Actions"
                ].map((key) => (
                  <th 
                    key={key} 
                    onClick={() => handleSort(key.replace(/\s+/g, '').toLowerCase())}
                  >
                    {key}
                    {sortConfig.key === key.replace(/\s+/g, '').toLowerCase() &&
                      (sortConfig.direction === "asc" ? " ↑" : " ↓")}
                  </th>
                ))}
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
                    <div className={styles.actionButtons}>
                      <button
                        className={styles.editButton}
                        onClick={() => setModal({ type: "edit", data: service })}
                        title="Edit Service"
                      >
                        <img src={editIcon} alt="Edit" />
                      </button>
                      <button
                        className={styles.deleteButton}
                        onClick={() => handleDeleteService(service.serviceId)}
                        title="Delete Service"
                      >
                        <img src={deleteIcon} alt="Delete" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {modal.type === "create" && (
        <ServiceCreateModal
          onClose={() => setModal({ type: null })}
          refresh={() => dispatch(fetchServices())}
        />
      )}
      {modal.type === "edit" && (
        <ServiceEditModal
          service={modal.data}
          onClose={() => setModal({ type: null })}
          refresh={() => dispatch(fetchServices())}
        />
      )}
      {modal.type === "createServiceType" && (
        <ServiceTypeCreateModal
          onClose={() => setModal({ type: null })}
          refresh={() => dispatch(fetchServiceTypes())}
        />
      )}
    </div>
  );
};

export default Services;