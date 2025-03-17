import { useState } from "react";
import { Select, DatePicker, TimePicker, Button, Modal, Card, Typography } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import "antd/dist/reset.css";
import styles from "./TableSchedule.module.css";

const { Title } = Typography;

const TableSchedule = () => {
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [selectedStylist, setSelectedStylist] = useState(null);
  const [selectedDate, setSelectedDate] = useState(null);
  const [selectedTime, setSelectedTime] = useState(null);

  const showModal = () => {
    setIsModalVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
  };

  const handleSave = () => {
    console.log("Saved:", {
      selectedStylist,
      selectedDate: selectedDate ? dayjs(selectedDate).format("MM/DD/YYYY") : null,
      selectedTime: selectedTime ? dayjs(selectedTime).format("HH:mm") : null,
    });
    setIsModalVisible(false);
  };

  const stylistOptions = [
    { value: "John Doe", label: "John Doe" },
    { value: "Jane Smith", label: "Jane Smith" },
  ];

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <Title level={2}>Stylist Schedule</Title>
        <Button type="primary" className={styles.createButton} icon={<PlusOutlined />} onClick={showModal}>
          Create Schedule
        </Button>
      </div>

      <Card className={styles.card}>
        <div className={styles.formGroup}>
          <label className={styles.label}>Select Stylist *</label>
          <Select
            className={styles.selectInput}
            placeholder="Choose a stylist"
            onChange={setSelectedStylist}
            value={selectedStylist}
            options={stylistOptions}
          />
        </div>

        <div className={styles.formGroup}>
          <label className={styles.label}>Select Date *</label>
          <DatePicker
            className={styles.datePicker}
            onChange={(date) => setSelectedDate(date)}
            format="MM/DD/YYYY"
            value={selectedDate}
          />
        </div>
      </Card>

      {/* Modal */}
      <Modal 
        title="Create Schedule" 
        open={isModalVisible} 
        onCancel={handleCancel} 
        footer={[
          <Button key="cancel" onClick={handleCancel} className={styles.cancelButton}>
            Cancel
          </Button>,
          <Button key="save" type="primary" className={styles.saveButton} onClick={handleSave}>
            Save Schedule
          </Button>,
        ]}
      >
        <div className={styles.formGroup}>
          <label className={styles.label}>Select Stylist:</label>
          <Select
            className={styles.selectInput}
            placeholder="Select a stylist"
            onChange={setSelectedStylist}
            value={selectedStylist}
            options={stylistOptions}
          />
        </div>

        <div className={styles.formGroup}>
          <label className={styles.label}>Select Date:</label>
          <DatePicker
            className={styles.datePicker}
            onChange={(date) => setSelectedDate(date)}
            format="MM/DD/YYYY"
            value={selectedDate}
          />
        </div>

        <div className={styles.formGroup}>
          <label className={styles.label}>Select Time Slots:</label>
          <TimePicker
            className={styles.timePicker}
            use12Hours
            format="h:mm A"
            minuteStep={30}
            onChange={setSelectedTime}
            value={selectedTime}
          />
        </div>
      </Modal>
    </div>
  );
};

export default TableSchedule;
