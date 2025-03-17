import { useState } from "react";
import { DatePicker, Select, Input, Button, Typography } from "antd";
import styles from "./ScheduleTherapistRegister.module.css";

const { Title } = Typography;
const { TextArea } = Input;

const ScheduleTherapistRegister = () => {
  const [selectedDate, setSelectedDate] = useState(null);
  const [selectedShift, setSelectedShift] = useState("morning");
  const [note, setNote] = useState("");

  const handleSubmit = () => {
    alert(`Lịch làm việc: ${selectedDate?.format("YYYY-MM-DD")} - ${selectedShift} \nGhi chú: ${note}`);
  };

  return (
    <div className={styles.container}>
      <Title level={3} className={styles.title}>Đăng ký lịch làm việc</Title>
      <div className={styles.form}>
        <label>Chọn ngày:</label>
        <DatePicker 
          onChange={(date) => setSelectedDate(date)} 
          className={styles.input} 
          format="YYYY-MM-DD"
        />

        <label>Chọn ca làm việc:</label>
        <Select
          value={selectedShift}
          onChange={(value) => setSelectedShift(value)}
          className={styles.select}
        >
          <Select.Option value="morning">Sáng (8:00 - 12:00)</Select.Option>
          <Select.Option value="afternoon">Chiều (13:00 - 17:00)</Select.Option>
          <Select.Option value="evening">Tối (18:00 - 22:00)</Select.Option>
        </Select>

        <label>Ghi chú:</label>
        <TextArea
          value={note}
          onChange={(e) => setNote(e.target.value)}
          className={styles.textarea}
          placeholder="Nhập ghi chú..."
        />

        <Button type="primary" onClick={handleSubmit} className={styles.button}>
          Đăng ký
        </Button>
      </div>
    </div>
  );
};

export default ScheduleTherapistRegister;