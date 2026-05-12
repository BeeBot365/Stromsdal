import { Navigate, Outlet, useNavigate } from "react-router-dom";
import { useAuthStore } from "../store/authStore";
export default function ProtectedRoute() {
  const user = useAuthStore((s) => s.user); // Selector!

  // Inte inloggad → skicka till login
  const navigate = useNavigate();
  if (!user) {
    navigate("/login", { replace: true });
    return null;
  }

  // Inloggad → visa sidan
  return <Outlet />;
}
