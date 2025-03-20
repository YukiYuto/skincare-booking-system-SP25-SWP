import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { GET_BLOG } from "../../config/apiConfig";

const BlogForCusList = () => {
  const { blogCategoryId } = useParams();
  const navigate = useNavigate();
  const [blogs, setBlogs] = useState([]);
  const [loading, setLoading] = useState(true);
  
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


  return (
    <div className="min-h-screen bg-gray-100 py-16 px-10 md:px-32">
      <button
        onClick={() => navigate(-1)}
        className="mb-6 px-6 py-3 bg-blue-500 hover:bg-blue-700 text-white text-lg font-semibold rounded-lg shadow-md transition duration-300"
      >
        ⬅ Back
      </button>
      <div className="max-w-6xl mx-auto bg-white p-12 rounded-lg shadow-xl">
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

export default BlogForCusList;
