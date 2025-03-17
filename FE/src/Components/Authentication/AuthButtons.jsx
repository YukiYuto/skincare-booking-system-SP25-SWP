import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { Dropdown, Menu, Spin, Avatar, Divider } from "antd";
import { toast } from "react-toastify";
import { LogoutOutlined, UserOutlined, DownOutlined, ShoppingCartOutlined } from "@ant-design/icons";
import { logout as logoutAction } from "../../redux/auth/thunks";
import styles from "./AuthButtons.module.css";
import { useState } from "react";

const AuthButtons = () => {
  const { isAuthenticated, user } = useSelector((state) => state.auth);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleLogout = () => {
    setLoading(true);
    try{
      dispatch(logoutAction());
      toast.success("Logout Successfully!")
      navigate("/")
    } finally {
      setLoading(false);
    }
    
  };

  // Menu dropdown
  const menu = (
    <Menu className={styles.dropdownMenu}>
      {/* Phần hiển thị user */}
      <div className={styles.userInfo}>
        <Avatar
          src={user?.imageUrl}
          icon={!user?.imageUrl && <UserOutlined />}
          size={48}
        />
        <div>
          <span className={styles.userName}>{user?.fullName || "Profile"}</span>
        </div>
      </div>

      <Divider className={styles.menuDivider} />

      <Menu.Item key="profile" icon={<UserOutlined className={styles.menuIcon} />}>
        <Link to="/profile">Profile</Link>
      </Menu.Item>
      <Menu.Item key="orders" icon={<ShoppingCartOutlined className={styles.menuIcon} />}>
        <Link to="/appointment">Appointment</Link>
      </Menu.Item>

      <Divider className={styles.menuDivider} />

      <Menu.Item key="logout" icon={<LogoutOutlined className={styles.menuIcon} />} onClick={handleLogout} disabled={loading}>
        {loading ? <Spin size="small" /> : "Logout"}
      </Menu.Item>
    </Menu>
  );
  return isAuthenticated ? (
    <Dropdown overlay={menu} trigger={["click"]} placement="bottomRight">
       <div className={styles.profileButton}>
       <Avatar 
          src={user?.imageUrl} 
          icon={!user?.imageUrl && <UserOutlined />}
          size={60} 
        />
        <DownOutlined className={styles.downIcon} />
      </div>
    </Dropdown>
  ) : (
    <div className={styles.authButtons}>
      <Link to="/register" className={styles.registerButton}>
        Register
      </Link>
      <Link to="/login" className={styles.loginButton + " text-white"}>
        Login
      </Link>
    </div>
  );
};

export default AuthButtons;
