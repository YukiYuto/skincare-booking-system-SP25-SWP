import React, { useEffect, useState } from "react";
import { Table, Tag, Button, Drawer, Spin, Input, Card } from "antd";
import {
  getCustomerOrders,
  getOrderDetailsByOrderId,
} from "../../../services/customerService";
import { toast } from "react-toastify";
import styles from "./CustomerOrders.module.css";

const CustomerOrders = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [drawerVisible, setDrawerVisible] = useState(false);

  const numberWithCommas = (x) => {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
  };
  // Fetch orders
  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const response = await getCustomerOrders();
        setOrders(response.result);
      } catch (error) {
        toast.error("Failed to fetch orders: " + error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  // Table columns
  const columns = [
    {
      title: "Order ID",
      dataIndex: "orderId",
      key: "orderId",
    },
    {
      title: "Issue time",
      dataIndex: "createdTime",
      key: "orderDate",
      render: (text) => new Date(text).toLocaleString("en-GB"),
    },
    {
      title: "Amount",
      dataIndex: "totalAmount",
      key: "totalAmount",
      render: (amount) => `${numberWithCommas(amount)} VND`,
    },
    {
      title: "Actions",
      key: "actions",
      render: (_, record) => (
        <Button type="link" onClick={() => openOrderDetails(record)}>
          View Details
        </Button>
      ),
    },
  ];

  // Open Drawer for Order Details
  const openOrderDetails = async (order) => {
    try {
      const response = await getOrderDetailsByOrderId(order.orderId);
      setSelectedOrder(response.result);
      setDrawerVisible(true);
    } catch (error) {
      toast.error("Failed to fetch order details: " + error.message);
    }
  };

  if (loading) return <Spin size="large" />;

  return (
    <div className={styles.container}>
      <h2>My Orders</h2>
      <Table
        columns={columns}
        dataSource={orders}
        rowKey="orderId"
        pagination={{ pageSize: 5 }}
      />

      {/* Order Details Drawer */}
      <Drawer
        title="Order Details"
        width={400}
        onClose={() => setDrawerVisible(false)}
        open={drawerVisible}
      >
        {selectedOrder ? (
          <div>
            <h3>Services:</h3>
            <div>
              {selectedOrder.orderDetails.map((item, index) => (
                <Card key={index} className={styles.card}>
                  <p>
                    <strong>{item.serviceName}</strong>
                  </p>
                  <p>
                    <strong> {numberWithCommas(item.price)} VND</strong>
                  </p>
                </Card>
              ))}
            </div>
          </div>
        ) : (
          <p>No order selected</p>
        )}
      </Drawer>
    </div>
  );
};

export default CustomerOrders;
