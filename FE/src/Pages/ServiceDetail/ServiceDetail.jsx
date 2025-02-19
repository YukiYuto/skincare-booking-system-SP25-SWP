import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ServiceLayout from "../../Components/ServiceDetail/ServiceLayout";
import ServiceList from "../../Components/ServiceList/ServiceList";
import styles from "./ServiceDetail.module.css";

const ServiceDetail = () => {
  const { id } = useParams();
  const [state, setState] = useState({
    service: null,
    serviceType: "",
    similarServices: [],
    serviceTypes: [],
    isLoading: true,
    error: null,
  });

  const fakeTherapists = [
    { id: 1, name: "Dr. Emily Carter" },
    { id: 2, name: "John Smith" },
    { id: 3, name: "Sophia Lee" },
    { id: 4, name: "Michael Brown" },
  ];
  

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [serviceRes, allServicesRes, allServiceTypesRes] =
          await Promise.all([
            fetch(`https://672741d4302d03037e702957.mockapi.io/Service/${id}`),
            fetch(`https://672741d4302d03037e702957.mockapi.io/Service`),
            fetch(`https://672741d4302d03037e702957.mockapi.io/ServiceType`),
          ]);

        if (!serviceRes.ok || !allServicesRes.ok || !allServiceTypesRes.ok) {
          throw new Error("Failed to fetch data");
        }

        const serviceData = await serviceRes.json();
        const serviceTypeData = await fetch(
          `https://672741d4302d03037e702957.mockapi.io/ServiceType/${serviceData.ServiceTypeID}`
        ).then((res) => res.json());

        const allServicesData = await allServicesRes.json();
        const similarServices = allServicesData
          .filter(
            (s) =>
              s.ServiceTypeID === serviceData.ServiceTypeID &&
              s.ID !== serviceData.ID
          )
          .slice(0, 4);

        const allServiceTypesData = await allServiceTypesRes.json();

        setState({
          service: serviceData,
          serviceType: serviceTypeData.Name,
          similarServices,
          serviceTypes: allServiceTypesData,
          isLoading: false,
          error: null,
        });
      } catch (error) {
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
    serviceType,
    similarServices,
    serviceTypes,
    isLoading,
    error,
  } = state;

  if (isLoading) return <div className={styles.loading}>Loading...</div>;
  if (error) return <div className={styles.error}>Error: {error}</div>;

  return (
    <div className={styles.container}>
      <ServiceLayout
        service={service}
        serviceType={serviceType}
        therapists={fakeTherapists}
      />

      {similarServices.length > 0 && (
        <div className={styles.similarSection}>
          <h2 className={styles.similarTitle}>Similar Services</h2>
          <ServiceList services={similarServices} serviceTypes={serviceTypes} />
        </div>
      )}
    </div>
  );
};

export default ServiceDetail;
