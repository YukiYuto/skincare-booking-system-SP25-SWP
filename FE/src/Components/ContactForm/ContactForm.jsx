import { Input, Button } from "antd";
import { UserOutlined, MailOutlined, PhoneOutlined, MessageOutlined } from "@ant-design/icons";
import styles from "./ContactForm.module.css";
import modelImage from "../../assets/images/model.jpg"; // Đổi đường dẫn ảnh nếu cần

const ContactForm = () => {
  return (
    <section className={styles.contactSection}>
      <div className={styles.container}>
        {/* Ảnh người mẫu */}
        <div className={styles.imageContainer}>
          <img src={modelImage} alt="Model" className={styles.modelImage} />
        </div>

        {/* Form liên hệ */}
        <div className={styles.formContainer}>
          <h2 className={styles.formTitle} style={{textAlign: 'center'}}>
            You may also fill out the form<br/> below and we will respond as<br/> quickly as possible.
          </h2>

          <div className={styles.inputGroup}>
            <Input placeholder="Full name" prefix={<UserOutlined />} className={styles.input} />
          </div>

          <div className={styles.inputRow}>
            <Input placeholder="Email" prefix={<MailOutlined />} className={styles.inputHalf} />
            <Input placeholder="Phone number" prefix={<PhoneOutlined />} className={styles.inputHalf} />
          </div>

          <div className={styles.inputGroup} style={{marginTop: "20px"}}>
            <Input.TextArea placeholder="Message" prefix={<MessageOutlined />} className={styles.textArea} rows={4} />
          </div>

          <Button type="primary" className={styles.submitButton}>
            Submit
          </Button>
        </div>
      </div>
    </section>
  );
};

export default ContactForm;
