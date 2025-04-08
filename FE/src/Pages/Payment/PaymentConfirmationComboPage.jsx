/* eslint-disable no-unused-vars */
import React from "react";
import PaymentConfirmationCombo from "../../Components/Payment/PaymentConfirmationCombo";
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
        <PaymentConfirmationCombo />
      </div>
    </div>
  );
};

export default PaymentConfirmationPage;
