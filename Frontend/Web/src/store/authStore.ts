import { create } from "zustand";
import { authService } from "../api/authService";

interface User {
  firstName: string;
  lastName: string;
  role: string;
}

interface AuthState {
  user: User | null;
  accessToken: string | null;
  isLoading: boolean;
  setAuth: (user: User, token: string) => void;
  setAccessToken: (token: string) => void;
  setLoading: (loading: boolean) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  // Startvärden
  user: null,
  accessToken: null,
  isLoading: true, // true från start – vi vet inte ännu om man är inloggad

  // Anropas vid lyckad login eller refresh
  setAuth: (user, token) => set({ user, accessToken: token }),

  // Anropas när vi får nytt access token (utan att user ändras)
  setAccessToken: (token) => set({ accessToken: token }),

  // Anropas när vi vet om man är inloggad eller inte
  setLoading: (loading) => set({ isLoading: loading }),

  // Anropas vid logout
  logout: () => {
    set({ user: null, accessToken: null });
    authService.logout();
  },
}));
