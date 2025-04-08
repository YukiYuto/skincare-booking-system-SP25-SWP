/* eslint-disable no-unused-vars */
import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { ConfigProvider, Button, Tag, Table } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import "antd/dist/reset.css"; // Import CSS của Ant Design
import styles from "./AppointmentTable.module.css";
import { getTodayAppointments } from "../../../services/staffService";
import { apiCall } from "../../../utils/apiUtils";
import { GET_APPOINTMENT_BY_ID_API } from "../../../config/apiConfig";
import AppointmentDetail from "../../Appointment/AppointmentDetail/AppointmentDetail";
import AppointmentDetails from "./AppointmentDetails";

const AppointmentTable = () => {
  const [appointments, setAppointments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [visible, setVisible] = useState(false);
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchTodayAppointments = async () => {
      setLoading(true);
      try {
        const response = await getTodayAppointments();
        setAppointments(response.result.appointments);
      } catch (error) {
        console.error("Fetch today appointments failed: ", error);
      } finally {
        setLoading(false);
      }
    };

    fetchTodayAppointments();
  }, []);

  const handleViewDetails = async (appointmentId) => {
    navigate(`/staff/appointments/${appointmentId}`);
  };

  const changeStatus = (status) => {
    if (status == 0) {
      return "Pending";
    } else if (status == 1) {
      return "Checked In";
    } else if (status == 2) {
      return "Checked Out";
    } else {
      return "Canceled";
    }
  };
  const statusColor = (status) => {
    if (status === 0) {
      return "orange";
    } else if (status === 1) {
      return "blue";
    } else if (status === 2) {
      return "green";
    } else {
      return "red";
    }
  };
  const columns = [
    { title: "Therapist", dataIndex: "therapist", key: "therapist" },
    { title: "Customer", dataIndex: "customer", key: "customer" },
    { title: "Time", dataIndex: "time", key: "time" },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      render: (status) => (
        <Tag color={statusColor(status)}>{changeStatus(status)}</Tag>
      ),
    },
    {
      title: "Action",
      key: "action",
      render: (_, record) => (
        <Button
          type="link"
          onClick={() => handleViewDetails(record.appointmentId)}
        >
          View Details
        </Button>
      ),
    },
  ];

  return (
    <div className={styles.container}>
      <ConfigProvider
        theme={{
          components: {
            Button: {
              colorPrimary: "#b38b4c",
              algorithm: true,
            },
          },
        }}
      >
        <div className={styles.headingSection}>
          <h2 className={styles.headingText}>Dashboard</h2>
        </div>
        <div className={styles.appointmentCard}>
          <div className={styles.titleSection}>
            <h3 className={styles.title}>Today{`'`}s Appointments</h3>
          </div>
          <div className={styles.tableSection}>
            <Table
              columns={columns}
              dataSource={appointments}
              loading={loading}
              rowKey="id"
              pagination={{ pageSize: 3 }}
            />
          </div>
        </div>
      </ConfigProvider>
    </div>
  );
};

export default AppointmentTable;
