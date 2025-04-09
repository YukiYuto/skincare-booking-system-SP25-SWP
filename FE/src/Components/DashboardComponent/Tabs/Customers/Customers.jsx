import React, { useEffect, useState } from "react";
import axios from "axios";
import { GET_ALL_CUSTOMERS_API } from "../../../../config/apiConfig";
import styles from "./Customers.module.css";
import infoIcon from "../../../../assets/icon/infoIcon.svg";
import CustomerDetail from "./CustomerDetail";
import { apiCall } from "../../../../utils/apiUtils";

const Customers = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [sortConfig, setSortConfig] = useState({
    key: "fullName",
    direction: "ascending",
  });

  useEffect(() => {
    const fetchCustomers = async () => {
      try {
        const response = await apiCall("get", GET_ALL_CUSTOMERS_API);
        setCustomers(response.result);
      } catch (err) {
        if (err.status === 404) {
          setError("Customers not found (404)");
        } else {
          setError(err.message);
        }
      } finally {
        setLoading(false);
      }
    };

    fetchCustomers();
  }, []);

  const handleSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  const sortedCustomers = [...customers].sort((a, b) => {
    if (!sortConfig.key) return 0;

    let valueA = a[sortConfig.key] ?? "";
    let valueB = b[sortConfig.key] ?? "";

    if (typeof valueA === "string") valueA = valueA.toLowerCase();
    if (typeof valueB === "string") valueB = valueB.toLowerCase();

    return valueA < valueB
      ? sortConfig.direction === "ascending"
        ? -1
        : 1
      : valueA > valueB
      ? sortConfig.direction === "ascending"
        ? 1
        : -1
      : 0;
  });

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <h2 className={styles.tabTitle}>Customers</h2>
      </div>

      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>Error: {error}</p>
      ) : (
        <div className={styles.customerTableContainer}>
          <table className={styles.customerTable}>
            <thead>
              <tr>
                {["fullName", "email", "gender", "phone", "address"].map(
                  (key) => (
                    <th key={key} onClick={() => handleSort(key)}>
                      {key.charAt(0).toUpperCase() + key.slice(1)}{" "}
                      {sortConfig.key === key &&
                        (sortConfig.direction === "ascending" ? "↑" : "↓")}
                    </th>
                  )
                )}
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {sortedCustomers.length > 0 ? (
                sortedCustomers.map((customer) => (
                  <tr key={customer.customerId}>
                    {[
                      "fullName",
                      "email",
                      "gender",
                      "phoneNumber",
                      "address",
                    ].map((key) => (
                      <td key={key}>{customer[key]}</td>
                    ))}
                    <td>
                      <button
                        className={styles.infoButton}
                        onClick={() => setSelectedCustomer(customer)}
                        aria-label="View customer details"
                      >
                        <img src={infoIcon} alt="Detail" />
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="7">No customers found</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}

      {selectedCustomer && (
        <CustomerDetail
          customer={selectedCustomer}
          onClose={() => setSelectedCustomer(null)}
        />
      )}
    </div>
  );
};

export default Customers;
