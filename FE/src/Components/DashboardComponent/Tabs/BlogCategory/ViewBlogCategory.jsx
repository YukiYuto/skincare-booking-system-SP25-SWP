import { useEffect, useState } from "react";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { FaEdit } from "react-icons/fa"; // Import icon Edit
import styles from "./ViewBlogCategory.module.css";
import BlogCategoryModal from "./BlogCategoryModal";
import { Button } from "antd";
import { useNavigate } from "react-router-dom";
import { GET_BLOG_CATEGORY } from "../../../../config/apiConfig";

const ViewBlogCategory = () => {
  const [showModal, setShowModal] = useState(false);
  const [categories, setCategories] = useState([]);
  const [editingCategory, setEditingCategory] = useState(null); // Lưu danh mục đang chỉnh sửa
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [searchQuery, setSearchQuery] = useState("");
  const navigate = useNavigate();
  const itemsPerPage = 12;

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    setLoading(true);
    try {
      const response = await axios.get(GET_BLOG_CATEGORY);
      setCategories(response.data.result || []); 
    } catch (error) {
      toast.error("❌ Error: " + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  // Cập nhật danh sách ngay khi tạo mới
  const addCategory = (newCategory) => {
    setCategories((prev) => [newCategory, ...prev]);
  };

  // Cập nhật danh sách khi chỉnh sửa xong
  const updateCategory = (updatedCategory) => {
    setCategories((prev) =>
      prev.map((cat) => (cat.id === updatedCategory.id ? updatedCategory : cat))
    );
  };

  const filteredCategories = categories.filter((category) =>
    category.name.toLowerCase().includes(searchQuery.toLowerCase())
  );

  // Tính toán dữ liệu cho trang hiện tại
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredCategories.slice(indexOfFirstItem, indexOfLastItem);

  // Tổng số trang
  const totalPages = Math.ceil(categories.length / itemsPerPage);

  return (
    <>
      <div>
        <button className={styles.buttonModal} onClick={() => {
          setEditingCategory(null); // Đặt lại giá trị khi tạo mới
          setShowModal(true);
        }}>
          Create Blog Category
        </button>
        <BlogCategoryModal 
          show={showModal} 
          handleClose={() => setShowModal(false)} 
          addCategory={addCategory} 
          updateCategory={updateCategory} 
          editingCategory={editingCategory} 
        />
      </div>

      <input
        type="text"
        className={styles.searchInput}
        placeholder="Search by name..."
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
      />

      <div className={styles.container}>
        <h2 className={styles.title}>Blog Category</h2>

        {loading ? (
          <p className={styles.loading}>Loading...</p>
        ) : (
          <>
            <div className={styles.grid}>
              {currentItems.length > 0 ? (
                currentItems.map((category) => (
                  <div key={category.blogCategoryId} className={styles.card} onClick={() => navigate(`/dashboard/view-blogcategory/${category.blogCategoryId}`)}>
                    <h3 className={styles.cardTitle}>{category.name}</h3>
                    <p className={styles.cardDescription}>{category.description}</p>
                    <Button 
                      className={styles.editButton} 
                      onClick={() => {
                        setEditingCategory(category);
                        setShowModal(true);
                      }}
                    >
                      <FaEdit />
                    </Button>
                  </div>
                ))
              ) : (
                <p className={styles.noData}>No Blog Category.</p>
              )}
            </div>

            {/* Pagination Controls */}
            <div className={styles.pagination}>
              <button className={styles.pageButton} onClick={() => setCurrentPage(currentPage - 1)} disabled={currentPage === 1}>
                ◀ Prev
              </button>
              <span className={styles.pageInfo}>Page {currentPage} / {totalPages}</span>
              <button className={styles.pageButton} onClick={() => setCurrentPage(currentPage + 1)} disabled={currentPage === totalPages}>
                Next ▶
              </button>
            </div>
          </>
        )}

        <ToastContainer position="top-right" autoClose={3000} />
      </div>
    </>
  );
};

export default ViewBlogCategory;
