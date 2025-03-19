import React, { useEffect, useState } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import { List, Card, Spin, message, Modal } from "antd";
import moment from "moment";
import styles from "./AppointmentPage.module.css";
import Header from "../../Components/Common/Header";
import {
  GET_APPOINTMENT_BY_CUSTOMER_API,
  GET_APPOINTMENT_BY_ID_API,
  GET_ALL_SLOTS_API,
  GET_THERAPIST_SCHEDULE_BY_THERAPIST_API,
} from "../../config/apiConfig";
import AppointmentDetail from "../../Components/Appointment/AppointmentDetail/AppointmentDetail";
import SlotBasedWeekView from "../../Components/Appointment/SlotBasedWeekView/SlotBasedWeekView";
import Sidebar from "../../Components/Appointment/Sidebar/Sidebar";

const AppointmentPage = () => {
  const [appointments, setAppointments] = useState([]);
  const [slots, setSlots] = useState([]);
  const [loading, setLoading] = useState(true);
  const [slotsLoading, setSlotsLoading] = useState(true);
  const [selectedDate, setSelectedDate] = useState(null);
  const [selectedAppointments, setSelectedAppointments] = useState([]);
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const [detailModalVisible, setDetailModalVisible] = useState(false);
  const [detailLoading, setDetailLoading] = useState(false);
  const [currentWeekStart, setCurrentWeekStart] = useState(
    moment().startOf("week")
  );
  const [selectedTherapistId, setSelectedTherapistId] = useState(null);

  // Get auth state from Redux
  const { accessToken } = useSelector((state) => state.auth);

  useEffect(() => {
    fetchAppointmentsData();
    fetchSlots();
  }, [accessToken, selectedTherapistId]);

  const formatDateToYYYYMMDD = (dateString) => {
    if (!dateString) return "";
    
    // Check if date is already in YYYY-MM-DD format
    if (/^\d{4}-\d{2}-\d{2}$/.test(dateString)) {
      return dateString;
    }
    
    // Handle M/D/YYYY format
    if (dateString.includes('/')) {
      const parts = dateString.split('/');
      if (parts.length === 3) {
        const month = parts[0].padStart(2, '0');
        const day = parts[1].padStart(2, '0');
        const year = parts[2];
        return `${year}-${month}-${day}`;
      }
    }
    
    // Fallback - try to parse with moment
    return moment(dateString).format("YYYY-MM-DD");
  };

  const fetchAppointmentsData = async () => {
    console.log("Fetching appointments...");
    setLoading(true);
    
    if (!accessToken) {
      message.error("No access token available");
      setLoading(false);
      return;
    }

    try {
      let response;
      
      if (selectedTherapistId) {
        // Fetch therapist's appointments
        console.log(`Fetching therapist schedule for ID: ${selectedTherapistId}`);
        const therapistUrl = GET_THERAPIST_SCHEDULE_BY_THERAPIST_API.replace("{therapistId}", selectedTherapistId);
        response = await axios.get(
          therapistUrl, 
          {
            headers: {
              Authorization: `Bearer ${accessToken}`,
            },
          }
        );
      } else {
        // Fetch customer's appointments
        response = await axios.get(GET_APPOINTMENT_BY_CUSTOMER_API, {
          headers: {
            Authorization: `Bearer ${accessToken}`,
          },
        });
      }

      if (response.data.isSuccess) {
        let appointmentsData = [];
        
        if (selectedTherapistId) {
          // Process therapist schedule data
          console.log("Raw therapist data:", response.data.result);
          
          // Check the structure of the response
          if (Array.isArray(response.data.result)) {
            // Handle array of therapist appointments
            appointmentsData = response.data.result.map(appointment => {
              return {
                ...appointment,
                appointmentId: appointment.appointmentId || `temp-${Math.random()}`,
                appointmentDate: formatDateToYYYYMMDD(appointment.appointmentDate),
                status: appointment.status || "SCHEDULED",
                // Ensure nested objects exist
                serviceInfo: appointment.serviceInfo || {},
                therapistInfo: appointment.therapistInfo || {},
                customerInfo: appointment.customerInfo || {}
              };
            });
          } else if (response.data.result && typeof response.data.result === 'object') {
            // Handle single therapist appointment object
            const appointment = response.data.result;
            appointmentsData = [{
              ...appointment,
              appointmentId: appointment.appointmentId || `temp-${Math.random()}`,
              appointmentDate: formatDateToYYYYMMDD(appointment.appointmentDate),
              status: appointment.status || "SCHEDULED",
              // Ensure nested objects exist
              serviceInfo: appointment.serviceInfo || {},
              therapistInfo: appointment.therapistInfo || {},
              customerInfo: appointment.customerInfo || {}
            }];
          }
        } else {
          // Process customer appointments data
          appointmentsData = Array.isArray(response.data.result) 
            ? response.data.result 
            : [response.data.result].filter(Boolean);
          
          // Ensure data consistency
          appointmentsData = appointmentsData.map(appointment => ({
            ...appointment,
            appointmentDate: formatDateToYYYYMMDD(appointment.appointmentDate)
          }));
        }
        
        console.log("Appointments fetched:", appointmentsData.length);
        console.log("Sample appointment data:", appointmentsData.length > 0 ? appointmentsData[0] : "No appointments");
        setAppointments(appointmentsData);
        
        // Reset selected date and appointments when switching between therapist/customer view
        setSelectedDate(null);
        setSelectedAppointments([]);
      } else {
        message.error(response.data.message || "Failed to load appointments");
      }
    } catch (error) {
      console.error("Error fetching appointments:", error);
      message.error(
        error.response?.data?.message ||
          `Failed to load appointments: ${error.message}`
      );
      // Set appointments to empty array on error
      setAppointments([]);
    } finally {
      setLoading(false);
    }
  };

  const fetchSlots = async () => {
    console.log("Fetching slots...");
    setSlotsLoading(true);

    if (!accessToken) {
      message.error("No access token available");
      setSlotsLoading(false);
      return;
    }

    try {
      const response = await axios.get(GET_ALL_SLOTS_API, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      if (response.data.isSuccess) {
        const slotsData = response.data.result || [];
        console.log("Slots fetched:", slotsData.length);
        // Sort slots by startTime
        const sortedSlots = slotsData.sort((a, b) => {
          return a.startTime.localeCompare(b.startTime);
        });
        setSlots(sortedSlots);
      } else {
        message.error(response.data.message || "Failed to load slots");
      }
    } catch (error) {
      console.error("Error fetching slots:", error);
      message.error(
        error.response?.data?.message ||
          `Failed to load slots: ${error.message}`
      );
    } finally {
      setSlotsLoading(false);
    }
  };

  const handleDateSelect = (date) => {
    console.log("Date selected:", date.format("YYYY-MM-DD"));
    setSelectedDate(date);
    const formattedDate = date.format("YYYY-MM-DD");
    
    const filteredAppointments = appointments.filter(appointment => {
      const appointmentDate = formatDateToYYYYMMDD(appointment.appointmentDate);
      return appointmentDate === formattedDate;
    });
    
    console.log("Filtered appointments:", filteredAppointments.length);
    setSelectedAppointments(filteredAppointments);
  };

  const handleAppointmentClick = async (appointmentId) => {
    console.log("Appointment clicked:", appointmentId);
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
      console.error("Error fetching appointment details:", error);
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

  const handlePreviousWeek = () => {
    setCurrentWeekStart(moment(currentWeekStart).subtract(1, "week"));
  };

  const handleNextWeek = () => {
    setCurrentWeekStart(moment(currentWeekStart).add(1, "week"));
  };

  const handleTherapistChange = (therapistId) => {
    console.log("Therapist changed:", therapistId);
    setSelectedTherapistId(therapistId);
  };

  return (
    <div className={styles.container}>
      <Header />
      <h2>
        {selectedTherapistId ? "Therapist Schedule" : "Your Appointments"}
      </h2>

      {loading ? (
        <div className={styles.spinnerContainer || "spinner"}>
          <Spin size="large" />
          <p>Loading appointments...</p>
        </div>
      ) : (
        <>
          <div className={styles.appointmentContainer}>
            <div className={styles.calendarSection}>
              <SlotBasedWeekView
                slots={slots}
                slotsLoading={slotsLoading}
                currentWeekStart={currentWeekStart}
                handlePreviousWeek={handlePreviousWeek}
                handleNextWeek={handleNextWeek}
                handleDateSelect={handleDateSelect}
                handleAppointmentClick={handleAppointmentClick}
                appointments={appointments}
              />
            </div>

            <div className={styles.sidebarSection}>
              <Sidebar onTherapistChange={handleTherapistChange} />
            </div>
          </div>
          <div className={styles.detailsSection}>
            {selectedDate ? (
              <>
                <h3>
                  {selectedTherapistId
                    ? `Schedule on ${selectedDate.format("MMMM D, YYYY")}`
                    : `Appointments on ${selectedDate.format("MMMM D, YYYY")}`}
                </h3>
                {selectedAppointments.length > 0 ? (
                  <List
                    dataSource={selectedAppointments}
                    renderItem={(appointment) => (
                      <List.Item key={appointment.appointmentId || Math.random()}>
                        <Card
                          title={`Time: ${appointment.appointmentTime}`}
                          className={styles.appointmentCard}
                          hoverable
                          onClick={() =>
                            handleAppointmentClick(appointment.appointmentId)
                          }
                        >
                          <p>
                            <strong>Status:</strong>{" "}
                            <span className={styles.status}>
                              {appointment.status}
                            </span>
                          </p>
                          {appointment.serviceInfo && appointment.serviceInfo.serviceName && (
                            <p>
                              <strong>Service:</strong>{" "}
                              {appointment.serviceInfo.serviceName}
                            </p>
                          )}
                          {appointment.therapistInfo && appointment.therapistInfo.therapistName && (
                            <p>
                              <strong>Therapist:</strong>{" "}
                              {appointment.therapistInfo.therapistName}
                            </p>
                          )}
                          {selectedTherapistId && appointment.customerInfo && appointment.customerInfo.customerName && (
                            <p>
                              <strong>Customer:</strong>{" "}
                              {appointment.customerInfo.customerName}
                            </p>
                          )}
                          {appointment.note && (
                            <p>
                              <strong>Note:</strong> {appointment.note}
                            </p>
                          )}
                        </Card>
                      </List.Item>
                    )}
                  />
                ) : (
                  <p>No appointments on this day</p>
                )}
              </>
            ) : (
              <p>Select a date to see appointments</p>
            )}
          </div>
        </>
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
          <AppointmentDetail appointment={selectedAppointment} />
        )}
      </Modal>
    </div>
  );
};

export default AppointmentPage;