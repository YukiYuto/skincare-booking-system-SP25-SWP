import { useEffect, useState } from "react";
import { Modal, Button, Steps, message, DatePicker } from "antd";
import styles from "./BookingModal.module.css"; // Import CSS module
import dayjs from "dayjs";

const { Step } = Steps;

// Dữ liệu mẫu

const BookingModal = ({ visible, onClose }) => {
  const [current, setCurrent] = useState(0);
  const [services, setServices] = useState([]); // Danh sách dịch vụ
  const [selectedService, setSelectedService] = useState(null); // Dịch vụ được chọn


  const [therapists, setTherapists] = useState([]); // Danh sách therapists
  const [selectedTherapist, setSelectedTherapist] = useState(""); // Therapist được chọn

  const [selectedDate, setSelectedDate] = useState(null);
  const [selectedTime, setSelectedTime] = useState(null);

  const [timeSlots, setTimeSlots] = useState([]);


  const next = () => setCurrent(current + 1);
  const prev = () => setCurrent(current - 1);
  const handleFinish = () => {
    message.success("Booking confirmed!");
    onClose();
  };

  const resetState = () => {
    setCurrent(0);
    setSelectedService(null);
    setSelectedTherapist("");
    setSelectedDate(null);
    setSelectedTime(null);
    setTherapists([]);
  };
  
  const handleClose = () => {
    resetState();
    onClose();
  };

  // Lấy danh sách dịch vụ
  useEffect(() => {
    fetch("https://localhost:7037/api/services", {
      method: "GET"
    })
      .then((res) => res.json())
      .then((data) => {
        if (data?.result?.services) {
          setServices(data.result.services);
        }
      })
      .catch((err) => console.error("Lỗi khi lấy danh sách dịch vụ:", err));
  }, []);

  // Xử lý khi chọn dịch vụ
  const handleSelectService = (e) => {
    const serviceId = e.target.value;
    
    if (!serviceId) {
      setSelectedService(null);
      setTherapists([]);
      return;
    }
  
    fetch(`https://localhost:7037/api/services/${serviceId}`)
      .then((res) => res.json())
      .then((data) => {
        if (data?.result) {
          setSelectedService(data.result);
          if (data.result.serviceTypeId) {
            fetchTherapists(data.result.serviceTypeId);
          } else {
            setTherapists([]);
          }
        }
      })
      .catch((err) => console.error("Lỗi khi lấy chi tiết dịch vụ:", err));
  };
  
  
  // Lấy danh sách therapists theo serviceTypeId
  const fetchTherapists = (serviceTypeId) => {
    fetch(`https://localhost:7037/api/bookings/therapists?serviceTypeId=${serviceTypeId}`)
      .then((res) => res.json())
      .then((data) => {
        if (data?.result) {
          setTherapists(data.result);
        } else {
          setTherapists([]); // Nếu không có therapists thì reset
        }
      })
      .catch((err) => console.error("Lỗi khi lấy therapists:", err));
  };

  // Xử lý khi chọn therapist
  const handleSelectTherapist = (e) => {
    setSelectedTherapist(e.target.value);
  };

  useEffect(() => {
    fetch("https://localhost:7037/api/slot")
        .then(res => res.json())
        .then(data => {
            if (data?.result) {
                setTimeSlots(data.result); 
            }
        })
        .catch(err => console.error("Lỗi khi lấy danh sách slot:", err));
}, []);



  return (
    <Modal open={visible} onCancel={handleClose} footer={null} width={700}>
      <h2 className={styles.modalTitle}>Book Your Appointment</h2>

      {/* Step Indicator */}
      <Steps current={current} className={styles.steps}>
        <Step title="Service" />
        <Step title="Therapist" />
        <Step title="Date & Time" />
        <Step title="Confirm" />
      </Steps>

      {/* Step Content */}
      <div className={styles.content}>
        {current === 0 && (
          <div className={styles.stepContainer}>
          <select onChange={handleSelectService}>
            <option value="">-- Chọn dịch vụ --</option>
            {services.map((service) => (
              <option key={service.serviceId} value={service.serviceId}>
                {service.serviceName}
              </option>
            ))}
          </select>


            {selectedService && (
              <div className={styles.detailContainer}>
                <img src={selectedService.imageUrl} alt={selectedService.serviceName} className={styles.image} />
                <div className={styles.info}>
                  <h3>{selectedService.serviceName}</h3>
                  <p>{selectedService.description}</p>
                  <p><strong>Price:</strong> {selectedService.price}</p>
                </div>
              </div>
            )}
          </div>
        )}

        {current === 1 && (
          <div className={styles.stepContainer}>
          <select onChange={handleSelectTherapist}>
            <option value="">-- Chọn therapist --</option>
            {therapists.map((therapist) => (
              <option key={therapist.therapistId} value={therapist.therapistId}>
                {therapist.fullName}
              </option>
            ))}
          </select>


            {selectedTherapist && (
              <div className={styles.detailContainer}>
                <img src={therapists.find(t => t.therapistId === selectedTherapist)?.imageUrl} alt={selectedTherapist.fullName} className={styles.image} />
                <div className={styles.info}>
                  <h3>{therapists.find(t => t.therapistId === selectedTherapist)?.fullName}</h3>
                  <p><strong>Experience:</strong> {therapists.find(t => t.therapistId === selectedTherapist)?.experience}</p>
                  <p><strong>Age:</strong> {therapists.find(t => t.therapistId === selectedTherapist)?.age}</p>
                </div>
              </div>
            )}
          </div>
        )}

        {current === 2 && (
        <div className={styles.content}>
          <DatePicker
            style={{ width: "100%", marginBottom: 10 }}
            onChange={(date) => setSelectedDate(dayjs(date).format("YYYY-MM-DD"))}
          />
          <select onChange={(e) => setSelectedTime(e.target.value)}>
              <option value="">-- Chọn thời gian --</option>
              {timeSlots.map((slot, index) => (
                  <option key={index} value={`${slot.startTime} - ${slot.endTime}`}>
                      {slot.startTime} - {slot.endTime}
                  </option>
              ))}
          </select>

        </div>
      )}

        {current === 3 && (
          <div className={styles.summary}>
            <h3>Review Your Booking</h3>
            <p><strong>Service:</strong> {selectedService?.serviceName || "Not selected"}</p>
            <p><strong>Therapist:</strong> {therapists.find(t => t.therapistId === selectedTherapist)?.fullName || "Not selected"}</p>
            <p><strong>Date:</strong> {selectedDate || "Not selected"}</p>
            <p><strong>Time:</strong> {selectedTime || "Not selected"}</p>
          </div>
        )}
      </div>

      {/* Buttons */}
      <div className={styles.buttons}>
        {current > 0 && <Button onClick={prev}>Previous</Button>}
        {current < 3 && <Button type="primary" onClick={next}>Next</Button>}
        {current === 3 && <Button type="primary" onClick={handleFinish}>Confirm</Button>}
      </div>
    </Modal>
  );
};

export default BookingModal;
