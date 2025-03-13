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
} from "../../config/apiConfig";
import Header from "../../Components/Common/Header";

const ServiceDetail = () => {
  const { id } = useParams();
  const [state, setState] = useState({
    service: null,
    similarServices: [],
    therapists: [],
    isLoading: true,
    error: null,
  });
  const [bookSelected, setBookSelected] = useState(false);

  const fetchData = async () => {
    try {
      console.log("Fetching service details for ID:", id);

      const serviceEndpoint = GET_SERVICE_BY_ID_API.replace("{id}", id);
      console.log("Service API URL:", serviceEndpoint);

      const serviceRes = await fetch(serviceEndpoint, {
        method: HTTP_METHODS.GET,
        headers: { "Content-Type": "application/json" }, 
      });

      if (!serviceRes.ok) {
        throw new Error(`Failed to fetch service data: ${serviceRes.status}`);
      }

      const serviceResponse = await serviceRes.json();
      console.log("Service Response:", serviceResponse);

      const serviceData = serviceResponse.result;
      if (!serviceData || !serviceData.serviceId) {
        throw new Error("Invalid service data structure");
      }

      console.log("Fetching all services...");
      const allServicesRes = await fetch(GET_ALL_SERVICES_API, {
        method: HTTP_METHODS.GET,
        headers: { "Content-Type": "application/json" },
      });

      if (!allServicesRes.ok) {
        throw new Error(
          `Failed to fetch all services: ${allServicesRes.status}`
        );
      }

      const allServicesResponse = await allServicesRes.json();
      const allServicesData = allServicesResponse.services || [];

      console.log("All Services Data:", allServicesData);

      console.log(
        "Fetching therapists for serviceTypeId:",
        serviceData.serviceTypeId
      );
      const therapistsEndpoint = GET_THERAPIST_BY_SERVICE_API.replace(
        "{serviceId}",
        serviceData.serviceId
      );
      console.log("Therapists API URL:", therapistsEndpoint);

      let therapistsData = [];
      try {
        const therapistsRes = await fetch(therapistsEndpoint, {
          method: HTTP_METHODS.GET,
          headers: { "Content-Type": "application/json" },
        });

        if (therapistsRes.ok) {
          const therapistsResponse = await therapistsRes.json();
          therapistsData = therapistsResponse.result || therapistsResponse;
          console.log("Therapists Data:", therapistsData);
        } else if (therapistsRes.status !== 404) {
          throw new Error(
            `Failed to fetch therapists: ${therapistsRes.status}`
          );
        } else {
          console.warn("No therapists found for this service.");
        }
      } catch (therapistError) {
        console.warn("Therapist API error:", therapistError);
      }

      const similarServices = allServicesData
        .filter(
          (s) =>
            s.serviceTypeId === serviceData.serviceTypeId &&
            s.serviceId !== serviceData.serviceId
        )
        .slice(0, 4);

      console.log("Similar Services:", similarServices);

      setState({
        service: serviceData,
        similarServices,
        therapists: therapistsData,
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

  useEffect(() => {
    fetchData();
  }, [id]);

  const { service, similarServices, therapists, isLoading, error } = state;

  if (isLoading) {
    console.log("Loading state active...");
    return <div className={styles.loading}>Loading...</div>;
  }

  if (error) {
    console.error("Error state:", error);
    return <div className={styles.error}>Error: {error}</div>;
  }

  if (!service) {
    console.warn("Service data not available!");
    return <div className={styles.error}>Service data not available</div>;
  }

  return (
    <>
      <Header />
      <div className={styles.container}>
        <ServiceLayout service={service} therapists={therapists || []} />
        {similarServices.length > 0 && (
          <div className={styles.similarSection}>
            <h2 className={styles.similarTitle}>Similar Services</h2>
            <ServiceList services={similarServices} />
          </div>
        )}
      </div>
    </>
  );
};

export default ServiceDetail;
