import React, { useEffect, useState } from "react";
import axios from "axios";
import styles from "./TherapistList.module.css";
import { GET_ALL_THERAPISTS_API } from "../../config/apiConfig";
import TherapistCard from "../TherapistCard/TherapistCard";
import Header from "../Common/Header";
import Footer from "../Footer/Footer";

const TherapistList = () => {
  const [therapists, setTherapists] = useState([]);
  const [filteredTherapists, setFilteredTherapists] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const [sortField, setSortField] = useState("");

  useEffect(() => {
    const fetchTherapists = async () => {
      try {
        const res = await axios.get(GET_ALL_THERAPISTS_API);
        if (res.data?.isSuccess) {
          setTherapists(res.data.result);
          setFilteredTherapists(res.data.result);
        } else {
          setError(res.data?.message || "Failed to fetch therapists.");
          console.error("Error:", res.data?.message);
        }
      } catch (err) {
        setError("An error occurred while fetching therapists.");
        console.error("Error fetching therapists:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchTherapists();
  }, []);

  useEffect(() => {
    let updatedTherapists = [...therapists];

    if (searchTerm) {
      updatedTherapists = updatedTherapists.filter((therapist) =>
        therapist.fullName.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    if (sortField === "name") {
      updatedTherapists.sort((a, b) => a.fullName.localeCompare(b.fullName));
    } else if (sortField === "experience") {
      updatedTherapists.sort((a, b) => b.experience - a.experience);
    }

    setFilteredTherapists(updatedTherapists);
  }, [searchTerm, sortField, therapists]);

  console.log("Therapists state:", therapists);

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>{error}</p>;
  }

  return (
    <>
      <Header />
      <div className={styles.container}>
        <h1 className={styles.title}>Therapist List</h1>
        <div className={styles.controls}>
          <input
            type="text"
            placeholder="Search by name"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className={styles.searchInput}
          />
          <select
            value={sortField}
            onChange={(e) => setSortField(e.target.value)}
            className={styles.sortDropdown}
          >
            <option value="">Sort By</option>
            <option value="name">Name</option>
            <option value="experience">Experience</option>
          </select>
        </div>
        <div className={styles.grid}>
          {filteredTherapists.length === 0 ? (
            <p>No therapists available</p>
          ) : (
            filteredTherapists.map((therapist) => (
              <TherapistCard
                key={therapist.skinTherapistId}
                skinTherapistId={therapist.skinTherapistId}
                fullName={therapist.fullName}
                email={therapist.email}
                phoneNumber={therapist.phoneNumber}
                imageUrl={therapist.imageUrl}
                experience={therapist.experience}
              />
            ))
          )}
        </div>
      </div>
      <Footer />
    </>
  );
};

export default TherapistList;
