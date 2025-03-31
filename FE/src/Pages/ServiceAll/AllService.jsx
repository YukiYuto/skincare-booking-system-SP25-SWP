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
  const [state, setState] = useState({
    services: [],
    serviceTypes: [],
    sortBy: "",
    selectedType: "",
    loading: false,
    error: null,
    searchQuery: "",
    filterQuery: "",
    filterOn: "",
    pagination: {
      pageNumber: 1,
      pageSize: 12,
      totalPages: 1,
      totalItems: 0,
    }
  });

  const { 
    services, serviceTypes, sortBy, selectedType, loading, error, 
    searchQuery, filterQuery, filterOn, pagination 
  } = state;

  const sortOptions = [
    { value: "", label: "Default" },
    { value: "price", label: "Price: Low to High" },
    { value: "price_desc", label: "Price: High to Low" },
    { value: "serviceName", label: "Name: A-Z" },
    { value: "serviceName_desc", label: "Name: Z-A" },
  ];

  const pageSizeOptions = [6, 12, 24, 48];

  const updateState = (newState) => setState(prev => ({ ...prev, ...newState }));
  const updatePagination = (newPaginationState) => updateState({ 
    pagination: { ...pagination, ...newPaginationState } 
  });

  const fetchData = useCallback(async () => {
    updateState({ loading: true, error: null });

    try {
      const params = new URLSearchParams();
      params.append("pageNumber", pagination.pageNumber);
      params.append("pageSize", pagination.pageSize);
      
      if (sortBy) params.append("sortBy", sortBy);
      if (filterOn) params.append("filterOn", filterOn);
      if (filterQuery) params.append("filterQuery", filterQuery);
      
      if (selectedType) {
        params.set("filterOn", "service_type_id");
        params.set("filterQuery", selectedType);
      }

      const [servicesResponse, serviceTypesResponse] = await Promise.all([
        fetch(`${GET_ALL_SERVICES_API}?${params}`).then(res => {
          if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`);
          return res.json();
        }),
        fetch(GET_ALL_SERVICE_TYPES_API).then(res => {
          if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`);
          return res.json();
        }),
      ]);

      updateState({
        services: servicesResponse.result?.services || [],
        serviceTypes: serviceTypesResponse.result || [],
        pagination: {
          ...pagination,
          totalPages: servicesResponse.result?.totalPages || 1,
          totalItems: servicesResponse.result?.totalItems || 0,
        }
      });
    } catch (error) {
      console.error("Error fetching data:", error);
      updateState({ error: error.message });
    } finally {
      updateState({ loading: false });
    }
  }, [pagination.pageNumber, pagination.pageSize, sortBy, filterOn, filterQuery, selectedType]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const handleSearchSubmit = (e) => {
    e.preventDefault();
    updateState({
      filterOn: "serviceName",
      filterQuery: searchQuery,
      selectedType: "",
      pagination: { ...pagination, pageNumber: 1 }
    });
  };

  const handleSearchChange = (e) => updateState({ searchQuery: e.target.value });
  
  const handleSortChange = (e) => updateState({ 
    sortBy: e.target.value, 
    pagination: { ...pagination, pageNumber: 1 } 
  });
  
  const handlePageSizeChange = (e) => updatePagination({ 
    pageSize: Number(e.target.value), 
    pageNumber: 1 
  });

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= pagination.totalPages) {
      updatePagination({ pageNumber: newPage });
      window.scrollTo(0, 0);
    }
  };

  const handleTypeSelect = (typeID) => updateState({
    selectedType: selectedType === typeID ? "" : typeID,
    filterOn: "",
    filterQuery: "",
    searchQuery: "",
    pagination: { ...pagination, pageNumber: 1 }
  });

  const getCurrentSortLabel = () => {
    const option = sortOptions.find((opt) => opt.value === sortBy);
    return option ? option.label : "Default";
  };

  const getSelectedTypeName = () => {
    if (!selectedType) return null;
    const type = serviceTypes.find(type => type.serviceTypeId === selectedType);
    return type ? type.serviceTypeName : null;
  };

  const renderPaginationButtons = () => {
    const buttons = [];
    const { pageNumber, totalPages } = pagination;
    const maxButtons = 5;
    let startPage = Math.max(1, pageNumber - Math.floor(maxButtons / 2));
    let endPage = Math.min(totalPages, startPage + maxButtons - 1);

    if (endPage - startPage + 1 < maxButtons) {
      startPage = Math.max(1, endPage - maxButtons + 1);
    }

    buttons.push(
      <button key="prev" onClick={() => handlePageChange(pageNumber - 1)}
        disabled={pageNumber === 1} className={styles.pageButton}>
        &laquo;
      </button>
    );

    if (startPage > 1) {
      buttons.push(
        <button key="1" onClick={() => handlePageChange(1)}
          className={1 === pageNumber ? styles.activePageButton : styles.pageButton}>
          1
        </button>
      );
      if (startPage > 2) {
        buttons.push(<span key="ellipsis1" className={styles.ellipsis}>...</span>);
      }
    }

    for (let i = startPage; i <= endPage; i++) {
      buttons.push(
        <button key={i} onClick={() => handlePageChange(i)}
          className={i === pageNumber ? styles.activePageButton : styles.pageButton}>
          {i}
        </button>
      );
    }

    if (endPage < totalPages) {
      if (endPage < totalPages - 1) {
        buttons.push(<span key="ellipsis2" className={styles.ellipsis}>...</span>);
      }
      buttons.push(
        <button key={totalPages} onClick={() => handlePageChange(totalPages)}
          className={totalPages === pageNumber ? styles.activePageButton : styles.pageButton}>
          {totalPages}
        </button>
      );
    }

    buttons.push(
      <button key="next" onClick={() => handlePageChange(pageNumber + 1)}
        disabled={pageNumber === totalPages} className={styles.pageButton}>
        &raquo;
      </button>
    );

    return buttons;
  };

  if (loading && services.length === 0) return <Loading />;
  if (error) return <p className={styles.errorMessage}>Error loading services: {error}</p>;

  return (
    <div>
      <Header />
      <div className={styles.heroContainer}>
        <Hero />
        <div className={styles.container}>
          <div className={styles.header}>
            <h1>Discover Our Services</h1>

            <div className={styles.controlsRow}>
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

              <div className={styles.sortContainer}>
                <select id="sort-select" value={sortBy} onChange={handleSortChange} className={styles.sortSelect}>
                  {sortOptions.map(option => (
                    <option key={option.value} value={option.value}>{option.label}</option>
                  ))}
                </select>
              </div>

              <div className={styles.pageSizeContainer}>
                <select id="page-size-select" value={pagination.pageSize} onChange={handlePageSizeChange} className={styles.pageSizeSelect}>
                  {pageSizeOptions.map(size => (
                    <option key={size} value={size}>{size}</option>
                  ))}
                </select>
              </div>
            </div>

            {(sortBy || filterQuery || selectedType) && (
              <div className={styles.activeFilters}>
                <h3>Active Filters:</h3>
                <div className={styles.filterTags}>
                  {sortBy && (
                    <span className={styles.filterTag}>
                      Sort: {getCurrentSortLabel()}
                      <button onClick={() => updateState({ sortBy: "", pagination: { ...pagination, pageNumber: 1 } })} className={styles.removeFilter}>×</button>
                    </span>
                  )}

                  {filterQuery && (
                    <span className={styles.filterTag}>
                      Search: {filterQuery}
                      <button onClick={() => updateState({ 
                        filterQuery: "", 
                        filterOn: "", 
                        searchQuery: "", 
                        pagination: { ...pagination, pageNumber: 1 } 
                      })} className={styles.removeFilter}>×</button>
                    </span>
                  )}

                  {selectedType && (
                    <span className={styles.filterTag}>
                      Type: {getSelectedTypeName()}
                      <button onClick={() => updateState({ 
                        selectedType: "", 
                        pagination: { ...pagination, pageNumber: 1 } 
                      })} className={styles.removeFilter}>×</button>
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
                {Array.isArray(serviceTypes) && serviceTypes.map(type => (
                  <li
                    key={type.serviceTypeId}
                    className={selectedType === type.serviceTypeId ? styles.selected : ""}
                    onClick={() => handleTypeSelect(type.serviceTypeId)}
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
                    Showing {(pagination.pageNumber - 1) * pagination.pageSize + 1}-
                    {Math.min(pagination.pageNumber * pagination.pageSize, pagination.totalItems)} of {pagination.totalItems} services
                  </div>
                </>
              )}
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default AllService;