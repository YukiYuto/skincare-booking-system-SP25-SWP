import { useState, useEffect } from "react";
import { useSelector } from "react-redux";
import { Table, Spin, Alert, Card, Button } from "antd";
import axios from "axios";
import dayjs from "dayjs";
import styles from "./TherapistDashboard.module.css"; 
import { GET_ALL_THERAPISTS_API } from "../../config/apiConfig";
import { useNavigate } from "react-router-dom";

const API_SCHEDULES = "https://lumiconnect.azurewebsites.net/api/therapist-schedules/therapist";
const API_APPOINTMENT = "https://lumiconnect.azurewebsites.net/api/appointment";

const TherapistDashboard = () => {
  const user = useSelector((state) => state.auth.user);
  const token = useSelector((state) => state.auth.accessToken);
  const navigate = useNavigate();

  const [appointments, setAppointments] = useState([]);
  const [upcomingAppointments, setUpcomingAppointments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [monthlyAppointments, setMonthlyAppointments] = useState(0);
const [yearlyAppointments, setYearlyAppointments] = useState(0);
const [monthlyRevenue, setMonthlyRevenue] = useState(0);
const [yearlyRevenue, setYearlyRevenue] = useState(0);

const handleViewDetails = async (appointmentId) => {
  navigate(`/therapist/appointments/${appointmentId}`);
};

useEffect(() => {
  const fetchAppointments = async () => {
    try {
      if (!token) throw new Error("Vui lòng đăng nhập!");

      const { data: therapistsData } = await axios.get(GET_ALL_THERAPISTS_API, {
        headers: { Authorization: `Bearer ${token}` },
      });

      const therapistsList = Array.isArray(therapistsData.result) ? therapistsData.result : [];
      if (!therapistsList.length) throw new Error("Không tìm thấy therapist!");

      const matchedTherapist = therapistsList.find((t) => t.phoneNumber === user.phoneNumber);
      if (!matchedTherapist) throw new Error("Không tìm thấy therapist!");

      const therapistId = matchedTherapist.skinTherapistId;
      const today = dayjs().format("YYYY-MM-DD");
      const tomorrow = dayjs().add(1, "day").format("YYYY-MM-DD");
      const twoDaysLater = dayjs().add(2, "day").format("YYYY-MM-DD");
      const currentMonth = dayjs().format("MM");
      const currentYear = dayjs().format("YYYY");

      const { data: scheduleData } = await axios.get(`${API_SCHEDULES}/${therapistId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });

      const scheduleList = scheduleData.result || [];
      if (!scheduleList.length) {
        setAppointments([]);
        setUpcomingAppointments([]);
        setMonthlyAppointments(0);
        setYearlyAppointments(0);
        setMonthlyRevenue(0);
        setYearlyRevenue(0);
        return;
      }

      let monthlyCount = 0;
      let yearlyCount = 0;
      let monthlySum = 0;
      let yearlySum = 0;

      const todayAppointments = [];
      const upcomingAppointments = [];

      for (const booking of scheduleList) {
        const appointmentDate = dayjs(booking.appointmentDate).format("YYYY-MM-DD");
        const appointmentMonth = dayjs(booking.appointmentDate).format("MM");
        const appointmentYear = dayjs(booking.appointmentDate).format("YYYY");

        try {
          const { data: appointmentData } = await axios.get(`${API_APPOINTMENT}/${booking.appointmentId}`, {
            headers: { Authorization: `Bearer ${token}` },
          });

          const customerInfo = appointmentData.result?.customerInfo || {};
          const serviceInfo = appointmentData.result?.serviceInfo || {};
          const time = appointmentData.result?.appointmentTime || "N/A";
          const status = appointmentData.result?.status || "N/A";
          const servicePrice = Number(appointmentData.result?.serviceInfo.servicePrice) || 0; 

          if (status === "CHECKEDOUT") {
            if (appointmentYear === currentYear) {
              yearlyCount++; 
              yearlySum += servicePrice;

              if (appointmentMonth === currentMonth) {
                monthlyCount++; 
                monthlySum += servicePrice;
              }
            }
          }

          const formattedAppointment = {
            id: booking.appointmentId,
            date: appointmentDate,
            service: serviceInfo.serviceName || "Không xác định",
            customerName: customerInfo.customerName || "Không xác định",
            phone: customerInfo.customerPhone || "N/A",
            email: customerInfo.customerEmail || "N/A",
            status,
            time,
          };

          if (appointmentDate === today) {
            todayAppointments.push(formattedAppointment);
          } else if (appointmentDate === tomorrow || appointmentDate === twoDaysLater) {
            upcomingAppointments.push(formattedAppointment);
          }
        } catch (err) {
          console.error("Lỗi khi lấy thông tin lịch hẹn:", err);
        }
      }

      setAppointments(todayAppointments);
      setUpcomingAppointments(upcomingAppointments);
      setMonthlyAppointments(monthlyCount);
      setYearlyAppointments(yearlyCount);
      setMonthlyRevenue(monthlySum);
      setYearlyRevenue(yearlySum);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  if (user?.fullName) {
    fetchAppointments();
  } else {
    setLoading(false);
  }
}, [user, token]);

  const getStatusTag = (status) => {
    switch (status) {
      case "CREATED":
        return <span className={`${styles.statusTag} ${styles.statusCreated}`}>CREATED</span>;
      case "CHECKEDIN":
        return <span className={`${styles.statusTag} ${styles.statusCheckedIn}`}>CHECKEDIN</span>;
      case "CHECKEDOUT":
        return <span className={`${styles.statusTag} ${styles.statusCheckedOut}`}>CHECKEDOUT</span>;
      case "COMPLETED":
        return <span className={`${styles.statusTag} ${styles.statusCompleted}`}>COMPLETED</span>;
      case "CANCELLED":
        return <span className={`${styles.statusTag} ${styles.statusCancelled}`}>CANCELLED</span>;
      default:
        return <span className={`${styles.statusTag}`}>N/A</span>;
    }
  };
  
  const columns = [
    {
      title: "Customer",
      dataIndex: "customerName",
      key: "customerName",
    },
    {
      title: "Service",
      dataIndex: "service",
      key: "service",
    },
    {
      title: "Date",
      dataIndex: "date",
      key: "date",
    },
    {
      title: "Time",
      dataIndex: "time",
      key: "time",
    },
    {
      title: "Status",
      key: "status",
      render: (_, record) => getStatusTag(record.status),
    },
    {
      title: "Action",
      key: "action",
      render: (_, record) => (
        <Button type="link" onClick={() => handleViewDetails(record.id)}>
          View Details
        </Button>
      ),
    }
  ];
  

  if (loading) return <Spin tip="Loading..." />;
  if (error) return <Alert message={error} type="error" showIcon />;

  return (
    <div className={styles.container}>
    <div className={styles.statsContainer}>
      <Card title="📅 Total Appointments This Month" className={styles.statsCard}>
        <h2>{monthlyAppointments}</h2>
      </Card>
      <Card title="📆 Total Appointments This Year" className={styles.statsCard}>
        <h2>{yearlyAppointments}</h2>
      </Card>
      <Card title="💰 Revenue This Month" className={styles.statsCard}>
        <h2>{monthlyRevenue.toLocaleString()} VND</h2>
      </Card>
      <Card title="💵 Revenue This Year" className={styles.statsCard}>
        <h2>{yearlyRevenue.toLocaleString()} VND</h2>
      </Card>
    </div>
      <Card title="📅 Appointment Today" className={styles.card}>
        {appointments.length > 0 ? (
          <Table columns={columns} dataSource={appointments} rowKey="id" pagination={false} />
        ) : (
          <p className={styles.noAppointments}>No appointment today!</p>
        )}
      </Card>

      <Card title="🔜 Appointment Incoming" className={styles.card} style={{ marginTop: "20px" }}>
        {upcomingAppointments.length > 0 ? (
          <Table columns={columns} dataSource={upcomingAppointments} rowKey="id" pagination={false} />
        ) : (
          <p className={styles.noAppointments}>No appointment incoming!</p>
        )}
      </Card>
    </div>
  );
};

export default TherapistDashboard;