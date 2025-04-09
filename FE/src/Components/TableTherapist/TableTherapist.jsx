import { useState, useEffect } from "react";
import { useSelector } from "react-redux";
import { Calendar, Badge, Card, Spin, Alert, Tag } from "antd";
import dayjs from "dayjs";
import axios from "axios";
import styles from "./TableTherapist.module.css";
import { GET_ALL_THERAPISTS_API } from "../../config/apiConfig";

const API_SCHEDULES =
  "https://lumiconnect.azurewebsites.net/api/therapist-schedules/therapist";
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

        const { data: therapistsData } = await axios.get(
          GET_ALL_THERAPISTS_API,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );

        const therapistsList = Array.isArray(therapistsData.result)
          ? therapistsData.result
          : [];
        if (!therapistsList.length)
          throw new Error("Therapist is not available!");

        const matchedTherapist = therapistsList.find(
          (t) => t.phoneNumber === user.phoneNumber
        );
        if (!matchedTherapist) throw new Error("No found therpist!");

        const therapistId = matchedTherapist.skinTherapistId;

        const { data: scheduleData } = await axios.get(
          `${API_SCHEDULES}/${therapistId}`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );

        const scheduleList = scheduleData.result || [];
        if (!scheduleList.length) {
          setBookings([]);
          return;
        }

        const formattedBookings = await Promise.all(
          scheduleList.map(async (booking) => {
            try {
              const { data: appointmentData } = await axios.get(
                `${API_APPOINTMENT}/${booking.appointmentId}`,
                {
                  headers: { Authorization: `Bearer ${token}` },
                }
              );

              const customerInfo = appointmentData.result?.customerInfo || {};
              const serviceInfo = appointmentData.result?.serviceInfo || {};
              const time = appointmentData.result?.appointmentTime || {};
              const status = appointmentData.result?.status || {};

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
                status,
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

  const getStatusTag = (status) => {
    switch (status) {
      case "CREATED":
        return <Tag color="gold">CREATED</Tag>;
      case "CHECKEDIN":
        return <Tag color="blue">CHECKEDIN</Tag>;
      case "CHECKEDOUT":
        return <Tag color="orange">CHECKEDOUT</Tag>;
      case "COMPLETED":
        return <Tag color="green">COMPLETED</Tag>;
      case "CANCELLED":
        return <Tag color="red">CANCELLED</Tag>;
      default:
        return <Tag color="default">N/A</Tag>;
    }
  };

  const selectedBookings = bookings.filter((b) => b.date === selectedDate);

  const getStatusColor = (status) => {
    switch (status) {
      case "CREATED":
        return "gold";
      case "CHECKEDIN":
        return "blue";
      case "CHECKEDOUT":
        return "orange";
      case "COMPLETED":
        return "green";
      case "CANCELLED":
        return "red";
      default:
        return "default";
    }
  };

  const cellRender = (value) => {
    const dateStr = value.format("YYYY-MM-DD");
    const bookingsOnDate = bookings.filter((b) => b.date === dateStr);

    return bookingsOnDate.length > 0 ? (
      <div>
        {bookingsOnDate.map((booking, index) => (
          <Badge
            key={index}
            color={getStatusColor(booking.status)}
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
        <Calendar
          cellRender={cellRender}
          onSelect={onSelectDate}
          className={styles.calendar}
        />
      </div>
      <div className={styles.detailSection}>
        <Card
          title={<span className={styles.cardTitle}>Booking Details</span>}
          className={styles.card}
        >
          {selectedBookings.length > 0 ? (
            selectedBookings.map((booking, index) => (
              <div key={index} className={styles.bookingItem}>
                <h3 style={{ textAlign: "center" }}>
                  <strong>Appointment with:</strong> {booking.customer.fullName}
                </h3>
                <p>
                  <strong>Date:</strong> {booking.date}
                </p>
                <p>
                  <strong>Service:</strong> {booking.service.serviceName}
                </p>
                <p>
                  <strong>Price:</strong> {booking.service.servicePrice}
                </p>
                <p>
                  <strong>Customer:</strong> {booking.customer.fullName}
                </p>
                <p>
                  <strong>Email:</strong> {booking.customer.email}
                </p>
                <p>
                  <strong>Phone:</strong> {booking.customer.phone}
                </p>
                <p>
                  <strong>Time:</strong> {booking.time}
                </p>
                <p>
                  <strong>Status:</strong> {getStatusTag(booking.status)}
                </p>
                <hr />
              </div>
            ))
          ) : (
            <p className={styles.noBooking}>Choose date to view appointment!</p>
          )}
        </Card>
      </div>
    </div>
  );
};

export default TableTherapist;
