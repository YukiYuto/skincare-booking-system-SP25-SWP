/* eslint-disable no-unused-vars */
import React from "react";
import { Link } from "react-router-dom";
import "../../styles/index.css"; // Import custom CSS file
import Brand from "../Brand/Brand";

const Header = () => {
  return (
    <>
      <div className="sticky-header">
        <div className="bg-white nav-header-wrapper header-wrapper--border-bottom">
          <div className="nav-header nav-page-width">
            <div className="nav-header__heading">
              <Link to="/" className="nav-header__heading-link">
                <Brand override={{ value: "text-4xl" }} />
              </Link>
            </div>
            <nav className="nav-header__inline-menu">
              <ul className="list-menu list-menu--inline">
                <li>
                  <Link
                    to="/services"
                    className=" header__menu-item list-menu-item list-menu-link"
                  >
                    Services
                  </Link>
                </li>
                <li>
                  <Link
                    to="/blogs"
                    className="header__menu-item list-menu-item list-menu-link"
                  >
                    Blogs
                  </Link>
                </li>
                <li>
                  <Link
                    to="/about"
                    className="header__menu-item list-menu-item list-menu-link"
                  >
                    About
                  </Link>
                </li>
                <li>
                  <Link
                    to="/skin-test"
                    className="header__menu-item list-menu-item list-menu-link"
                  >
                    Skin Mapper
                  </Link>
                </li>
                <li>
                  <Link
                    to="/contact"
                    className="header__menu-item list-menu-item list-menu-link"
                  >
                    Promotion
                  </Link>
                </li>
              </ul>
            </nav>
          </div>
        </div>
      </div>
    </>
  );
};

export default Header;
