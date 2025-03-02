import React, { useEffect, useState, useCallback } from "react";
import api from "../../../../config/axios";
import styles from "./SkinTherapists.module.css";
import infoIcon from "../../../../assets/icon/infoIcon.svg";
import SkinTherapistDetail from "./SkinTherapistDetail";

const SkinTherapists = () => {
  const [therapists, setTherapists] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedTherapist, setSelectedTherapist] = useState(null);
  const [sortConfig, setSortConfig] = useState({
    key: null,
    direction: "ascending",
  });

  const fetchData = useCallback(async () => {
    setLoading(true);
    try {
      const therapistsRes = await api.get("SkinTherapist/skin-therapists");
      setTherapists(therapistsRes.data.result);
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
          <h2 className={styles.tabTitle}>Skin Therapists</h2>
        </div>
      </div>
      {loading ? (
        <p>Loading...</p>
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
                  <td>{therapist.experience}</td>
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
    </div>
  );
};

export default SkinTherapists;
