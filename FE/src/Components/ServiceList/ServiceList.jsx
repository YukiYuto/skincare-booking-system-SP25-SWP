import React from "react";
import ServiceCard from "../../Components/ServiceCard/ServiceCard";
import styles from "./ServiceList.module.css";

const ServiceList = ({ services, serviceTypes }) => {
  return (
    <div className={styles.itemList}>
      {services.map((serviceItem) => {
        const matchedServiceType = serviceTypes.find(
          (type) => type.ID === String(serviceItem.ServiceTypeID)
        );
        const serviceName = matchedServiceType ? matchedServiceType.Name : "Unknown";

        return (
          <ServiceCard
            key={serviceItem.ID}
            service={serviceItem}
            serviceName={serviceName}
          />
        );
      })}
    </div>
  );
};

export default ServiceList;
