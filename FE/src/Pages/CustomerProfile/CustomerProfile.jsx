import { useEffect, useState } from 'react';
import { Form, Input, Button, Row, Col, Upload, notification, Flex } from 'antd';
import { LoadingOutlined, PlusOutlined } from '@ant-design/icons';
import styles from './CustomerProfile.module.css';
import axios from 'axios';

const API_URL = 'https://670027404da5bd237553603f.mockapi.io/Users/1';

const CustomerProfile = () => {
  const [isEditing, setIsEditing] = useState(false);
  const [form] = Form.useForm();
  const [userData, setUserData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [imageUrl, setImageUrl] = useState('');

  // 🟢 Fetch dữ liệu người dùng từ MockAPI
  useEffect(() => {
    axios.get(API_URL)
      .then((response) => {
        setUserData(response.data);
        form.setFieldsValue(response.data); // Điền dữ liệu vào form
        setImageUrl(response.data.image); // Hiển thị avatar nếu có
      })
      .catch((error) => {
        console.error('Error fetching user data:', error);
      });
  }, []);

   // 🟢 Cập nhật dữ liệu lên API
   const handleUpdate = async (values) => {
    try {
      const updatedData = { ...values, image: imageUrl }; // Cập nhật ảnh
      await axios.put(API_URL, updatedData);
      setUserData(updatedData);
      notification.success({
        message: 'Update Successful',
        description: 'Your profile has been updated successfully.',
      });
      setIsEditing(false);
    } catch (error) {
      console.error('Error updating user:', error);
      notification.error({
        message: 'Update Failed',
        description: 'There was an error updating your profile.',
      });
    }
  };

  const handleEdit = () => {
    setIsEditing(true);
  };

  // 🔴 Hủy chỉnh sửa
  const handleCancel = () => {
    setIsEditing(false);
    form.setFieldsValue(userData); // Reset lại dữ liệu
    setImageUrl(userData.image); // Reset ảnh
  };

  // 🟢 Xử lý ảnh upload
  const handleChange = (info) => {
    if (info.file.status === 'uploading') {
      setLoading(true);
      return;
    }
    if (info.file.status === 'done') {
      const reader = new FileReader();
      reader.readAsDataURL(info.file.originFileObj);
      reader.onload = () => {
        setImageUrl(reader.result);
        form.setFieldValue('avatar', reader.result);
        setLoading(false);
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
        className="avatar-uploader"
        showUploadList={false}
        action="#" // Không upload lên server, chỉ xử lý local
        onChange={handleChange}
      >
        {imageUrl ? (
          <img src={imageUrl} alt="avatar" style={{ width: '100%' }} />
        ) : (
          <div>
            {loading ? <LoadingOutlined /> : <PlusOutlined />}
            <div>Upload</div>
          </div>
        )}
      </Upload>
      </Flex>
          <Form
            form={form}
            onFinish={handleUpdate}
            layout="vertical"
            disabled={!isEditing}
          >
            <Row gutter={16}>
              <Col span={12}>
                <Form.Item
                  label="Full Name"
                  name="FullName"
                  rules={[{ required: true, message: 'Please enter your full name!' }]}
                >
                  <Input style={{ color: 'black' }} />
                </Form.Item>
              </Col>
              <Col span={12}>
                <Form.Item
                  label="Email"
                  name="Email"
                  rules={[{ required: true, message: 'Please enter your email!' }, { type: 'email', message: 'Please enter a valid email!' }]}
                >
                  <Input style={{ color: 'black' }} />
                </Form.Item>
              </Col>
            </Row>
            <Row gutter={16}>
              <Col span={12}>
                <Form.Item
                  label="Password"
                  name="Password"
                  rules={[{ required: true, message: 'Please enter your password!' }]}
                >
                  <Input.Password style={{ color: 'black' }} />
                </Form.Item>
              </Col>
              <Col span={12}>
                <Form.Item
                  label="Phone"
                  name="Phone"
                  rules={[{ required: true, message: 'Please enter your phone number!' }]}
                >
                  <Input style={{ color: 'black' }} />
                </Form.Item>
              </Col>
            </Row>
            <Row gutter={16}>
              <Col span={12}>
                <Form.Item
                  label="Address"
                  name="Address"
                  rules={[{ required: true, message: 'Please enter your address!' }]}
                >
                  <Input style={{ color: 'black' }} />
                </Form.Item>
              </Col>
              <Col span={12}>
                <Form.Item
                  label="Age"
                  name="Age"
                  rules={[{ required: true, message: 'Please enter your age!' }]}
                >
                  <Input type="number" style={{ color: 'black' }} />
                </Form.Item>
              </Col>
            </Row>

            <div className={styles.buttonContainer}>
              <Button type="primary" htmlType="submit">
                Update
              </Button>
              <Button onClick={handleCancel} style={{ marginLeft: 8 }}>
                Cancel
              </Button>
            </div>
          </Form>
          {!isEditing && (
            <Button onClick={handleEdit } type="default" style={{ marginTop: 20 }}>
              Edit Profile
            </Button>
          )}
    </div>
  );
};

export default CustomerProfile;
