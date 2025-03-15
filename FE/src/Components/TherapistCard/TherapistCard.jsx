import { useEffect, useState } from 'react';
import axios from 'axios';
import { Card, Button, Spin, Alert, Pagination, Input, Select } from 'antd';
import styles from './TherapistCard.module.css';
import Header from '../Common/Header';
import { useNavigate } from 'react-router-dom';

const { Meta } = Card;
const { Search } = Input;
const { Option } = Select;

const TherapistCard = () => {
  const [therapists, setTherapists] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(10);
  const [searchQuery, setSearchQuery] = useState('');
  const [sortBy, setSortBy] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    setTherapists((prev) => [...prev].sort((a, b) => {
      if (sortBy === "fullName") return a.fullName.localeCompare(b.fullName);
      if (sortBy === "age") return Number(a.age || 0) - Number(b.age || 0);
      return 0;
    }));
  }, [sortBy]);
  

  useEffect(() => {
    axios
      .get('https://lumiconnect.azurewebsites.net/api/therapists')
      .then((response) => {
        setTherapists(Array.isArray(response.data.result) ? response.data.result : []);
        setLoading(false);
      })
      .catch((error) => {
        setError(error.message);
        setLoading(false);
      });
  }, []);

  if (loading) return <Spin size="large" className={styles.loading} />;
  if (error) return <Alert message="Fail to load data!" description={error} type="error" showIcon />;

  const filteredTherapists = therapists.filter((therapist) =>
    therapist.fullName.toLowerCase().includes(searchQuery.toLowerCase()) ||
    therapist.phoneNumber.includes(searchQuery)
  );

  const handleSortChange = (value) => {
    setSortBy(value);
  
    // Sắp xếp danh sách ngay lập tức
    setTherapists((prev) => [...prev].sort((a, b) => {
      if (value === "fullName") return a.fullName.localeCompare(b.fullName);
      if (value === "age") return Number(a.age || 0) - Number(b.age || 0);
      return 0;
    }));
  };

  const sortedTherapists = [...filteredTherapists].sort((a, b) => {
    if (sortBy === "fullName") return a.fullName.localeCompare(b.fullName);
    if (sortBy === "age") return Number(a.age || 0) - Number(b.age || 0);
    return 0;
  });  

  

  const indexOfLastTherapist = currentPage * pageSize;
  const indexOfFirstTherapist = indexOfLastTherapist - pageSize;
  const currentTherapists = filteredTherapists.slice(indexOfFirstTherapist, indexOfLastTherapist);

  return (
    <div className={styles.therapistList}>
    <Header />
    <h1 style={{ textAlign: 'center', margin: '20px 0' }}>Therapist</h1>
    <div className={styles.container}>
      <div className={styles.controls}>
        <Search
            placeholder="Search..."
            size="large"
            onChange={(e) => setSearchQuery(e.target.value)}
            className={styles.search}
        />
        <Select
              placeholder="Sort by"
              onChange={handleSortChange}
              className={styles.sort}
              allowClear
            >
              <Option value="fullName">Name (A-Z)</Option>
              <Option value="age">Age (Ascending)</Option>
        </Select>
      </div>
      {currentTherapists.length > 0 ? (
        <>
        <div className={styles.grid}>
        {currentTherapists.map((therapist) => (
          <Card
            key={therapist.id}
            hoverable
            className={styles.card}
            cover={<img alt={therapist.fullName} src={therapist.imageUrl} className={styles.image} />}
          >
            <Meta title={therapist.fullName} />
            <Button type="primary" className={styles.button} onClick={() => navigate(`/therapist/${therapist.skinTherapistId}`)}>Detail</Button>
          </Card>
        ))}
        </div>
        <Pagination
            current={currentPage}
            pageSize={pageSize}
            total={sortedTherapists.length}
            onChange={(page) => setCurrentPage(page)}
            className={styles.pagination}
          />
        </>
      ) : (
        <Alert message="Không có dữ liệu therapists" type="info" showIcon />
      )}
    </div>
    </div>
  );
};

export default TherapistCard;
