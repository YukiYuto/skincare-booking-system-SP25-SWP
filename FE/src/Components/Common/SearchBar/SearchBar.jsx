/* eslint-disable no-unused-vars */
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const SearchBar = () => {
  const [searchQuery, setSearchQuery] = useState("");
  const navigate = useNavigate();

  return (
    <div className="flex items-stretch flex-col">
      <form className="self-end ml-0 mr-3 border-b-1 border-gray-600">
        <input
          className="py-1 px-2"
          type="text"
          placeholder="Search services..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />
        <button type="submit"></button>
      </form>
    </div>
  );
};

export default SearchBar;
