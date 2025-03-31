import { useEffect, useState, useMemo } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import { Card, Pagination, Row, Col, Form, Button, Input, Select, Modal } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import styles from "./StaffBlog.module.css";
import slugify from "slugify";
import { toast } from "react-toastify";
import { useSelector } from "react-redux";
import { GET_BLOG, GET_BLOG_CATEGORY, POST_BLOG, POST_CUSTOMER_AVATAR_API } from "../../config/apiConfig";

const { Meta } = Card;

const StaffBlog = () => {
  const [blogs, setBlogs] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [imageFile, setImageFile] = useState(null);
  const [adding, setAdding] = useState(false);
  const [form] = Form.useForm();

  const pageSize = 8;
  const navigate = useNavigate();
  const { categoryName } = useParams();

  const { user, accessToken } = useSelector((state) => state.auth);

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
  

  const handleImageUpload = async () => {
    if (!imageFile) {
      toast.error("Please select an image!");
      return null;
    }
  
    setUploading(true);
    const formData = new FormData();
    formData.append("file", imageFile);
  
    try {
      const response = await axios.post(POST_CUSTOMER_AVATAR_API, formData, {
        headers: {
           Authorization: `Bearer ${accessToken}`,
           "Content-Type": "multipart/form-data"
           },
      });
  
      if (response.data && response.data.result) {
        return response.data.result; 
      } else {
        toast.error("Upload failed: No image URL returned!");
        return null;
      }
    } catch (error) {
      toast.error("Image upload failed: " + (error.response?.data?.message || error.message));
      return null;
    } finally {
      setUploading(false);
    }
  };

  const handleAddBlog = async (values) => {
    setAdding(true);
    setUploading(true);
  
    try {
      if (!values.title || values.title.length < 5 || values.title.length > 30) {
        throw new Error("Title must be between 5 and 30 characters.");
      }
      if (!values.content || values.content.length < 20 || values.content.length > 500) {
        throw new Error("Content must be between 20 and 500 characters.");
      }
      let imageUrl = values.imageUrl;

      if (imageFile) {
        const uploadedUrl = await handleImageUpload();
        if (!uploadedUrl) {
          throw new Error("Image upload failed!");
        }
        imageUrl = uploadedUrl;
      }
  
      const headers = {
        "Content-Type": "application/json",
        Authorization: `Bearer ${accessToken}`,
      };
  
      const payload = {
        ...values,
        imageUrl,
        userId: user.id, 
      };
  
      await axios.post(POST_BLOG, payload, { headers });
      
      toast.success("Blog added successfully!");
      setIsModalOpen(false);
      form.resetFields();
      fetchData();
    } catch (error) {
      console.error("Error adding blog:", error.response?.data || error.message);
  
      const errorMessage = error.response?.data?.message || "Failed to add blog. Please try again.";
      toast.error(errorMessage);
    } finally {
      setAdding(false);
      setUploading(false);
    }
  };
  
  const showModal = () => setIsModalOpen(true);
  const handleCancel = () => {
    setIsModalOpen(false);
    form.resetFields();
  };


  const formattedCategoryName = categoryName ? categoryName.replace(/-/g, " ") : null;
  const selectedCategory = categories.find((cat) => cat.name === formattedCategoryName)?.blogCategoryId || null;

  const filteredBlogs = useMemo(() => {
    return selectedCategory
      ? blogs.filter((blog) => blog.blogCategoryId === selectedCategory)
      : blogs;
  }, [selectedCategory, blogs]);

  const mainCategories = categories.slice(0, 6);
  const moreCategories = categories.slice(6);

  const handleCategoryClick = (category) => {
    if (category) {
      const formattedName = category.name.replace(/\s+/g, "-"); 
      navigate(`/staff-blogs/${formattedName}`);
    } else {
      navigate("/staff-blogs");
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
    <div className={styles.addBlogContainer}>
    <Button className={styles.addBlog} type="primary" icon={<PlusOutlined />} onClick={showModal}>
      Write Blog
    </Button>
    </div>
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
                onClick={() => navigate(`/detail/${slugify(blog.title, { lower: true, strict: true })}`)}
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
                      {blog.status.includes("1") ? (
                        <>✍ {blog.createdBy} | 🕒 {new Date(blog.createdTime).toLocaleString("vi-VN", {
                          year: "numeric",
                          month: "2-digit",
                          day: "2-digit",
                        })}</>
                      ) : blog.status.includes("MODIFIED") ? (
                        <>✍ {blog.updatedBy} | 🕒 {new Date(blog.updatedTime).toLocaleString("vi-VN", {
                          year: "numeric",
                          month: "2-digit",
                          day: "2-digit",
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
      <Modal
        title="Add New Blog"
        open={isModalOpen}
        onCancel={handleCancel}
        footer={null}
      >
        <Form form={form} layout="vertical" onFinish={handleAddBlog}>
          <Form.Item name="title" label="Title" rules={[
            { required: true, message: "Please enter title" },
            { min: 5, message: "Title must be at least 5 characters long" },
            { max: 30, message: "Title cannot exceed 30 characters" }
            ]}>
            <Input />
          </Form.Item>

          <Form.Item name="content" label="Content" rules={[
            { required: true, message: "Please enter content" },
            { min: 20, message: "Content must be at least 20 characters long" },
            { max: 500, message: "Content cannot exceed 500 characters" }
            ]}>
            <Input.TextArea rows={4} />
          </Form.Item>

          <Form.Item name="tags" label="Tag" rules={[{ required: true, message: "Please enter tag" }]}>
            <Input />
          </Form.Item>

          <Form.Item name="blogCategoryId" label="Category" rules={[{ required: true, message: "Please select category" }]}>
            <Select placeholder="Select a category">
              {categories.map((cat) => (
                <Select.Option key={cat.blogCategoryId} value={cat.blogCategoryId}>
                  {cat.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>

          <Form.Item label="Image">
            <input type="file" accept="image/*" onChange={(e) => setImageFile(e.target.files[0])} />
          </Form.Item>


          <Form.Item>
            <Button type="primary" htmlType="submit" loading={adding || uploading} block>
              {uploading ? "Uploading Image..." : adding ? "Adding Blog..." : "Add Blog"}
            </Button>
          </Form.Item>

        </Form>
      </Modal>
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

export default StaffBlog;