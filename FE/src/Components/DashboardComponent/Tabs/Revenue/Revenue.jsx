import React, { useState, useEffect } from "react";
import style from "./Revenue.module.css";
import Overview from "./Overview";
import Table from "./Table";

const Revenue = () => {
  const [params, setParams] = useState({
    startDate: getDefaultStartDate(),
    endDate: getDefaultEndDate(),
    pageNumber: 1,
    pageSize: 10,
  });

  function getDefaultEndDate() {
    const today = new Date();
    return today.toISOString().split("T")[0];
  }

  function getDefaultStartDate() {
    const today = new Date();
    const thirtyDaysAgo = new Date(today);
    thirtyDaysAgo.setDate(today.getDate() - 30);
    return thirtyDaysAgo.toISOString().split("T")[0];
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setParams((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  const handlePageChange = (newPage) => {
    setParams((prev) => ({
      ...prev,
      pageNumber: newPage,
    }));
  };

  return (
    <div className={style.revenueContainer}>
      <h2 className={style.title}>Revenue Dashboard</h2>

      <div className={style.topSection}>
        <Overview params={params} />
        <form onSubmit={handleSubmit} className={style.filterForm}>
          <div className={style.inputGroup}>
            <label htmlFor="startDate">Start Date:</label>
            <input
              type="date"
              id="startDate"
              name="startDate"
              value={params.startDate}
              onChange={handleInputChange}
            />
          </div>

          <div className={style.inputGroup}>
            <label htmlFor="endDate">End Date:</label>
            <input
              type="date"
              id="endDate"
              name="endDate"
              value={params.endDate}
              onChange={handleInputChange}
            />
          </div>

          <div className={style.inputGroup}>
            <label htmlFor="pageSize">Page Size:</label>
            <select
              id="pageSize"
              name="pageSize"
              value={params.pageSize}
              onChange={handleInputChange}
            >
              <option value="5">5</option>
              <option value="10">10</option>
              <option value="20">20</option>
              <option value="50">50</option>
            </select>
          </div>

          <button type="submit" className={style.submitButton}>
            Apply Filters
          </button>
        </form>
      </div>

      <Table params={params} onPageChange={handlePageChange} />
    </div>
  );
};

export default Revenue;
