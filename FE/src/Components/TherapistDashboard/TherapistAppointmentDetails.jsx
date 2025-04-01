/* eslint-disable no-unused-vars */
import React, { useEffect, useState } from "react";
import StatusProgress from "../Appointment/Status/StatusProgress";
import styles from "./TherapistAppointmentDetails.module.css";
import { Avatar, Button, Card, Spin } from "antd";
import { useNavigate, useParams } from "react-router-dom";
import { getAppointmentById } from "../../services/staffService";
import { toast } from "react-toastify";
import { PLACEHOLDER_IMAGE_URL } from "../../utils/avatarUtils";
import { completeService } from "../../services/therapistService";
const { Meta } = Card;

const TherapistAppointmentDetails = () => {
  const { appointmentId } = useParams();
  const navigate = useNavigate();
  const [appointment, setAppointment] = useState(null);
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState(false);

  useEffect(() => {
    const fetchAppointment = async () => {
      try {
        const response = await getAppointmentById(appointmentId);
        console.log(response.result);
        setAppointment(response.result);
      } catch (error) {
        toast.error("Fetch appointment failed. " + error.message);
        console.error("Fetch appointment failed: ", error);
      } finally {
        setLoading(false);
      }
    };

    fetchAppointment();
  }, [appointmentId]);

  const handleComplete = async () => {
    if (!appointment) return;
    console.log(appointmentId);
    setUpdating(true);
    try {
      await completeService(appointmentId);
      setAppointment((prev) => ({ ...prev, status: "COMPLETED" }));
      toast.success("Service completed successfully!");
    } catch (error) {
      toast.error("Complete service failed. " + error.message);
      console.error("Complete service failed: ", error);
    } finally {
      setUpdating(false);
    }
  };

  if (loading) return <Spin size="large" />;
  if (!appointment) return <div>Appointment not found</div>;

  return (
    <div className={styles.container}>
      <div className={styles.detailGrid}>
        <div className={styles.title}>
          <Button type="link" onClick={() => navigate(-1)}>
            Back
          </Button>
          <span>Appointment Details</span>
        </div>
        <div className={styles.appointment}>
          <Card title="Appointment Info" variant="outlined">
            <div className={styles.appointmentInfoContainer}>
              <div>
                <p>
                  <strong>Date: </strong>{" "}
                  {new Date(appointment.appointmentDate).toLocaleDateString(
                    "en-GB"
                  )}
                </p>
                <p>
                  <strong>Time: </strong> {appointment.appointmentTime}
                </p>
              </div>
              <div>
                <p>
                  <strong>Service: </strong>{" "}
                  {appointment.serviceInfo?.serviceName}
                </p>
              </div>
            </div>
            <StatusProgress status={appointment.status} />

            <div className={styles.buttonGroup}>
              {appointment.status === "CHECKEDIN" && (
                <>
                  <Button
                    color='cyan'
                    variant="filled"
                    size="large"
                    onClick={handleComplete}
                    loading={updating}
                  >
                    Complete Service
                  </Button>
                </>
              )}
            </div>
          </Card>
        </div>
        <div className={styles.customer}>
          <Card title="Customer Info" variant="outlined">
            <Meta
              avatar={
                <Avatar
                  src={
                    appointment.customerInfo?.customerAvatar ??
                    `${PLACEHOLDER_IMAGE_URL}${appointment.customerInfo?.customerName}`
                  }
                  size={50}
                />
              }
              title={appointment.customerInfo?.customerName}
              description={
                <>
                  <p>
                    <strong>Skin profile:</strong>{" "}
                    {appointment.customerInfo?.customerSkinProfile ??
                      "Not provided"}
                  </p>
                  <p>
                    <strong>Age:</strong>{" "}
                    {appointment.customerInfo?.customerAge ?? "Not provided"}
                  </p>
                  <p>
                    <strong>Gender:</strong>{" "}
                    {appointment.customerInfo?.customerGender ?? "Not provided"}
                  </p>
                </>
              }
            />
          </Card>
        </div>
        <div className={styles.therapist}>
          <Card title="Therapist Info" variant="outlined">
            <Meta
              avatar={
                <Avatar
                  src={
                    appointment.therapistInfo?.therapistAvatarUrl.length != 0
                      ? appointment.therapistInfo?.therapistAvatarUrl
                      : `${PLACEHOLDER_IMAGE_URL}${appointment.therapistInfo?.therapistName}}`
                  }
                  size={50}
                />
              }
              title={appointment.therapistInfo?.therapistName}
              description={
                <>
                  <p>
                    <strong>Experience:</strong>{" "}
                    {appointment.therapistInfo?.therapistExperience ?? "1x"}{" "}
                    years
                  </p>
                  <p>
                    <strong>Age:</strong>{" "}
                    {appointment.therapistInfo?.therapistAge ?? "Not provided"}
                  </p>
                  <p>
                    <strong>Gender:</strong>{" "}
                    {appointment.therapistInfo?.therapistGender ??
                      "Not provided"}
                  </p>
                </>
              }
            />
          </Card>
        </div>
      </div>
    </div>
  );
};
export default TherapistAppointmentDetails;
