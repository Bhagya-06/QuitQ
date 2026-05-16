import { Routes, Route } from "react-router-dom";
import MainLayout from "../layouts/MainLayout.jsx";
import SellerLayout from "../layouts/SellerLayout.jsx";
import AdminLayout from "../layouts/AdminLayout.jsx";
import ProtectedRoute from "./ProtectedRoute";
import RoleRoute from "./RoleRoute";
import { publicRoutes, protectedRoutes, sellerRoutes, adminRoutes } from "./routeConfig";
import NotFound from "../pages/common/NotFound";

function AppRoutes() {
  return (
    <Routes>
      <Route element={<MainLayout />}>
        {publicRoutes.map((route) => (
          <Route key={route.path} path={route.path} element={route.element} />
        ))}

        {protectedRoutes.map((route) => (
          <Route
            key={route.path}
            path={route.path}
            element={<ProtectedRoute>{route.element}</ProtectedRoute>}
          />
        ))}

        <Route path="*" element={<NotFound />} />
      </Route>

      <Route element={<SellerLayout />}>
        {sellerRoutes.map((route) => (
          <Route
            key={route.path}
            path={route.path}
            element={
              <ProtectedRoute>
                <RoleRoute allowedRoles={["Seller"]}>{route.element}</RoleRoute>
              </ProtectedRoute>
            }
          />
        ))}
      </Route>

      <Route element={<AdminLayout />}>
        {adminRoutes.map((route) => (
          <Route
            key={route.path}
            path={route.path}
            element={
              <ProtectedRoute>
                <RoleRoute allowedRoles={["Admin"]}>{route.element}</RoleRoute>
              </ProtectedRoute>
            }
          />
        ))}
      </Route>
    </Routes>
  );
}

export default AppRoutes;
