import React, { useState } from "react";
import styles from "./Revenue.module.css";
import Order from "./Section/Order/Order";
import Profit from "./Section/Profit/Profit";
import Transactions from "./Section/Transaction/Transaction";

const Revenue = () => {
  const [selectedSection, setSelectedSection] = useState("orders");

  const renderContent = () => {
    switch (selectedSection) {
      case "orders":
        return <Order />;
      case "profit":
        return <Profit />;
      case "transactions":
        return <Transactions />;
      default:
        return null;
    }
  };

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <h2 className={styles.tabTitle}>Revenue</h2>
        <div className={styles.revenueSections}>
          <section
            className={styles.revenueSection}
            onClick={() => setSelectedSection("orders")}
          >
            <h3>Orders</h3>
          </section>
          <section
            className={styles.revenueSection}
            onClick={() => setSelectedSection("profit")}
          >
            <h3>Profit</h3>
          </section>
          <section
            className={styles.revenueSection}
            onClick={() => setSelectedSection("transactions")}
          >
            <h3>Transactions</h3>
          </section>
        </div>
      </div>
      <div className={styles.contentSection}>{renderContent()}</div>
    </div>
  );
};

export default Revenue;
