/* eslint-disable no-unused-vars */
/* eslint-disable react/prop-types */
import { DatePicker, Button } from "antd";
import dayjs from "dayjs";
import customParseFormat from "dayjs/plugin/customParseFormat";
import styles from "./BookingModal.module.css";

dayjs.extend(customParseFormat);

const isDateValid = (date) => {
  const currentDate = dayjs().startOf("day");
  const selectedDate = dayjs(date).startOf("day");
  const maxDate = currentDate.add(14, "day");

  // Check if date is in valid range (not before today and not after today + 14 days)
  return (
    selectedDate.isAfter(currentDate.subtract(1, "day")) &&
    selectedDate.isBefore(maxDate.add(1, "day"))
  );
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
      disabledDate={(date) => !isDateValid(date)}
    />
    {selectedDate && (
      <div className={styles.slotContainer}>
        {timeSlots.map((slot, index) => {
          const slotValue = `${slot.startTime} - ${slot.endTime}`;
          //~ Change: Check occupied slots by slotId, not by slotValue (since slotValue is string)
          const isOccupied = occupiedSlots.some(
            (s) => s.slotId === slot.slotId
          );
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
