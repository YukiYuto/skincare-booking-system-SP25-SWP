import { Button, Col, Form, Input, Modal, Row, Spin } from "antd";
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
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [passwords, setPasswords] = useState({
    oldPassword: "",
    newPassword: "",
    confirmNewPassword: "",
  });
  const [passwordLoading, setPasswordLoading] = useState(false);

  const accessToken = user.accessToken;

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
        setUserData((prevData) => ({
          ...prevData,
          imageUrl: data.result, // Cập nhật imageUrl vào userData để đảm bảo gửi đúng khi save
        }));
        // dispatch(updateUser({ imageUrl: data.result })); // Cập nhật Redux store
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
    console.log("Final userData before sending:", userData);
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
        localStorage.setItem("user", JSON.stringify(userData));
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

  // Hiển thị modal đổi mật khẩu
const showChangePasswordModal = () => {
  setIsModalVisible(true);
};

// Đóng modal
const handleCancel = () => {
  setIsModalVisible(false);
  setPasswords({ oldPassword: "", newPassword: "", confirmNewPassword: "" });
};

// Xử lý input thay đổi
const handlePasswordChange = (e) => {
  setPasswords({ ...passwords, [e.target.name]: e.target.value });
};

// Gửi yêu cầu đổi mật khẩu
const handleChangePassword = async () => {
  if (passwords.newPassword !== passwords.confirmNewPassword) {
    toast.error("New Password and Confirm Password do not match!");
    return;
  }

  setPasswordLoading(true);

  try {
    const response = await fetch("https://localhost:7037/api/Auth/password/change", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${accessToken}`,
      },
      body: JSON.stringify({
        userId: user.id, // Truyền userId từ state
        oldPassword: passwords.oldPassword,
        newPassword: passwords.newPassword,
        confirmNewPassword: passwords.confirmNewPassword,
      }),
    });

    // Kiểm tra nếu server trả về text thay vì JSON
    const textResponse = await response.text();

    if (response.ok) {
      toast.success(textResponse); // Hiển thị thông báo thành công
      handleCancel(); // Đóng modal nếu có
    } else {
      toast.error(`Error: ${textResponse}`); // Hiển thị lỗi nếu có
    }

  } catch (error) {
    toast.error("Error changing password!");
  } finally {
    setPasswordLoading(false);
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
              <Button type="default" onClick={showChangePasswordModal}>
                Change password
              </Button>
        </div>
        {/* Modal đổi mật khẩu */}
        <Modal
          title="Change Password"
          open={isModalVisible}
          onCancel={handleCancel}
          footer={[
            <Button key="cancel" onClick={handleCancel}>
              Cancel
            </Button>,
            <Button key="submit" type="primary" onClick={handleChangePassword} disabled={passwordLoading}>
              {passwordLoading ? <Spin /> : "Save"}
            </Button>,
          ]}
        >
          <Form layout="vertical">
            <Form.Item label="Old Password">
              <Input.Password name="oldPassword" value={passwords.oldPassword} onChange={handlePasswordChange} />
            </Form.Item>
            <Form.Item label="New Password">
              <Input.Password name="newPassword" value={passwords.newPassword} onChange={handlePasswordChange} />
            </Form.Item>
            <Form.Item label="Confirm New Password">
              <Input.Password name="confirmNewPassword" value={passwords.confirmNewPassword} onChange={handlePasswordChange} />
            </Form.Item>
          </Form>
        </Modal>
      </div>
    </div>
  );
};

export default UserProfile;


