import React, { useState } from "react";
import { RefreshCcw } from "lucide-react";

const RPG = ({ onChange }) => {
  const [password, setPassword] = useState("");

  const generatePassword = () => {
    const length = 8; 
    const chars = {
      lower: "abcdefghijklmnopqrstuvwxyz",
      upper: "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
      number: "0123456789",
      special: "!@#$%^&*()-_+[]{}|;:,.?/",
    };

    let newPassword = "";
    newPassword += chars.lower[Math.floor(Math.random() * chars.lower.length)];
    newPassword += chars.upper[Math.floor(Math.random() * chars.upper.length)];
    newPassword += chars.number[Math.floor(Math.random() * chars.number.length)];
    newPassword += chars.special[Math.floor(Math.random() * chars.special.length)];

    const allChars = chars.lower + chars.upper + chars.number + chars.special;
    for (let i = 4; i < length; i++) {
      newPassword += allChars[Math.floor(Math.random() * allChars.length)];
    }

    newPassword = newPassword.split("").sort(() => Math.random() - 0.5).join("");

    setPassword(newPassword);
    onChange && onChange(newPassword);
  };

  return (
    <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
      <span
        style={{
          fontSize: "16px",
          fontWeight: "bold",
          padding: "6px 12px",
          background: "#f3f3f3",
          borderRadius: "6px",
          border: "1px solid #ccc",
          minWidth: "200px",
          textAlign: "center",
          display: "inline-block",
        }}
      >
        {password || "Click to generate"}
      </span>
      <button
        type="button"
        onClick={generatePassword}
        style={{
          background: "none",
          border: "none",
          cursor: "pointer",
          padding: "5px",
        }}
      >
        <RefreshCcw size={24} color="#007bff" />
      </button>
    </div>
  );
};

export default RPG;
