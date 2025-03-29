import { useState, useEffect } from "react";
import { useSelector } from "react-redux";
import { Calendar, Badge, Card, Spin, Alert } from "antd";
import dayjs from "dayjs";
import axios from "axios";
import styles from "./TableTherapist.module.css";
import { GET_ALL_THERAPISTS_API } from "../../config/apiConfig";

const API_SCHEDULES = "https://lumiconnect.azurewebsites.net/api/therapist-schedules/therapist";
const API_APPOINTMENT = "https://lumiconnect.azurewebsites.net/api/appointment";

const TableTherapist = () => {
  const user = useSelector((state) => state.auth.user);
  const token = useSelector((state) => state.auth.accessToken);

  const [bookings, setBookings] = useState([]);
  const [selectedDate, setSelectedDate] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchTherapistSchedule = async () => {
      try {
        if (!token) throw new Error("Please login!");

        const { data: therapistsData } = await axios.get(GET_ALL_THERAPISTS_API, {
          headers: { Authorization: `Bearer ${token}` },
        });

        const therapistsList = Array.isArray(therapistsData.result) ? therapistsData.result : [];
        if (!therapistsList.length) throw new Error("Therapist is not available!");

        const matchedTherapist = therapistsList.find((t) => t.fullName === user.fullName);
        if (!matchedTherapist) throw new Error("No found therpist!");

        const therapistId = matchedTherapist.skinTherapistId;

        const { data: scheduleData } = await axios.get(`${API_SCHEDULES}/${therapistId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        const scheduleList = scheduleData.result || [];
        if (!scheduleList.length) {
          setBookings([]);
          return;
        }

        const formattedBookings = await Promise.all(
          scheduleList.map(async (booking) => {
            try {
              const { data: appointmentData } = await axios.get(`${API_APPOINTMENT}/${booking.appointmentId}`, {
                headers: { Authorization: `Bearer ${token}` },
              });

              const customerInfo = appointmentData.result?.customerInfo || {};
              const serviceInfo = appointmentData.result?.serviceInfo || {};
              const time = appointmentData.result?.appointmentTime || {};

              return {
                id: booking.appointmentId, 
                date: dayjs(booking.appointmentDate).format("YYYY-MM-DD"),
                service: {
                  serviceName: serviceInfo.serviceName || "Không xác định",
                  servicePrice: serviceInfo.servicePrice || "N/A",
                },
                customer: {
                  fullName: customerInfo.customerName || "Không xác định",
                  phone: customerInfo.customerPhone || "N/A",
                  email: customerInfo.customerEmail || "N/A",
                },
                time,
                isCompleted: false,
              };
            } catch (err) {
              console.error("Failed to load appointment:", err);
              return null;
            }
          })
        );

        setBookings(formattedBookings.filter(Boolean));
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    if (user?.fullName) {
      fetchTherapistSchedule();
    } else {
      setLoading(false);
    }
  }, [user, token]);

  const toggleCompletion = (id) => {
    setBookings((prevBookings) =>
      prevBookings.map((booking) =>
        booking.id === id ? { ...booking, isCompleted: !booking.isCompleted } : booking
      )
    );
  };

  const selectedBookings = bookings.filter((b) => b.date === selectedDate);

  const cellRender = (value) => {
    const dateStr = value.format("YYYY-MM-DD");
    const bookingsOnDate = bookings.filter((b) => b.date === dateStr);

    return bookingsOnDate.length > 0 ? (
      <div>
        {bookingsOnDate.map((booking, index) => (
          <Badge
            key={index}
            color={booking.isCompleted ? "green" : "red"}
            text={`Appointment: ${booking.customer.fullName}`}
            className={styles.badge}
          />
        ))}
      </div>
    ) : null;
  };

  const onSelectDate = (value) => {
    setSelectedDate(value.format("YYYY-MM-DD"));
  };

  if (loading) return <Spin tip="Loading..." />;
  if (error) return <Alert message={error} type="error" showIcon />;

  return (
    <div className={styles.container}>
      <div className={styles.calendarSection}>
        <h2 className={styles.title}>Booking Calendar</h2>
        <Calendar cellRender={cellRender} onSelect={onSelectDate} className={styles.calendar} />
      </div>
      <div className={styles.detailSection}>
        <Card title={<span className={styles.cardTitle}>Booking Details</span>} className={styles.card}>
          {selectedBookings.length > 0 ? (
            selectedBookings.map((booking, index) => (
              <div key={index} className={styles.bookingItem}>
                <h3 style={{ textAlign: "center" }}>
                  <strong>Appointment with:</strong> {booking.customer.fullName}
                </h3>
                <p><strong>Date:</strong> {booking.date}</p>
                <p><strong>Service:</strong> {booking.service.serviceName}</p>
                <p><strong>Price:</strong> {booking.service.servicePrice}</p>
                <p><strong>Customer:</strong> {booking.customer.fullName}</p>
                <p><strong>Email:</strong> {booking.customer.email}</p>
                <p><strong>Phone:</strong> {booking.customer.phone}</p>
                <p><strong>Time:</strong> {booking.time}</p>

                <p>
                  <strong>Status:</strong>{" "}
                  <span style={{ color: booking.isCompleted ? "green" : "red", fontWeight: "bold" }}>
                    {booking.isCompleted ? "Done" : "Not yet"}
                  </span>
                </p>

                <button
                  onClick={() => toggleCompletion(booking.id)}
                  style={{
                    backgroundColor: booking.isCompleted ? "red" : "green",
                    color: "white",
                    padding: "5px 10px",
                    border: "none",
                    borderRadius: "5px",
                    cursor: "pointer",
                  }}
                >
                  {booking.isCompleted ? "Not yet" : "Done"}
                </button>
                <hr />
              </div>
            ))
          ) : (
            <p className={styles.noBooking}>Chọn một ngày để xem chi tiết</p>
          )}
        </Card>
      </div>
    </div>
  );
};

export default TableTherapist;
