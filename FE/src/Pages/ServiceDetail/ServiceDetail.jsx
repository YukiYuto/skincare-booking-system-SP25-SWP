import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ServiceLayout from "../../Components/ServiceDetail/ServiceLayout";
import styles from "./ServiceDetail.module.css";
import { GET_SERVICE_BY_ID_API, HTTP_METHODS } from "../../config/apiConfig";
import Header from "../../Components/Common/Header";
import BookingModal from "../../Components/BookingModal/BookingModal";
import SimilarService from "../../Components/ServiceSimilar/SimilarService";

const ServiceDetail = () => {
  const { id } = useParams();
  const [state, setState] = useState({
    service: null,
    isLoading: true,
    error: null,
  });
  const [visible, setVisible] = useState(false);

  const handleBook = () => {
    setVisible(true);
  };

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

      setState({
        service: serviceData,
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

  const { service, isLoading, error } = state;

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
        <ServiceLayout service={service} onBookButtonClick={handleBook} />

        <BookingModal
          visible={visible}
          onClose={() => setVisible(false)}
          selectedService={service}
        />
      </div>
      <SimilarService serviceId={service.serviceId} />
    </>
  );
};

export default ServiceDetail;
