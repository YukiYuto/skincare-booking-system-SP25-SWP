import { useEffect, useState } from "react";
import { Form, Input, Button, Row, Col, Upload, notification, Flex } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useDispatch, useSelector } from "react-redux";
import { setUser } from "../../redux/auth/slice";
import styles from "./CustomerProfile.module.css";
import axios from "axios";

// Định nghĩa API endpoint
const USER_PROFILE_API = "https://localhost:7037/api/Auth/user";

// Hàm tạo headers chứa token
const getAuthHeaders = () => {
  const token = localStorage.getItem("token");
  return {
    Authorization: `Bearer ${token}`,
    "Content-Type": "application/json",
  };
};

const CustomerProfile = () => {
  const dispatch = useDispatch();
  const { user } = useSelector((state) => state.auth);
  const [isEditing, setIsEditing] = useState(false);
  const [form] = Form.useForm();
  const [imageUrl, setImageUrl] = useState("");
  const [formChanged, setFormChanged] = useState(false);

  // Gọi API để lấy dữ liệu người dùng
  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const response = await axios.get(USER_PROFILE_API, {
          headers: getAuthHeaders(),
        });

        dispatch(setUser(response.data)); // Cập nhật Redux với dữ liệu từ API
        form.setFieldsValue(response.data); // Đặt dữ liệu vào form
        setImageUrl(response.data.image || ""); // Cập nhật avatar nếu có
      } catch (error) {
        console.error("Error fetching user profile:", error);
        notification.error({
          message: "Error",
          description: "Could not fetch user profile. Please try again.",
        });
      }
    };

    fetchUserProfile();
  }, [dispatch, form]);

  // Xử lý cập nhật thông tin người dùng
  const handleUpdate = async (values) => {
    try {
      const updatedData = { ...values, image: imageUrl };

      const response = await axios.put(USER_PROFILE_API, updatedData, {
        headers: getAuthHeaders(),
      });

      dispatch(setUser(response.data)); // Cập nhật Redux với dữ liệu mới từ API
      notification.success({
        message: "Update Successful",
        description: "Your profile has been updated successfully.",
      });

      setIsEditing(false);
    } catch (error) {
      console.error("Error updating profile:", error);
      notification.error({
        message: "Update Failed",
        description: "Could not update profile. Please try again.",
      });
    }
  };

  // Bật chế độ chỉnh sửa
  const handleEdit = () => setIsEditing(true);

  // Hủy chỉnh sửa, phục hồi dữ liệu ban đầu
  const handleCancel = () => {
    setIsEditing(false);
    form.setFieldsValue(user);
    setImageUrl(user.image || "");
    setFormChanged(false);
  };

  // Xử lý khi upload avatar
  const handleChange = (info) => {
    if (info.file.status === "uploading") return;
    if (info.file.status === "done") {
      const reader = new FileReader();
      reader.readAsDataURL(info.file.originFileObj);
      reader.onload = () => {
        setImageUrl(reader.result);
        form.setFieldValue("avatar", reader.result);
      };
    }
  };

  return (
    <div className={styles.profileContainer}>
      <h2>Customer Profile</h2>

      <Flex gap="middle" wrap>
        <Upload
          name="avatar"
          listType="picture-card"
          showUploadList={false}
          action="#"
          onChange={handleChange}
        >
          {imageUrl ? (
            <img src={imageUrl} alt="avatar" style={{ width: "100%" }} />
          ) : (
            <div>
              <PlusOutlined /> <div>Upload</div>
            </div>
          )}
        </Upload>
      </Flex>

      <Form 
        form={form} 
        onFinish={handleUpdate} 
        layout="vertical" 
        disabled={!isEditing} 
        onValuesChange={() => setFormChanged(true)}
      >
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item 
              label="Full Name" 
              name="fullName" 
              rules={[{ required: true, message: "Please enter your full name!" }]}
            >
              <Input style={{ backgroundColor: "white" }} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item 
              label="Email" 
              name="email" 
              rules={[
                { required: true, message: "Please enter your email!" },
                { type: "email", message: "Invalid email!" }
              ]}
            >
              <Input style={{ backgroundColor: "white" }} disabled />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item 
              label="Phone" 
              name="phone" 
              rules={[{ required: true, message: "Please enter your phone number!" }]}
            >
              <Input style={{ backgroundColor: "white" }} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Address" name="address">
              <Input style={{ backgroundColor: "white" }} />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item 
              style={{ width: "70px" }} 
              label="Age" 
              name="age" 
              rules={[{ required: true, message: "Please enter your age!" }]}
            >
              <Input style={{ backgroundColor: "white" }} />
            </Form.Item>
          </Col>
        </Row>

        <div className={styles.buttonContainer}>
          <Button type="primary" htmlType="submit" disabled={!formChanged}>Update</Button>
          <Button onClick={handleCancel} style={{ marginLeft: 8 }}>Cancel</Button>
        </div>
      </Form>

      {!isEditing && (
        <Button onClick={handleEdit} type="default" style={{ marginTop: 20 }}>
          Edit Profile
        </Button>
      )}
    </div>
  );
};

export default CustomerProfile;
