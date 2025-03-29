import React, { useState, useEffect } from "react";
import { GET_REVENUE_BY_PROFIT_API } from "../../../../../../config/apiConfig";
import style from "./Profit.module.css";
import { apiCall } from "../../../../../../utils/apiUtils";

const Profit = () => {
  const [startDate, setStartDate] = useState("2025-02-20");
  const [endDate, setEndDate] = useState("2025-04-20");
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [loading, setLoading] = useState(false);
  const [response, setResponse] = useState(null);
  const [error, setError] = useState(null);

  const fetchProfit = async () => {
    try {
      setLoading(true);
      setError(null);

      const data = await apiCall("GET", GET_REVENUE_BY_PROFIT_API, null, {
        startDate,
        endDate,
        pageNumber,
        pageSize,
      });

      if (!data.isSuccess) {
        throw new Error(data.message || "Failed to fetch profit data");
      }

      setResponse(data);
    } catch (error) {
      console.error("API Error:", error);
      setError(error.message || "An error occurred while fetching profit data");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProfit();
  }, [pageNumber]);

  const handleSubmit = (e) => {
    e.preventDefault();
    fetchProfit();
  };

  return (
    <div className={style.container}>
      <form onSubmit={handleSubmit} className={style.form}>
        <div className={style.dateGroup}>
          <label className={style.label}>Start Date</label>
          <input
            type="date"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
            className={style.input}
            required
          />
        </div>
        <div className={style.dateGroup}>
          <label className={style.label}>End Date</label>
          <input
            type="date"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            className={style.input}
            required
          />
        </div>

        <div className={style.numberGroup}>
          <label className={style.label}>
            Page of {response?.result?.totalPages || 0}
          </label>
          <input
            type="number"
            min="1"
            value={pageNumber}
            onChange={(e) => setPageNumber(Number(e.target.value))}
            className={style.input}
            required
          />
        </div>
        <div className={style.numberGroup}>
          <label className={style.label}>Page Size</label>
          <input
            type="number"
            min="1"
            value={pageSize}
            onChange={(e) => setPageSize(Number(e.target.value))}
            className={style.input}
            required
          />
        </div>

        <div className={style.buttonGroup}>
          <button
            type="submit"
            className={`${style.button} ${loading ? style.buttonDisabled : ""}`}
            disabled={loading}
          >
            {loading ? "Loading..." : "Search Profit"}
          </button>
        </div>
      </form>

      {error && <div className={style.errorMessage}>{error}</div>}

      {response?.result ? (
        <div className={style.card}>
          <div className={style.cardHeader}>
            <h2 className={style.cardTitle}>Results</h2>
            <p className={style.statValue}>
              Total Profit: {response.result.totalProfit || 0} VND
            </p>
          </div>

          <div className={style.tableContainer}>
            <table className={style.table}>
              <thead>
                <tr>
                  <th className={style.tableHeader}>#</th>
                  <th className={style.tableHeader}>Transaction ID</th>
                  <th className={style.tableHeader}>Order ID</th>
                  <th className={style.tableHeader}>Amount</th>
                  <th className={style.tableHeader}>Date</th>
                  <th className={style.tableHeader}>Method</th>
                </tr>
              </thead>
              <tbody>
                {response.result.transactions?.map((transaction, index) => (
                  <tr
                    key={transaction.transactionId}
                    className={
                      index % 2 === 0 ? style.tableRowEven : style.tableRowOdd
                    }
                  >
                    <td className={style.tableCell}>
                      {(response.result.currentPage - 1) *
                        response.result.pageSize +
                        index +
                        1}
                    </td>
                    <td className={style.tableCell}>
                      {transaction.transactionId}
                    </td>
                    <td className={style.tableCell}>{transaction.orderId}</td>
                    <td className={style.tableCell}>{transaction.amount}</td>
                    <td className={style.tableCell}>
                      {new Date(
                        transaction.transactionDateTime
                      ).toLocaleString()}
                    </td>
                    <td className={style.tableCell}>
                      {transaction.transactionMethod}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className={style.pagination}>
            <button
              onClick={() => {
                if (pageNumber > 1) {
                  setPageNumber((prev) => prev - 1);
                }
              }}
              disabled={pageNumber <= 1 || loading}
              className={`${style.paginationButton} ${
                pageNumber <= 1 || loading ? style.buttonDisabled : ""
              }`}
            >
              Previous
            </button>
            <div className={style.pageInfo}>
              Page {response.result.currentPage} of {response.result.totalPages}
            </div>
            <button
              onClick={() => {
                if (pageNumber < response.result.totalPages) {
                  setPageNumber((prev) => prev + 1);
                }
              }}
              disabled={pageNumber >= response.result.totalPages || loading}
              className={`${style.paginationButton} ${
                pageNumber >= response.result.totalPages || loading
                  ? style.buttonDisabled
                  : ""
              }`}
            >
              Next
            </button>
          </div>
        </div>
      ) : (
        <p>No data available.</p>
      )}
    </div>
  );
};

export default Profit;
