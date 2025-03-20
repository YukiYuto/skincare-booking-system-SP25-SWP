import { useState, useEffect } from "react";
import { useSelector } from "react-redux";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import DOMPurify from "dompurify";
import { Modal, Button, Form, Input, Spin } from "antd";
import { POST_BLOG_CATEGORY, UPDATE_BLOG_CATEGORY } from "../../../../config/apiConfig";
import "./BlogCategoryModal.css";

const BlogCategoryModal = ({ show, handleClose, addCategory, updateCategory, editingCategory }) => {
  const { accessToken } = useSelector((state) => state.auth);
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (editingCategory) {
      setName(editingCategory.name);
      setDescription(editingCategory.description);
    } else {
      setName("");
      setDescription("");
    }
  }, [editingCategory]);

  // Xóa tất cả thẻ HTML khỏi description
  const stripHtmlTags = (html) => DOMPurify.sanitize(html, { ALLOWED_TAGS: [] });

  const handleSubmit = async () => {
    setLoading(true);
    const cleanDescription = stripHtmlTags(description);

    try {
      if (editingCategory) {
        console.log("Editing Category:", editingCategory);
        // API cập nhật Blog Category
        const response = await axios.put(
          UPDATE_BLOG_CATEGORY,
          {
            blogCategoryId: editingCategory.blogCategoryId,
            name,
            description: cleanDescription,
          },
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${accessToken}`,
            },
          }
        );

        updateCategory(response.data.result); // Cập nhật UI sau khi chỉnh sửa
        toast.success(`✅ Updated Blog Category: ${response.data.result.name} successfully!`);
      } else {
        // API tạo mới Blog Category
        const response = await axios.post(
          POST_BLOG_CATEGORY,
          { name, description: cleanDescription },
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${accessToken}`,
            },
          }
        );

        addCategory(response.data.result); // Cập nhật UI sau khi tạo mới
        toast.success(`🎉 Created Blog Category: ${response.data.result.name} successfully!`);
      }

      setName("");
      setDescription("");
      handleClose();
    } catch (error) {
      toast.error("❌ Error: " + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Modal 
        title={editingCategory ? "Edit Blog Category" : "Create Blog Category"} 
        open={show} 
        onCancel={handleClose} 
        footer={null} 
        centered
      >
        <Form layout="vertical" onFinish={handleSubmit}>
          <Form.Item label="Name" required>
            <Input value={name} onChange={(e) => setName(e.target.value)} required />
          </Form.Item>

          <Form.Item label="Description">
            <ReactQuill value={description} onChange={setDescription} theme="snow" />
          </Form.Item>

          <div style={{ textAlign: "right" }}>
            <Button onClick={handleClose} style={{ marginRight: 8 }}>Cancel</Button>
            <Button type="primary" htmlType="submit" disabled={loading}>
              {loading ? <Spin size="small" /> : editingCategory ? "Update" : "Create"}
            </Button>
          </div>
        </Form>
      </Modal>
      <ToastContainer position="top-right" autoClose={3000} />
    </>
  );
};

export default BlogCategoryModal;
