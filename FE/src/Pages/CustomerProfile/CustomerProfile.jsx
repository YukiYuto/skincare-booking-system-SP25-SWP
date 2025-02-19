import { useEffect, useState } from "react";
import { Form, Input, Button, Row, Col, Upload, notification, Flex } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useDispatch, useSelector } from "react-redux";
import { setUser } from "../../redux/auth/slice";
import styles from "./CustomerProfile.module.css";
import { AUTH_HEADERS, USER_PROFILE_API } from "../../config/apiConfig";
import axios from "axios";

const CustomerProfile = () => {
  const dispatch = useDispatch();
  const { user } = useSelector((state) => state.auth);
  const [isEditing, setIsEditing] = useState(false);
  const [form] = Form.useForm();
  const [imageUrl, setImageUrl] = useState("");
  const [formChanged, setFormChanged] = useState(false);

  // Trong useEffect, gọi API để lấy dữ liệu user
useEffect(() => {
  const fetchUserProfile = async () => {
    try {
      const token = localStorage.getItem("token"); // Lấy token từ localStorage
      const response = await axios.get(USER_PROFILE_API, {
        headers: AUTH_HEADERS(token),
      });

      dispatch(setUser(response.data)); // Cập nhật Redux với dữ liệu từ API
      form.setFieldsValue(response.data); // Set dữ liệu vào form
      setImageUrl(response.data.image || ""); // Hiển thị avatar nếu có
    } catch (error) {
      console.error("Error fetching user profile:", error);
    }
  };

  fetchUserProfile();
}, []);

const handleUpdate = async (values) => {
  try {
    const token = localStorage.getItem("token");
    const updatedData = { ...values, image: imageUrl };

    // Gửi yêu cầu PUT lên API
    const response = await axios.put(USER_PROFILE_API, updatedData, {
      headers: AUTH_HEADERS(token),
    });

    // Cập nhật Redux với dữ liệu mới từ API
    dispatch(setUser(response.data));

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


  const handleEdit = () => setIsEditing(true);

  const handleCancel = () => {
    setIsEditing(false);
    form.setFieldsValue(user);
    setImageUrl(user.image || "");
    setFormChanged(false);
  };

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
            <div>{<PlusOutlined />} <div>Upload</div></div>
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
            <Form.Item label="Full Name" name="fullName" rules={[{ required: true, message: "Please enter your full name!" }]}>
              <Input style={{backgroundColor:"white"}} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Email" name="email" rules={[{ required: true, message: "Please enter your email!" }, { type: "email", message: "Invalid email!" }]}>
              <Input style={{backgroundColor:"white"}} disabled />
            </Form.Item>
          </Col>
        </Row>
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item label="Phone" name="phone" rules={[{ required: true, message: "Please enter your phone number!" }]}>
              <Input style={{backgroundColor:"white"}} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Address" name="address">
              <Input style={{backgroundColor:"white"}} />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item style={{width: "70px"}} label="Age" name="age" rules={[{ required: true, message: "Please enter your age!" }]}>
              <Input style={{backgroundColor:"white"}} />
            </Form.Item>
          </Col>
        </Row>

        <div className={styles.buttonContainer}>
          <Button type="primary" htmlType="submit" disabled={!formChanged}>Update</Button>
          <Button onClick={handleCancel} style={{ marginLeft: 8 }}>Cancel</Button>
        </div>
      </Form>

      {!isEditing && <Button onClick={handleEdit} type="default" style={{ marginTop: 20 }}>Edit Profile</Button>}
    </div>
  );
};

export default CustomerProfile;
