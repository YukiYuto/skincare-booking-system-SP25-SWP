/* eslint-disable no-unused-vars */
import React, { useState } from "react";
import PropTypes from "prop-types";
import styles from "./CancellationModal.module.css";
import { Modal, Button, Radio, Input } from "antd";

const CancellationModal = ({ visible, onCancel, onConfirm, appointment }) => {
  const [selectedReason, setSelectedReason] = useState("");
  const [otherReason, setOtherReason] = useState("");

  const predefinedReasons = [
    "Customer request",
    "Therapist unavailable",
    "Double booking",
    "Customer absent",
  ];

  const handleConfirm = () => {
    const reason = selectedReason === "Other" ? otherReason : selectedReason;
    if (!reason) return;
    onConfirm(reason);
  };

  return (
    <Modal
      title="Cancellation Reason"
      open={visible}
      onCancel={onCancel}
      footer={[
        <Button key="cancel" onClick={onCancel}>
          No, keep the appointment
        </Button>,
        <Button
          key="confirm"
          type="primary"
          danger
          onClick={handleConfirm}
          disabled={
            !selectedReason || (selectedReason === "Other" && !otherReason)
          }
        >
          Yes, cancel the appointment
        </Button>,
      ]}
    >
      <div className={styles.modalContent}>
        <p>
          <strong>Appointment Time:</strong> {appointment?.appointmentTime}
        </p>
        <p>
          <strong>Customer:</strong> {appointment?.customerInfo.customerName}
        </p>
        <p>
          <strong>Therapist:</strong> {appointment?.therapistInfo.therapistName}
        </p>
        <p>
          <strong>Service:</strong> {appointment?.serviceInfo.serviceName}
        </p>

        <h4>Reason for Cancellation</h4>
        <Radio.Group
          onChange={(e) => setSelectedReason(e.target.value)}
          value={selectedReason}
        >
          {predefinedReasons.map((reason) => (
            <Radio key={reason} value={reason}>
              {reason}
            </Radio>
          ))}
          <Radio value="Other">Other</Radio>
        </Radio.Group>

        {selectedReason === "Other" && (
          <Input
            placeholder="Enter reason..."
            value={otherReason}
            onChange={(e) => setOtherReason(e.target.value)}
            className={styles.inputField}
          />
        )}
      </div>
    </Modal>
  );
};

CancellationModal.propTypes = {
  visible: PropTypes.bool.isRequired,
  onCancel: PropTypes.func.isRequired,
  onConfirm: PropTypes.func.isRequired,
  appointment: PropTypes.object.isRequired,
};
export default CancellationModal;
