import styles from "./AppointmentSection.module.css"; // Import CSS
import { Input, Button, Rate } from "antd"; // Dùng Ant Design
import { UserOutlined, MailOutlined, CommentOutlined } from "@ant-design/icons";
import relax from "../../assets/images/relax.jpg";
import fb88 from "../../assets/images/fb88.png";
import fpt from "../../assets/images/fpt.png";
import laliga from "../../assets/images/laliga.png";
import xoilac from "../../assets/images/xoilac.png";
import premierleague from "../../assets/images/premierleague.png";
import vleague from "../../assets/images/v-league.png";   

const AppointmentSection = () => {
  return (
    <div className={styles.container}>
      {/* Phần Our Sponsors */}
      <div className={styles.sponsors}>
        <h2 className={styles.title}>Our sponsors</h2>
        <div className={styles.logoContainer}>
          <span><img style={{width: "30px"}} src={fb88} alt=""/> FB88</span>
          <span><img style={{width: "30px"}} src={fpt} alt=""/> FPT</span>
          <span><img style={{width: "20px"}} src={laliga} alt=""/> Laliga</span>
          <span><img style={{width: "30px"}} src={xoilac} alt=""/> Xoilac</span>
          <span><img style={{width: "25px"}} src={premierleague} alt=""/> Premier</span>
          <span><img style={{width: "20px"}} src={vleague} alt=""/> V-League</span>
        </div>
        <img src={relax} alt="Spa Treatment" className={styles.spaImage} />
      </div>

      {/* Phần Form đặt lịch hẹn */}
      <div className={styles.formContainer}>
        <h2 className={styles.formTitle}>Votes & Comments</h2>
        <div className={styles.form}>
          
        <div className={styles.inputRow}>
          <span className={styles.label}>Your Vote:</span>
          <Rate className={styles.rate} />
        </div>

          <div className={styles.inputRow}>
          <Input placeholder="Full name" prefix={<UserOutlined />} className={styles.input} />
          <Input placeholder="Email" prefix={<MailOutlined />} className={styles.input} />
        </div>

        <Input.TextArea placeholder="Write your comment here..." prefix={<CommentOutlined />} className={styles.textArea} rows={4} />

        <Button type="primary" className={styles.submitButton}>Submit Vote</Button>
        </div>
      </div>
    </div>
  );
};

export default AppointmentSection;
