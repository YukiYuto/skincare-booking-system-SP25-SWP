import { useEffect, useState } from "react";
import styles from "./FeedbackModal.module.css";
import { Modal, Input, Button, Spin, Rate } from "antd";
import { useSelector } from "react-redux";
import { toast } from "react-toastify";

const FeedbackModal = ({ isOpen, onClose }) => {
  const { user } = useSelector((state) => state.auth);
  const [appointments, setAppointments] = useState([]);
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const [loading, setLoading] = useState(false);
  const [feedback, setFeedback] = useState({
    title: "",
    content: "",
    rating: 1,
  });

  useEffect(() => {
    if (isOpen) {
      fetchAppointments();
    }
  }, [isOpen]);

  const fetchAppointments = async () => {
    try {
      setLoading(true);
      const response = await fetch(
        "https://lumiconnect.azurewebsites.net/api/appointment",
        {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        }
      );
      const data = await response.json();

      if (data?.result) {
        const createdAppointments = data.result.filter((appt) => appt.status === "COMPLETED");

        const validAppointments = await Promise.all(
          createdAppointments.map(async (appt) => {
            const appointmentDetails = await fetchAppointmentDetails(appt.appointmentId);
            if (appointmentDetails?.customerInfo?.customerPhone === user.phoneNumber) {
              const hasFeedback = await checkFeedback(appt.appointmentId);
              return hasFeedback ? null : { ...appt, serviceInfo: appointmentDetails?.serviceInfo };
            }
            return null;
          })
        );

        const filteredAppointments = validAppointments.filter(Boolean);

        setAppointments(filteredAppointments);
        setSelectedAppointment(filteredAppointments[0] || null);
      }
    } catch (error) {
      console.error("Error fetching appointments:", error);
    } finally {
      setLoading(false);
    }
  };

  const fetchAppointmentDetails = async (appointmentId) => {
    try {
      const response = await fetch(
        `https://lumiconnect.azurewebsites.net/api/appointment/${appointmentId}`,
        {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        }
      );
      const data = await response.json();
      return data?.result || null;
    } catch (error) {
      console.error(`Error fetching appointment details for ID ${appointmentId}:`, error);
      return null;
    }
  };

  const checkFeedback = async (appointmentId) => {
    try {
      const response = await fetch(
        `https://lumiconnect.azurewebsites.net/api/Feedback/appointment/${appointmentId}`,
        {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        }
      );
      const data = await response.json();
      return data?.result?.length > 0;
    } catch (error) {
      console.error("Error checking feedback:", error);
      return false;
    }
  };

  const handleSubmit = async () => {
    if (!selectedAppointment) {
      toast.error("Please choose your appointment.");
      return;
    }

    if (!feedback.title || !feedback.content) {
      toast.error("Please fill in all fields.");
      return;
    }

    try {
      setLoading(true);
      const requestBody = {
        title: feedback.title,
        content: feedback.content,
        rating: feedback.rating,
        appointmentId: selectedAppointment.appointmentId,
      };

      const response = await fetch(
        "https://lumiconnect.azurewebsites.net/api/Feedback/create",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${user.accessToken}`,
          },
          body: JSON.stringify(requestBody),
        }
      );

      const responseData = await response.json();

      if (response.ok) {
        toast.success("Thanks for your feedback!");
        fetchAppointments(); 
      } else {
        toast.error(responseData.message || "Cannot send feedback!");
      }
    } catch (error) {
      console.error("Error submitting feedback:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal
      title="Feedback Appointment"
      open={isOpen}
      onCancel={onClose}
      footer={null}
    >
      {loading ? (
        <Spin />
      ) : appointments.length === 0 ? (
        <p style={{textAlign:"center"}}>No appointment to give feedback.</p>
      ) : (
        <>
          <div className={styles.appointmentSelect}>
            <label>Choose your appointment:</label>
            <select
              value={selectedAppointment?.appointmentId || ""}
              onChange={(e) =>
                setSelectedAppointment(
                  appointments.find((appt) => appt.appointmentId === e.target.value)
                )
              }
            >
              {appointments.map((appt) => (
                <option key={appt.appointmentId} value={appt.appointmentId}>
                  {appt?.serviceInfo?.serviceName} - {new Date(appt.appointmentDate).toLocaleDateString()}
                </option>
              ))}
            </select>
          </div>

          <Input
            placeholder="Title"
            value={feedback.title}
            onChange={(e) => setFeedback({ ...feedback, title: e.target.value })}
          />

          <Input.TextArea
            placeholder="Write your content..."
            value={feedback.content}
            onChange={(e) => setFeedback({ ...feedback, content: e.target.value })}
          />

          <div style={{ display: "flex", alignItems: "center", gap: "10px", marginTop: "10px" }}>
            <span style={{ fontWeight: "bold", minWidth: "70px" }}>Rating:</span>
            <Rate
              allowHalf
              value={feedback.rating}
              onChange={(value) => setFeedback({ ...feedback, rating: value })}
            />
          </div>

          <Button
            style={{ marginTop: "10px" }}
            type="primary"
            onClick={handleSubmit}
            disabled={loading || !selectedAppointment}
          >
            Send feedback
          </Button>
        </>
      )}
    </Modal>
  );
};

export default FeedbackModal;
