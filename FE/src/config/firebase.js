// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";
import { getFirestore } from "firebase/firestore";
// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyDxhCTDd9VPi0XvraWCKkNc8H16GG5vuoU",
  authDomain: "lumiconnect-swp391.firebaseapp.com",
  projectId: "lumiconnect-swp391",
  storageBucket: "lumiconnect-swp391.firebasestorage.app",
  messagingSenderId: "733632497320",
  appId: "1:733632497320:web:6e28902eb65fad50c6249d",
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

export const auth = getAuth();
export const db = getFirestore(app);
export default app;