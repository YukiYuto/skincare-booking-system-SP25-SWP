// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";
import { GoogleAuthProvider } from "firebase/auth";
import { getFirestore } from "firebase/firestore";
import { getStorage } from "firebase/storage";

// just change config below to start
const firebaseConfig = {
  apiKey: "AIzaSyBWj4xlnMiRengkwp3GGBUe3SFyfHurSAM",
  authDomain: "swp391-791a4.firebaseapp.com",
  projectId: "swp391-791a4",
  storageBucket: "swp391-791a4.appspot.com",
  messagingSenderId: "276263249414",
  appId: "1:276263249414:web:ec7ed1567705739610c587",
  measurementId: "G-SJEMJTYD1D"
};
// Initialize Firebase
export const app = initializeApp(firebaseConfig);
export const auth = getAuth();
export const storage = getStorage();
export const db = getFirestore();
export const ggProvider = new GoogleAuthProvider();