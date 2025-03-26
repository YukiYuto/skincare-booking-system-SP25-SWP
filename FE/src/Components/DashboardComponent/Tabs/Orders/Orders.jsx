import React, { useState, useEffect, useMemo, useCallback } from "react";
import styles from "./Orders.module.css";
import {
  GET_ALL_ORDERS_API,
  GET_ALL_CUSTOMERS_API,
} from "../../../../config/apiConfig";
import OrderEditModal from "./OrderEditModal";

const OrderTable = () => {
  const [orders, setOrders] = useState([]);
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [sortConfig, setSortConfig] = useState({ key: null, direction: "asc" });
  const [modal, setModal] = useState({ type: null, data: null });

  const fetchData = useCallback(async () => {
    try {
      setLoading(true);

      // Fetch orders
      const ordersResponse = await fetch(GET_ALL_ORDERS_API);
      const ordersData = await ordersResponse.json();

      // Fetch customers
      const customersResponse = await fetch(GET_ALL_CUSTOMERS_API);
      const customersData = await customersResponse.json();

      if (ordersData.isSuccess && customersData.isSuccess) {
        setOrders(ordersData.result);
        setCustomers(customersData.result);
      } else {
        setError("Failed to fetch data");
      }
    } catch (err) {
      setError("Error fetching data: " + err.message);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const getCustomerName = (customerId) => {
    const customer = customers.find((c) => c.customerId === customerId);
    return customer ? customer.fullName : "Unknown Customer";
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleString();
  };

  const formatPrice = (price) => {
    return price.toLocaleString() + " đ";
  };

  const handleSort = (key) => {
    setSortConfig((prev) => ({
      key,
      direction: prev.key === key && prev.direction === "asc" ? "desc" : "asc",
    }));
  };

  const sortedOrders = useMemo(() => {
    return [...orders].sort((a, b) => {
      if (!sortConfig.key) return 0;
      let valueA = a[sortConfig.key];
      let valueB = b[sortConfig.key];

      if (typeof valueA === "string") valueA = valueA.toLowerCase();
      if (typeof valueB === "string") valueB = valueB.toLowerCase();

      return sortConfig.direction === "asc"
        ? valueA > valueB
          ? 1
          : -1
        : valueA < valueB
        ? 1
        : -1;
    });
  }, [orders, sortConfig]);

  if (error) {
    return (
      <div className={styles.errorContainer}>
        <h2>Error Loading Orders</h2>
        <p>{error}</p>
        <button onClick={fetchData} className={styles.retryButton}>
          Retry Loading
        </button>
      </div>
    );
  }

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <div className={styles.order}>
          <h2 className={styles.tabTitle}>Orders</h2>
        </div>
      </div>

      {loading ? (
        <div className={styles.loadingContainer}>
          <p>Loading orders...</p>
        </div>
      ) : (
        <div className={styles.orderTableContainer}>
          <table className={styles.orderTable}>
            <thead>
              <tr>
                {[
                  "Customer Name",
                  "Order Number",
                  "Total Price",
                  "Created Time",
                  "Actions",
                ].map((key) => (
                  <th
                    key={key}
                    onClick={() =>
                      handleSort(key.replace(/\s+/g, "").toLowerCase())
                    }
                  >
                    {key}
                    {sortConfig.key === key.replace(/\s+/g, "").toLowerCase() &&
                      (sortConfig.direction === "asc" ? " ↑" : " ↓")}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {sortedOrders.map((order) => (
                <tr key={order.orderId}>
                  <td>{getCustomerName(order.customerId)}</td>
                  <td>{order.orderNumber}</td>
                  <td className={styles.rightAlign}>
                    {order.totalPrice > 0
                      ? formatPrice(order.totalPrice)
                      : "0 đ"}
                  </td>
                  <td>{formatDate(order.createdTime)}</td>
                  <td>
                    <button
                      onClick={() => setModal({ type: "edit", data: order })}
                      className={styles.iconButton}
                    >
                      Edit
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {modal.type === "edit" && (
        <OrderEditModal
          order={modal.data}
          onClose={() => setModal({ type: null })}
          refresh={fetchData}
        />
      )}
    </div>
  );
};

export default OrderTable;
