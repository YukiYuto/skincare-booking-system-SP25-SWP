import React, { useState, useEffect } from "react";
import styles from "./FeedbackContainer.module.css";
import { apiCall } from "../../utils/apiUtils";
import {
  POST_FEEDBACK,
  GET_FEEDBACK_BY_APPOINTMENT,
  PUT_FEEDBACK,
  DELETE_FEEDBACK,
} from "../../config/apiConfig";

const FeedbackContainer = ({
  appointmentId,
  defaultTitle,
  appointmentStatus,
  therapistName,
}) => {
  const [feedbackList, setFeedbackList] = useState([]);
  const [isEditing, setIsEditing] = useState(null);
  const [showFeedbackForm, setShowFeedbackForm] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [newFeedback, setNewFeedback] = useState({
    title: defaultTitle || "Feedback",
    content: "",
    rating: 0,
    appointmentId: appointmentId,
  });

  // Fetch feedback on component mount
  useEffect(() => {
    if (appointmentId) {
      fetchFeedback();
    }
  }, [appointmentId]);

  const fetchFeedback = async () => {
    setIsLoading(true);
    try {
      const feedbackUrl = GET_FEEDBACK_BY_APPOINTMENT.replace(
        "{appointmentId}",
        appointmentId
      );

      const response = await apiCall("GET", feedbackUrl);
      console.log("API Response for feedback:", response);

      if (!response || !response.result) {
        console.error("Invalid API response format:", response);
        setFeedbackList([]);
        return;
      }

      const feedbackData = response.result;
      if (Array.isArray(feedbackData)) {
        feedbackData.forEach((item) => {
          if (!item.feedbackId) {
            console.warn("Missing feedbackId in feedback item:", item);
          }
        });

        setFeedbackList(feedbackData);
      } else {
        console.error("Unexpected feedback data format:", feedbackData);
        setFeedbackList([]);
      }
    } catch (error) {
      console.error("Error fetching feedback:", error);
      setFeedbackList([]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = (e, feedbackId = null) => {
    const { name, value } = e.target;
    const parsedValue = name === "rating" ? parseInt(value, 10) : value;

    if (feedbackId) {
      setFeedbackList((prev) =>
        prev.map((fb) =>
          fb.feedbackId === feedbackId ? { ...fb, [name]: parsedValue } : fb
        )
      );
    } else {
      setNewFeedback((prev) => ({ ...prev, [name]: parsedValue }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);

    const feedbackToSubmit = {
      ...newFeedback,
      appointmentId: appointmentId,
    };

    console.log("Submitting feedback:", feedbackToSubmit);

    try {
      const response = await apiCall("POST", POST_FEEDBACK, feedbackToSubmit);

      if (response.isSuccess && response.result) {
        setFeedbackList((prev) => [...prev, response.result]);
        setShowFeedbackForm(false);
        setNewFeedback({
          title: defaultTitle || "Feedback",
          content: "",
          rating: 0,
          appointmentId: appointmentId,
        });
        console.log("Feedback submitted successfully:", response.result);
      } else {
        console.error("Failed to submit feedback:", response);
      }
    } catch (error) {
      console.error("Error submitting feedback:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleUpdate = async (feedbackId) => {
    const feedbackToUpdate = feedbackList.find(
      (fb) => fb.feedbackId === feedbackId
    );
    if (!feedbackToUpdate) {
      console.error("Cannot find feedback to update with ID:", feedbackId);
      return;
    }

    try {
      const response = await apiCall("PUT", PUT_FEEDBACK, feedbackToUpdate);

      if (response.isSuccess && response.result) {
        setFeedbackList((prev) =>
          prev.map((fb) =>
            fb.feedbackId === feedbackId ? response.result : fb
          )
        );
        setIsEditing(null);
        console.log("Feedback updated successfully:", response.result);
      } else {
        console.error("Failed to update feedback:", response);
      }
    } catch (error) {
      console.error("Error updating feedback:", error);
    }
  };

  const canLeaveFeedback = () => {
    return (
      (appointmentStatus === "CHECKEDIN" ||
        appointmentStatus === "COMPLETED") &&
      feedbackList.length === 0
    );
  };

  // Render star rating
  const renderStars = (rating) => {
    return `${rating}/5`;
  };

  if (isLoading) {
    return (
      <div className={styles.loadingState}>Loading feedback information...</div>
    );
  }

  return (
    <div className={styles.feedbackContainer}>
      {feedbackList.length > 0 ? (
        <div className={styles.feedbackSection}>
          {feedbackList.map((fb) => (
            <div
              key={fb.feedbackId || `feedback-${Math.random()}`}
              className={styles.feedbackItem}
            >
              {isEditing === fb.feedbackId ? (
                <form className={styles.editForm}>
                  <div className={styles.formGroup}>
                    <input
                      id={`title-${fb.feedbackId}`}
                      type="text"
                      name="title"
                      value={fb.title}
                      onChange={(e) => handleChange(e, fb.feedbackId)}
                      hidden
                    />
                  </div>
                  <div className={styles.formGroup}>
                    <label htmlFor={`rating-${fb.feedbackId}`}>Rating</label>
                    <select
                      id={`rating-${fb.feedbackId}`}
                      name="rating"
                      value={fb.rating}
                      onChange={(e) => handleChange(e, fb.feedbackId)}
                    >
                      <option value="1">1 - Poor</option>
                      <option value="2">2 - Fair</option>
                      <option value="3">3 - Good</option>
                      <option value="4">4 - Very Good</option>
                      <option value="5">5 - Excellent</option>
                    </select>
                  </div>
                  <div className={styles.formGroup}>
                    <label htmlFor={`content-${fb.feedbackId}`}>Comments</label>
                    <textarea
                      id={`content-${fb.feedbackId}`}
                      name="content"
                      value={fb.content}
                      onChange={(e) => handleChange(e, fb.feedbackId)}
                    />
                  </div>
                  <div className={styles.buttonGroup}>
                    <button
                      type="button"
                      className={styles.cancelButton}
                      onClick={() => {
                        setIsEditing(null);
                        fetchFeedback();
                      }}
                    >
                      Cancel
                    </button>
                    <button
                      type="button"
                      className={styles.saveButton}
                      onClick={() => handleUpdate(fb.feedbackId)}
                    >
                      Save Changes
                    </button>
                  </div>
                </form>
              ) : (
                <div className={styles.feedbackRaw}>
                  <h2>{fb.title}</h2>
                  <div className={styles.flexGroup}>
                    <div className={styles.contentGroup}>
                      <p className={styles.rating}>{renderStars(fb.rating)}</p>
                      <p className={styles.content}>{fb.content}</p>
                    </div>
                    <div className={styles.buttonGroup}>
                      <button
                        className={styles.editButton}
                        onClick={() => setIsEditing(fb.feedbackId)}
                      >
                        Edit
                      </button>
                    </div>
                  </div>
                </div>
              )}
            </div>
          ))}
        </div>
      ) : (
        <p>No feedback available for this appointment.</p>
      )}

      {canLeaveFeedback() && !showFeedbackForm && (
        <button
          className={styles.feedbackButton}
          onClick={() => {
            setNewFeedback((prev) => ({
              ...prev,
              title: `Feedback for ${therapistName || "Appointment"}`,
              appointmentId: appointmentId,
            }));
            setShowFeedbackForm(true);
          }}
        >
          Leave Feedback
        </button>
      )}

      {showFeedbackForm && (
        <div className={styles.feedbackForm}>
          <form onSubmit={handleSubmit}>
            <div className={styles.formGroup}>
              <label htmlFor="rating">Rating (1-5)</label>
              <select
                id="rating"
                name="rating"
                value={newFeedback.rating}
                onChange={(e) => handleChange(e)}
                required
              >
                <option value="0">Select rating</option>
                <option value="1">1 - Poor</option>
                <option value="2">2 - Fair</option>
                <option value="3">3 - Good</option>
                <option value="4">4 - Very Good</option>
                <option value="5">5 - Excellent</option>
              </select>
            </div>
            <div className={styles.formGroup}>
              <label htmlFor="content">Comments</label>
              <textarea
                id="content"
                name="content"
                value={newFeedback.content}
                onChange={(e) => handleChange(e)}
                required
                placeholder="Please share your experience with this appointment..."
              />
            </div>
            <div className={styles.formActions}>
              <button
                type="button"
                onClick={() => setShowFeedbackForm(false)}
                className={styles.cancelButton}
              >
                Cancel
              </button>
              <button
                type="submit"
                className={styles.submitButton}
                disabled={isSubmitting}
              >
                {isSubmitting ? "Submitting..." : "Submit Feedback"}
              </button>
            </div>
          </form>
        </div>
      )}
    </div>
  );
};

export default FeedbackContainer;
