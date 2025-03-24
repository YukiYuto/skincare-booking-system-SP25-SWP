import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import slugify from "slugify";
import { GET_BLOG } from "../../../config/apiConfig";
import styles from "./BlogForCusList.module.css";

const BlogForCusList = () => {
  const { title } = useParams();
  const navigate = useNavigate();
  const [blog, setBlog] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchBlogDetail = async () => {
      try {
        const response = await axios.get(GET_BLOG);
        const blogs = response.data.result || [];

        const foundBlog = blogs.find(
          (blog) => slugify(blog.title, { lower: true, strict: true }) === title
        );

        if (foundBlog) {
          setBlog(foundBlog);
        } else {
          setError("No found blogs!");
        }
      } catch (err) {
        setError("Faild to load data!");
      } finally {
        setLoading(false);
      }
    };

    fetchBlogDetail();
  }, [title]);

  if (loading) return <p>Loading...</p>;
  if (error) return <p className={styles.error}>{error}</p>;
  if (!blog) return <p>No blogs were found!</p>;

  return (
    <>
    <button onClick={() => navigate(-1)} className={styles.backButton}>
          ⬅ Back
    </button>
    <div className={styles.container}>
      <h1 className={styles.title}>{blog.title}</h1>

      <div className={styles.meta}>
      {blog.status.includes("PUBLISHED") ? (
                  <>
                    ✍ {blog.createdBy} | 🕒 {new Date(blog.createdTime).toLocaleString("vi-VN", {
                      year: "numeric",
                      month: "2-digit",
                      day: "2-digit",
                      hour: "2-digit",
                      minute: "2-digit",
                      second: "2-digit"
                    })}
                  </>
                ) : blog.status.includes("MODIFIED") ? (
                  <>
                    ✍ {blog.updatedBy} | 🕒 {new Date(blog.updatedTime).toLocaleString("vi-VN", {
                      year: "numeric",
                      month: "2-digit",
                      day: "2-digit",
                      hour: "2-digit",
                      minute: "2-digit",
                      second: "2-digit"
                    })}
                  </>
                ) : (
                  <>
                    ⚠ Unknown status
                  </>
                )}
      </div>
      <div className={styles.imageContainer}>
      <img src={blog.imageUrl} alt={blog.title} className={styles.image} />
      </div>
      <div className={styles.content}>{blog.content}</div>
    </div>
    </>
  );
};

export default BlogForCusList;
