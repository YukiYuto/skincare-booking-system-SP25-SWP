import React, { useEffect, useState, useCallback } from "react";
import {
  GET_ALL_THERAPISTS_API,
  POST_THERAPIST_API,
  POST_STAFF_API,
} from "../../../../config/apiConfig";
import styles from "./SkinTherapists.module.css";
import infoIcon from "../../../../assets/icon/infoIcon.svg";
import addIcon from "../../../../assets/icon/addIcon.svg";
import SkinTherapistDetail from "./SkinTherapistDetail";
import TherapistAddModal from "./TherapistAddModal";
import StaffAddModal from "../Staff/StaffAddModal";

const SkinTherapists = () => {
  const [therapists, setTherapists] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedTherapist, setSelectedTherapist] = useState(null);
  const [sortConfig, setSortConfig] = useState({
    key: null,
    direction: "ascending",
  });
  const [error, setError] = useState(null);
  const [modal, setModal] = useState({ type: null, data: null });

  const fetchData = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);

      const response = await fetch(GET_ALL_THERAPISTS_API);
      const data = await response.json();

      if (data.isSuccess) {
        setTherapists(data.result);
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

  const handleSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  const sortedTherapists = [...therapists].sort((a, b) => {
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

  const handleOpenDetail = (therapist) => {
    setSelectedTherapist(therapist);
  };

  const handleCloseDetail = () => {
    setSelectedTherapist(null);
  };

  return (
    <div className={styles.tabContainer}>
      <div className={styles.tabHeader}>
        <div className={styles.tabTitleContainer}>
          <div className={styles.tabTitleGroup}>
            <h2 className={styles.tabTitle}>Skin Therapists</h2>
            <button
              onClick={() => setModal({ type: "createTherapist" })}
              className={styles.iconButton}
            >
              <img src={addIcon} alt="Add Therapist" />
            </button>
          </div>
          <div className={styles.tabTitleGroup}>
            <h2 className={styles.tabTitle}>Staff</h2>
            <button
              onClick={() => setModal({ type: "createStaff" })}
              className={styles.iconButton}
            >
              <img src={addIcon} alt="Add Staff" />
            </button>
          </div>
        </div>
      </div>
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>{error}</p>
      ) : (
        <div className={styles.therapistTableContainer}>
          <table className={styles.therapistTable}>
            <thead>
              <tr>
                <th onClick={() => handleSort("fullName")}>
                  Name{" "}
                  {sortConfig.key === "fullName" &&
                    (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("email")}>
                  Email{" "}
                  {sortConfig.key === "email" &&
                    (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("age")}>
                  Age{" "}
                  {sortConfig.key === "age" &&
                    (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("gender")}>
                  Gender{" "}
                  {sortConfig.key === "gender" &&
                    (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("phoneNumber")}>
                  Phone Number{" "}
                  {sortConfig.key === "phoneNumber" &&
                    (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th onClick={() => handleSort("experience")}>
                  Experience{" "}
                  {sortConfig.key === "experience" &&
                    (sortConfig.direction === "ascending" ? "↑" : "↓")}
                </th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {sortedTherapists.map((therapist) => (
                <tr key={therapist.skinTherapistId}>
                  <td>{therapist.fullName}</td>
                  <td>{therapist.email}</td>
                  <td>{therapist.age}</td>
                  <td>{therapist.gender}</td>
                  <td>{therapist.phoneNumber}</td>
                  <td>{therapist.experience} years</td>
                  <td>
                    <button
                      className={styles.infoButton}
                      onClick={() => handleOpenDetail(therapist)}
                      aria-label="View therapist details"
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

      {selectedTherapist && (
        <SkinTherapistDetail
          therapist={selectedTherapist}
          onClose={handleCloseDetail}
        />
      )}

      {modal.type === "createTherapist" && (
        <TherapistAddModal
          onClose={() => setModal({ type: null })}
          refresh={fetchData}
        />
      )}

      {modal.type === "createStaff" && (
        <StaffAddModal
          onClose={() => setModal({ type: null })}
          refresh={fetchData}
        />
      )}
    </div>
  );
};

export default SkinTherapists;
