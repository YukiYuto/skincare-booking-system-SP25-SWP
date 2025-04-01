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

const formatDateToYYYYMMDD = (dateString) => {
  if (!dateString) return null;

  try {
    if (/^\d{4}-\d{2}-\d{2}$/.test(dateString)) return dateString;

    const date = moment(dateString);
    return date.isValid() ? date.format("YYYY-MM-DD") : null;
  } catch (error) {
    console.error("Error formatting date:", error);
    return null;
  }
};

const parseAppointmentTime = (timeString) => {
  if (!timeString) return null;
  try {
    return timeString.split(" - ")[0];
  } catch (error) {
    console.error("Error parsing appointment time:", error);
    return null;
  }
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

  const { accessToken } = useSelector((state) => state.auth);

  useEffect(() => {
    const loadData = async () => {
      await fetchSlots();
      await fetchCustomerTimetable();
    };
    loadData();
  }, [accessToken]);

  const fetchSlots = async () => {
    setSlotsLoading(true);

    if (!accessToken) {
      message.error("No access token available");
      setSlotsLoading(false);
      return Promise.resolve();
    }

    try {
      const response = await axios.get(GET_ALL_SLOTS_API, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });

      if (response.data.isSuccess) {
        const slotsData = response.data.result || [];
        const sortedSlots = slotsData.sort((a, b) =>
          a.startTime.localeCompare(b.startTime)
        );
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

  const fetchCustomerTimetable = async () => {
    setLoading(true);

    if (!accessToken) {
      message.error("No access token available");
      setLoading(false);
      return Promise.resolve();
    }

    try {
      const response = await axios.get(GET_CUSTOMER_TIMETABLE_API, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });

      if (response.data.isSuccess) {
        const timetableData = response.data.result || {};
        const appointmentsData = timetableData.appointments || [];

        const processedAppointments = appointmentsData.map((appointment) => ({
          ...appointment,
          appointmentDate: formatDateToYYYYMMDD(appointment.appointmentDate),
          status: appointment.scheduleStatus === 0 ? "CREATED" : "SCHEDULED",
        }));

        setAppointments(processedAppointments);
        console.log(processedAppointments);
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
      setAppointments([]);
    } finally {
      setLoading(false);
    }
  };

  const handleDateSelect = (date) => {
    setSelectedDate(date);
    const formattedDate = date.format("YYYY-MM-DD");

    const filteredAppointments = appointments.filter((appointment) => {
      const appointmentDate = formatDateToYYYYMMDD(appointment.appointmentDate);
      return appointmentDate === formattedDate;
    });

    setSelectedAppointments(filteredAppointments);
  };

  const handleAppointmentClick = async (appointmentId) => {
    setDetailLoading(true);
    setDetailModalVisible(true);

    try {
      const response = await axios.get(
        GET_APPOINTMENT_BY_ID_API.replace("{appointmentId}", appointmentId),
        {
          headers: { Authorization: `Bearer ${accessToken}` },
        }
      );

      if (response.data.isSuccess) {
        const appointmentData = response.data.result;

        if (appointmentData.appointmentDate) {
          appointmentData.appointmentDate = formatDateToYYYYMMDD(
            appointmentData.appointmentDate
          );
        }

        if (appointmentData.slotId) {
          const matchingSlot = slots.find(
            (s) => s.slotId === appointmentData.slotId
          );
          if (matchingSlot) {
            appointmentData.matchingSlot = matchingSlot;
          }
        }
        console.log({ appointmentId, ...appointmentData });
        setSelectedAppointment({ appointmentId, ...appointmentData });
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

  return (
    <div>
      <Header />
      <div className={styles.container}>
        {loading ? (
          <div className={styles.spinnerContainer || "spinner"}>
            <Spin size="large" />
            <p>Loading appointments...</p>
          </div>
        ) : (
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
    </div>
  );
};

export default AppointmentPage;
