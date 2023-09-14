import { Fragment, lazy } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import PrivateRoute from "./core/components/PrivateRoute";
// Admin
const Admin = lazy(() => import("./admin/pages/Admin"));
const Profile = lazy(() => import("./admin/pages/Profile"));
const Login = lazy(() => import("./auth/pages/Login"));

// Core
const Forbidden = lazy(() => import("./core/pages/Forbidden"));
const NotFound = lazy(() => import("./core/pages/NotFound"));

const GreenField = lazy(() => import("./admin/pages/Home"));

const AppRoutes = () => {
  return (
    <Fragment>
      <Routes>
        <Route path="/" element={<PrivateRoute component={Admin} />}>
          <Route path="greenfield" element={<PrivateRoute component={GreenField} />} />
          <Route
            path="profile"
            element={<PrivateRoute component={Profile} />}
          />
        </Route>
        <Route path="login" element={<Login />} />
        <Route path="403" element={<Forbidden />} />
        <Route path="404" element={<NotFound />} />
        <Route path="*" element={<Navigate to={`/404`} replace />} />
      </Routes>
    </Fragment>
  );
};

export default AppRoutes;
