import React from 'react';
import styles from './Header.module.css';

const Header = () => {
  return (
    <header className={styles.header}>
      <h1 className={styles.title}>Dashboard</h1>
      <div className={styles.userProfile}>
        <span className={styles.userName}>Welcome, Manager</span>
      </div>
    </header>
  );
};

export default Header;