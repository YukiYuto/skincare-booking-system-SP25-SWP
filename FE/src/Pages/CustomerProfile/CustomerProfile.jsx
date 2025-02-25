import { Col, Form, Row } from "antd";
import { useSelector } from "react-redux";
import styles from "./CustomerProfile.module.css";
import Header from "../../Components/Common/Header";
import { useState } from "react";
import { toast } from "react-toastify";

const UserProfile = () => {
  const { user } = useSelector((state) => state.auth);
  const [form] = Form.useForm();
  const [image, setImage] = useState(null);
  const [loading, setLoading] = useState(false);
  const [imageUrl, setImageUrl] = useState(user.imageUrl);

  const accessToken = localStorage.getItem("accessToken");

  // Chọn file ảnh
  const handleFileChange = (event) => {
    setImage(event.target.files[0]);
  };

  // Upload ảnh bằng fetch
  const handleUpload = async () => {
    if (!image) {
      toast.warn("Vui lòng chọn ảnh!");
      return;
    }
  
    setLoading(true);
    const formData = new FormData();
    formData.append("file", image);
  
    try {
      const response = await fetch(
        `https://localhost:7037/api/UserManagement/avatar?AccessToken=${accessToken}`,
        {
          method: "POST",
          body: formData,
        }
      );
  
      const data = await response.json();
  
      if (data.isSuccess && data.result) { // Kiểm tra isSuccess
        setImageUrl(data.result); // Cập nhật ảnh mới
  
        toast.success("Upload thành công!");
      } else {
        toast.warn("Upload thất bại!");
      }
    } catch (error) {
      toast.warn("Upload thất bại");
    } finally {
      setLoading(false);
    }
  };
  
  return (
    <div className={styles.bodyPage}>
      <Header />
      <div className={styles.profileContainer}>
        <h2>Customer Profile</h2>
        <div>
          <input className={styles.fileinput} type="file" onChange={handleFileChange} />
          <button className={styles.uploadbutton} onClick={handleUpload} disabled={loading}>
            {loading ? "Uploading..." : "Upload Avatar"}
          </button>
          <img 
            className={styles.avatarpreview}
            src={imageUrl || user.imageUrl} 
            alt="Avatar" 
          />
        </div>

        <Form form={form} layout="vertical">
          <Row style={{marginTop: "20px"}} gutter={16}>
            <Col span={12}>
              <Form.Item label="Full Name">
                <p>{user.fullName}</p>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Email">
                <p>{user.email}</p>
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="Phone">
                <p>{user.phoneNumber}</p>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Address">
                <p>{user.address}</p>
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="Age">
                <p>{user.age}</p>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Gender">
                <p>{user.gender}</p>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </div>
    </div>
  );
};

export default UserProfile;
