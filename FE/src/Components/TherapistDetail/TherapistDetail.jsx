import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { Card, Descriptions, Spin, Alert, Button } from 'antd';
import { ArrowLeftOutlined, MailOutlined, PhoneOutlined } from '@ant-design/icons';
import styles from './TherapistDetail.module.css';
import Header from '../Common/Header';

const TherapistDetail = () => {
  const { therapistId } = useParams();
  const [therapist, setTherapist] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get(`https://lumiconnect.azurewebsites.net/api/therapists/details/${therapistId}`)
      .then((response) => {
        setTherapist(response.data.result);
        setLoading(false);
      })
      .catch((error) => {
        setError(error.message);
        setLoading(false);
      });
  }, [therapistId]);

  if (loading) return <Spin size="large" className={styles.loading} />;
  if (error) return <Alert message="Fail to load data!" description={error} type="error" showIcon />;

  return (
    <div className={styles.therapistDetail}>
      <Header />
      <Button icon={<ArrowLeftOutlined />} className={styles.backButton} onClick={() => navigate(-1)}>
        Back
      </Button>
    <div className={styles.container}>
      <Card hoverable className={styles.card} cover={<img alt={therapist.fullName} src={therapist.imageUrl} className={styles.image} />}>
        <Descriptions title={therapist.fullName} column={1} bordered>
          <Descriptions.Item label="Age">{therapist.age}</Descriptions.Item>
          <Descriptions.Item label="Gender">{therapist.gender}</Descriptions.Item>
          <Descriptions.Item label="Experience">{therapist.experience}</Descriptions.Item>
          <Descriptions.Item label="Phone Number">
            <PhoneOutlined /> {therapist.phoneNumber}
          </Descriptions.Item>
          <Descriptions.Item label="Email">
            <MailOutlined /> {therapist.email}
          </Descriptions.Item>
        </Descriptions>
      </Card>
    </div>
    </div>
  );
};

export default TherapistDetail;
