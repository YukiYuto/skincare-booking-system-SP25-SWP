import React, { useEffect, useState } from "react";
import styles from "./ServiceComboDetailModal.module.css";
import { GET_SERVICE_COMBO_DETAIL_BY_ID_API } from "../../../../config/apiConfig";

const ServiceComboDetailModal = ({ data, onClose, refresh }) => {
  const [comboDetail, setComboDetail] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    // Log the complete data object received to see what we're working with
    console.log("Modal data received:", data);
    console.log("Data keys:", Object.keys(data));

    const fetchComboDetail = async () => {
      setLoading(true);
      
      // Check for missing combo ID - updated to use serviceComboId
      if (!data || !data.serviceComboId) {
        console.error("No combo ID found in data:", data);
        setError("Missing combo ID information");
        setLoading(false);
        return;
      }
      
      try {
        // Use serviceComboId instead of comboId
        const url = GET_SERVICE_COMBO_DETAIL_BY_ID_API.replace("{serviceComboId}", data.serviceComboId);
        
        const response = await fetch(url);
        console.log("Response status:", response.status);
        
        const result = await response.json();
        console.log("API Response:", result);
        
        if (result.isSuccess) {
          setComboDetail(result.result);
        } else {
          setError(result.message || "Failed to load combo details");
        }
      } catch (err) {
        console.error("Fetch error:", err);
        setError("Failed to fetch service combo details.");
      } finally {
        setLoading(false);
      }
    };

    fetchComboDetail();
  }, [data]);

  const renderFallbackContent = () => {
    if (!data) return null;
    
    return (
      <div>
        <h2>{data.comboName || "Unknown Combo"}</h2>
        {data.imageUrl && (
          <img
            src={data.imageUrl}
            alt={data.comboName || "Service Combo"}
            className={styles.image}
          />
        )}
        <p>
          <strong>Price:</strong> {data.price ? (data.price / 1000).toFixed(3) : "N/A"}₫
        </p>
        <p><strong>Available Properties:</strong></p>
        <ul>
          {Object.keys(data).map(key => (
            <li key={key}>
              <strong>{key}:</strong> {
                typeof data[key] === 'object' 
                  ? JSON.stringify(data[key]) 
                  : String(data[key])
              }
            </li>
          ))}
        </ul>
      </div>
    );
  };

  if (loading) return <div className={styles.modal}>Loading...</div>;
  
  return (
    <div className={styles.modal}>
      <button className={styles.closeButton} onClick={onClose}>
        &times;
      </button>
      
      {error && (
        <div className={styles.error}>
          <p>Error: {error}</p>
          {data && <div>{renderFallbackContent()}</div>}
        </div>
      )}
      
      {!error && comboDetail && (
        <div>
          <h2>{comboDetail.comboName}</h2>
          {comboDetail.imageUrl && (
            <img
              src={comboDetail.imageUrl}
              alt={comboDetail.comboName}
              className={styles.image}
            />
          )}
          <p>
            <strong>Description:</strong> {comboDetail.description || "No description available"}
          </p>
          <p>
            <strong>Price:</strong> {(comboDetail.price / 1000).toFixed(3)}₫
          </p>
          <p>
            <strong>Number of Services:</strong> {comboDetail.numberOfService || 0}
          </p>
          <p>
            <strong>Status:</strong> {comboDetail.status || "Unknown"}
          </p>
          <h3>Services:</h3>
          {comboDetail.services && comboDetail.services.length > 0 ? (
            <ul>
              {comboDetail.services.map((service) => (
                <li key={service.serviceId}>{service.serviceName}</li>
              ))}
            </ul>
          ) : (
            <p>No services found in this combo.</p>
          )}
        </div>
      )}
      
      {!error && !comboDetail && !loading && renderFallbackContent()}
    </div>
  );
};

export default ServiceComboDetailModal;