import { useEffect, useState, useCallback } from "react";
import Header from "../../Components/Common/Header.jsx";
import Hero from "../../Components/Hero/Hero";
import ServiceList from "../../Components/ServiceList/ServiceList.jsx";
import styles from "./AllService.module.css";
import Loading from "../../Components/Common/Loading/Loading.jsx";
import {
  GET_ALL_SERVICES_API,
  GET_ALL_SERVICE_TYPES_API,
} from "../../config/apiConfig";
import Footer from "../../Components/Footer/Footer.jsx";

const AllService = () => {
  const [services, setServices] = useState([]);
  const [serviceTypes, setServiceTypes] = useState([]);
  const [sortBy, setSortBy] = useState(""); // Backend sorting parameter
  const [selectedTypes, setSelectedTypes] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [searchQuery, setSearchQuery] = useState("");
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 12,
    totalPages: 1,
    totalItems: 0,
  });
  const [filterQuery, setFilterQuery] = useState("");
  const [filterOn, setFilterOn] = useState("");

  // Sort options for the dropdown
  const sortOptions = [
    { value: "", label: "Default" },
    { value: "price", label: "Price: Low to High" },
    { value: "price_desc", label: "Price: High to Low" },
    { value: "serviceName", label: "Name: A-Z" },
    { value: "serviceName_desc", label: "Name: Z-A" },
  ];

  // Page size options
  const pageSizeOptions = [6, 12, 24, 48];

  const fetchData = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      // Build query parameters for services
      const params = new URLSearchParams();
      params.append("pageNumber", pagination.pageNumber);
      params.append("pageSize", pagination.pageSize);

      if (sortBy) {
        params.append("sortBy", sortBy);
      }

      if (filterOn) {
        params.append("filterOn", filterOn);
      }

      if (filterQuery) {
        params.append("filterQuery", filterQuery);
      }

      // If service types are selected, add them as filter
      if (selectedTypes) {
        // Override filterOn and filterQuery if types are selected
        params.set("filterOn", "serviceTypeIds");
        params.set("filterQuery", selectedTypes);
      }

      const [servicesResponse, serviceTypesResponse] = await Promise.all([
        fetch(`${GET_ALL_SERVICES_API}?${params}`).then((res) => {
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

      if (servicesResponse.result) {
        setServices(servicesResponse.result.services || []);
        setPagination((prev) => ({
          ...prev,
          totalPages: servicesResponse.result.totalPages || 1,
          totalItems: servicesResponse.result.totalItems || 0,
        }));
      }

      setServiceTypes(serviceTypesResponse.result || []);
    } catch (error) {
      console.error("Error fetching data:", error);
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }, [
    pagination.pageNumber,
    pagination.pageSize,
    sortBy,
    filterOn,
    filterQuery,
    selectedTypes,
  ]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  // Handle search submission
  const handleSearchSubmit = (e) => {
    e.preventDefault();
    setFilterOn("serviceName");
    setFilterQuery(searchQuery);
    // Reset to first page when searching
    setPagination((prev) => ({
      ...prev,
      pageNumber: 1,
    }));
  };

  // Handle search input changes
  const handleSearchChange = (e) => {
    setSearchQuery(e.target.value);
  };

  // Handle sort change
  const handleSortChange = (e) => {
    setSortBy(e.target.value);
    // Reset to first page when sort changes
    setPagination((prev) => ({
      ...prev,
      pageNumber: 1,
    }));
  };

  // Handle page size change
  const handlePageSizeChange = (e) => {
    setPagination((prev) => ({
      ...prev,
      pageSize: Number(e.target.value),
      pageNumber: 1, // Reset to first page when page size changes
    }));
  };

  // Handle page change
  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= pagination.totalPages) {
      setPagination((prev) => ({
        ...prev,
        pageNumber: newPage,
      }));
      // Scroll to top when changing pages
      window.scrollTo(0, 0);
    }
  };

  const toggleType = (typeID) => {
    setSelectedTypes((prevSelected) => {
      const typesArray = prevSelected.split(",").filter(Boolean);
      return typesArray.includes(typeID)
        ? typesArray.filter((id) => id !== typeID).join(",")
        : [...typesArray, typeID].join(",");
    });
    // Reset to first page when filters change
    setPagination((prev) => ({
      ...prev,
      pageNumber: 1,
    }));
  };

  const isTypeSelected = (typeID) => {
    return selectedTypes.split(",").includes(typeID);
  };

  // Find the current sort option label
  const getCurrentSortLabel = () => {
    const option = sortOptions.find((opt) => opt.value === sortBy);
    return option ? option.label : "Default";
  };

  // Generate pagination buttons
  const renderPaginationButtons = () => {
    const buttons = [];
    const { pageNumber, totalPages } = pagination;

    // Calculate range of buttons to show
    const maxButtons = 5;
    let startPage = Math.max(1, pageNumber - Math.floor(maxButtons / 2));
    let endPage = Math.min(totalPages, startPage + maxButtons - 1);

    if (endPage - startPage + 1 < maxButtons) {
      startPage = Math.max(1, endPage - maxButtons + 1);
    }

    // Previous button
    buttons.push(
      <button
        key="prev"
        onClick={() => handlePageChange(pageNumber - 1)}
        disabled={pageNumber === 1}
        className={styles.pageButton}
      >
        &laquo;
      </button>
    );

    // First page
    if (startPage > 1) {
      buttons.push(
        <button
          key="1"
          onClick={() => handlePageChange(1)}
          className={
            1 === pageNumber ? styles.activePageButton : styles.pageButton
          }
        >
          1
        </button>
      );

      if (startPage > 2) {
        buttons.push(
          <span key="ellipsis1" className={styles.ellipsis}>
            ...
          </span>
        );
      }
    }

    // Page numbers
    for (let i = startPage; i <= endPage; i++) {
      buttons.push(
        <button
          key={i}
          onClick={() => handlePageChange(i)}
          className={
            i === pageNumber ? styles.activePageButton : styles.pageButton
          }
        >
          {i}
        </button>
      );
    }

    // Last page
    if (endPage < totalPages) {
      if (endPage < totalPages - 1) {
        buttons.push(
          <span key="ellipsis2" className={styles.ellipsis}>
            ...
          </span>
        );
      }

      buttons.push(
        <button
          key={totalPages}
          onClick={() => handlePageChange(totalPages)}
          className={
            totalPages === pageNumber
              ? styles.activePageButton
              : styles.pageButton
          }
        >
          {totalPages}
        </button>
      );
    }

    // Next button
    buttons.push(
      <button
        key="next"
        onClick={() => handlePageChange(pageNumber + 1)}
        disabled={pageNumber === totalPages}
        className={styles.pageButton}
      >
        &raquo;
      </button>
    );

    return buttons;
  };

  if (loading && services.length === 0) return <Loading />;
  if (error)
    return (
      <p className={styles.errorMessage}>Error loading services: {error}</p>
    );

  return (
    <div>
      <Header />
      <Hero />
      <div className={styles.container}>
        <div className={styles.header}>
          <h1>Discover Our Services</h1>

          <div className={styles.controlsRow}>
            {/* Search Bar */}
            <form onSubmit={handleSearchSubmit} className={styles.searchForm}>
              <input
                type="text"
                placeholder="Search services..."
                value={searchQuery}
                onChange={handleSearchChange}
                className={styles.searchInput}
              />
              <button type="submit" className={styles.searchButton}></button>
            </form>

            {/* Sort Dropdown */}
            <div className={styles.sortContainer}>
              <select
                id="sort-select"
                value={sortBy}
                onChange={handleSortChange}
                className={styles.sortSelect}
              >
                {sortOptions.map((option) => (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                ))}
              </select>
            </div>

            {/* Page Size Selector */}
            <div className={styles.pageSizeContainer}>
              <select
                id="page-size-select"
                value={pagination.pageSize}
                onChange={handlePageSizeChange}
                className={styles.pageSizeSelect}
              >
                {pageSizeOptions.map((size) => (
                  <option key={size} value={size}>
                    {size}
                  </option>
                ))}
              </select>
            </div>
          </div>

          {/* Active Filters Display */}
          {(sortBy || filterQuery) && (
            <div className={styles.activeFilters}>
              <h3>Active Filters:</h3>
              <div className={styles.filterTags}>
                {sortBy && (
                  <span className={styles.filterTag}>
                    Sort: {getCurrentSortLabel()}
                    <button
                      onClick={() => {
                        setSortBy("");
                        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
                      }}
                      className={styles.removeFilter}
                    >
                      ×
                    </button>
                  </span>
                )}

                {filterQuery && (
                  <span className={styles.filterTag}>
                    Search: {filterQuery}
                    <button
                      onClick={() => {
                        setFilterQuery("");
                        setFilterOn("");
                        setSearchQuery("");
                        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
                      }}
                      className={styles.removeFilter}
                    >
                      ×
                    </button>
                  </span>
                )}
              </div>
            </div>
          )}
        </div>

        <div className={styles.service}>
          <div className={styles.filter}>
            <label>Filter by Type</label>
            <ul>
              {Array.isArray(serviceTypes) &&
                serviceTypes.map((type) => (
                  <li
                    key={type.serviceTypeId}
                    className={
                      isTypeSelected(type.serviceTypeId) ? styles.selected : ""
                    }
                    onClick={() => toggleType(type.serviceTypeId)}
                  >
                    {type.serviceTypeName}
                  </li>
                ))}
            </ul>
          </div>

          <div className={styles.serviceListContainer}>
            {loading && (
              <div className={styles.loadingOverlay}>
                <Loading />
              </div>
            )}

            {services.length === 0 && !loading ? (
              <div className={styles.noResults}>
                <h3>No services found</h3>
                <p>Try adjusting your search or filters</p>
              </div>
            ) : (
              <>
                <ServiceList services={services} serviceTypes={serviceTypes} />

                {/* Pagination Controls */}
                {pagination.totalPages > 1 && (
                  <div className={styles.paginationContainer}>
                    <div className={styles.paginationButtons}>
                      {renderPaginationButtons()}
                    </div>
                    <div className={styles.paginationInfo}>
                      Page {pagination.pageNumber} of {pagination.totalPages}
                    </div>
                  </div>
                )}

                <div className={styles.resultsInfo}>
                  Showing{" "}
                  {(pagination.pageNumber - 1) * pagination.pageSize + 1}-
                  {Math.min(
                    pagination.pageNumber * pagination.pageSize,
                    pagination.totalItems
                  )}{" "}
                  of {pagination.totalItems} services
                </div>
              </>
            )}
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default AllService;
