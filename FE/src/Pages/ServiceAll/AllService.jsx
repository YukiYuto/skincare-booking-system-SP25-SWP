import React, { useEffect, useState, useCallback } from "react";
import Header from "../../Components/Common/Header.jsx";
import Hero from "../../Components/Hero/Hero";
import ServiceSort from "../../Components/ServiceSort/ServiceSort.jsx";
import ServiceList from "../../Components/ServiceList/ServiceList.jsx";
import styles from "./AllService.module.css";
import Loading from "../../Components/Common/Loading/Loading.jsx";
import {
  GET_ALL_SERVICES_API,
  GET_ALL_SERVICE_TYPES_API,
} from "../../config/apiConfig";

const AllService = () => {
  const [services, setServices] = useState([]);
  const [serviceTypes, setServiceTypes] = useState([]);
  const [filter, setFilter] = useState("Most Popular");
  const [selectedTypes, setSelectedTypes] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchData = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      const [servicesResponse, serviceTypesResponse] = await Promise.all([
        fetch(GET_ALL_SERVICES_API).then((res) => {
          if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`);
          return res.json();
        }),
        fetch(GET_ALL_SERVICE_TYPES_API).then((res) => {
          if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`);
          return res.json();
        }),
      ]);

      console.log("Services Data:", servicesResponse);
      console.log("Service Types Data:", serviceTypesResponse);

      setServices(servicesResponse.result?.services || []);
      setServiceTypes(serviceTypesResponse.result || []);
    } catch (error) {
      console.error("Error fetching data:", error);
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchData();
    setTimeout(fetchData, 500);
  }, [fetchData]);

  if (loading) return <Loading />;
  if (error) return <p>Error loading services.</p>;

  const handleFilterChange = (event) => setFilter(event.target.value);

  const toggleType = (typeID) => {
    setSelectedTypes((prevSelected) => {
      const typesArray = prevSelected.split(",").filter(Boolean);
      return typesArray.includes(typeID)
        ? typesArray.filter((id) => id !== typeID).join(",")
        : [...typesArray, typeID].join(",");
    });
  };

  const getFilteredServices = () => {
    let filtered = [...services];

    if (selectedTypes) {
      const selectedArray = selectedTypes.split(",");
      filtered = filtered.filter((service) =>
        selectedArray.includes(service.serviceTypeId.toString())
      );
    }

    const sortingOptions = {
      "Most Popular": (a, b) => b.popularity - a.popularity,
      "Price: Low to High": (a, b) => a.price - b.price,
      "Price: High to Low": (a, b) => b.price - a.price,
      "Name: A-Z": (a, b) => a.serviceName.localeCompare(b.serviceName),
      "Name: Z-A": (a, b) => b.serviceName.localeCompare(a.serviceName),
      Oldest: (a, b) => new Date(a.createdAt) - new Date(b.createdAt),
      Newest: (a, b) => new Date(b.createdAt) - new Date(a.createdAt),
    };

    return filtered.sort(sortingOptions[filter] || (() => 0));
  };

  return (
    <div>
      <Header />
      <Hero />
      <div className={styles.container}>
        <div className={styles.header}>
          <h1>Discover Our Services</h1>
          <ServiceSort
            filter={filter}
            handleFilterChange={handleFilterChange}
          />
        </div>
        <div className={styles.service}>
          <div className={styles.filter}>
            <label>Filter by Type</label>
            <ul>
              {Array.isArray(serviceTypes) &&
                serviceTypes.map((type) => (
                  <li
                    key={type.serviceTypeId}
                    onClick={() => toggleType(type.serviceTypeId)}
                  >
                    {type.serviceTypeName}
                  </li>
                ))}
            </ul>
          </div>
          <ServiceList
            services={getFilteredServices()}
            serviceTypes={serviceTypes}
          />
        </div>
      </div>
    </div>
  );
};

export default AllService;
