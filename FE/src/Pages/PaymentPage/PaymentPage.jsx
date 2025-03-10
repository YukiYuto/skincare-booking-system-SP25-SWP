import { useState, useEffect } from "react";
import axios from "axios";
import { CREATE_PAYMENT_LINK } from "../../config/apiConfig";

const PaymentPage = () => {
  const [paymentData, setPaymentData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const orderNumber = localStorage.getItem("orderNumber");
    if (!orderNumber) {
      setError("Không tìm thấy Order Number trong LocalStorage");
      return;
    }
    
    const createPaymentLink = async () => {
      setLoading(true);
      try {
        const response = await axios.post(CREATE_PAYMENT_LINK, {
          orderNumber: orderNumber,
        });
        
        if (response.data.isSuccess) {
          setPaymentData(response.data.result.result);
        } else {
          setError(response.data.message || "Lỗi khi tạo link thanh toán");
        }
      } catch (err) {
        setError("Lỗi kết nối đến server: " + err.message);
      }
      setLoading(false);
    };

    createPaymentLink();
  }, []);

  return (
    <div className="container mt-5">
      <h2 className="text-center">Thanh Toán</h2>
      {loading && <p>Đang xử lý...</p>}
      {error && <p className="text-danger">{error}</p>}
      {paymentData && (
        <div className="card p-3">
          <p><strong>Mã đơn hàng:</strong> {paymentData.orderCode}</p>
          <p><strong>Số tiền:</strong> {paymentData.amount.toLocaleString()} {paymentData.currency}</p>
          <p><strong>Trạng thái:</strong> {paymentData.status}</p>
          <p><strong>Mô tả:</strong> {paymentData.description}</p>
          <p><strong>Số tài khoản:</strong> {paymentData.accountNumber}</p>
          <img src={`data:image/png;base64,${paymentData.qrCode}`} alt="QR Code" className="img-fluid" />
          <a href={paymentData.checkoutUrl} target="_blank" rel="noopener noreferrer" className="btn btn-primary mt-3">
            Thanh toán ngay
          </a>
        </div>
      )}
    </div>
  );
};

export default PaymentPage;
