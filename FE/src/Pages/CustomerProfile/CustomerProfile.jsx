import { Button, Col, Form, Input, Modal, Row, Spin, DatePicker } from "antd";
import { useDispatch, useSelector } from "react-redux";
import styles from "./CustomerProfile.module.css";
import Header from "../../Components/Common/Header";
import { useState } from "react";
import { toast } from "react-toastify";
import { updateUser } from "../../redux/auth/slice";
import { POST_CUSTOMER_AVATAR_API } from "../../config/apiConfig";
import {
  validateAddress,
  validatePhoneNumber,
} from "../../utils/validationUtils";
import { updateUserProfile } from "../../services/userService";
import { changePassword } from "../../services/authService";
import dayjs from "dayjs";

const UserProfile = () => {
  const { user, accessToken } = useSelector((state) => state.auth);
  const dispatch = useDispatch();
  const [form] = Form.useForm();
  const [uploadLoading, setUploadLoading] = useState(false);
  const [updateLoading, setUpdateLoading] = useState(false);
  const [imageUrl, setImageUrl] = useState(user.imageUrl);

  const calculateAge = (birthDateStr) => {
    const birthDate = new Date(birthDateStr);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age;
  };

  const [userData, setUserData] = useState({
    ...user,
    age: user.birthDate ? calculateAge(user.birthDate) : "",
  });

  const [isModalVisible, setIsModalVisible] = useState(false);
  const [passwords, setPasswords] = useState({
    oldPassword: "",
    newPassword: "",
    confirmNewPassword: "",
  });
  const [passwordLoading, setPasswordLoading] = useState(false);
  const [errors, setErrors] = useState({
    fullName: "",
    phoneNumber: "",
    address: "",
  });

  const formatFullname = (fullName) => {
    return fullName
      .split(" ")
      .map((word) => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
      .join(" ")
      .trim();
  };

  const handleFileChange = async (event) => {
    const file = event.target.files[0];
    if (!file) return;

    setUploadLoading(true);

    const formData = new FormData();
    formData.append("file", file);

    try {
      const response = await fetch(POST_CUSTOMER_AVATAR_API, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
        body: formData,
      });

      const data = await response.json();

      if (data.isSuccess && data.result) {
        setImageUrl(data.result);
        setUserData((prevData) => ({
          ...prevData,
          imageUrl: data.result,
        }));
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

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setUserData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
    setErrors((prevErrors) => ({
      ...prevErrors,
      [name]: "",
    }));
  };

  const validateProfileForm = () => {
    const fullNameError = userData.fullName ? "" : "Full Name is required!";
    const phoneError = validatePhoneNumber(userData.phoneNumber);
    const addressError = validateAddress(userData.address);

    setErrors({
      fullName: fullNameError,
      phoneNumber: phoneError,
      address: addressError,
    });

    return !(fullNameError || phoneError || addressError);
  };

  const handleUpdateProfile = async (e) => {
    e.preventDefault();
    setUpdateLoading(true);

    if (!validateProfileForm()) {
      setUpdateLoading(false);
      return;
    }

    const updatedData = {
      ...userData,
      fullName: formatFullname(userData.fullName),
      imageUrl,
    };

    try {
      const response = await updateUserProfile(updatedData);

      if (response.isSuccess) {
        dispatch(updateUser(updatedData));
        localStorage.setItem("user", JSON.stringify(updatedData));
        toast.success("Update Successfully!");
      } else {
        toast.error(response.message || "Update Failed!");
      }
    } catch (error) {
      toast.error("Error updating information: " + error.message);
    } finally {
      setUpdateLoading(false);
    }
  };

  const showChangePasswordModal = () => {
    setIsModalVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    setPasswords({ oldPassword: "", newPassword: "", confirmNewPassword: "" });
  };

  const handlePasswordChange = (e) => {
    setPasswords({ ...passwords, [e.target.name]: e.target.value });
  };

  const handleChangePassword = async () => {
    if (passwords.newPassword !== passwords.confirmNewPassword) {
      toast.error("New Password and Confirm Password do not match!");
      return;
    }

    setPasswordLoading(true);

    try {
      const response = await changePassword({
        userId: user.id,
        oldPassword: passwords.oldPassword,
        newPassword: passwords.newPassword,
        confirmNewPassword: passwords.confirmNewPassword,
      });

      if (response.isSuccess) {
        toast.success("Change password successfully!");
        setIsModalVisible(false);
      } else {
        toast.error(response.message || "Error changing password!");
      }
    } catch (error) {
      toast.error("Error changing password: " + error.message);
    } finally {
      setPasswordLoading(false);
    }
  };

  return (
    <div className={styles.bodyPage}>
      <Header />
      <div className={styles.profileContainer}>
        <h2>{user.roles} Profile</h2>
        <div>
          <p>Choose file here:</p>
          <input
            className={styles.fileinput}
            type="file"
            onChange={handleFileChange}
            disabled={uploadLoading}
          />
          <img
            className={styles.avatarpreview}
            src={imageUrl || userData.imageUrl}
            alt="Avatar"
          />
          {uploadLoading && <p>Uploading...</p>}
        </div>

        <Form form={form} layout="vertical">
          <Row style={{ marginTop: "20px" }} gutter={16}>
            <Col span={12}>
              <Form.Item label="Full Name">
                <Input
                  name="fullName"
                  value={userData.fullName}
                  onChange={handleInputChange}
                />
                {errors.fullName && (
                  <p className={styles.error}>{errors.fullName}</p>
                )}
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
                {errors.phoneNumber && (
                  <p className={styles.error}>{errors.phoneNumber}</p>
                )}
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Address">
                <Input
                  name="address"
                  value={userData.address}
                  onChange={handleInputChange}
                />
                {errors.address && (
                  <p className={styles.error}>{errors.address}</p>
                )}
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="Birth Date">
                <DatePicker
                  value={userData.birthDate ? dayjs(userData.birthDate) : null}
                  onChange={(date) => {
                    if (date) {
                      const isoDate = date.toISOString(); // send ISO format
                      setUserData((prev) => ({
                        ...prev,
                        birthDate: isoDate,
                        age: calculateAge(isoDate),
                      }));
                    } else {
                      setUserData((prev) => ({
                        ...prev,
                        birthDate: "",
                        age: "",
                      }));
                    }
                  }}
                  format="YYYY-MM-DD"
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Age">
                <p>{userData.age}</p>
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={24}>
              <Form.Item label="Gender">
                <div className={styles.genderContainer}>
                  <div className={styles.gender}>
                    <Input
                      type="radio"
                      name="gender"
                      value="Male"
                      checked={userData.gender === "Male"}
                      onChange={handleInputChange}
                    />
                    <p>Male</p>
                  </div>
                  <div className={styles.gender}>
                    <Input
                      type="radio"
                      name="gender"
                      value="Female"
                      checked={userData.gender === "Female"}
                      onChange={handleInputChange}
                    />
                    <p>Female</p>
                  </div>
                </div>
              </Form.Item>
            </Col>
          </Row>
        </Form>

        <div className={styles.buttonContainer}>
          <Button className={styles.updateButton} onClick={handleUpdateProfile}>
            {updateLoading ? "Saving..." : "Save"}
          </Button>
          <Button className={styles.passwordButton} onClick={showChangePasswordModal}>
            Change password
          </Button>
        </div>

        <Modal
          title="Change Password"
          open={isModalVisible}
          onCancel={handleCancel}
          footer={[
            <Button key="cancel" onClick={handleCancel}>
              Cancel
            </Button>,
            <Button
              key="submit"
              type="primary"
              onClick={handleChangePassword}
              disabled={passwordLoading}
            >
              {passwordLoading ? <Spin /> : "Save"}
            </Button>,
          ]}
        >
          <Form layout="vertical">
            <Form.Item label="Old Password">
              <Input.Password
                name="oldPassword"
                value={passwords.oldPassword}
                onChange={handlePasswordChange}
              />
            </Form.Item>
            <Form.Item label="New Password">
              <Input.Password
                name="newPassword"
                value={passwords.newPassword}
                onChange={handlePasswordChange}
              />
            </Form.Item>
            <Form.Item label="Confirm New Password">
              <Input.Password
                name="confirmNewPassword"
                value={passwords.confirmNewPassword}
                onChange={handlePasswordChange}
              />
            </Form.Item>
          </Form>
        </Modal>
      </div>
    </div>
  );
};

export default UserProfile;
