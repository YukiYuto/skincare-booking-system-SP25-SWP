import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchCustomers } from "../../../../redux/Customer/CustomerThunk";
import styles from "./Customers.module.css";
import infoIcon from "../../../../assets/icon/infoIcon.svg";
import CustomerDetail from "./CustomerDetail";

const Customers = () => {
  const dispatch = useDispatch();
  const {
    customers = [],
    loading,
    error,
  } = useSelector((state) => state.customer); // Ensure customers is an array
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [sortConfig, setSortConfig] = useState({
    key: "fullName",
    direction: "ascending",
  });

  useEffect(() => {
    dispatch(fetchCustomers());
  }, [dispatch]);

  const handleSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  const customerList = Array.isArray(customers.result) ? customers.result : [];

  const sortedCustomers = [...customerList].sort((a, b) => {
    if (!sortConfig.key) return 0;

    let valueA = a[sortConfig.key] ?? ""; // Ensure a valid value
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
                {[
                  "fullName",
                  "email",
                  "age",
                  "gender",
                  "phoneNumber",
                  "address",
                ].map((key) => (
                  <th key={key} onClick={() => handleSort(key)}>
                    {key.charAt(0).toUpperCase() + key.slice(1)}{" "}
                    {sortConfig.key === key &&
                      (sortConfig.direction === "ascending" ? "↑" : "↓")}
                  </th>
                ))}
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
                      "age",
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
