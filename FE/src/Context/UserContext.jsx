import { createContext, useState, useContext, useEffect } from 'react';

// Create a context to manage user data
const UserContext = createContext();

// UserProvider component
export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  // Load user data from localStorage when the app loads
  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser)); // Parse stored user data
    }
  }, []);

  // Function to update user data and store it in localStorage
  const updateUser = (userData) => {
    setUser(userData);
    if (userData) {
      localStorage.setItem('user', JSON.stringify(userData)); // Store user as JSON
    } else {
      localStorage.removeItem('user'); // Remove user if null/empty
    }
  };

  return (
    <UserContext.Provider value={{ user, setUser: updateUser }}>
      {children}
    </UserContext.Provider>
  );
};

// Custom hook to access user data
export const useUser = () => {
  return useContext(UserContext);
};
