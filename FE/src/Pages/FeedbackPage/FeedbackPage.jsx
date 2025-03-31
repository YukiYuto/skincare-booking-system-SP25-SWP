import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import axios from "axios";
import styles from "./FeedbackPage.module.css";
import { EditOutlined } from "@ant-design/icons";
import Header from "../../Components/Common/Header";
import Footer from "../../Components/Footer/Footer";
import { Spin } from "antd";
import { toast } from "react-toastify";

const FeedbackPage = () => {
  const [feedbacks, setFeedbacks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [updatingFeedback, setUpdatingFeedback] = useState(null);
  const [updateTitle, setUpdateTitle] = useState("");
  const [updateContent, setUpdateContent] = useState("");
  const [updateRating, setUpdateRating] = useState(5);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [allowedToUpdate, setAllowedToUpdate] = useState({});
  const [customerNames, setCustomerNames] = useState({});

  const { user } = useSelector((state) => state.auth);

  useEffect(() => {
    const fetchFeedbacks = async () => {
      try {
        const response = await axios.get(
          "https://lumiconnect.azurewebsites.net/api/Feedback/all",
          {
            headers: { Authorization: `Bearer ${user.accessToken}` },
          }
        );

        const feedbackList = response.data.result;
        const permissions = {};
        const names = {};

        for (let feedback of feedbackList) {
          if (feedback.appointmentId) {
            try {
              const appointmentRes = await axios.get(
                `https://lumiconnect.azurewebsites.net/api/appointment/${feedback.appointmentId}`,
                {
                  headers: { Authorization: `Bearer ${user.accessToken}` },
                }
              );

              const customerInfo = appointmentRes.data.result?.customerInfo;
              if (customerInfo) {
                names[feedback.appointmentId] = customerInfo.customerName || "Không rõ";
                permissions[feedback.feedbackId] = customerInfo.customerPhone === user.phoneNumber;
              }
            } catch (error) {
              console.error(`❌ API lỗi với appointmentId ${feedback.appointmentId}:`, error.response?.status);
              permissions[feedback.feedbackId] = false;
            }
          }
        }

        setFeedbacks(feedbackList);
        setAllowedToUpdate(permissions);
        setCustomerNames(names);
        setLoading(false);
      } catch (error) {
        console.error("Lỗi khi lấy feedback:", error);
        setError("Không thể tải phản hồi.");
        setLoading(false);
      }
    };

    fetchFeedbacks();
  }, [user.phoneNumber]);

  const renderStars = (rating) => "⭐".repeat(rating);

  const openUpdateModal = (feedback) => {
    setUpdatingFeedback(feedback);
    setUpdateTitle(feedback.title);
    setUpdateContent(feedback.content);
    setUpdateRating(feedback.rating);
    setIsModalOpen(true);
  };

  const closeUpdateModal = () => {
    setUpdatingFeedback(null);
    setIsModalOpen(false);
  };

  const handleUpdateSubmit = async () => {
    try {
      const currentTime = new Date().toISOString(); // Lấy thời gian hiện hành

      const updatedFeedback = {
        feedbackId: updatingFeedback.feedbackId,
        title: updateTitle,
        content: updateContent,
        rating: updateRating,
        appointmentId: updatingFeedback.appointmentId,
      };

      await axios.put(
        "https://lumiconnect.azurewebsites.net/api/Feedback/update",
        updatedFeedback,
        {
          headers: { 
            Authorization: `Bearer ${user.accessToken}`,
            "Content-Type": "application/json"
          },
        }
      );

      setFeedbacks((prevFeedbacks) =>
        prevFeedbacks.map((fb) =>
          fb.feedbackId === updatingFeedback.feedbackId
            ? { ...updatedFeedback, createdTime: currentTime } // Cập nhật thời gian mới
            : fb
        )
      );
      toast.success("Update successfully!")
      closeUpdateModal();
    } catch (error) {
      console.error("Lỗi khi cập nhật feedback:", error);
    }
  };

  if (loading) return <div style={{textAlign: "center", marginTop:"400px"}}><Spin /></div>;
  if (error) return <p className={styles.error}>{error}</p>;

  return (
    <>
    <Header />
    <div className={styles.container}>
      <h2 className={styles.title}>Feedback List</h2>
      <ul className={styles.feedbackList}>
        {feedbacks.map((feedback) => (
          <li key={feedback.feedbackId} className={styles.feedbackItem}>
            <h3>{customerNames[feedback.appointmentId] || "N/A"}</h3>
            <p><strong>Title:</strong> {feedback.title}</p>
            <p><strong>Time:</strong> {new Date(feedback.createdTime).toLocaleString()}</p>
            <p><strong>Content:</strong> {feedback.content}</p>
            <p><strong>Rating:</strong> {renderStars(feedback.rating)}</p>

            {allowedToUpdate[feedback.feedbackId] && (
              <button className={styles.updateButton} onClick={() => openUpdateModal(feedback)}>
              <EditOutlined />
              </button>
            )}
          </li>
        ))}
      </ul>

      {/* Modal cập nhật feedback */}
      {isModalOpen && (
        <div className={styles.modalOverlay}>
          <div className={styles.modalContent}>
            <h3>Edit feedback</h3>
            <textarea
              value={updateTitle}
              onChange={(e) => setUpdateTitle(e.target.value)}
              rows="4"
            />
            <br />
            <textarea
              value={updateContent}
              onChange={(e) => setUpdateContent(e.target.value)}
              rows="4"
            />
            <br />
            <label>Rating: </label>
            <select value={updateRating} onChange={(e) => setUpdateRating(Number(e.target.value))}>
              {[1, 2, 3, 4, 5].map((num) => (
                <option key={num} value={num}>{num} ⭐</option>
              ))}
            </select>
            <br />
            <button className={styles.saveButton} onClick={handleUpdateSubmit}>Save</button>
            <button className={styles.cancelButton} onClick={closeUpdateModal}>Cancel</button>
          </div>
        </div>
      )}
    </div>
    <Footer />
    </>
  );
};

export default FeedbackPage;
