import { apiOptions } from "./apiOption";
import { useAuthStore } from "../store/authStore";

export async function apiCall(url: string, options?: RequestInit) {
  const token = useAuthStore.getState().accessToken;

  const response = await fetch(url, {
    ...options,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options?.headers,
    },
  });

  if (response.status === 401) {
    const refreshed = await tryRefresh();

    if (!refreshed) {
      useAuthStore.getState().logout();
      window.location.href = "/login";
      return response;
    }

    const newToken = useAuthStore.getState().accessToken;
    return fetch(url, {
      ...options,
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${newToken}`,
        ...options?.headers,
      },
    });
  }
  if (!response.ok) {
    throw new ApiError(response.status, await response.text());
  }

  return response;
}

export async function tryRefresh() {
  const response = await fetch(
    getUrl(apiOptions.baseUrl, apiOptions.resfreshTokenEndpoint),
    {
      method: "POST",
      credentials: "include", // httpOnly cookie
    },
  );

  if (!response.ok) return false;

  const data = await response.json();

  useAuthStore.getState().setAccessToken(data.token);
  return true;
}

export function getUrl(baseUrl: string, endpoint: string): string {
  return baseUrl + endpoint;
}

export class ApiError extends Error {
  public status: number;

  constructor(status: number, message: string) {
    super(message);
    this.name = "ApiError";
    this.status = status;
  }
}
