import React, { useEffect, useState } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import { List, Card, Spin, message, Modal } from "antd";
import moment from "moment";
import styles from "./AppointmentPage.module.css";
import Header from "../../Components/Common/Header";
import {
  GET_CUSTOMER_TIMETABLE_API,
  GET_APPOINTMENT_BY_ID_API,
  GET_ALL_SLOTS_API,
} from "../../config/apiConfig";
import AppointmentDetail from "../../Components/Appointment/AppointmentDetail/AppointmentDetail";
import SlotBasedWeekView from "../../Components/Appointment/SlotBasedWeekView/SlotBasedWeekView";

// Helper function to format date to YYYY-MM-DD
const formatDateToYYYYMMDD = (dateString) => {
  if (!dateString) return null;

  try {
    // If already in YYYY-MM-DD format, return as is
    if (/^\d{4}-\d{2}-\d{2}$/.test(dateString)) {
      return dateString;
    }

    // Try to parse the date using moment
    const date = moment(dateString);
    if (date.isValid()) {
      return date.format("YYYY-MM-DD");
    }

    return null;
  } catch (error) {
    console.error("Error formatting date:", error);
    return null;
  }
};

// Helper function to parse appointment times
const parseAppointmentTime = (timeString) => {
  if (!timeString) return null;

  try {
    // Format: "12:00:00 - 13:00:00"
    const startTime = timeString.split(" - ")[0]; // Get "12:00:00"
    return startTime; // Return just the start time
  } catch (error) {
    console.error("Error parsing appointment time:", error);
    return null;
  }
};

// Helper function to check if appointment matches a slot
const doesAppointmentMatchSlot = (appointment, slot) => {
  // First, check if the appointment has a slotId that matches the slot
  if (appointment.slotId && slot.slotId && appointment.slotId === slot.slotId) {
    console.log(
      `Direct slotId match found between appointment ${appointment.appointmentId} and slot ${slot.slotId}`
    );
    return true;
  }

  // If no direct slotId match, try to match by time
  const appointmentStartTime = parseAppointmentTime(
    appointment.appointmentTime
  );
  if (!appointmentStartTime || !slot.startTime) return false;

  // Compare just the hour and minute parts
  const appointmentHourMinute = appointmentStartTime.substring(0, 5); // "12:00"
  const slotHourMinute = slot.startTime.substring(0, 5); // Assuming slot.startTime is like "12:00:00"

  console.log(
    `Comparing appointment time ${appointmentHourMinute} with slot time ${slotHourMinute}`
  );
  return appointmentHourMinute === slotHourMinute;
};

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

  // Get auth state from Redux
  const { accessToken } = useSelector((state) => state.auth);

  useEffect(() => {
    // First fetch slots, then fetch appointments
    const loadData = async () => {
      await fetchSlots();
      await fetchCustomerTimetable();
    };

    loadData();
  }, [accessToken]);

  // Fetch time slots
  const fetchSlots = async () => {
    console.log("Fetching slots...");
    setSlotsLoading(true);

    if (!accessToken) {
      message.error("No access token available");
      setSlotsLoading(false);
      return Promise.resolve();
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

        // Log the first slot object to inspect its structure
        if (slotsData.length > 0) {
          console.log("Sample slot object:", slotsData[0]);
        }

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
      return Promise.resolve();
    }
  };

  // Fetch customer timetable
  const fetchCustomerTimetable = async () => {
    console.log("Fetching customer timetable...");
    setLoading(true);

    if (!accessToken) {
      message.error("No access token available");
      setLoading(false);
      return Promise.resolve();
    }

    try {
      const response = await axios.get(GET_CUSTOMER_TIMETABLE_API, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      if (response.data.isSuccess) {
        const timetableData = response.data.result || {};
        const appointmentsData = timetableData.appointments || [];

        // Process appointments data
        const processedAppointments = appointmentsData.map((appointment) => ({
          ...appointment,
          appointmentDate: formatDateToYYYYMMDD(appointment.appointmentDate),
          status: appointment.scheduleStatus === 0 ? "CREATED" : "SCHEDULED", // Map status based on scheduleStatus
        }));

        console.log("Appointments fetched:", processedAppointments.length);

        // Log the first appointment object to inspect its structure
        if (processedAppointments.length > 0) {
          console.log("Sample appointment object:", processedAppointments[0]);
        }

        setAppointments(processedAppointments);

        // Reset selected date and appointments
        setSelectedDate(null);
        setSelectedAppointments([]);
      } else {
        message.error(response.data.message || "Failed to load timetable");
      }
    } catch (error) {
      console.error("Error fetching customer timetable:", error);
      message.error(
        error.response?.data?.message ||
          `Failed to load timetable: ${error.message}`
      );
      // Set appointments to empty array on error
      setAppointments([]);
    } finally {
      setLoading(false);
    }
  };

  const handleDateSelect = (date) => {
    console.log("Date selected:", date.format("YYYY-MM-DD"));
    setSelectedDate(date);
    const formattedDate = date.format("YYYY-MM-DD");

    const filteredAppointments = appointments.filter((appointment) => {
      const appointmentDate = formatDateToYYYYMMDD(appointment.appointmentDate);
      return appointmentDate === formattedDate;
    });

    console.log("Filtered appointments:", filteredAppointments.length);

    // Match appointments with slots
    filteredAppointments.forEach((appointment) => {
      console.log("Checking appointment:", appointment);
      const matchingSlot = slots.find((slot) =>
        doesAppointmentMatchSlot(appointment, slot)
      );

      if (matchingSlot) {
        console.log(
          `Appointment ${appointment.appointmentId} matches slot ${matchingSlot.slotId}`
        );
        console.log("Appointment slotId:", appointment.slotId);
        console.log("Slot slotId:", matchingSlot.slotId);
        console.log("Appointment time:", appointment.appointmentTime);
        console.log(
          "Slot time:",
          matchingSlot.startTime,
          "-",
          matchingSlot.endTime
        );
      } else {
        console.log(
          `No matching slot found for appointment ${appointment.appointmentId}`
        );
        console.log("Appointment slotId:", appointment.slotId);
        console.log("Appointment time:", appointment.appointmentTime);

        // Check if there's a slot ID issue - list all available slotIds
        console.log("Available slot IDs:");
        slots.forEach((s) => console.log(s.slotId));
      }
    });

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
        // Process the appointment data to ensure all fields are properly formatted
        const appointmentData = response.data.result;

        // Format the date if it exists
        if (appointmentData.appointmentDate) {
          appointmentData.appointmentDate = formatDateToYYYYMMDD(
            appointmentData.appointmentDate
          );
        }

        // Log the full appointment detail
        console.log("Full appointment details:", appointmentData);

        // Find matching slot information
        if (appointmentData.slotId) {
          const matchingSlot = slots.find(
            (s) => s.slotId === appointmentData.slotId
          );
          if (matchingSlot) {
            console.log("Found matching slot for detail view:", matchingSlot);
            appointmentData.matchingSlot = matchingSlot;
          }
        }

        setSelectedAppointment(appointmentData);
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

  // Function to get slot info by ID
  const getSlotById = (slotId) => {
    return slots.find((slot) => slot.slotId === slotId);
  };

  return (
    <div className={styles.container}>
      <Header />

      {loading ? (
        <div className={styles.spinnerContainer || "spinner"}>
          <Spin size="large" />
          <p>Loading appointments...</p>
        </div>
      ) : (
        <>
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

          <div className={styles.detailsSection}>
            {selectedDate ? (
              <>
                <h3>Appointments on {selectedDate.format("MMMM D, YYYY")}</h3>
                {selectedAppointments.length > 0 ? (
                  <List
                    dataSource={selectedAppointments}
                    renderItem={(appointment) => {
                      // Find matching slot
                      const matchingSlot = appointment.slotId
                        ? getSlotById(appointment.slotId)
                        : slots.find((slot) =>
                            doesAppointmentMatchSlot(appointment, slot)
                          );

                      return (
                        <List.Item
                          key={appointment.appointmentId || Math.random()}
                        >
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
                            {appointment.slotId && (
                              <p>
                                <strong>Slot ID:</strong> {appointment.slotId}
                              </p>
                            )}
                            {matchingSlot && (
                              <p>
                                <strong>Slot Time:</strong>{" "}
                                {matchingSlot.startTime} -{" "}
                                {matchingSlot.endTime}
                              </p>
                            )}
                          </Card>
                        </List.Item>
                      );
                    }}
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
          <>
            <AppointmentDetail appointment={selectedAppointment} />
            {selectedAppointment && selectedAppointment.slotId && (
              <div
                style={{
                  marginTop: "20px",
                  padding: "10px",
                  border: "1px solid #eee",
                  borderRadius: "4px",
                }}
              >
                <h4>Associated Slot Information</h4>
                {(() => {
                  const slotInfo = getSlotById(selectedAppointment.slotId);
                  return slotInfo ? (
                    <div>
                      <p>
                        <strong>Slot Time:</strong> {slotInfo.startTime} -{" "}
                        {slotInfo.endTime}
                      </p>
                      <p>
                        <strong>Slot Status:</strong> {slotInfo.status}
                      </p>
                      <p>
                        <strong>Created By:</strong> {slotInfo.createdBy}
                      </p>
                    </div>
                  ) : (
                    <p>No matching slot information found.</p>
                  );
                })()}
              </div>
            )}
          </>
        )}
      </Modal>
    </div>
  );
};

export default AppointmentPage;
