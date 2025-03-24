import { useEffect, useState, useMemo } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import { Card, Pagination, Row, Col } from "antd";
import styles from "./BlogForCus.module.css";
import slugify from "slugify";
import { GET_BLOG, GET_BLOG_CATEGORY } from "../../config/apiConfig";
import Header from "../Common/Header";

const { Meta } = Card;

const BlogForCus = () => {
  const [blogs, setBlogs] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);

  const pageSize = 8;
  const navigate = useNavigate();
  const { categoryName } = useParams();

  const fetchData = async () => {
    setLoading(true);
    setError(null);
    try {
      const [categoryResponse, blogResponse] = await Promise.all([
        axios.get(GET_BLOG_CATEGORY),
        axios.get(GET_BLOG),
      ]);
      setCategories(categoryResponse.data.result || []);
      setBlogs(blogResponse.data.result || []);
    } catch (err) {
      setError("Failed to load data. Please try again.");
    } finally {
      setLoading(false);
    }
  };
  
  useEffect(() => {
    fetchData();
  }, []);

  const formattedCategoryName = categoryName ? categoryName.replace(/-/g, " ") : null;
  const selectedCategory = categories.find((cat) => cat.name === formattedCategoryName)?.blogCategoryId || null;

  const filteredBlogs = useMemo(() => {
    return selectedCategory
      ? blogs.filter((blog) => blog.blogCategoryId === selectedCategory)
      : blogs;
  }, [selectedCategory, blogs]);

  const mainCategories = categories.slice(0, 8);
  const moreCategories = categories.slice(8);

  const handleCategoryClick = (category) => {
    if (category) {
      const formattedName = category.name.replace(/\s+/g, "-"); 
      navigate(`/blogs/${formattedName}`);
    } else {
      navigate("/blogs");
    }
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const sortedBlogs = useMemo(() => {
    return [...filteredBlogs].sort((a, b) => new Date(b.createdTime) - new Date(a.createdTime));
  }, [filteredBlogs]);
  

  const paginatedBlogs = useMemo(() => {
    const startIndex = (currentPage - 1) * pageSize;
    return sortedBlogs.slice(startIndex, startIndex + pageSize);
  }, [sortedBlogs, currentPage, pageSize]);  

  return (
    <>
    <Header />
    <div className={styles.container}>
      <div className={styles.categoryBar}>
        <button
          className={!categoryName ? styles.active : ""}
          onClick={() => handleCategoryClick(null)}
        >
          View All
        </button>
        {mainCategories.map((category) => (
          <button
            key={category.blogCategoryId}
            className={formattedCategoryName === category.name ? styles.active : ""}
            onClick={() => handleCategoryClick(category)}
          >
            {category.name}
          </button>
        ))}
        {moreCategories.length > 0 && (
          <div className={styles.dropdownContainer}>
            <button className={styles.menuItem}>More...</button>
            <div className={styles.dropdownMenu}>
              {moreCategories.map((category) => (
                <button
                  key={category.blogCategoryId}
                  className={styles.dropdownItem}
                  onClick={() => handleCategoryClick(category)}
                >
                  {category.name}
                </button>
              ))}
            </div>
          </div>
        )}
      </div>

      <Row gutter={[16, 16]}>
        {loading ? (
          <p>Loading...</p>
        ) : error ? (
          <p className={styles.error}>{error}</p>
        ) : paginatedBlogs.length === 0 ? (
          <p>No blogs available.</p>
        ) : (
          paginatedBlogs.map((blog) => (
            <Col key={blog.id} xs={24} sm={12} md={6}>
              <Card
                hoverable
                cover={<img alt={blog.title} src={blog.imageUrl} className={styles.blogImage} />}
                onClick={() => navigate(`/blogs-detail/${slugify(blog.title, { lower: true, strict: true })}`)}
                className={styles.blogCard}
              >
                <Meta
                title={
                  <div className="flex justify-between items-center">
                    <span>{blog.title}</span>
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-5 h-5 text-gray-500">
                      <path strokeLinecap="round" strokeLinejoin="round" d="m4.5 19.5 15-15m0 0H8.25m11.25 0v11.25" />
                    </svg>
                  </div>
                } 
                description={
                  <>
                    <p className={styles.date}>
                      {blog.status.includes("PUBLISHED") ? (
                        <>✍ {blog.createdBy} | 🕒 {new Date(blog.createdTime).toLocaleString("vi-VN", {
                          year: "numeric",
                          month: "2-digit",
                          day: "2-digit",
                          hour: "2-digit",
                          minute: "2-digit",
                          second: "2-digit"
                        })}</>
                      ) : blog.status.includes("MODIFIED") ? (
                        <>✍ {blog.updatedBy} | 🕒 {new Date(blog.updatedTime).toLocaleString("vi-VN", {
                          year: "numeric",
                          month: "2-digit",
                          day: "2-digit",
                          hour: "2-digit",
                          minute: "2-digit",
                          second: "2-digit"
                        })}</>
                      ) : (
                        <>⚠ Unknown status</>
                      )}
                    </p>
                    <p>{blog.content.substring(0, 40)}...</p>
                  <div className={styles.tags}>             
                    <span className={styles.tag}>{blog.tags}</span>
                  </div>

                  </>
                } />
              </Card>
            </Col>
          ))
        )}
      </Row>
      <Pagination
        current={currentPage}
        pageSize={pageSize}
        total={filteredBlogs.length}
        onChange={handlePageChange}
        className={styles.pagination}
      />
    </div>
    </>
  );
};

export default BlogForCus;