import styles from "./AppointmentTable.module.css";
import CustomerDetail from "../../DashboardComponent/Tabs/Customers/CustomerDetail";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { getTodayAppointments } from "../../../services/staffService";

const AppointmentTable = () => {
  const [appointments, setAppointments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [showCustomerModal, setShowCustomerModal] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchTodayAppointments = async () => {
      setLoading(true);
      try {
        const response = await getTodayAppointments();
        setAppointments(response.result.appointments);
      } catch (error) {
        console.error("Fetch today appointments failed: ", error);
      } finally {
        setLoading(false);
      }
    };

    fetchTodayAppointments();
  }, []);

  const statusMap = {
    0: { text: "Created", color: "blue" },
    1: { text: "Checked-in", color: "yellow" },
    2: { text: "Completed", color: "green" },
  };

  const handleViewDetails = (appointmentId) => {
    navigate(`/staff/appointments/${appointmentId}`);
  };

  const handleCustomerClick = (customer) => {
    setSelectedCustomer(customer);
    setShowCustomerModal(true);
  };

  const closeCustomerModal = () => {
    setSelectedCustomer(null);
    setShowCustomerModal(false);
  };

  return (
    <div className={styles.container}>
      <div className={styles.headingSection}>
        <h2 className={styles.headingText}>Dashboard</h2>
      </div>
      <div className={styles.appointmentCard}>
        <div className={styles.titleSection}>
          <h3 className={styles.title}>Today's Appointments</h3>
        </div>
        <div className={styles.tableSection}>
          {loading ? (
            <p>Loading...</p>
          ) : (
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Therapist</th>
                  <th>Customer</th>
                  <th>Time</th>
                  <th>Status</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                {appointments.map((a) => {
                  const { text, color } = statusMap[a.status] || {
                    text: "Unknown",
                    color: "gray",
                  };
                  return (
                    <tr key={a.id}>
                      <td>{a.therapist}</td>
                      <td
                        className={styles.customerName}
                        onClick={() => handleCustomerClick(a)}
                      >
                        {a.customer}
                      </td>
                      <td>{a.time}</td>
                      <td style={{ color }}>{text}</td>
                      <td>
                        <button
                          onClick={() => handleViewDetails(a.appointmentId)}
                          style={{
                            background: "none",
                            border: "none",
                            color: "#b38b4c",
                            cursor: "pointer",
                          }}
                        >
                          View Details
                        </button>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          )}
        </div>
      </div>

      {showCustomerModal && selectedCustomer && (
        <CustomerDetail
          customer={selectedCustomer}
          onClose={closeCustomerModal}
        />
      )}
    </div>
  );
};

export default AppointmentTable;
