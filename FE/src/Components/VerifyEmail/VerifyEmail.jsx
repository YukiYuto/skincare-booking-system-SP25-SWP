import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { toast } from "react-toastify";
import { confirmEmailVerification } from "../../services/authService";
import styles from "./VerifyEmail.module.css";
import { FaCheckCircle, FaSpinner, FaTimesCircle } from "react-icons/fa";

const VerifyEmail = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const userId = searchParams.get("userId");
  const token = searchParams.get("token");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!userId || !token) {
      setError("Invalid verification link.");
      setLoading(false);
      toast.error("Invalid verification link.");
      navigate("/register");
      return;
    }

    const verifyEmail = async () => {
        setLoading(true);
      try {
        await confirmEmailVerification(userId, token);
        toast.success("Email verified successfully! Please log in.");
        navigate("/login");
      } catch (error) {
        setError(error.message || "Verification failed. Please try again.");
        toast.error(error.message);
        navigate("/register");
      } finally {
        setLoading(false);
      }
    };

    verifyEmail();
  }, [userId, token, navigate]);

  return (
    <div className={styles.verifyContainer}>
      <div className={styles.verifyBox}>
        {loading ? (
          <>
            <FaSpinner className={styles.spinner} />
            <p className={styles.verifyText}>Verifying email...</p>
          </>
        ) : error ? (
          <>
            <FaTimesCircle className={styles.errorIcon} />
            <p className={styles.errorText}>{error}</p>
          </>
        ) : (
          <>
            <FaCheckCircle className={styles.successIcon} />
            <p className={styles.successText}>
              Email verified successfully! Redirecting to login...
            </p>
          </>
        )}
      </div>
    </div>
  );  
};

export default VerifyEmail;
