import React from "react";
import styles from "./ServiceSort.module.css";

const FilterDropdown = ({ filter, handleFilterChange }) => {
  return (
    <div>
      <label>By </label>
      <select className={styles.by} value={filter} onChange={handleFilterChange}>
        <option>Most Popular</option>
        <option>Price: Low to High</option>
        <option>Price: High to Low</option>
        <option>Name: A-Z</option>
        <option>Name: Z-A</option>
        <option>Oldest</option>
        <option>Newest</option>
      </select>
    </div>
  );
};

export default FilterDropdown;
