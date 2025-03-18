import React, { useEffect, useState } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import { List, Card, Spin, message, Modal } from "antd";
import styles from "./AppointmentPage.module.css";
import Header from "../../Components/Common/Header";
import {
  GET_APPOINTMENT_BY_CUSTOMER_API,
  GET_APPOINTMENT_BY_ID_API,
} from "../../config/apiConfig";

const AppointmentPage = () => {
  const [appointments, setAppointments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const [detailModalVisible, setDetailModalVisible] = useState(false);
  const [detailLoading, setDetailLoading] = useState(false);

  // Get auth state from Redux
  const { accessToken } = useSelector((state) => state.auth);

  useEffect(() => {
    const fetchAppointments = async () => {
      if (!accessToken) {
        message.error("No access token available");
        setLoading(false);
        return;
      }

      try {
        const response = await axios.get(GET_APPOINTMENT_BY_CUSTOMER_API, {
          headers: {
            Authorization: `Bearer ${accessToken}`,
          },
        });

        if (response.data.isSuccess) {
          setAppointments(response.data.result || []);
        } else {
          message.error(response.data.message || "Failed to load appointments");
        }
      } catch (error) {
        message.error(
          error.response?.data?.message ||
            `Failed to load appointments: ${error.message}`
        );
      } finally {
        setLoading(false);
      }
    };

    fetchAppointments();
  }, [accessToken]);

  const handleAppointmentClick = async (appointmentId) => {
    setDetailLoading(true);
    setDetailModalVisible(true);

    try {
      const response = await axios.get(
        GET_APPOINTMENT_BY_ID_API.replace("{appointmentId}", appointmentId),
        {
          headers: {
            Authorization: `Bearer ${accessToken}`,
          },
        }
      );

      if (response.data.isSuccess) {
        setSelectedAppointment(response.data.result);
      } else {
        message.error(
          response.data.message || "Failed to load appointment details"
        );
      }
    } catch (error) {
      message.error(
        error.response?.data?.message ||
          `Failed to load appointment details: ${error.message}`
      );
    } finally {
      setDetailLoading(false);
    }
  };

  const handleCloseModal = () => {
    setDetailModalVisible(false);
    setSelectedAppointment(null);
  };

  const renderAppointmentDetail = () => {
    if (!selectedAppointment) return null;

    const {
      customerInfo,
      serviceInfo,
      therapistInfo,
      appointmentDate,
      appointmentTime,
      createdTime,
      note,
      status,
    } = selectedAppointment;

    return (
      <div className={styles.appointmentDetail}>
        <h3>Appointment Details</h3>

        <div className={styles.detailSection}>
          <h4>Customer Information</h4>
          <p>
            <strong>Name:</strong> {customerInfo.customerName}
          </p>
          <p>
            <strong>Phone:</strong> {customerInfo.customerPhone}
          </p>
          <p>
            <strong>Email:</strong> {customerInfo.customerEmail}
          </p>
        </div>

        <div className={styles.detailSection}>
          <h4>Service Information</h4>
          <p>
            <strong>Service:</strong> {serviceInfo.serviceName}
          </p>
          <p>
            <strong>Price:</strong> {serviceInfo.servicePrice}
          </p>
          {serviceInfo.serviceDuration && (
            <p>
              <strong>Duration:</strong> {serviceInfo.serviceDuration}
            </p>
          )}
        </div>

        <div className={styles.detailSection}>
          <h4>Therapist Information</h4>
          <p>
            <strong>Name:</strong> {therapistInfo.therapistName}
          </p>
          <p>
            <strong>Age:</strong> {therapistInfo.therapistAge}
          </p>
        </div>

        <div className={styles.detailSection}>
          <h4>Appointment Information</h4>
          <p>
            <strong>Date:</strong> {appointmentDate}
          </p>
          <p>
            <strong>Time:</strong> {appointmentTime}
          </p>
          <p>
            <strong>Created:</strong> {createdTime}
          </p>
          <p>
            <strong>Status:</strong>{" "}
            <span className={styles.status}>{status}</span>
          </p>
          {note && (
            <p>
              <strong>Note:</strong> {note}
            </p>
          )}
        </div>
      </div>
    );
  };

  return (
    <div className={styles.container}>
      <Header />
      <h2>Your Appointments</h2>

      {loading ? (
        <div className={styles.spinnerContainer}>
          <Spin size="large" />
          <p>Loading your appointments...</p>
        </div>
      ) : appointments.length === 0 ? (
        <div className={styles.emptyState}>
          <p>You don't have any appointments scheduled.</p>
        </div>
      ) : (
        <List
          grid={{ gutter: 16, xs: 1, sm: 1, md: 1, lg: 1, xl: 1, xxl: 1 }}
          dataSource={appointments}
          renderItem={(appointment) => (
            <List.Item>
              <Card
                title={`Appointment on ${appointment.appointmentDate}`}
                className={styles.appointmentCard}
                hoverable
                onClick={() =>
                  handleAppointmentClick(appointment.appointmentId)
                }
              >
                <p>
                  <strong>Time:</strong> {appointment.appointmentTime}
                </p>
                <p>
                  <strong>Status:</strong>{" "}
                  <span className={styles.status}>{appointment.status}</span>
                </p>
                {appointment.note && (
                  <p>
                    <strong>Note:</strong> {appointment.note}
                  </p>
                )}
                <p>
                  <strong>Created:</strong>{" "}
                  {new Date(appointment.createdTime).toLocaleString()}
                </p>
              </Card>
            </List.Item>
          )}
        />
      )}

      <Modal
        title="Appointment Details"
        open={detailModalVisible}
        onCancel={handleCloseModal}
        footer={null}
        width={700}
      >
        {detailLoading ? (
          <div className={styles.modalLoading}>
            <Spin />
            <p>Loading appointment details...</p>
          </div>
        ) : (
          renderAppointmentDetail()
        )}
      </Modal>
    </div>
  );
};

export default AppointmentPage;
