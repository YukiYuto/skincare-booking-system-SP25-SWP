import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import slugify from "slugify";
import { Modal, Input, Button, Upload } from "antd"; 
import { GET_BLOG, UPDATE_BLOG, POST_CUSTOMER_AVATAR_API, DELETE_BLOG } from "../../../config/apiConfig";
import { toast } from "react-toastify";
import styles from "./BlogDetail.module.css";
import { useSelector } from "react-redux";

const { TextArea } = Input;

const BlogDetail = () => {
  const { title } = useParams();
  const navigate = useNavigate();
  const [blog, setBlog] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [imageFile, setImageFile] = useState(null);
  const [imageFileName, setImageFileName] = useState("");

  const [editTitle, setEditTitle] = useState("");
  const [editContent, setEditContent] = useState("");
  const [editTags, setEditTags] = useState("");
  const [editImageUrl, setEditImageUrl] = useState("");

  const { user, accessToken } = useSelector((state) => state.auth);

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
          setEditTitle(foundBlog.title);
          setEditContent(foundBlog.content);
          setEditTags(foundBlog.tags || "");
          setEditImageUrl(foundBlog.imageUrl || "");
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

  const handleImageUpload = async () => {
    if (!imageFile) {
      toast.error("Please choose image!");
      return null;
    }

    setUploading(true);
    const formData = new FormData();
    formData.append("file", imageFile);

    try {
      const response = await axios.post(POST_CUSTOMER_AVATAR_API, formData, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
          "Content-Type": "multipart/form-data",
        },
      });

      if (response.data && response.data.result) {
        return response.data.result; 
      } else {
        return null;
      }
    } catch (error) {
      toast.error("Lỗi Upload ảnh: " + (error.response?.data?.message || error.message));
      return null;
    } finally {
      setUploading(false);
    }
  };

  const handleSaveEdit = async () => {
    let finalImageUrl = editImageUrl; 
  
    if (imageFile) {
      const uploadedImageUrl = await handleImageUpload();
      if (uploadedImageUrl) {
        finalImageUrl = uploadedImageUrl; 
      }
    }
  
    try {
      const payload = {
        blogId: blog.blogId,
        title: editTitle,
        content: editContent,
        tags: editTags,
        imageUrl: finalImageUrl,
        userId: user.id,
        blogCategoryId: blog.blogCategoryId,
      };
  
      await axios.put(UPDATE_BLOG, payload, {
        headers: {
          "Content-Type": "application/json", 
          Authorization: `Bearer ${accessToken}`, 
        },
      });
  
      setBlog({
        ...blog,
        title: editTitle,
        content: editContent,
        tags: editTags,
        imageUrl: finalImageUrl,
      });
  
      toast.success("Edit successfully!");
      setIsModalOpen(false);
    } catch (error) {
      toast.error("Failed to edit: " + (error.response?.data?.message || error.message));
    }
  };

  const handleDeleteBlog = () => {
    Modal.confirm({
      title: "Confirm Deletion",
      content: "Are you sure you want to delete this blog?",
      okText: "Yes, Delete",
      cancelText: "Cancel",
      okType: "danger",
      onOk: async () => {
        try {
          await axios.delete(`${DELETE_BLOG}/${blog.blogId}`, {
            headers: {
              Authorization: `Bearer ${accessToken}`,
            },
          });
  
          toast.success("Deleted successfully!");
          navigate("/staff-blogs"); 
        } catch (error) {
          toast.error("Failed to delete: " + (error.response?.data?.message || error.message));
        }
      },
    });
  };
  

  if (loading) return <p>Loading...</p>;
  if (error) return <p className={styles.error}>{error}</p>;
  if (!blog) return <p>No blogs were found!</p>;

  return (
    <>
    <button onClick={() => navigate(-1)} className={styles.backButton}>
          ⬅ Back
    </button>
    <div className={styles.container}>
      <div className={styles.actionBar}>
        <button onClick={() => setIsModalOpen(true)} className={styles.editButton}>
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6 hover:text-blue-500 transition duration-200">
          <path strokeLinecap="round" strokeLinejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L6.832 19.82a4.5 4.5 0 0 1-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 0 1 1.13-1.897L16.863 4.487Zm0 0L19.5 7.125" />
        </svg>
        </button>
        <button onClick={handleDeleteBlog} className={styles.deleteButton}>
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6">
          <path strokeLinecap="round" strokeLinejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
        </svg>
        </button>
      </div>

      <h1 className={styles.title}>{blog.title}</h1>

      <div className={styles.meta}>
      {blog.status.includes("1") ? (
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

      <Modal
        title="Chỉnh sửa bài viết"
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        footer={[
          <Button key="cancel" onClick={() => setIsModalOpen(false)}>
            Cancel
          </Button>,
          <Button key="save" type="primary" onClick={handleSaveEdit} loading={uploading}>
            Save
          </Button>,
        ]}
      >
        <label>Title:</label>
        <Input value={editTitle} onChange={(e) => setEditTitle(e.target.value)} className={styles.input} />

        <label>Content:</label>
        <TextArea value={editContent} onChange={(e) => setEditContent(e.target.value)} rows={6} className={styles.input} />

        <label>Tags:</label>
        <Input value={editTags} onChange={(e) => setEditTags(e.target.value)} className={styles.input} />

        <label>Current image:</label>
        <img src={editImageUrl} alt="Preview" className={styles.previewImage} />

        <label>New image:</label>
        <Upload
          beforeUpload={(file) => {
            setImageFile(file);
            setImageFileName(file.name);
            return false; // Không upload ngay, chỉ lưu vào state
          }}
          showUploadList={false}
        >
          <Button>📤 Choose image</Button>
        </Upload>
        {imageFileName && (
        <p className={styles.fileName}>📂 Image was chose: {imageFileName}</p>
        )}
      </Modal>
    </div>
    </>
  );
};

export default BlogDetail;
