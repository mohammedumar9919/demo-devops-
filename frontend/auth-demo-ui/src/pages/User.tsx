import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

export default function User() {
  const auth = useAuth();
  const nav = useNavigate();

  function onLogout() {
    auth.logout();
    nav("/login", { replace: true });
  }

  return (
    <div className="container" style={{ minHeight: "90vh", display: "grid", placeItems: "center" }}>
      <div className="card" style={{ width: "100%", maxWidth: 720 }}>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", gap: 12 }}>
          <div>
            <div className="h1">Welcome User</div>
            <div className="p">
              Signed in as <span className="badge">{auth.me?.username}</span>
            </div>
          </div>

          <button className="btn" onClick={onLogout}>Logout</button>
        </div>

        <hr className="hr" />

        <div className="p">
          You have <span className="badge">user</span> access.
        </div>
      </div>
    </div>
  );
}