/* eslint-disable no-unused-vars */
import React, { useEffect, useState } from "react";
import StatusProgress from "../Appointment/Status/StatusProgress";
import styles from "./TherapistAppointmentDetails.module.css";
import { Avatar, Button, Card, Spin } from "antd";
import { useNavigate, useParams } from "react-router-dom";
import { getAppointmentById } from "../../services/staffService";
import { toast } from "react-toastify";
import { PLACEHOLDER_IMAGE_URL } from "../../utils/avatarUtils";
import {
  completeService,
  createTherapistAdvice,
  getTherapistScheduleAdvice,
  getTherapistScheduleId,
} from "../../services/therapistService";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import { MdKeyboardDoubleArrowLeft } from "react-icons/md";
import {
  CiCalendarDate,
  CiClock2,
  CiMedicalClipboard,
  CiCoins1,
  CiSaveUp2,
} from "react-icons/ci";
import { FaRegUser, FaRegEnvelope, FaPhoneAlt } from "react-icons/fa";
import { GiAges } from "react-icons/gi";
import { IoMaleFemaleOutline } from "react-icons/io5";
import { LuBadgeCheck } from "react-icons/lu";
import { PiNotePencilDuotone } from "react-icons/pi";
import { formatDate } from "../../utils/formatDate.js";

const TherapistAppointmentDetails = () => {
  const { appointmentId } = useParams();
  const navigate = useNavigate();
  const [appointment, setAppointment] = useState(null);
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState(false);
  const [note, setNote] = useState("");
  const [isOpenNote, setIsOpenNote] = useState(false);
  const [therapistScheduleId, setTherapistScheduleId] = useState(null);
  const [isAddNote, setIsAddNote] = useState(false);
  const [advices, setAdvices] = useState([]);
  useEffect(() => {
    const fetchAppointment = async () => {
      try {
        const response = await getAppointmentById(appointmentId);
        setAppointment(response.result);
        console.log(response.result);
      } catch (error) {
        toast.error("Fetch appointment failed. " + error.message);
        console.error("Fetch appointment failed: ", error);
      } finally {
        setLoading(false);
      }
    };

    fetchAppointment();
  }, [appointmentId]);

  useEffect(() => {
    const fetchData = async () => {
      const response = await getTherapistScheduleId();
      let appointmentId = appointment?.appointmentId;

      const matched = response?.result.find(
        (item) => item.appointmentId === appointmentId
      );
      setTherapistScheduleId(matched?.therapistScheduleId || null);
    };
    fetchData();
  }, [appointment]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await getTherapistScheduleAdvice(
          appointment?.appointmentId
        );
        if (response.statusCode === 200) {
          setAdvices(response.result);
        }
      } catch (error) {
        // toast.error("Fetch advice failed. " + error.message);
        console.error("Fetch advice failed: ", error);
      }
    };
    fetchData();
  }, [appointment, advices]);

  const handleComplete = async (e) => {
    e.preventDefault();
    if (advices.length <= 0) {
      toast.warn("Please add advice before completing the service.");
      return;
    }
    if (!appointment) return;
    console.log(appointmentId);
    setUpdating(true);
    try {
      await completeService(appointmentId);
      setAppointment((prev) => ({ ...prev, status: "COMPLETED" }));
      toast.success("Service completed successfully!");
    } catch (error) {
      toast.error("Complete service failed. " + error.message);
      console.error("Complete service failed: ", error);
    } finally {
      setUpdating(false);
    }
  };
  const handleChange = (value) => {
    setNote(value);
  };
  const handleOpenNote = () => {
    if (appointment.status !== "CHECKEDIN") {
      toast.warn("You can only add notes in checked-in stage.");
      return;
    } else {
      setIsOpenNote(!isOpenNote);
    }
  };

  const handleAddNote = async () => {
    try {
      const response = await createTherapistAdvice(
        therapistScheduleId,
        appointment?.customerInfo?.customerId,
        note
      );

      if (response.statusCode == 201) {
        toast.success("Note added successfully!");
        setIsAddNote(true);
        setIsOpenNote(false);
        setNote("");
      } else {
        toast.error("Add note failed. " + response.message);
      }
    } catch (error) {
      toast.error("Add note failed. " + error.message);
      console.error("Add note failed: ", error);
    }
  };
  if (loading) return <Spin size="large" />;
  if (!appointment) return <div>Appointment not found</div>;

  return (
    <main style={{ display: "flex", flexDirection: "column", gap: "50px" }}>
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          border: "1px solid #ccc",
          padding: "20px",
          borderRadius: "15px",
        }}
      >
        <div
          style={{
            display: "flex",
            width: "100%",
            justifyContent: "space-between",
            alignItems: "center",
            fontSize: "30px",
          }}
        >
          <h3 style={{ margin: "0px" }}>Appointment Details</h3>
          <MdKeyboardDoubleArrowLeft
            style={{ cursor: "pointer" }}
            onClick={() => navigate(-1)}
          />
        </div>

        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            margin: "40px 0px 20px",
          }}
        >
          <span style={{ display: "flex", alignItems: "center", gap: "10px" }}>
            <CiCalendarDate style={{ fontSize: "20px" }} strokeWidth={1} />
            <strong>Date:</strong>
            <i>{appointment?.appointmentDate || "Unknown"}</i>
          </span>

          <span style={{ display: "flex", alignItems: "center", gap: "10px" }}>
            <CiClock2 style={{ fontSize: "20px" }} strokeWidth={1} />
            <strong>Time:</strong>
            <i>{appointment?.appointmentTime || "Unknown"}</i>
          </span>

          <span style={{ display: "flex", alignItems: "center", gap: "10px" }}>
            <CiMedicalClipboard style={{ fontSize: "20px" }} strokeWidth={1} />
            <strong>Service:</strong>
            <i>{appointment?.serviceInfo?.serviceName || "Unknown"}</i>
          </span>

          <span style={{ display: "flex", alignItems: "center", gap: "10px" }}>
            <CiCoins1 style={{ fontSize: "20px" }} strokeWidth={1} />
            <strong>Price:</strong>
            <i style={{ color: "green" }}>
              {Number(appointment?.serviceInfo?.servicePrice)?.toLocaleString(
                "vi-VN",
                {
                  style: "currency",
                  currency: "VND",
                }
              ) || "Unknown"}
            </i>
          </span>
        </div>

        <StatusProgress status={appointment.status} />
        {appointment.status === "CHECKEDIN" && (
          <div style={{ textAlign: "center" }}>
            <button
              style={{
                border: "1px solid #28ad2f",
                padding: "10px 15px ",
                borderRadius: "5px",
                marginBottom: "20px",
                backgroundColor: "#28ad2f",
                color: "#fff",
                cursor: "pointer",
                fontWeight: 500,
              }}
              onClick={(e) => handleComplete(e)}
            >
              Complete Service
            </button>
          </div>
        )}
      </div>
      <div
        style={{
          display: "flex",
          width: "100%",
          justifyContent: "space-between",
        }}
      >
        <div
          style={{
            border: "1px solid #ccc",
            padding: "20px 0px",
            borderRadius: "15px",
            width: "45%",
          }}
        >
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              borderBottom: "1px solid #ccc",
              padding: "0px 20px 20px",
            }}
          >
            <h5 style={{ margin: 0 }}>Customer Information</h5>
            <div
              style={{
                display: "flex",
                alignItems: "center",
                gap: "10px",
                cursor: "pointer",
              }}
              onClick={() => handleOpenNote()}
            >
              <span style={{ fontSize: "12px" }}>Add advices here</span>
              <PiNotePencilDuotone
                style={{ color: "green", fontSize: "18px" }}
              />
            </div>
          </div>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              gap: "30px",
              padding: "20px 20px 0px",
              width: "100%",
            }}
          >
            <Avatar
              src={
                appointment?.customerInfo?.customerAvatar ||
                PLACEHOLDER_IMAGE_URL
              }
              size={100}
              style={{
                marginBottom: "20px",
                border: "1px solid #ccc",
                boxShadow: "0 0 5px rgba(0,0,0,0.1)",
              }}
            />
            <div
              style={{
                display: "flex",
                flexDirection: "column",
                gap: "10px",
                width: "80%",
              }}
            >
              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <FaRegUser style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Name:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.customerInfo?.customerName || "Unknown"}
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <FaRegEnvelope style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Email:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.customerInfo?.customerEmail || "Unknown"}
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <FaPhoneAlt style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Phone:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.customerInfo?.customerPhone || "Unknown"}
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <GiAges style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>BirthDate:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {formatDate(appointment?.customerInfo?.customerAge)}
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <IoMaleFemaleOutline style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Gender:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.customerInfo?.customerGender || "Unknown"}
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <LuBadgeCheck style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Skin Profile:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.customerInfo?.skinProfileName || "Unknown"}
                </span>
              </div>
            </div>
          </div>
        </div>
        <div
          style={{
            border: "1px solid #ccc",
            padding: "20px 0px",
            borderRadius: "15px",
            width: "45%",
          }}
        >
          <h5
            style={{
              borderBottom: "1px solid #ccc",
              padding: "0px 20px 20px",
            }}
          >
            Therapist Information
          </h5>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              gap: "30px",
              padding: "20px 20px 0px",
              width: "100%",
            }}
          >
            <Avatar
              src={
                appointment?.therapistInfo?.therapistAvatarUrl ||
                PLACEHOLDER_IMAGE_URL
              }
              size={100}
              style={{
                marginBottom: "20px",
                border: "1px solid #ccc",
                boxShadow: "0 0 5px rgba(0,0,0,0.1)",
              }}
            />
            <div
              style={{
                display: "flex",
                flexDirection: "column",
                gap: "10px",
                width: "80%",
              }}
            >
              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <FaRegUser style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Name:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.therapistInfo?.therapistName || "Unknown"}
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <LuBadgeCheck style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Experience:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.therapistInfo?.therapistExperience || "Unknown"}
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <GiAges style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Age:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.therapistInfo?.therapistAge || "Unknown"}
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  gap: "10px",
                  width: "80%",
                  justifyContent: "space-between",
                }}
              >
                <div style={{ display: "flex", gap: "10px" }}>
                  <IoMaleFemaleOutline style={{ color: "#1167f2" }} />
                  <span style={{ fontWeight: 500 }}>Gender:</span>
                </div>
                <span style={{ fontSize: "16px" }}>
                  {appointment?.therapistInfo?.therapistGender || "Unknown"}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div style={{ width: "100%", marginBottom: "20px" }}>
        <h4 style={{ margin: "0px 0px 20px", fontWeight: "bold" }}>
          Therapist's Advices
        </h4>
        {advices && advices.length > 0 ? (
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(4, 1fr)",
              gap: "20px",
              width: "100%",
            }}
          >
            {advices.map((advice, index) => {
              return (
                <div
                  dangerouslySetInnerHTML={{ __html: advice.adviceContent }}
                  key={index}
                  style={{
                    border: "1px solid #ccc",
                    padding: "20px",
                    borderRadius: "10px",
                  }}
                />
              );
            })}
          </div>
        ) : (
          <div>No advices</div>
        )}
        <div></div>
      </div>
      {isOpenNote && (
        <div
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            width: "100vw",
            height: "100vh",
            backgroundColor: "rgba(0,0,0,0.5)",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            zIndex: 9999,
          }}
        >
          <div
            style={{
              backgroundColor: "#fff",
              width: "90%",
              maxWidth: "800px",
              height: "90%",
              padding: "20px",
              borderRadius: "10px",
              display: "flex",
              flexDirection: "column",
              justifyContent: "top",
              alignItems: "center",
              gap: "10px",
              overflow: "auto",
              position: "relative",
            }}
          >
            <button
              onClick={() => setIsOpenNote(false)}
              style={{
                position: "absolute",
                top: "10px",
                right: "20px",
                background: "transparent",
                border: "none",
                fontSize: "24px",
                cursor: "pointer",
              }}
            >
              &times;
            </button>

            <label htmlFor="note" style={{ fontWeight: 500, fontSize: "20px" }}>
              Advices
            </label>
            <ReactQuill
              id="note"
              value={note}
              onChange={handleChange}
              placeholder="Add advices here..."
              theme="snow"
              style={{ height: "650px", width: "100%" }}
              modules={{
                toolbar: [
                  [{ header: [1, 2, false] }],
                  ["bold", "italic", "underline"],
                  ["link"],
                  ["clean"],
                ],
              }}
            />
            <button
              style={{
                display: "flex",
                gap: "10px",
                position: "absolute",
                bottom: "50px",
                backgroundColor: "#28ad2f",
                border: "1px solid #1ebd0f",
                borderRadius: "5px",
                padding: "10px 15px",
                color: "#fff",
                cursor: "pointer",
              }}
              onClick={() => handleAddNote()}
            >
              <CiSaveUp2 />
              Save
            </button>
          </div>
        </div>
      )}
    </main>
  );
};
export default TherapistAppointmentDetails;
