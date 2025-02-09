import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import styles from "./ServiceDetail.module.css";

const ServiceDetail = () => {
  const { id } = useParams();
  const [service, setService] = useState(null);
  const [serviceType, setServiceType] = useState("");
  const [similarServices, setSimilarServices] = useState([]);

  useEffect(() => {
    const fetchServiceDetail = async () => {
      try {
        const serviceData = await fetch(
          `https://672741d4302d03037e702957.mockapi.io/Service/${id}`
        ).then((res) => res.json());

        const serviceTypeData = await fetch(
          `https://672741d4302d03037e702957.mockapi.io/ServiceType/${serviceData.ServiceTypeID}`
        ).then((res) => res.json());

        setService(serviceData);
        setServiceType(serviceTypeData.Name);

        const similarServicesData = await fetch(
          `https://672741d4302d03037e702957.mockapi.io/Service`
        ).then((res) => res.json());

        const filteredServices = similarServicesData.filter(
          (s) => s.ServiceTypeID === serviceData.ServiceTypeID && s.ID !== serviceData.ID
        ).slice(0, 4);

        setSimilarServices(filteredServices);
      } catch (error) {
        console.error(error);
      }
    };

    fetchServiceDetail();
  }, [id]);

  if (!service) {
    return <div className={styles.loading}>Loading...</div>;
  }

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>{service.ServiceName}</h1>
      <img src={service.imgUrl} alt={service.ServiceName} className={styles.image} />
      <p className={styles.type}>Type: {serviceType}</p>
      <p className={styles.description}>{service.Description}</p>
      <p className={styles.price}>Price: ${service.Price}</p>
      <p className={styles.popularity}>Popularity: {service.Popularity}</p>

      <div className={styles.similarSection}>
        <h2 className={styles.similarTitle}>Similar Services</h2>
        <div className={styles.similarGrid}>
          {similarServices.map((simService) => (
            <Link to={`/service/${simService.ID}`} key={simService.ID} className={styles.similarCard}>
              <img src={simService.imgUrl} alt={simService.ServiceName} className={styles.similarImage} />
              <p className={styles.similarName}>{simService.ServiceName}</p>
              <p className={styles.similarPrice}>${simService.Price}</p>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ServiceDetail;