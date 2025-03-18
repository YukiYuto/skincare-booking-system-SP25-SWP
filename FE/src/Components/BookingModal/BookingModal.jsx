/* eslint-disable no-unused-vars */
/* eslint-disable react/prop-types */
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Modal, Button, Steps, message, DatePicker } from "antd";
import styles from "./BookingModal.module.css"; // Import CSS module
import dayjs from "dayjs";
import { toast } from "react-toastify";
import {
  bundleOrder,
  getAllSlots,
  getOccupiedSlots,
  getTherapistsByService,
} from "../../services/bookingService";
import { getCustomerDetails } from "../../services/customerService";
import TherapistSelection from "./TherapistSelection";
import DateTimeSelection from "./DateTimeSelection";
import Confirmation from "./Confirmation";
import { createPaymentLink } from "../../services/paymentService";
import {
  PAYMENT_CANCEL_URL,
  PAYMENT_CONFIRMATION_URL,
} from "../../config/clientUrlConfig";
import { setBookingDetails } from "../../redux/booking/slice";

const { Step } = Steps;

const BookingModal = ({ visible, onClose, selectedService }) => {
  const [current, setCurrent] = useState(0);
  const [isLoading, setIsLoading] = useState(false);

  const [therapists, setTherapists] = useState([]); // Danh sách therapists
  const [selectedTherapist, setSelectedTherapist] = useState(""); // Therapist được chọn

  const [selectedDate, setSelectedDate] = useState(null);
  const [selectedTime, setSelectedTime] = useState(null);

  const [note, setNote] = useState("");

  const [timeSlots, setTimeSlots] = useState([]);
  const [selectedSlot, setSelectedSlot] = useState(null);
  const [occupiedSlots, setOccupiedSlots] = useState([]);

  const { user } = useSelector((state) => state.auth);
  const bookingDetails = useSelector((state) => state.booking);
  const dispatch = useDispatch();

  const next = () => setCurrent(current + 1);
  const prev = () => setCurrent(current - 1);

  const resetState = () => {
    setCurrent(0);
    setSelectedTherapist("");
    setSelectedDate(null);
    setSelectedTime(null);
    setTherapists([]);
  };

  const handleClose = () => {
    resetState();
    onClose();
  };

  // Fetch therapists based on selectedService
  useEffect(() => {
    if (selectedService?.serviceId && visible) {
      const fetchTherapists = async (serviceId) => {
        const response = await getTherapistsByService(serviceId);
        if (response?.result) {
          setTherapists(response.result);
        }
      };

      fetchTherapists(selectedService.serviceId);
    }
  }, [selectedService, visible]);

  useEffect(() => {
    const fetchTimeSlots = async () => {
      const response = await getAllSlots();
      if (response?.result) {
        setTimeSlots(response.result);
      }
    };

    fetchTimeSlots();
  }, []);

  useEffect(() => {
    if (selectedDate && selectedTherapist) {
      const fetchOccupiedSlots = async () => {
        const response = await getOccupiedSlots(
          selectedTherapist.skinTherapistId,
          selectedDate
        );
        if (response?.result) {
          setOccupiedSlots(response.result);
        }
      };

      fetchOccupiedSlots();
    }
  }, [selectedDate]);

  const handleFinish = async () => {
    if (!selectedTherapist || !selectedDate || !selectedTime) {
      message.error(
        "Please complete all selections before proceed to booking!"
      );
      return;
    }
    setIsLoading(true);

    try {
      const customerResponse = await getCustomerDetails();
      const customerId = customerResponse.result;
      const orderData = {
        order: {
          customerId,
          totalPrice: selectedService?.price || 0,
        },
        orderDetails: [
          {
            serviceId: selectedService?.serviceId,
            price: selectedService?.price || 0,
            description: `Booking for ${selectedService?.serviceName}`
          },
        ],
      };

      //~ Call the Booking API to bundle a new order
      const bookingResponse = await bundleOrder(orderData);
      const orderNumber = bookingResponse.result.orderNumber;
      localStorage.setItem("orderNumber", orderNumber);

      const bookingData = {
        therapistId: selectedTherapist.skinTherapistId,
        slotId: selectedSlot.slotId,
        customerId,
        appointmentDate: selectedDate,
        appointmentTime: selectedTime,
        note: note,
        orderNumber,
      };
      console.log(bookingData);
      //~ Store booking details in Redux
      dispatch(setBookingDetails(bookingData));

      // Call the Payment API
      const paymentResponse = await createPaymentLink(
        bookingResponse.result.orderNumber,
        PAYMENT_CANCEL_URL,
        PAYMENT_CONFIRMATION_URL
      );

      const checkoutUrl = paymentResponse.result.result.checkoutUrl;

      if (checkoutUrl) {
        toast.success("Booking successful! Redirecting to payment page...");
        setTimeout(() => {
          onClose();
          window.open(checkoutUrl, "_blank");
        }, 1000);
        resetState();
      } else {
        toast.error(
          "Error occurred while initiating payment. Please try again."
        );
      }
    } catch (err) {
      toast.error(`Error occurred: ${err.message}`);
    } finally {
      setIsLoading(false);
    }
  };

  const steps = [
    {
      title: "Therapist",
      content: (
        <TherapistSelection
          therapists={therapists}
          selectedTherapist={selectedTherapist}
          setSelectedTherapist={setSelectedTherapist}
        />
      ),
    },
    {
      title: "Date & Time",
      content: (
        <DateTimeSelection
          selectedDate={selectedDate}
          setSelectedDate={setSelectedDate}
          selectedTime={selectedTime}
          setSelectedTime={setSelectedTime}
          selectedSlot={selectedSlot}
          setSelectedSlot={setSelectedSlot}
          timeSlots={timeSlots}
          occupiedSlots={occupiedSlots}
        />
      ),
    },
    {
      title: "Confirm",
      content: (
        <Confirmation
          selectedService={selectedService}
          selectedTherapist={selectedTherapist}
          selectedDate={selectedDate}
          selectedTime={selectedTime}
        />
      ),
    },
  ];

  return (
    <Modal
      open={visible}
      onCancel={() => {
        resetState();
        onClose();
      }}
      footer={null}
      width={700}
    >
      <h2 className={styles.modalTitle}>Book Your Appointment</h2>
      <Steps current={current} className={styles.steps}>
        {steps.map((item) => (
          <Step key={item.title} title={item.title} />
        ))}
      </Steps>
      <div className={styles.content}>{steps[current].content}</div>
      <div className={styles.buttons}>
        {current > 0 && (
          <Button onClick={() => setCurrent(current - 1)}>Previous</Button>
        )}
        {current < steps.length - 1 && (
          <Button
            type="primary"
            onClick={() => setCurrent(current + 1)}
            disabled={
              (current === 0 && !selectedTherapist) ||
              (current === 1 && (!selectedDate || !selectedTime))
            }
          >
            Next
          </Button>
        )}
        {current === steps.length - 1 && (
          <Button type="primary" onClick={handleFinish} loading={isLoading}>
            Confirm
          </Button>
        )}
      </div>
    </Modal>
  );
};

export default BookingModal;
