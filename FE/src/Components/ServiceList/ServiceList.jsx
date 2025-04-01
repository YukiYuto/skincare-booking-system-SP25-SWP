import React from "react";
import ServiceCard from "../../Components/ServiceCard/ServiceCard";
import styles from "./ServiceList.module.css";

const ServiceList = ({ services, serviceTypes }) => {
  return (
    <div className={styles.itemList}>
      {services.map((serviceItem) => {
        const matchedServiceType = serviceTypes.find(
          (type) => type.serviceId === serviceItem.serviceTypeId
        );
        const serviceName = matchedServiceType
          ? matchedServiceType.serviceName
          : "Unknown";

        return (
          <ServiceCard
            key={serviceItem.serviceId}
            service={serviceItem}
            serviceName={serviceName}
          />
        );
      })}
    </div>
  );
};

export default ServiceList;
