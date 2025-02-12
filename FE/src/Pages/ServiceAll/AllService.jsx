import React, { useEffect, useState } from "react";
import Header from "../../Components/Common/Header.jsx";
import Hero from "../../Components/Hero/Hero";
import ServiceSort from "../../Components/ServiceSort/ServiceSort.jsx";
import ServiceList from "../../Components/ServiceList/ServiceList.jsx";
import styles from "./AllService.module.css";
import Loading from "../../Components/Common/Loading/Loading.jsx";

const AllService = () => {
  const [services, setServices] = useState([]);
  const [serviceTypes, setServiceTypes] = useState([]);
  const [filter, setFilter] = useState("Most Popular");
  const [selectedTypes, setSelectedTypes] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchServices = async () => {
      try {
        const [servicesData, serviceTypesData] = await Promise.all([
          fetch("https://672741d4302d03037e702957.mockapi.io/Service").then(
            (res) => res.json()
          ),
          fetch("https://672741d4302d03037e702957.mockapi.io/ServiceType").then(
            (res) => res.json()
          ),
        ]);
        setServices(servicesData);
        setServiceTypes(serviceTypesData);
        setLoading(false);
      } catch (error) {
        console.error(error);
        setLoading(false);
      }
    };
    fetchServices();
  }, []);

  if (loading) {
    return <Loading />;
  }

  const handleFilterChange = (event) => setFilter(event.target.value);

  const toggleType = (typeID) => {
    setSelectedTypes((prevSelected) => {
      const typesArray = prevSelected.split(",").filter(Boolean);
      if (typesArray.includes(typeID)) {
        return typesArray.filter((id) => id !== typeID).join(",");
      } else {
        return [...typesArray, typeID].join(",");
      }
    });
  };

  const getFilteredServices = () => {
    let filtered = [...services];

    if (selectedTypes) {
      const selectedArray = selectedTypes.split(",");
      filtered = filtered.filter((service) =>
        selectedArray.includes(service.ServiceTypeID.toString())
      );
    }

    switch (filter) {
      case "Most Popular":
        return filtered.sort((a, b) => b.Popularity - a.Popularity);
      case "Price: Low to High":
        return filtered.sort((a, b) => a.Price - b.Price);
      case "Price: High to Low":
        return filtered.sort((a, b) => b.Price - a.Price);
      case "Name: A-Z":
        return filtered.sort((a, b) =>
          a.ServiceName.localeCompare(b.ServiceName)
        );
      case "Name: Z-A":
        return filtered.sort((a, b) =>
          b.ServiceName.localeCompare(a.ServiceName)
        );
      case "Oldest":
        return filtered.sort((a, b) => a.createdAt - b.createdAt);
      case "Newest":
        return filtered.sort((a, b) => b.createdAt - a.createdAt);
      default:
        return filtered;
    }
  };

  return (
    <>
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
                {serviceTypes.map((type) => (
                  <li
                    key={type.ID}
                    onClick={() => toggleType(type.ID)}
                    className={
                      selectedTypes.includes(type.ID)
                        ? styles.selected
                        : styles.unselected
                    }
                  >
                    {type.Name}
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
    </>
  );
};

export default AllService;
