import React from "react";
import ServiceComboCard from "../ServiceComboCard/ServiceComboCard";
import styles from "./ServiceComboList.module.css";

const ServiceComboList = ({ serviceCombos }) => {
  return (
    <div className={styles.comboList}>
      {serviceCombos.map((combo) => (
        <ServiceComboCard key={combo.serviceComboId} combo={combo} />
      ))}
    </div>
  );
};

export default ServiceComboList;
