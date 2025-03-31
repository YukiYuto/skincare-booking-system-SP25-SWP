import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { Dropdown, Button, Menu } from "antd";
import { logout } from "../../actions/authActions";
import { useNavigate } from "react-router-dom";

const AuthButtons = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { isAuthenticated, user } = useSelector((state) => state.auth);

  const handleLogout = () => {
    dispatch(logout());
    navigate("/login");
  };

  const menu = (
    <Menu>
      <Menu.Item key="1">Profile</Menu.Item>
      <Menu.Item key="2" onClick={handleLogout}>
        Logout
      </Menu.Item>
    </Menu>
  );

  return isAuthenticated ? (
    <Dropdown menu={menu}>
      <Button>{user.name}</Button>
    </Dropdown>
  ) : (
    <Button>Login</Button>
  );
};

export default AuthButtons;
