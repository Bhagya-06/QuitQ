import { Outlet } from "react-router-dom";
import Navbar from "../components/common/Navbar";

const AdminLayout = () => {
  return (
    <div className="d-flex flex-column min-vh-100">
      <Navbar />
      <main className="container py-4">
        <Outlet />
      </main>
    </div>
  );
};

export default AdminLayout;
