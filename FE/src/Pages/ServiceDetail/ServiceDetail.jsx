import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import ServiceLayout from "../../Components/ServiceDetail/ServiceLayout";
import styles from "./ServiceDetail.module.css";
import { GET_SERVICE_BY_ID_API, HTTP_METHODS } from "../../config/apiConfig";
import Header from "../../Components/Common/Header";
import BookingModal from "../../Components/BookingModal/BookingModal";
import SimilarService from "../../Components/ServiceSimilar/SimilarService";
import { useSelector, useDispatch } from "react-redux";
import { toast } from "react-toastify";
import { clearUser } from "../../redux/auth/slice";

const ServiceDetail = () => {
  const { id } = useParams();
  const [state, setState] = useState({
    service: null,
    isLoading: true,
    error: null,
  });
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [visible, setVisible] = useState(false);
  const { user, isAuthenticated } = useSelector((state) => state.auth);
  const handleBook = () => {
    if (!isAuthenticated || !user) {
      toast.warning("You need to login to book this service.");
      dispatch(clearUser());
      navigate("/login");
      return;
    }
    setVisible(true);
  };

  const fetchData = async () => {
    try {
      const serviceEndpoint = GET_SERVICE_BY_ID_API.replace("{id}", id);
      const serviceRes = await fetch(serviceEndpoint, {
        method: HTTP_METHODS.GET,
        headers: { "Content-Type": "application/json" },
      });

      if (!serviceRes.ok) {
        throw new Error(`Failed to fetch service data: ${serviceRes.status}`);
      }

      const serviceResponse = await serviceRes.json();

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
    return <div className={styles.loading}>Loading...</div>;
  }

  if (error) {
    return <div className={styles.error}>Error: {error}</div>;
  }

  if (!service) {
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
