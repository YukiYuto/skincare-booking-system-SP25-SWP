import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { GET_SERVICE_COMBO_DETAIL_BY_ID_API } from "../../config/apiConfig";
import styles from "./ServiceComboDetail.module.css";
import Header from "../Common/Header";
import ComboInclude from "../ComboInclude/ComboInclude";
import BookingBundleModal from "../BookingModalMultiple/BookingBundleModal";

const ServiceComboLayout = () => {
  const { id } = useParams();
  const [state, setState] = useState({
    combo: null,
    isLoading: true,
    error: null,
  });

  const [showModal, setShowModal] = useState(false);

  const handleBook = () => {
    setShowModal(true);
  };

  const fetchComboData = async () => {
    try {
      const comboEndpoint = GET_SERVICE_COMBO_DETAIL_BY_ID_API.replace(
        "{serviceComboId}",
        id
      );
      const response = await fetch(comboEndpoint, {
        method: "GET",
        headers: { "Content-Type": "application/json" },
      });

      if (!response.ok) {
        throw new Error(`Failed to fetch combo data: ${response.status}`);
      }

      const comboResponse = await response.json();
      setState({
        combo: comboResponse.result,
        isLoading: false,
        error: null,
      });
    } catch (error) {
      setState({
        combo: null,
        isLoading: false,
        error: error.message,
      });
    }
  };

  useEffect(() => {
    fetchComboData();
  }, [id]);

  const { combo, isLoading, error } = state;

  if (isLoading) {
    return <div className={styles.loading}>Loading...</div>;
  }

  if (error) {
    return <div className={styles.error}>Error: {error}</div>;
  }

  if (!combo) {
    return <div className={styles.error}>Combo data not available</div>;
  }

  // Extract service information from the combo
  const serviceIds = combo.services?.map((service) => service.serviceId) || [];

  return (
    <div className={styles.layoutContainer}>
      <Header />
      <div className={styles.container}>
        <div className={styles.imageContainer}>
          <img
            src={combo.imageUrl}
            alt={combo.comboName}
            className={styles.image}
          />
        </div>
        <div className={styles.detailContainer}>
          <h1 className={styles.title}>{combo.comboName}</h1>
          <p className={styles.description}>{combo.description}</p>
          <p className={styles.price}>{combo.price.toLocaleString()}₫</p>
          <p className={styles.numberOfService}>
            Number of Services: {combo.numberOfService}
          </p>
          <button className={styles.order} onClick={handleBook}>
            BOOK NOW
          </button>
        </div>
      </div>
      <div className={styles.servicesList}>
        <h2>Included Services:</h2>
        <ComboInclude serviceIds={serviceIds} />
      </div>
      {showModal && (
        <BookingBundleModal
          serviceComboId={combo.serviceComboId}
          comboPrice={combo.price}
          services={combo.services || []}
          onClose={() => setShowModal(false)}
        />
      )}
    </div>
  );
};

export default ServiceComboLayout;
