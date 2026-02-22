import React, { useContext } from "react";
import { Navigate } from "react-router-dom";
import { AuthContext } from "./AuthContext";
import type { Role } from "./AuthContext";

export default function ProtectedRoute({
  requiredRole,
  children,
}: {
  requiredRole: Role;
  children: React.ReactNode;
}) {
  const auth = useContext(AuthContext);
  if (!auth) throw new Error("AuthContext missing");

  if (auth.loading) return <div className="container">Loading...</div>;
  if (!auth.me) return <Navigate to="/login" replace />;

  if (auth.me.role !== requiredRole) {
    // spec: prevent accessing other role view; redirect to own view
    return <Navigate to={auth.me.role === "admin" ? "/admin" : "/user"} replace />;
  }

  return <>{children}</>;
}