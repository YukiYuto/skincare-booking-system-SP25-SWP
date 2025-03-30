import React, { useState, useEffect } from "react";
import style from "./Overview.module.css";
import {
  GET_REVENUE_BY_TRANSACTIONS_API,
  GET_REVENUE_BY_ORDERS_API,
  GET_REVENUE_BY_PROFIT_API,
  GET_ALL_CUSTOMERS_API,
  GET_ALL_THERAPISTS_API,
  GET_ALL_ORDERS_API,
} from "../../../../config/apiConfig";
import { apiCall } from "../../../../utils/apiUtils";

const Overview = ({ params }) => {
  const [summaryData, setSummaryData] = useState({
    totalTransactions: 0,
    totalOrders: 0,
    totalProfit: 0,
    totalCustomers: 0,
    totalTherapists: 0,
  });

  const [loading, setLoading] = useState({
    transactions: false,
    orders: false,
    profit: false,
    customers: false,
    therapists: false,
  });
  const [error, setError] = useState(null);

  const fetchData = async () => {
    const queryParams = {
      startDate: params.startDate,
      endDate: params.endDate,
      pageNumber: 1,
      pageSize: 1,
    };

    try {
      setLoading({
        transactions: true,
        orders: true,
        profit: true,
        customers: true,
        therapists: true,
      });
      setError(null);

      const transactionsPromise = apiCall(
        "GET",
        GET_REVENUE_BY_TRANSACTIONS_API,
        null,
        queryParams
      );

      const ordersPromise = apiCall(
        "GET",
        GET_REVENUE_BY_ORDERS_API,
        null,
        queryParams
      );

      const profitPromise = apiCall(
        "GET",
        GET_REVENUE_BY_PROFIT_API,
        null,
        queryParams
      );

      const customersPromise = apiCall(
        "GET",
        GET_ALL_CUSTOMERS_API,
        null,
        null
      );

      const therapistsPromise = apiCall(
        "GET",
        GET_ALL_THERAPISTS_API,
        null,
        null
      );

      const [
        transactionsResult,
        ordersResult,
        profitResult,
        customersResult,
        therapistsResult,
      ] = await Promise.all([
        transactionsPromise,
        ordersPromise,
        profitPromise,
        customersPromise,
        therapistsPromise,
      ]);

      setSummaryData({
        totalTransactions: transactionsResult.isSuccess
          ? transactionsResult.result.totalTransactions
          : 0,
        totalOrders: ordersResult.isSuccess
          ? ordersResult.result.totalOrders
          : 0,
        totalProfit: profitResult.isSuccess
          ? profitResult.result.totalProfit
          : 0,
        totalCustomers: customersResult.isSuccess
          ? customersResult.result.length
          : 0,
        totalTherapists: therapistsResult.isSuccess
          ? therapistsResult.result.length
          : 0,
      });
    } catch (err) {
      setError(err.message || "Failed to fetch overview data");
    } finally {
      setLoading({
        transactions: false,
        orders: false,
        profit: false,
        customers: false,
        therapists: false,
      });
    }
  };

  const formatCurrency = (amount) => {
    return amount.toLocaleString("vi-VN", {
      style: "currency",
      currency: "VND",
    });
  };

  useEffect(() => {
    fetchData();
  }, [params.startDate, params.endDate]);

  const isLoading = Object.values(loading).some((status) => status);

  return (
    <div className={style.overviewContainer}>
      {error && <div className={style.error}>{error}</div>}

      {isLoading ? (
        <div className={style.loading}>Loading overview data...</div>
      ) : (
        <div className={style.statsContainer}>
          <div className={style.statCard}>
            <div className={style.statIcon}>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <rect x="2" y="5" width="20" height="14" rx="2" />
                <line x1="2" y1="10" x2="22" y2="10" />
              </svg>
            </div>
            <div className={style.statInfo}>
              <h3>Total Transactions</h3>
              <div className={style.statValue}>
                {summaryData.totalTransactions.toLocaleString()}
              </div>
              <div className={style.statPeriod}>
                {params.startDate} to {params.endDate}
              </div>
            </div>
          </div>

          <div className={style.statCard}>
            <div className={style.statIcon}>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z" />
                <line x1="3" y1="6" x2="21" y2="6" />
                <path d="M16 10a4 4 0 0 1-8 0" />
              </svg>
            </div>
            <div className={style.statInfo}>
              <h3>Total Orders</h3>
              <div className={style.statValue}>
                {summaryData.totalOrders.toLocaleString()}
              </div>
              <div className={style.statPeriod}>
                {params.startDate} to {params.endDate}
              </div>
            </div>
          </div>

          <div className={style.statCard}>
            <div className={style.statIcon}>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <line x1="12" y1="1" x2="12" y2="23" />
                <path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6" />
              </svg>
            </div>
            <div className={style.statInfo}>
              <h3>Total Profit</h3>
              <div className={style.statValue}>
                {formatCurrency(summaryData.totalProfit)}
              </div>
              <div className={style.statPeriod}>
                {params.startDate} to {params.endDate}
              </div>
            </div>
          </div>

          <div className={style.statCard}>
            <div className={style.statIcon}>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
                <circle cx="9" cy="7" r="4" />
                <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
                <path d="M16 3.13a4 4 0 0 1 0 7.75" />
              </svg>
            </div>
            <div className={style.statInfo}>
              <h3>Total Customers</h3>
              <div className={style.statValue}>
                {summaryData.totalCustomers.toLocaleString()}
              </div>
              <div className={style.statPeriod}>All time</div>
            </div>
          </div>

          <div className={style.statCard}>
            <div className={style.statIcon}>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z" />
                <polyline points="9 22 9 12 15 12 15 22" />
              </svg>
            </div>
            <div className={style.statInfo}>
              <h3>Total Therapists</h3>
              <div className={style.statValue}>
                {summaryData.totalTherapists.toLocaleString()}
              </div>
              <div className={style.statPeriod}>All time</div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Overview;
