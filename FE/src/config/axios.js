import axios from "axios";

const baseUrl = "https://localhost:7037/api/";
const config = {
  baseURL: baseUrl,
  timeout: 3000000,
};

const api = axios.create(config);

const handleBefore = (config) => {
  const token = localStorage.getItem("token")?.replaceAll('"', "");
  if (token) {
    config.headers["Authorization"] = `Bearer ${token}`;
  }
  return config;
};

const handleError = (error) => {
  console.error(error);
  return Promise.reject(error);
};

api.interceptors.request.use(handleBefore, handleError);

export default api;
