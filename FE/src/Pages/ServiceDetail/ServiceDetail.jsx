import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ServiceList from "../../Components/ServiceList/ServiceList";
import styles from "./ServiceDetail.module.css";

const ServiceDetail = () => {
  const { id } = useParams();
  const [service, setService] = useState(null);
  const [serviceType, setServiceType] = useState("");
  const [similarServices, setSimilarServices] = useState([]);
  const [serviceTypes, setServiceTypes] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [serviceRes, allServicesRes, allServiceTypesRes] = await Promise.all([
          fetch(`https://672741d4302d03037e702957.mockapi.io/Service/${id}`),
          fetch(`https://672741d4302d03037e702957.mockapi.io/Service`),
          fetch(`https://672741d4302d03037e702957.mockapi.io/ServiceType`)
        ]);

        const serviceData = await serviceRes.json();
        setService(serviceData);

        const serviceTypeData = await fetch(
          `https://672741d4302d03037e702957.mockapi.io/ServiceType/${serviceData.ServiceTypeID}`
        ).then(res => res.json());
        setServiceType(serviceTypeData.Name);

        const allServicesData = await allServicesRes.json();
        const filteredServices = allServicesData.filter(
          (s) => s.ServiceTypeID === serviceData.ServiceTypeID && s.ID !== serviceData.ID
        ).slice(0, 4);
        setSimilarServices(filteredServices);

        const allServiceTypesData = await allServiceTypesRes.json();
        setServiceTypes(allServiceTypesData);
      } catch (error) {
        console.error("Failed to fetch service details:", error);
      }
    };

    fetchData();
  }, [id]);

  if (!service) return <div className={styles.loading}>Loading...</div>;

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>{service.ServiceName}</h1>
      <img src={service.imgUrl} alt={service.ServiceName} className={styles.image} />
      <p className={styles.type}>Type: {serviceType}</p>
      <p className={styles.description}>{service.Description}</p>
      <p className={styles.price}>Price: ${service.Price}</p>
      <p className={styles.popularity}>Popularity: {service.Popularity}</p>

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