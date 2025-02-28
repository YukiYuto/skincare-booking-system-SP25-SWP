import React, { useEffect, useState, useCallback } from "react";
import api from "../../../../config/axios";
import styles from "./Customers.module.css";
import infoIcon from "../../../../assets/icon/infoIcon.svg";
import CustomerDetail from "./CustomerDetail";

const Customers = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [sortConfig, setSortConfig] = useState({
    key: null,
    direction: "ascending",
  });

  const fetchData = useCallback(async () => {
    setLoading(true);
    try {
      const customersRes = await api.get("Customer/list");
      setCustomers(customersRes.data.result);
    } catch (error) {
      console.error("Error fetching data:", error);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const handleSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  const sortedCustomers = [...customers].sort((a, b) => {
    if (!sortConfig.key) return 0;

    let valueA =
      typeof a[sortConfig.key] === "string"
        ? a[sortConfig.key].toLowerCase()
        : a[sortConfig.key];
    let valueB =
      typeof b[sortConfig.key] === "string"
        ? b[sortConfig.key].toLowerCase()
        : b[sortConfig.key];

    if (valueA < valueB) return sortConfig.direction === "ascending" ? -1 : 1;
    if (valueA > valueB) return sortConfig.direction === "ascending" ? 1 : -1;
    return 0;
  });

  const handleOpenDetail = (customer) => {
    setSelectedCustomer(customer);
  };

  const handleCloseDetail = () => {
    setSelectedCustomer(null);
  };

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <div className={styles.tabTitleContainer}>
          <h2 className={styles.tabTitle}>Customers</h2>
        </div>
      </div>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <div className={styles.customerTableContainer}>
          <table className={styles.customerTable}>
            <thead>
              <tr>
                <th onClick={() => handleSort("fullName")}>
                  Name {sortConfig.key === "fullName" && (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("email")}>
                  Email {sortConfig.key === "email" && (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("age")}>
                  Age {sortConfig.key === "age" && (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("gender")}>
                  Gender {sortConfig.key === "gender" && (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("phoneNumber")}>
                  Phone Number {sortConfig.key === "phoneNumber" && (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("address")}>
                  Address {sortConfig.key === "address" && (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {sortedCustomers.map((customer) => (
                <tr key={customer.customerId}>
                  <td>{customer.fullName}</td>
                  <td>{customer.email}</td>
                  <td>{customer.age}</td>
                  <td>{customer.gender}</td>
                  <td>{customer.phoneNumber}</td>
                  <td>{customer.address}</td>
                  <td>
                    <button
                      className={styles.infoButton}
                      onClick={() => handleOpenDetail(customer)}
                      aria-label="View customer details"
                    >
                      <img src={infoIcon} alt="Detail" />
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
      
      {selectedCustomer && (
        <CustomerDetail 
          customer={selectedCustomer} 
          onClose={handleCloseDetail} 
        />
      )}
    </div>
  );
};

export default Customers;