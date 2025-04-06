import React, { useEffect, useState } from "react";
import axios from "axios";
import styles from "./TherapistList.module.css";
import { GET_ALL_THERAPISTS_API } from "../../config/apiConfig";
import TherapistCard from "../TherapistCard/TherapistCard";
import Header from "../Common/Header";
import Footer from "../Footer/Footer";

const TherapistList = () => {
  const [therapists, setTherapists] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchTherapists = async () => {
      try {
        const res = await axios.get(GET_ALL_THERAPISTS_API);
        if (res.data?.isSuccess) {
          setTherapists(res.data.result);
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
        <div className={styles.grid}>
          {therapists.length === 0 ? (
            <p>No therapists available</p>
          ) : (
            therapists.map((therapist) => (
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
