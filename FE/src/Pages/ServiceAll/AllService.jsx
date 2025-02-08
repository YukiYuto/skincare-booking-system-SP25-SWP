import React, { useEffect, useState } from "react";
import Hero from "../../Components/Hero/Hero";
import ServiceCard from "../../Components/ServiceCard/ServiceCard";
import styles from "./AllService.module.css";

const AllService = () => {
  const [services, setServices] = useState([]);
  const [filter, setFilter] = useState("Most Popular");

  useEffect(() => {
    fetch("https://672741d4302d03037e702957.mockapi.io/Service")
      .then((response) => response.json())
      .then((data) => setServices(data));
  }, []);

  const handleFilterChange = (event) => {
    setFilter(event.target.value);
  };

  const filteredServices = () => {
    switch (filter) {
      case "Most Popular":
        return services.sort((a, b) => b.Popularity - a.Popularity);
      case "Price: Low to High":
        return services.sort((a, b) => a.Price - b.Price);
      case "Price: High to Low":
        return services.sort((a, b) => b.Price - a.Price);
      case "Name: A-Z":
        return services.sort((a, b) =>
          a.ServiceName.localeCompare(b.ServiceName)
        );
      case "Name: Z-A":
        return services.sort((a, b) =>
          b.ServiceName.localeCompare(a.ServiceName)
        );
      case "Oldest":
        return services.sort((a, b) => a.createdAt - b.createdAt);
      case "Newest":
        return services.sort((a, b) => b.createdAt - a.createdAt);
      default:
        return services;
    }
  };

  return (
    <>
      <Hero />
      <div className={styles.container}>
        <div className={styles.header}>
          <h1>Discover Our Services</h1>
          <div>
            <label>By </label>
            <span>
              <select
                className={styles.by}
                value={filter}
                onChange={handleFilterChange}
              >
                <option>Most Popular</option>
                <option>Price: Low to High</option>
                <option>Price: High to Low</option>
                <option>Name: A-Z</option>
                <option>Name: Z-A</option>
                <option>Oldest</option>
                <option>Newest</option>
              </select>
            </span>
          </div>
        </div>
        <div className={styles.service}>
          <div className={styles.filter}></div>
          <div className={styles.itemList}>
            {filteredServices().map((service) => (
              <ServiceCard key={service.ID} service={service} />
            ))}
          </div>
        </div>
      </div>
    </>
  );
};

export default AllService;
