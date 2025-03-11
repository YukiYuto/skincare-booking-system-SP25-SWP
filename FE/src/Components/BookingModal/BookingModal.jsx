import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { Modal, Button, Steps, message, DatePicker } from "antd";
import styles from "./BookingModal.module.css"; // Import CSS module
import dayjs from "dayjs";
import axios from "axios";
import { toast } from "react-toastify";
import {
  GET_ALL_SERVICES_API,
  GET_SERVICE_BY_ID_API,
  GET_THERAPIST_BY_SERVICE_API,
  GET_ALL_SLOTS_API,
  GET_BOOKING_SLOT_API,
  GET_CUSTOMER_USER_API,
  POST_BOOKING_API,
} from "../../config/apiConfig";

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

  const [occupiedSlots, setOccupiedSlots] = useState([]);
  const { user } = useSelector((state) => state.auth);

  const next = () => setCurrent(current + 1);
  const prev = () => setCurrent(current - 1);

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
    fetch(GET_ALL_SERVICES_API, {
      method: "GET",
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

    fetch(GET_SERVICE_BY_ID_API.replace("{id}", serviceId))
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
    fetch(`${GET_THERAPIST_BY_SERVICE_API}?serviceTypeId=${serviceTypeId}`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${user.accessToken}`,
      },
    })
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
    fetch(GET_ALL_SLOTS_API)
      .then((res) => res.json())
      .then((data) => {
        if (data?.result) {
          setTimeSlots(data.result);
        }
      })
      .catch((err) => console.error("Lỗi khi lấy danh sách slot:", err));
  }, []);

  useEffect(() => {
    if (selectedDate && selectedTherapist) {
      fetch(
        `${GET_BOOKING_SLOT_API}?therapistId=${selectedTherapist}&date=${selectedDate}`
      )
        .then((res) => res.json())
        .then((data) => {
          if (data?.result) {
            setOccupiedSlots(data.result);
          } else {
            setOccupiedSlots([]);
          }
        })
        .catch((err) => console.error("Lỗi khi lấy slot đã đặt:", err));
    }
  }, [selectedDate, selectedTherapist]);

  const handleFinish = async () => {
    if (
      !selectedService ||
      !selectedTherapist ||
      !selectedDate ||
      !selectedTime
    ) {
      message.error("Vui lòng chọn đầy đủ thông tin trước khi đặt lịch!");
      return;
    }
    const customerResponse = await axios.get(GET_CUSTOMER_USER_API, {
      headers: {
        Authorization: `Bearer ${user.accessToken}`,
      },
    });
    const customerId = customerResponse.data.result;
    const orderData = {
      order: {
        customerId: customerId, // Lấy từ Redux store
        totalPrice: selectedService?.price || 0,
      },
      orderDetails: [
        {
          serviceId: selectedService?.serviceId,
          price: selectedService?.price || 0,
          description: `Booking for ${selectedService?.serviceName} with ${
            therapists.find((t) => t.therapistId === selectedTherapist)
              ?.fullName
          } on ${selectedDate} at ${selectedTime}`,
        },
      ],
    };

    fetch(POST_BOOKING_API, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${user.accessToken}`,
      },
      body: JSON.stringify(orderData),
    })
      .then((res) => {
        if (!res.ok) {
          return res.text().then((text) => {
            throw new Error(`Lỗi API: ${text}`);
          });
        }
        return res.json();
      })
      .then((data) => {
        console.log("API Response:", data); // Debug dữ liệu API trả về
        toast.success("Đặt lịch thành công!");
        localStorage.setItem("orderNumber", data.result.orderNumber);
        onClose();
        resetState();
      })
      .catch((err) => {
        toast.error(`Có lỗi xảy ra: ${err.message}`);
      });
  };

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
                <img
                  src={selectedService.imageUrl}
                  alt={selectedService.serviceName}
                  className={styles.image}
                />
                <div className={styles.info}>
                  <h3>{selectedService.serviceName}</h3>
                  <p>{selectedService.description}</p>
                  <p>
                    <strong>Price:</strong> {selectedService.price}
                  </p>
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
                <option
                  key={therapist.therapistId}
                  value={therapist.therapistId}
                >
                  {therapist.fullName}
                </option>
              ))}
            </select>

            {selectedTherapist && (
              <div className={styles.detailContainer}>
                <img
                  src={
                    therapists.find((t) => t.therapistId === selectedTherapist)
                      ?.imageUrl
                  }
                  alt={selectedTherapist.fullName}
                  className={styles.image}
                />
                <div className={styles.info}>
                  <h3>
                    {
                      therapists.find(
                        (t) => t.therapistId === selectedTherapist
                      )?.fullName
                    }
                  </h3>
                  <p>
                    <strong>Experience:</strong>{" "}
                    {
                      therapists.find(
                        (t) => t.therapistId === selectedTherapist
                      )?.experience
                    }
                  </p>
                  <p>
                    <strong>Age:</strong>{" "}
                    {
                      therapists.find(
                        (t) => t.therapistId === selectedTherapist
                      )?.age
                    }
                  </p>
                </div>
              </div>
            )}
          </div>
        )}

        {current === 2 && (
          <div className={styles.content}>
            <DatePicker
              style={{ width: "100%", marginBottom: 10 }}
              onChange={(date) =>
                setSelectedDate(dayjs(date).format("YYYY-MM-DD"))
              }
            />

            {/* Chỉ hiển thị time slots nếu đã chọn ngày */}
            {selectedDate && (
              <div className={styles.slotContainer}>
                {timeSlots.map((slot, index) => {
                  const slotValue = `${slot.startTime} - ${slot.endTime}`;
                  const isOccupied = occupiedSlots.includes(slotValue);

                  return (
                    <Button
                      key={index}
                      type="default"
                      disabled={isOccupied}
                      className={`${styles.slotButton} ${
                        selectedTime === slotValue ? styles.selectedSlot : ""
                      } ${isOccupied ? styles.disabledSlot : ""}`}
                      onClick={() =>
                        !isOccupied &&
                        setSelectedTime(
                          selectedTime === slotValue ? null : slotValue
                        )
                      }
                    >
                      {slot.startTime} - {slot.endTime}
                    </Button>
                  );
                })}
              </div>
            )}
          </div>
        )}

        {current === 3 && (
          <div className={styles.summary}>
            <h3>Review Your Booking</h3>
            <p>
              <strong>Service:</strong>{" "}
              {selectedService?.serviceName || "Not selected"}
            </p>
            <p>
              <strong>Therapist:</strong>{" "}
              {therapists.find((t) => t.therapistId === selectedTherapist)
                ?.fullName || "Not selected"}
            </p>
            <p>
              <strong>Date:</strong> {selectedDate || "Not selected"}
            </p>
            <p>
              <strong>Time:</strong> {selectedTime || "Not selected"}
            </p>
          </div>
        )}
      </div>

      {/* Buttons */}
      <div className={styles.buttons}>
        {current > 0 && <Button onClick={prev}>Previous</Button>}
        {current < 3 && (
          <Button type="primary" onClick={next}>
            Next
          </Button>
        )}
        {current === 3 && (
          <Button type="primary" onClick={handleFinish}>
            Confirm
          </Button>
        )}
      </div>
    </Modal>
  );
};

export default BookingModal;
