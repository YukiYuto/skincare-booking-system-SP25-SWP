import { useState } from "react";
import { DatePicker } from "antd";
import "antd/dist/reset.css"; // Import CSS của Ant Design
import styles from "./TableStaff.module.css";

const TableStaff = () => {
  const [selectedDate, setSelectedDate] = useState(null);

  const handleDateChange = (date, dateString) => {
    setSelectedDate(dateString);
  };

  return (
    <div className={styles.container}>
      <h1>Staff Booking Management</h1>

      <div className={styles.datePickerContainer}>
        <b>Select Date: </b>
        <DatePicker 
          onChange={handleDateChange} 
          format="MM/DD/YYYY" 
          className={styles.datePicker} 
        />
      </div>

      <table className={styles.table}>
        <thead>
          <tr>
            <th>Stylist Name</th>
            <th>Customer Name</th>
            <th>Email</th>
            <th>Time</th>
            <th>Status</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td colSpan="6" className={styles.noData}>
              No bookings found for the selected date.
            </td>
          </tr>
        </tbody>
      </table>

      <p className={styles.footer}>
        Bookings for {selectedDate ? selectedDate : "Select a date"}.
      </p>
    </div>
  );
};

export default TableStaff;
