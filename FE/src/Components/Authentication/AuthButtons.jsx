import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { Dropdown, Menu, Spin, Avatar, Divider } from "antd";
import { toast } from "react-toastify";
import {
  LogoutOutlined,
  UserOutlined,
  DownOutlined,
  ShoppingCartOutlined,
  DashboardOutlined,
  LineChartOutlined,
  BarChartOutlined,
  MessageOutlined,
} from "@ant-design/icons";
import { logout as logoutAction } from "../../redux/auth/thunks";
import styles from "./AuthButtons.module.css";
import { useState } from "react";
import FeedbackModal from "../Feedback/FeedbackModal";

const AuthButtons = () => {
  const { isAuthenticated, user } = useSelector((state) => state.auth);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [isFeedbackModalOpen, setFeedbackModalOpen] = useState(false); // State để mở modal

  const handleLogout = () => {
    setLoading(true);
    try {
      dispatch(logoutAction());
      toast.success("Logout Successfully!");
      navigate("/login");
    } finally {
      setLoading(false);
    }
  };

  // Xác định đường dẫn dựa trên quyền user
  const getRoleBasedPath = () => {
    if (!user?.roles || !Array.isArray(user.roles)) return null;

    if (user.roles.includes("CUSTOMER")) {
      return {
        path: "/appointments",
        label: "Appointment",
        icon: <ShoppingCartOutlined />,
      };
    }

    if (user.roles.includes("SKINTHERAPIST")) {
      return { path: "/therapist-management", label: "Therapist Management", icon: <BarChartOutlined /> };
    }

    if (user.roles.includes("STAFF")) {
      return {
        path: "/staff-management",
        label: "Staff Management",
        icon: <LineChartOutlined />,
      };
    }

    if (user.roles.includes("ADMIN") || user.roles.includes("MANAGER")) {
      return {
        path: "/dashboard",
        label: "Dashboard",
        icon: <DashboardOutlined />,
      };
    }

    return null;
  };

  const roleBasedItem = getRoleBasedPath();

  const menu = (
    <Menu className={styles.dropdownMenu}>
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

      {roleBasedItem && (
        <Menu.Item key={roleBasedItem.path} icon={roleBasedItem.icon} className={styles.menuIcon}>
          <Link to={roleBasedItem.path}>{roleBasedItem.label}</Link>
        </Menu.Item>
      )}

      {/* Nút mở modal feedback cho CUSTOMER */}
      {user?.roles?.includes("CUSTOMER") && (
        <Menu.Item
          key="feedback"
          icon={<MessageOutlined className={styles.menuIcon} />}
          onClick={() => setFeedbackModalOpen(true)}
        >
          Feedback
        </Menu.Item>
      )}

      <Divider className={styles.menuDivider} />

      <Menu.Item key="logout" icon={<LogoutOutlined className={styles.menuIcon} />} onClick={handleLogout} disabled={loading}>
        {loading ? <Spin size="small" /> : "Logout"}
      </Menu.Item>
    </Menu>
  );

  return (
    <>
      {isAuthenticated ? (
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
      )}

      {/* Hiển thị Modal Feedback nếu mở */}
      <FeedbackModal isOpen={isFeedbackModalOpen} onClose={() => setFeedbackModalOpen(false)} />
    </>
  );
};

export default AuthButtons;
