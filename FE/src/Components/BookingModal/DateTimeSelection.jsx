/* eslint-disable no-unused-vars */
/* eslint-disable react/prop-types */
import { DatePicker, Button } from "antd";
import dayjs from "dayjs";
import styles from "./BookingModal.module.css";

const disabledPastDates = (current) => {
  return current && current < dayjs().startOf("day"); // Disable past dates
};

const DateTimeSelection = ({
  selectedDate,
  setSelectedDate,
  selectedTime,
  setSelectedTime,
  selectedSlot,
  setSelectedSlot,
  timeSlots,
  occupiedSlots,
}) => (
  <div className={styles.content}>
    <DatePicker
      style={{ width: "100%", marginBottom: 10 }}
      onChange={(date) =>
        setSelectedDate(date ? dayjs(date).format("YYYY-MM-DD") : null)
      }
      disabledDate={disabledPastDates}
    />
    {selectedDate && (
      <div className={styles.slotContainer}>
        {timeSlots.map((slot, index) => {
          const slotValue = `${slot.startTime} - ${slot.endTime}`;
          //~ Change: Check occupied slots by slotId, not by slotValue (since slotValue is string)
          const isOccupied = occupiedSlots.some(s => s.slotId === slot.slotId);
          return (
            <Button
              key={index}
              type="default"
              disabled={isOccupied}
              className={`${styles.slotButton} ${
                selectedTime === slotValue ? styles.selectedSlot : ""
              } ${isOccupied ? styles.disabledSlot : ""}`}
              onClick={() => {
                if (!isOccupied) {
                  setSelectedTime(
                    selectedTime === slotValue ? null : slotValue
                  );
                  setSelectedSlot(slot);
                }
              }}
            >
              {slot.startTime} - {slot.endTime}
            </Button>
          );
        })}
      </div>
    )}
  </div>
);

export default DateTimeSelection;
