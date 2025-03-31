import React, { useState, useEffect } from "react";
import style from "./Table.module.css";
import {
  GET_REVENUE_BY_TRANSACTIONS_API,
  GET_ALL_CUSTOMERS_API,
  GET_ALL_ORDERS_API,
} from "../../../../config/apiConfig";
import { apiCall } from "../../../../utils/apiUtils";

const Table = ({ params, onPageChange }) => {
  const [data, setData] = useState({
    transactions: [],
    totalTransactions: 0,
    totalPages: 0,
    currentPage: 1,
  });
  const [customerMap, setCustomerMap] = useState({});
  const [orderMap, setOrderMap] = useState({});
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchMappingData = async () => {
    try {
      const customersResult = await apiCall(
        "GET",
        GET_ALL_CUSTOMERS_API,
        null,
        null
      );

      if (customersResult.isSuccess) {
        const newCustomerMap = {};
        const customers = Array.isArray(customersResult.result)
          ? customersResult.result
          : customersResult.result.result;

        if (customers) {
          customers.forEach((customer) => {
            newCustomerMap[customer.customerId] = customer.fullName;
          });
          setCustomerMap(newCustomerMap);
        }
      }

      const ordersResult = await apiCall("GET", GET_ALL_ORDERS_API, null, null);

      if (ordersResult.isSuccess) {
        const newOrderMap = {};
        const orders = Array.isArray(ordersResult.result)
          ? ordersResult.result
          : ordersResult.result.result;

        if (orders) {
          orders.forEach((order) => {
            const orderIdKey = order.orderId || order.id;
            const orderNumberValue =
              order.orderNumber || `Order #${orderIdKey.substring(0, 8)}`;
            newOrderMap[orderIdKey] = orderNumberValue;
          });
          setOrderMap(newOrderMap);
        }
      }
    } catch (err) {
      setError(
        (prevError) => prevError || "Failed to load customer and order data"
      );
    }
  };

  const fetchTransactions = async () => {
    setLoading(true);
    setError(null);

    try {
      const queryParams = {
        startDate: params.startDate,
        endDate: params.endDate,
        pageNumber: params.pageNumber,
        pageSize: params.pageSize,
      };

      const result = await apiCall(
        "GET",
        GET_REVENUE_BY_TRANSACTIONS_API,
        null,
        queryParams
      );

      if (result.isSuccess) {
        setData({
          transactions: result.result.transactions,
          totalTransactions: result.result.totalTransactions,
          totalPages: result.result.totalPages,
          currentPage: result.result.currentPage,
        });
      } else {
        throw new Error(result.message || "Failed to fetch transactions");
      }
    } catch (err) {
      setError(err.message || "An error occurred while fetching data");
    } finally {
      setLoading(false);
    }
  };

  const formatDateTime = (dateTimeString) => {
    const date = new Date(dateTimeString);
    return date.toLocaleString();
  };

  const formatAmount = (amount) => {
    return amount.toLocaleString("vi-VN", {
      style: "currency",
      currency: "VND",
    });
  };

  const renderPagination = () => {
    const pages = [];
    for (let i = 1; i <= data.totalPages; i++) {
      pages.push(
        <button
          key={i}
          onClick={() => onPageChange(i)}
          className={`${style.paginationButton} ${
            data.currentPage === i ? style.activePage : ""
          }`}
        >
          {i}
        </button>
      );
    }
    return (
      <div className={style.pagination}>
        <button
          onClick={() => onPageChange(data.currentPage - 1)}
          disabled={data.currentPage === 1}
          className={style.paginationButton}
        >
          Previous
        </button>
        {pages}
        <button
          onClick={() => onPageChange(data.currentPage + 1)}
          disabled={data.currentPage === data.totalPages}
          className={style.paginationButton}
        >
          Next
        </button>
      </div>
    );
  };

  const getCustomerName = (customerId) => {
    return customerMap[customerId] || `Unknown (${customerId})`;
  };

  const getOrderNumber = (orderId) => {
    return orderMap[orderId] || `Unknown (${orderId})`;
  };

  useEffect(() => {
    fetchMappingData();
  }, []);

  useEffect(() => {
    fetchTransactions();
  }, [params]);

  return (
    <div className={style.container}>
      <h2 className={style.title}>Transaction Data</h2>

      {loading && <div className={style.loading}>Loading...</div>}
      {error && <div className={style.error}>Error: {error}</div>}

      {!loading && !error && (
        <>
          <div className={style.tableWrapper}>
            <table className={style.table}>
              <thead>
                <tr>
                  <th>Transaction ID</th>
                  <th>Customer</th>
                  <th>Order Number</th>
                  <th>Amount</th>
                  <th>Date & Time</th>
                  <th>Status</th>
                  <th>Method</th>
                </tr>
              </thead>
              <tbody>
                {data.transactions && data.transactions.length > 0 ? (
                  data.transactions.map((transaction) => (
                    <tr key={transaction.transactionId}>
                      <td className={style.idCell}>
                        {transaction.transactionId}
                      </td>
                      <td className={style.customerCell}>
                        {getCustomerName(transaction.customerId)}
                      </td>
                      <td className={style.orderCell}>
                        {getOrderNumber(transaction.orderId)}
                      </td>
                      <td className={style.amountCell}>
                        {formatAmount(transaction.amount)}
                      </td>
                      <td>{formatDateTime(transaction.transactionDateTime)}</td>
                      <td>
                        {transaction.status === 0
                          ? "Pending"
                          : transaction.status === 1
                          ? "Purchased"
                          : "Cancelled"}
                      </td>
                      <td>{transaction.paymentMethod}</td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="7" className={style.noData}>
                      No transaction data available
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>

          {renderPagination()}
        </>
      )}
    </div>
  );
};

export default Table;
