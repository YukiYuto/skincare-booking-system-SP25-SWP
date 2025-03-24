import React, { useEffect, useState, useCallback } from "react";
import styles from "./Services.module.css";
import ServiceCreateModal from "./ServiceCreateModal";
import ServiceEditModal from "./ServiceEditModal";
import ServiceTypeCreateModal from "../Services/ServiceType/ServiceTypeCreateModal";
import addIcon from "../../../../assets/icon/addIcon.svg";
import editIcon from "../../../../assets/icon/editIcon.svg";
import {
  GET_ALL_SERVICES_API,
  GET_ALL_SERVICE_TYPES_API,
} from "../../../../config/apiConfig";

const DEFAULT_PAGE_SIZE = 10;
const DEFAULT_PAGE_NUMBER = 1;

const Services = () => {
  const [services, setServices] = useState([]);
  const [serviceTypes, setServiceTypes] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [pagination, setPagination] = useState({
    pageNumber: DEFAULT_PAGE_NUMBER,
    pageSize: DEFAULT_PAGE_SIZE,
  });
  const [modal, setModal] = useState({ type: null, data: null });
  const [selectedServiceType, setSelectedServiceType] = useState("all");
  const [totalPages, setTotalPages] = useState(1);

  const fetchData = useCallback(async () => {
    setLoading(true);
    try {
      // Fetch services
      const response = await fetch(
        `${GET_ALL_SERVICES_API}?pageNumber=${pagination.pageNumber}&pageSize=${pagination.pageSize}${
          selectedServiceType !== "all" ? `&serviceTypeId=${selectedServiceType}` : ""
        }`
      );
      const data = await response.json();
      setServices(data.result.services);
      setTotalPages(data.result.totalPages);

      // Fetch service types
      const serviceTypesResponse = await fetch(GET_ALL_SERVICE_TYPES_API);
      const serviceTypesData = await serviceTypesResponse.json();
      setServiceTypes(serviceTypesData.result);
    } catch (error) {
      setError(error);
    } finally {
      setLoading(false);
    }
  }, [pagination, selectedServiceType]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  // Function to get service type name based on serviceTypeId
  const getServiceTypeName = (serviceTypeId) => {
    const serviceType = serviceTypes.find(
      (type) => type.serviceTypeId === serviceTypeId
    );
    return serviceType ? serviceType.serviceTypeName : "Unknown";
  };

  // Function to generate service type display text with additional counts
  const getServiceTypeDisplay = (service) => {
    if (!service.serviceTypeIds || service.serviceTypeIds.length === 0) {
      return "None";
    }
    
    const firstTypeId = service.serviceTypeIds[0];
    const firstTypeName = getServiceTypeName(firstTypeId);
    
    if (service.serviceTypeIds.length === 1) {
      return firstTypeName;
    }
    
    // If there are additional service types, show +1, +2, etc.
    return `${firstTypeName} +${service.serviceTypeIds.length - 1}`;
  };

  // Function to get all service type names for tooltip
  const getAllServiceTypeNames = (service) => {
    if (!service.serviceTypeIds || service.serviceTypeIds.length === 0) {
      return "No service types";
    }
    
    return service.serviceTypeIds
      .map(id => getServiceTypeName(id))
      .join(", ");
  };

  // Reset page number when changing service type filter
  useEffect(() => {
    setPagination((prev) => ({
      ...prev,
      pageNumber: DEFAULT_PAGE_NUMBER,
    }));
  }, [selectedServiceType]);

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <div className={styles.service}>
          <h2 className={styles.tabTitle}>Services</h2>
          <button
            onClick={() => setModal({ type: "create" })}
            className={styles.iconButton}
          >
            <img src={addIcon} alt="Add" />
          </button>
        </div>
        <div className={styles.service}>
          {" "}
          Page
          <button
            onClick={() =>
              setPagination((prev) => ({
                ...prev,
                pageSize: prev.pageSize === 10 ? 20 : 10,
                pageNumber: DEFAULT_PAGE_NUMBER, // Reset to page 1 when changing page size
              }))
            }
            className={styles.iconButton}
          >
            {pagination.pageSize}
          </button>{" "}
          Size
        </div>
        <div className={styles.serviceType}>
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
          >
            <img src={addIcon} alt="Add" />
          </button>
        </div>
      </div>
      <div className={styles.serviceTableContainer}>
        {loading && <p>Loading services...</p>}
        {error && <p>Error loading services</p>}
        <table className={styles.serviceTable}>
          <thead>
            <tr>
              <th>Service Name</th>
              <th>Price</th>
              <th>Service Type</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {services.length === 0 ? (
              <tr>
                <td colSpan="4">No services found.</td>
              </tr>
            ) : (
              services.map((service) => (
                <tr key={service.serviceId}>
                  <td>{service.serviceName}</td>
                  <td>{(service.price / 100).toFixed(2)}₫</td>
                  <td title={getAllServiceTypeNames(service)} className={styles.serviceTypeCell}>
                    {getServiceTypeDisplay(service)}
                  </td>
                  <td>
                    <button
                      onClick={() => setModal({ type: "edit", data: service })}
                      className={styles.iconButton}
                    >
                      <img src={editIcon} alt="Edit" />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      <div className={styles.paginationControls}>
        <button
          disabled={pagination.pageNumber === 1}
          onClick={() =>
            setPagination((prev) => ({
              ...prev,
              pageNumber: prev.pageNumber - 1,
            }))
          }
          className={styles.iconButton}
        >
          <b>&lt;</b>
        </button>
        <span>{pagination.pageNumber}</span>
        <button
          disabled={pagination.pageNumber === totalPages}
          onClick={() =>
            setPagination((prev) => ({
              ...prev,
              pageNumber: prev.pageNumber + 1,
            }))
          }
          className={styles.iconButton}
        >
          <b>&gt;</b>
        </button>
      </div>

      {modal.type === "create" && (
        <ServiceCreateModal
          onClose={() => setModal({ type: null })}
          refresh={fetchData}
        />
      )}
      {modal.type === "edit" && (
        <ServiceEditModal
          service={modal.data}
          onClose={() => setModal({ type: null })}
          refresh={fetchData}
        />
      )}
      {modal.type === "createServiceType" && (
        <ServiceTypeCreateModal
          onClose={() => setModal({ type: null })}
          refresh={fetchData}
        />
      )}
    </div>
  );
};

export default Services;