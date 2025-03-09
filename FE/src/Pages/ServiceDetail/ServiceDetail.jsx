import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ServiceLayout from "../../Components/ServiceDetail/ServiceLayout";
import ServiceList from "../../Components/ServiceList/ServiceList";
import styles from "./ServiceDetail.module.css";
import {
  GET_SERVICE_BY_ID_API,
  GET_ALL_SERVICES_API,
  GET_THERAPIST_BY_SERVICE_API,
  HTTP_METHODS,
  AUTH_HEADERS
} from "../../config/apiConfig";

const ServiceDetail = () => {
  const { id } = useParams();
  const [state, setState] = useState({
    service: null,
    similarServices: [],
    therapists: [],
    isLoading: true,
    error: null,
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Get token from local storage or state management
        const token = localStorage.getItem('token');
        const headers = token ? AUTH_HEADERS(token) : { "Content-Type": "application/json" };

        // Replace hardcoded API URLs with endpoints from apiconfig
        const serviceEndpoint = GET_SERVICE_BY_ID_API.replace('{id}', id);
        
        console.log("Fetching service from:", serviceEndpoint);
        
        // Fetch service data first
        const serviceRes = await fetch(serviceEndpoint, {
          method: HTTP_METHODS.GET,
          headers
        });

        if (!serviceRes.ok) {
          throw new Error(`Failed to fetch service data: ${serviceRes.status}`);
        }

        const serviceResponse = await serviceRes.json();
        console.log("Service data:", serviceResponse);
        
        // Extract the actual service data from the response
        const serviceData = serviceResponse.result;
        
        if (!serviceData || !serviceData.serviceTypeId) {
          throw new Error("Invalid service data structure");
        }

        // Then fetch all services
        const allServicesRes = await fetch(GET_ALL_SERVICES_API, {
          method: HTTP_METHODS.GET,
          headers
        });

        if (!allServicesRes.ok) {
          throw new Error(`Failed to fetch all services: ${allServicesRes.status}`);
        }

        const allServicesResponse = await allServicesRes.json();
        // Extract the actual services array from the response
        const allServicesData = allServicesResponse.result || allServicesResponse;
        console.log("All services data:", allServicesData);

        // Get therapists for this service type
        const therapistsEndpoint = GET_THERAPIST_BY_SERVICE_API.replace('{serviceTypeId}', serviceData.serviceTypeId);
        console.log("Fetching therapists from:", therapistsEndpoint);
        
        const therapistsRes = await fetch(therapistsEndpoint, {
          method: HTTP_METHODS.GET,
          headers
        });
        
        if (!therapistsRes.ok) {
          throw new Error(`Failed to fetch therapists: ${therapistsRes.status}`);
        }
        
        const therapistsResponse = await therapistsRes.json();
        // Extract the actual therapists data from the response
        const therapistsData = therapistsResponse.result || therapistsResponse;
        console.log("Therapists data:", therapistsData);
        
        // Find similar services (same service type, different ID)
        const similarServices = allServicesData
          .filter(
            (s) =>
              s.serviceTypeId === serviceData.serviceTypeId &&
              s.serviceId !== serviceData.serviceId
          )
          .slice(0, 4);

        console.log("Similar services:", similarServices);

        setState({
          service: serviceData,
          similarServices,
          therapists: therapistsData || [],
          isLoading: false,
          error: null,
        });
      } catch (error) {
        console.error("Error in fetchData:", error);
        setState((prev) => ({
          ...prev,
          isLoading: false,
          error: error.message,
        }));
      }
    };

    fetchData();
  }, [id]);

  const {
    service,
    similarServices,
    therapists,
    isLoading,
    error,
  } = state;

  if (isLoading) return <div className={styles.loading}>Loading...</div>;
  if (error) return <div className={styles.error}>Error: {error}</div>;

  // Add safety checks before rendering components
  if (!service) return <div className={styles.error}>Service data not available</div>;

  return (
    <div className={styles.container}>
      <ServiceLayout
        service={service}
        therapists={therapists || []}
      />

      {similarServices.length > 0 && (
        <div className={styles.similarSection}>
          <h2 className={styles.similarTitle}>Similar Services</h2>
          <ServiceList services={similarServices} />
        </div>
      )}
    </div>
  );
};

export default ServiceDetail;