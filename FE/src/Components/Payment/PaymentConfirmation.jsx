/* eslint-disable react/jsx-key */
/* eslint-disable no-unused-vars */
import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { Button, message, Result, Spin } from "antd";
import {
  PAYMENT_STATUS,
  PAYMENT_CONFIRMATION_CODE,
  PAYMENT_URL_PARAMS,
} from "../../config/paymentConfig";
import { confirmPayment } from "../../services/paymentService";
import { finalizeAppointment } from "../../services/bookingService";
import { useDispatch } from "react-redux";
import { clearBookingDetails } from "../../redux/booking/slice";

const PaymentConfirmation = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const queryParams = new URLSearchParams(location.search);
  //~ 1. Get a bunch of query parameters from the URL here.
  // code: The payment confirmation code (00 for success, 01 for failure)
  // id: The payment ID
  // isCancelled: Whether the payment was cancelled (true/false)
  // status: The payment status (PAID/ PENDING/ PROCESSING/ CANCELLED)
  // orderCode: The order code (for validation when calling the API)
  const code = queryParams.get(PAYMENT_URL_PARAMS.CODE);
  const id = queryParams.get(PAYMENT_URL_PARAMS.ID);
  const isCancelled =
    queryParams.get(PAYMENT_URL_PARAMS.IS_CANCELLED) === "true";
  const status = queryParams.get(PAYMENT_URL_PARAMS.STATUS);
  const orderCode = queryParams.get(PAYMENT_URL_PARAMS.ORDER_CODE);

  const [isLoading, setIsLoading] = useState(false);
  const [paymentSuccess, setPaymentSuccess] = useState(false);
  const [appointmentSuccess, setAppointmentSuccess] = useState(false);
  const bookingDetails = useSelector((state) => state.booking);

  useEffect(() => {
    console.log("Updated Booking Details in Redux:", bookingDetails);
  }, []);
  //~ 2. Validate the payment based on the parameters retrieved.
  useEffect(() => {
    if (status === PAYMENT_STATUS.PAID) {
      finalizeAppointmentSchedule(orderCode);
    } else {
      handlePaymentFailure();
    }
  }, []);

  //~ 3. Perform payment confirmation
  const finalizeAppointmentSchedule = async (orderCode) => {
    setIsLoading(true);

    try {
      const paymentResponse = await confirmPayment(orderCode);
      if (!paymentResponse?.isSuccess) {
        setPaymentSuccess(false);
        message.error("Payment confirmation failed.");
        return;
      }
      console.log(bookingDetails);
      //~ 3.1. Retrieve booking data
      const appointmentData = {
        therapistId: bookingDetails.therapistId,
        slotId: bookingDetails.slotId,
        customerId: bookingDetails.customerId,
        appointmentDate: bookingDetails.appointmentDate,
        appointmentTime: bookingDetails.appointmentTime,
        note: bookingDetails.note,
        orderNumber: Number.parseInt(orderCode),
      };

      //~ 3.2. Call the API to create a new appointment
      const appointmentResponse = await finalizeAppointment(appointmentData);
      console.log(appointmentResponse);
      if (appointmentResponse?.isSuccess) {
        setPaymentSuccess(true);
        setAppointmentSuccess(true);
        message.success("Appointment has been scheduled successfully!");
      } else {
        setPaymentSuccess(false);
        throw new Error("Failed to create appointment.");
      }
    } catch (error) {
      setPaymentSuccess(false);
      setAppointmentSuccess(false);
    } finally {
      setIsLoading(false);
    }
  };

  const handlePaymentFailure = () => {
    setPaymentSuccess(false);
    setIsLoading(false);

    if (isCancelled) {
      message.warning("Payment has been cancelled.");
    } else {
      message.error("Payment failed. Please try again.");
    }
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", marginTop: 50 }}>
      {isLoading ? (
        <Spin size="large" />
      ) : paymentSuccess && appointmentSuccess ? (
        <Result
          status="success"
          title="Payment & Appointment Confirmed!"
          subTitle="Your appointment has been successfully scheduled."
          extra={[
            <Button type="primary" onClick={() => navigate("/appointments")}>
              View Appointment
            </Button>,
            <Button onClick={() => navigate("/")}>Go Home</Button>,
          ]}
        />
      ) : (
        <Result
          status="error"
          title="Payment or Appointment Failed"
          subTitle="There was an issue with the booking process."
          extra={[
            <Button type="primary" onClick={() => navigate("/")}>
              Try Again
            </Button>,
          ]}
        />
      )}
    </div>
  );
};

export default PaymentConfirmation;
