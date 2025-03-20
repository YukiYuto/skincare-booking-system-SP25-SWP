import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { GET_BLOG, POST_BLOG } from "../../config/apiConfig";

const BlogContentList = () => {
  const { blogCategoryId } = useParams();
  const navigate = useNavigate();
  const [blogs, setBlogs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [newBlog, setNewBlog] = useState({ title: "", content: "", tags: "" });
  const [showModal, setShowModal] = useState(false);
  
  const { user, accessToken } = useSelector((state) => state.auth);

  useEffect(() => {
    fetchBlogs();
  }, [blogCategoryId]);

  const fetchBlogs = async () => {
    setLoading(true);
    try {
      const response = await axios.get(GET_BLOG);
      const allBlogs = response.data.result || [];
      const filteredBlogs = allBlogs
        .filter((blog) => blog.blogCategoryId === blogCategoryId)
        .sort((a, b) => new Date(b.createdTime) - new Date(a.createdTime));

      setBlogs(filteredBlogs);
    } catch (error) {
      toast.error("❌ Error: " + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  const handleCreateBlog = async () => {
    if (!newBlog.title || !newBlog.content || !newBlog.tags) {
      toast.error(" Please fill in all fields!");
      return;
    }

    try {
      await axios.post(
        POST_BLOG,
        {
          title: newBlog.title,
          content: newBlog.content,
          blogCategoryId: blogCategoryId,
          userId: user.id,
          tags: newBlog.tags,
        },
        {
          headers: {
            Authorization: `Bearer ${accessToken}`,
            "Content-Type": "application/json",
          },
        }
      );
      
      toast.success("Blog created successfully!");
      setNewBlog({ title: "", content: "", tags: "" });
      setShowModal(false);
      fetchBlogs();
    } catch (error) {
      toast.error(" Failed to create blog: " + (error.response?.data?.message || error.message));
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 py-16 px-10 md:px-32">
      <button
        onClick={() => navigate(-1)}
        className="mb-6 px-6 py-3 bg-blue-500 hover:bg-blue-700 text-white text-lg font-semibold rounded-lg shadow-md transition duration-300"
      >
        ⬅ Back
      </button>

      <button
        onClick={() => setShowModal(true)}
        className="mb-6 ml-4 px-6 py-3 bg-green-500 hover:bg-green-700 text-white text-lg font-semibold rounded-lg shadow-md transition duration-300"
      >
        ➕ Create New Blog
      </button>

      {showModal && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white p-8 rounded-lg shadow-md max-w-2xl w-full">
            <h3 className="text-3xl font-bold text-gray-800 mb-4">📝 Create New Blog</h3>
            <input
              type="text"
              placeholder="Enter blog title"
              value={newBlog.title}
              onChange={(e) => setNewBlog({ ...newBlog, title: e.target.value })}
              className="w-full px-4 py-2 mb-4 border border-gray-300 rounded-md text-lg"
            />
            <textarea
              placeholder="Enter blog content"
              value={newBlog.content}
              onChange={(e) => setNewBlog({ ...newBlog, content: e.target.value })}
              className="w-full px-4 py-2 mb-4 border border-gray-300 rounded-md text-lg h-32"
            />
            <input
              type="text"
              placeholder="Enter tags (comma separated)"
              value={newBlog.tags}
              onChange={(e) => setNewBlog({ ...newBlog, tags: e.target.value })}
              className="w-full px-4 py-2 mb-4 border border-gray-300 rounded-md text-lg"
            />
            <div className="flex justify-end space-x-4">
              <button
                onClick={() => setShowModal(false)}
                className="px-6 py-3 bg-gray-500 hover:bg-gray-700 text-white text-lg font-semibold rounded-lg shadow-md transition duration-300"
              >
                ❌ Cancel
              </button>
              <button
                onClick={handleCreateBlog}
                className="px-6 py-3 bg-purple-500 hover:bg-purple-700 text-white text-lg font-semibold rounded-lg shadow-md transition duration-300"
              >
                🚀 Create Blog
              </button>
            </div>
          </div>
        </div>
      )}

      <div className="max-w-6xl mx-auto bg-white p-12 rounded-lg shadow-xl" style={{marginTop: "30px"}}>
        <h2 className="text-6xl font-bold text-gray-900 text-center mb-12">🌿 Blog Spa</h2>

        {loading ? (
          <p className="text-center text-gray-500 text-2xl">Loading...</p>
        ) : blogs.length > 0 ? (
          blogs.map((blog, index) => (
            <article key={blog.blogId} className="mb-20 pb-12 border-b border-gray-300">
              <h3 className="text-5xl font-bold text-gray-900 mb-6">
                {index + 1}. {blog.title}
              </h3>
              <p className="text-gray-600 text-2xl mb-6">
                ✍ {blog.createdBy} | 📅 {new Date(blog.createdTime).toLocaleDateString()}
              </p>
              <img src={blog.imageUrl} alt={blog.title} className="w-full h-[500px] object-cover rounded-lg shadow-lg mb-10" />
              <div className="text-2xl text-gray-800 leading-relaxed">{blog.content}</div>
              <div className="mt-10">
                <span className="text-xl text-gray-700 bg-gray-300 px-5 py-2 rounded-full">🏷 {blog.tags}</span>
              </div>
            </article>
          ))
        ) : (
          <p className="text-center text-gray-500 text-2xl">No any Blogs.</p>
        )}

        <ToastContainer position="top-right" autoClose={3000} />
      </div>
    </div>
  );
};

export default BlogContentList;