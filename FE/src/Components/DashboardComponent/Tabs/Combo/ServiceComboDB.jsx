import React, { useEffect, useState, useCallback } from "react";
import styles from "./ServiceCombo.module.css";
import ServiceComboCreateModal from "./ServiceComboCreateModal";
import ServiceComboDetailModal from "./ServiceComboDetailModal";
import { GET_ALL_SERVICE_COMBO_API } from "../../../../config/apiConfig";
import addIcon from "../../../../assets/icon/addIcon.svg";
import editIcon from "../../../../assets/icon/editIcon.svg";

const DEFAULT_PAGE_SIZE = 10;
const DEFAULT_PAGE_NUMBER = 1;

const ServiceComboDB = () => {
  const [serviceCombos, setServiceCombos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [pagination, setPagination] = useState({
    pageNumber: DEFAULT_PAGE_NUMBER,
    pageSize: DEFAULT_PAGE_SIZE,
  });
  const [modal, setModal] = useState({ type: null, data: null });
  const [totalPages, setTotalPages] = useState(1);

  const fetchData = useCallback(async () => {
    setLoading(true);
    try {
      const response = await fetch(
        `${GET_ALL_SERVICE_COMBO_API}?pageNumber=${pagination.pageNumber}&pageSize=${pagination.pageSize}`
      );
      const data = await response.json();
      setServiceCombos(data.result.serviceCombos);
      setTotalPages(data.result.totalPages);
    } catch (error) {
      setError(error);
    } finally {
      setLoading(false);
    }
  }, [pagination]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <div className={styles.serviceCombo}>
          <h2 className={styles.tabTitle}>Service Combos</h2>
          <button
            onClick={() => setModal({ type: "create" })}
            className={styles.iconButton}
          >
            <img src={addIcon} alt="Add" />
          </button>
        </div>
        <div className={styles.serviceCombo}>
          Page
          <button
            onClick={() =>
              setPagination((prev) => ({
                ...prev,
                pageSize: prev.pageSize === 10 ? 20 : 10,
                pageNumber: DEFAULT_PAGE_NUMBER,
              }))
            }
            className={styles.iconButton}
          >
            {pagination.pageSize}
          </button>
          Size
        </div>
      </div>
      <div className={styles.serviceTableContainer}>
        {loading && <p>Loading service combos...</p>}
        {error && <p>Error loading service combos</p>}
        <table className={styles.serviceTable}>
          <thead>
            <tr>
              <th>Combo Name</th>
              <th>Price</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {serviceCombos.length === 0 ? (
              <tr>
                <td colSpan="3">No service combos found.</td>
              </tr>
            ) : (
              serviceCombos.map((combo) => (
                <tr key={combo.comboId}>
                  <td>{combo.comboName}</td>
                  <td>{(combo.price / 1000).toFixed(3)}₫</td>
                  <td>
                    <button
                      onClick={() => setModal({ type: "detail", data: combo })}
                      className={styles.iconButton}
                    >
                      <img src={editIcon} alt="Detail" />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      <div className={styles.paginationControls}>
        <button
          disabled={pagination.pageNumber === 1}
          onClick={() =>
            setPagination((prev) => ({
              ...prev,
              pageNumber: prev.pageNumber - 1,
            }))
          }
          className={styles.iconButton}
        >
          <b>&lt;</b>
        </button>
        <span>{pagination.pageNumber}</span>
        <button
          disabled={pagination.pageNumber === totalPages}
          onClick={() =>
            setPagination((prev) => ({
              ...prev,
              pageNumber: prev.pageNumber + 1,
            }))
          }
          className={styles.iconButton}
        >
          <b>&gt;</b>
        </button>
      </div>

      {modal.type === "create" && (
        <ServiceComboCreateModal
          onClose={() => setModal({ type: null })}
          refresh={fetchData}
        />
      )}
      {modal.type === "detail" && (
        <ServiceComboDetailModal
          data={modal.data}
          onClose={() => setModal({ type: null })}
          refresh={fetchData}
        />
      )}
    </div>
  );
};

export default ServiceComboDB;
