import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { Dropdown, Menu, Button, Spin } from "antd";
import { toast } from "react-toastify";
import { LogoutOutlined, UserOutlined, ProfileOutlined, DownOutlined } from "@ant-design/icons";
import { logout as logoutAction } from "../../redux/auth/thunks";
import styles from "./AuthButtons.module.css";
import { useState } from "react";

const AuthButtons = () => {
  const { isAuthenticated, user } = useSelector((state) => state.auth);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogout = () => {
    setLoading(true);
    try{
      dispatch(logoutAction());
      navigate("/");
      toast.success("Logout Successfully!")
      navigate("/")
    } finally {
      setLoading(false);
    }
    
  };

  // Menu dropdown
  const menu = (
    <Menu>
      <Menu.Item key="profile" icon={<UserOutlined />}>
        <Link to="/profile">Profile</Link>
      </Menu.Item>
      <Menu.Item key="orders" icon={<ProfileOutlined />}>
        <Link to="/orders">Orders</Link>
      </Menu.Item>
      <Menu.Divider />
      <Menu.Item key="logout" icon={<LogoutOutlined />} onClick={handleLogout} disabled={loading}>
         {loading ? <Spin size="small" /> : "Logout"}
      </Menu.Item>
    </Menu>
  );

  return isAuthenticated ? (
    <Dropdown overlay={menu} trigger={["click"]} placement="bottomRight">
       <Button className={styles.profileButton}>
        Hello, {user?.fullName || "Profile"} <DownOutlined />
      </Button>
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
