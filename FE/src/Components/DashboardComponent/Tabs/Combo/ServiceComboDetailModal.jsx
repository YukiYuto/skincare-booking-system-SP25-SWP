import React, { useEffect, useState } from "react";
import styles from "./ServiceComboDetailModal.module.css";
import { GET_SERVICE_COMBO_DETAIL_BY_ID_API } from "../../../../config/apiConfig";

const ServiceComboDetailModal = ({ data, onClose, refresh }) => {
  const [comboDetail, setComboDetail] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchComboDetail = async () => {
      setLoading(true);
      try {
        const response = await fetch(
          `${GET_SERVICE_COMBO_DETAIL_BY_ID_API}/${data.comboId}`
        );
        const result = await response.json();
        if (result.isSuccess) {
          setComboDetail(result.result);
        } else {
          setError(result.message);
        }
      } catch (err) {
        setError("Failed to fetch service combo details.");
      } finally {
        setLoading(false);
      }
    };

    fetchComboDetail();
  }, [data.comboId]);

  if (loading) return <div className={styles.modal}>Loading...</div>;
  if (error) return <div className={styles.modal}>Error: {error}</div>;

  return (
    <div className={styles.modal}>
      <button className={styles.closeButton} onClick={onClose}>
        &times;
      </button>
      {comboDetail && (
        <div>
          <h2>{comboDetail.comboName}</h2>
          <img
            src={comboDetail.imageUrl}
            alt={comboDetail.comboName}
            className={styles.image}
          />
          <p>
            <strong>Description:</strong> {comboDetail.description}
          </p>
          <p>
            <strong>Price:</strong> {(comboDetail.price / 1000).toFixed(3)}₫
          </p>
          <p>
            <strong>Number of Services:</strong> {comboDetail.numberOfService}
          </p>
          <p>
            <strong>Status:</strong> {comboDetail.status}
          </p>
          <h3>Services:</h3>
          <ul>
            {comboDetail.services.map((service) => (
              <li key={service.serviceId}>{service.serviceName}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default ServiceComboDetailModal;
