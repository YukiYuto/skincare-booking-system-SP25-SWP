import { Button, Col, Form, Input, Row } from "antd";
import { useDispatch, useSelector } from "react-redux";
import styles from "./CustomerProfile.module.css";
import Header from "../../Components/Common/Header";
import { useState } from "react";
import { toast } from "react-toastify";
import { updateUser } from "../../redux/auth/slice";

const UserProfile = () => {
  const { user } = useSelector((state) => state.auth);
  const dispatch = useDispatch();
  const [form] = Form.useForm();
  const [uploadLoading, setUploadLoading] = useState(false);
  const [updateLoading, setUpdateLoading] = useState(false);
  const [imageUrl, setImageUrl] = useState(user.imageUrl);
  const [userData, setUserData] = useState({ ...user });

  const accessToken = localStorage.getItem("accessToken");

  // Khi chọn file, upload luôn avatar
  const handleFileChange = async (event) => {
    const file = event.target.files[0];
    if (!file) return;

    setUploadLoading(true);

    const formData = new FormData();
    formData.append("file", file);

    try {
      const response = await fetch(
        `https://localhost:7037/api/UserManagement/avatar`,
        {
          method: "POST",
          headers: {
              Authorization: `Bearer ${accessToken}`,
          },
          body: formData,
        }
      );

      const data = await response.json();

      if (data.isSuccess && data.result) {
        setImageUrl(data.result); // Cập nhật ảnh mới
        dispatch(updateUser({ imageUrl: data.result })); // Cập nhật Redux store
        toast.success("Upload Successfully!");
      } else {
        toast.error("Upload Failed!");
      }
    } catch (error) {
      toast.error("Error uploading image!");
    } finally {
      setUploadLoading(false);
    }
  };

  // Bắt đầu chỉnh sửa
  const handleInputChange = (e) => {
    setUserData({ ...userData, [e.target.name]: e.target.value });
  };

  // Gửi API cập nhật profile
  const handleUpdateProfile = async () => {
    setUpdateLoading(true);
    try {
      const response = await fetch("https://localhost:7037/api/Auth/profile", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${accessToken}`,
        },
        body: JSON.stringify(userData),
      });

      const data = await response.json();

      if (data.isSuccess) {
        dispatch(updateUser(userData)); // Cập nhật Redux store
        toast.success("Update Successfully!");
      } else {
        toast.error("Update Failed!");
      }
    } catch (error) {
      toast.error("Error updating information!");
    } finally{
      setUpdateLoading(false);
    }
  };
  
  return (
    <div className={styles.bodyPage}>
      <Header />
      <div className={styles.profileContainer}>
        <h2>Customer Profile</h2>
        <div>
          <p>Choose file here:</p><input className={styles.fileinput} type="file" onChange={handleFileChange} disabled={uploadLoading} />
          <img 
            className={styles.avatarpreview}
            src={imageUrl || userData.imageUrl} 
            alt="Avatar" 
          />
          {uploadLoading && <p>Uploading...</p>}
        </div>

        <Form form={form} layout="vertical">
          <Row style={{marginTop: "20px"}} gutter={16}>
            <Col span={12}>
              <Form.Item label="Full Name">
              <Input
                  name="fullName"
                  value={userData.fullName}
                  onChange={handleInputChange}
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Email">
              <p>{userData.email}</p>
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="Phone">
              <Input
                  name="phoneNumber"
                  value={userData.phoneNumber}
                  onChange={handleInputChange}
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Address">
              <Input
                  name="address"
                  value={userData.address}
                  onChange={handleInputChange}
                />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="Age">
              <Input
                  name="age"
                  value={userData.age}
                  onChange={handleInputChange}
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Gender">
              <Input
                  name="gender"
                  value={userData.gender}
                  onChange={handleInputChange}
                />
              </Form.Item>
            </Col>
          </Row>
        </Form>
        <div style={{ marginTop: "20px" }}>
              <Button type="primary" onClick={handleUpdateProfile}>
              {updateLoading ? "Saving..." : "Save"}
              </Button>
              <Button type="default">
                Change password
              </Button>
        </div>
      </div>
    </div>
  );
};

export default UserProfile;
