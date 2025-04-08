import React from "react";
import Header from "../../Components/Common/Header";
import CustomerOrders from "../../Components/Orders/CustomerOrders/CustomerOrders";

const OrderListPage = () => {
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
        <CustomerOrders />
      </div>
    </div>
  );
};

export default OrderListPage;
