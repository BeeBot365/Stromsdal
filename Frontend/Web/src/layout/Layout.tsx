import Outlet from "@mui/icons-material/Outlet";
import Header from "./Header";

export default function Layout() {
  return (
    <>
      <Header />
      <main>
        <Outlet />
      </main>
    </>
  );
}
