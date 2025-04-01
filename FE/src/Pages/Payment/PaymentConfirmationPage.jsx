/* eslint-disable no-unused-vars */
import React from "react";
import PaymentConfirmation from "../../Components/Payment/PaymentConfirmation";
import Header from "../../Components/Common/Header";

const PaymentConfirmationPage = () => {
  return (
    <div>
      <Header />
      <div
        style={{
          minHeight: "100vh",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <PaymentConfirmation />
      </div>
    </div>
  );
};

export default PaymentConfirmationPage;
