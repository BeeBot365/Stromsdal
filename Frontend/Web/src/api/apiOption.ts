interface APIOptions {
  baseUrl: string;
  loginEndpoint: string;
  resfreshTokenEndpoint: string;
  logoutEndpoint: string;
}

export const apiOptions: APIOptions = {
  baseUrl: "http://localhost:5262",
  loginEndpoint: "/api/auth/login",
  resfreshTokenEndpoint: "/api/auth/refresh",
  logoutEndpoint: "/api/auth/logout",
};
