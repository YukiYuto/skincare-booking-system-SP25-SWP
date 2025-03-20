import { useState } from "react";
import { DatePicker, Spin } from "antd";
import "antd/dist/reset.css"; // Import CSS của Ant Design
import styles from "./TableStaff.module.css";

const API_URL = "https://lumiconnect.azurewebsites.net/api/appointment";

const TableStaff = () => {
  const [selectedDate, setSelectedDate] = useState(null);
  const [appointments, setAppointments] = useState([]);
  const [loading, setLoading] = useState(false);

  const fetchAppointments = async (date) => {
    if (!date) return;

    setLoading(true);
    try {
      const response = await fetch(API_URL);
      const data = await response.json();
      
      if (data.result) {
        // Lọc lịch hẹn theo ngày được chọn
        const filteredAppointments = data.result.filter(
          (appointment) => appointment.appointmentDate === date
        );
        setAppointments(filteredAppointments);
      } else {
        setAppointments([]);
      }
    } catch (error) {
      console.error("Error fetching appointments:", error);
      setAppointments([]);
    }
    setLoading(false);
  };

  const handleDateChange = (date, dateString) => {
    setSelectedDate(dateString);
    fetchAppointments(dateString);
  };

  return (
    <div className={styles.container}>
      <h1>Staff Booking Management</h1>

      <div className={styles.datePickerContainer}>
        <b>Select Date: </b>
        <DatePicker 
          onChange={handleDateChange} 
          format="YYYY-MM-DD" 
          className={styles.datePicker} 
        />
      </div>

      {loading ? (
        <Spin size="large" className={styles.spinner} />
      ) : (
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Stylist Name</th>
              <th>Customer Name</th>
              <th>Phone</th>
              <th>Time</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {appointments.length > 0 ? (
              appointments.map((appointment) => (
                <tr key={appointment.appointmentId}>
                  <td>{appointment.createdBy || "N/A"}</td>
                  <td>{appointment.customer?.name || "Unknown"}</td>
                  <td>{appointment.customer?.phone || "No Phone"}</td>
                  <td>{appointment.appointmentTime || "No Time"}</td>
                  <td>{appointment.status}</td>
                  <td>
                    <button className={styles.actionButton}>View</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="6" className={styles.noData}>
                  No bookings found for the selected date.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      )}

      <p className={styles.footer}>
        Bookings for {selectedDate ? selectedDate : "Select a date"}.
      </p>
    </div>
  );
};

export default TableStaff;