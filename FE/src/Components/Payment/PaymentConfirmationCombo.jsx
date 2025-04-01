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
  const bookingDetails = useSelector((state) => state.booking);

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
      if (paymentResponse?.isSuccess) {
        setPaymentSuccess(true);
        message.success("Payment confirmed successfully!");
      }
    } catch (error) {
      setPaymentSuccess(false);
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
      ) : paymentSuccess ? (
        <Result
          status="success"
          title="Payment Confirmed!"
          subTitle="Your payment for service combo has been confirmed successfully."
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
          title="Payment confirmation failed"
          subTitle="There was an issue with the payment process."
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
