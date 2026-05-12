import type { LoginResponse } from "../models/login/loginModel";
import { apiOptions } from "./apiOption";
import { apiCall, getUrl } from "./client";

const options = apiOptions;

export const authService = {
  login: async (
    email: string,
    password: string,
  ): Promise<LoginResponse | null> => {
    const response = await fetch(
      getUrl(options.baseUrl, options.loginEndpoint),
      {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      },
    );
    if (response.ok) return response.json();
    return null;
  },

  logout: async () => {
    await apiCall(getUrl(options.baseUrl, options.logoutEndpoint), {
      method: "POST",
    });
  },

  refresh: async (): Promise<LoginResponse> => {
    const response = await fetch(
      getUrl(options.baseUrl, options.resfreshTokenEndpoint),
      {
        method: "POST",
        credentials: "include",
      },
    );
    if (!response.ok) throw new Error("Refresh failed");
    return response.json();
  },
};
