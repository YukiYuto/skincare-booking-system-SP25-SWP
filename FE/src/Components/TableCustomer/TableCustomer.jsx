import { useState } from "react";
import { Calendar, Badge, Card } from "antd";
import dayjs from "dayjs";
import styles from "./TableCustomer.module.css"; // Import file CSS module

const bookings = [
  { date: "2025-02-28", service: "Facial Treatment", therapist: "Dr. Alice", time: "10:00 AM" },
  { date: "2025-03-01", service: "Acne Removal", therapist: "Dr. Bob", time: "2:00 PM" },
  { date: "2025-03-05", service: "Skin Hydration", therapist: "Dr. Charlie", time: "4:00 PM" },
];

const TableCustomer = () => {
  const [selectedDate, setSelectedDate] = useState(null);
  const selectedBooking = bookings.find((b) => b.date === selectedDate);

  const cellRender = (value) => {
    const dateStr = dayjs(value).format("YYYY-MM-DD");
    const isBooked = bookings.some((b) => b.date === dateStr);
    return isBooked ? <Badge status="error" text="Booked" /> : null;
  };

  const onSelectDate = (value) => {
    setSelectedDate(value.format("YYYY-MM-DD"));
  };

  return (
    <div className={styles.container}>
      <div className={styles.calendarSection}>
        <Calendar 
          cellRender={cellRender}
          onSelect={onSelectDate} 
          className={styles.calendar}
        />
      </div>
      <div className={styles.detailSection}>
        {selectedBooking ? (
          <Card title="Booking Details" className={styles.card}>
            <p><strong>Date:</strong> {selectedBooking.date}</p>
            <p><strong>Service:</strong> {selectedBooking.service}</p>
            <p><strong>Therapist:</strong> {selectedBooking.therapist}</p>
            <p><strong>Time:</strong> {selectedBooking.time}</p>
          </Card>
        ) : (
          <Card title="Booking Details" className={styles.card}>
            <p>No booking for this date.</p>
          </Card>
        )}
      </div>
    </div>
  );
};

export default TableCustomer;