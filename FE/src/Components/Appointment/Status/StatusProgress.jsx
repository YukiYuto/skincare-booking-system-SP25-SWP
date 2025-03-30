// eslint-disable-next-line no-unused-vars
import React from "react";
import { Steps } from "antd";
import styles from "./StatusProgress.module.css";

const StatusProgress = ({ status }) => {
  // Define the statuses in the correct order
  const statusMap = [
    { key: "CREATED", label: "Pending", color: "gray" },
    { key: "CHECKEDIN", label: "Checked-In", color: "blue" },
    { key: "COMPLETED", label: "Service Finished", color: "purple" },
    { key: "CHECKEDOUT", label: "Checked-Out", color: "green" },
  ];

  // Determine the current active step
  const currentStep = statusMap.findIndex((s) => s.key === status);
  const isCancelled = status === "CANCELLED";

  return (
    <div className={styles.progressContainer}>
      <Steps current={isCancelled ? 0 : currentStep}>
        {statusMap.map(({ key, label, color }, idx) => (
          <Steps.Step
            key={key}
            title={label}
            status={
              isCancelled ? "error" : idx <= currentStep ? "process" : "wait"
            }
            style={{ color: isCancelled ? "red" : color }}
          />
        ))}
        {isCancelled && (
          <Steps.Step
            title="Cancelled"
            status="error"
            style={{ color: "red" }}
          />
        )}
      </Steps>
    </div>
  );
};

export default StatusProgress;
