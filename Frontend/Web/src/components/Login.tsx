import { Visibility, VisibilityOff } from "@mui/icons-material";
import EmailIcon from "@mui/icons-material/Email";
import {
  Button,
  IconButton,
  InputAdornment,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import forest from "../assets/skog.png.avif";
import Box from "@mui/material/Box";
import { useState } from "react";
import "../css/header.css";
import { useAuthStore } from "../store/authStore";
import { useNavigate } from "react-router-dom";
import { authService } from "../api/authService";
export default function Login() {
  const [showPassword, setShowPassword] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const setAuth = useAuthStore((s) => s.setAuth);
  const navigate = useNavigate();
  return (
    <Box
      sx={{
        minHeight: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        px: 2,
        backgroundImage: `
      linear-gradient(rgba(0,0,0,0.45), rgba(0,0,0,0.45)),
      url(${forest})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
        flexDirection: "column",
        overflow: "hidden",
      }}
    >
      <Typography
        sx={{
          fontFamily: "Playfair Display, serif",
          fontWeight: 500,
          letterSpacing: "0.35em",
          textAlign: "center",
          marginBottom: 4,
          color: "rgba(255,255,255,0.85)",
          fontSize: {
            xs: "2rem",
            sm: "2rem",
            md: "2.6rem",
          },
        }}
      >
        STRÖMSDAL
      </Typography>
      <Paper
        elevation={4}
        sx={{
          width: "100%",
          maxWidth: 420,
          p: 4,
          borderRadius: 4,
          backgroundColor: "rgba(255,255,255,0.65)",
          backdropFilter: "blur(8px)",
          WebkitBackdropFilter: "blur(8px)",
        }}
      >
        <Typography
          sx={{
            fontSize: "1.5rem",
            fontWeight: 400,
            color: "rgba(0,0,0,0.65)",
            textAlign: "center",
            marginBottom: 3,
          }}
        >
          Logga in
        </Typography>

        <Stack spacing={3}>
          <TextField
            label="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            fullWidth
            slotProps={{
              input: {
                endAdornment: (
                  <InputAdornment position="end">
                    <EmailIcon sx={{ color: "rgba(0,0,0,0.35)" }} />
                  </InputAdornment>
                ),
              },
            }}
          />

          <TextField
            label="Password"
            type={showPassword ? "text" : "password"}
            value={password}
            onChange={(e) => {
              setPassword(e.target.value);
            }}
            fullWidth
            slotProps={{
              input: {
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      onClick={() => setShowPassword(!showPassword)}
                      edge="end"
                    >
                      {showPassword ? (
                        <VisibilityOff sx={{ color: "rgba(0,0,0,0.35)" }} />
                      ) : (
                        <Visibility sx={{ color: "rgba(0,0,0,0.35)" }} />
                      )}
                    </IconButton>
                  </InputAdornment>
                ),
              },
            }}
          />
          {error && (
            <Typography color="error" variant="body2" sx={{ mt: 1 }}>
              {error}
            </Typography>
          )}
          <Button
            variant="contained"
            size="large"
            fullWidth
            sx={{
              borderRadius: 3,
              textTransform: "none",
              fontWeight: 600,
              py: 1.5,
              backgroundColor: "#1f3a2d",
              "&:hover": {
                backgroundColor: "#2b4d3b",
              },
            }}
            onClick={async () => {
              if (email === "" || password === "") {
                setError("Vänligen fyll i både email och lösenord.");
                return;
              }
              let result = await authService.login(email, password);
              if (result === null) {
                setError("Fel email eller lösenord");
                return;
              }
              setAuth(
                {
                  firstName: result.firstName,
                  lastName: result.lastName,
                  role: result.role,
                },
                result.token,
              );
              navigate("/");
            }}
          >
            Logga in
          </Button>
          <Button
            variant="outlined"
            size="large"
            fullWidth
            sx={{
              borderRadius: 3,
              textTransform: "none",
              py: 1.5,
              borderColor: "rgba(0,0,0,0.25)",
              color: "#1f3a2d",
            }}
          >
            Glömt lösenord?
          </Button>
        </Stack>
      </Paper>
    </Box>
  );
}
