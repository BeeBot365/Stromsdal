import { useEffect } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { useAuthStore } from "./store/authStore";
import Login from "./components/Login";
import "./global.css";
import ProtectedRoute from "./components/ProtectedRoute";
import Layout from "./layout/Layout";
import Header from "./layout/Header";
import { authService } from "./api/authService";
import CircularProgress from "@mui/material/CircularProgress";
export default function App() {
  const isLoading = useAuthStore((s) => s.isLoading);
  const setLoading = useAuthStore((s) => s.setLoading);
  const setAuth = useAuthStore((s) => s.setAuth);

  useEffect(() => {
    // Vid varje sidladdning – kolla om refresh cookie finns
    authService
      .refresh()
      .then((data) => {
        // Cookie fanns → fortfarande inloggad → fyll på storen
        setAuth(
          {
            firstName: data.firstName,
            lastName: data.lastName,
            role: data.role,
          },
          data.token,
        );
      })
      .catch(() => {
        // Ingen cookie → inte inloggad, gör ingenting
      })
      .finally(() => {
        setLoading(false); // Nu vet vi svaret oavsett
      });
  }, []);

  if (isLoading) return <CircularProgress aria-label="Loading…" />;

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route element={<ProtectedRoute />}>
          <Route element={<Layout />}>
            <Route path="/" element={<Header />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
