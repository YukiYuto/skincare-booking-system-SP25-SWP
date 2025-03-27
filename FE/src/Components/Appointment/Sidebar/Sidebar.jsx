import React, { useState } from "react";
import { List, Modal } from "antd";
import style from "./Sidebar.module.css";
import AppointmentDetail from "../AppointmentDetail/AppointmentDetail";

const Sidebar = ({
  selectedDate,
  selectedAppointments,
  getSlotById,
  onAppointmentClick, 
}) => {
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);

  const handleAppointmentClick = (appointment) => {
    onAppointmentClick(appointment.appointmentId);
    setSelectedAppointment(appointment);
    setModalVisible(true);
  };

  const handleCloseModal = () => {
    setModalVisible(false);
    setSelectedAppointment(null);
  };

  return (
    <div className={style.container}>
      <div className={style.detailsSection}>
        {selectedDate ? (
          <>
            <h3>Appointments on {selectedDate.format("MMMM D, YYYY")}</h3>
            {selectedAppointments.length > 0 ? (
              <List
                dataSource={selectedAppointments}
                renderItem={(appointment) => {
                  const matchingSlot = appointment.slotId
                    ? getSlotById(appointment.slotId)
                    : null;

                  return (
                    <List.Item
                      key={appointment.appointmentId}
                      onClick={() => handleAppointmentClick(appointment)}
                      style={{ cursor: "pointer" }}
                    >
                      <List.Item.Meta
                        title={
                          appointment.serviceInfo?.serviceName || "Appointment"
                        }
                        description={`Time: ${
                          matchingSlot?.startTime || "N/A"
                        } - ${matchingSlot?.endTime || "N/A"}`}
                      />
                    </List.Item>
                  );
                }}
              />
            ) : (
              <p>No appointments for this date.</p>
            )}
          </>
        ) : (
          <p>Select a date to see appointments</p>
        )}
      </div>
    </div>
  );
};

export default Sidebar;
