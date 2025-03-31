import React, { useEffect, useState, useCallback } from "react";
import Header from "../../Components/Common/Header.jsx";
import Hero from "../../Components/Hero/Hero";
import ServiceComboList from "../../Components/ServiceComboList/ServiceComboList.jsx";
import styles from "./AllServiceCombo.module.css";
import Loading from "../../Components/Common/Loading/Loading.jsx";
import { GET_ALL_SERVICE_COMBO_API } from "../../config/apiConfig";

const AllServiceCombo = () => {
  const [serviceCombos, setServiceCombos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchServiceCombos = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      const response = await fetch(GET_ALL_SERVICE_COMBO_API);
      
      if (!response.ok)
        throw new Error(`HTTP error! Status: ${response.status}`);
      
      const data = await response.json();
      
      // Extract the serviceCombos array from the result object
      const combos = data.result?.serviceCombos || [];
      
      setServiceCombos(combos);
    } catch (error) {
      console.error("Error fetching service combos:", error);
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchServiceCombos();
  }, [fetchServiceCombos]);


  if (loading) return <Loading />;
  if (error) return <p className={styles.errorMessage}>Error: {error}</p>;

  return (
    <div>
      <Header />
      <div className={styles.heroContainer}>
        <Hero />
        <div className={styles.container}>
          <div className={styles.header}>
            <h1>Discover Our Service Combos</h1>
          </div>
          <div className={styles.serviceComboListContainer}>
            {serviceCombos.length === 0 ? (
              <div className={styles.noResults}>
                <h3>No service combos found</h3>
                <p>Try again later</p>
              </div>
            ) : (
              <ServiceComboList serviceCombos={serviceCombos} />
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AllServiceCombo;