import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

export default function Login() {
  const auth = useAuth();
  if (!auth) throw new Error("AuthContext missing");

  const nav = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [err, setErr] = useState("");

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErr("");

    try {
      const me = await auth.login(username.trim(), password);
      nav(me.role === "admin" ? "/admin" : "/user", { replace: true });
    } catch {
      setErr("Invalid username or password");
    }
  }

  return (
    <div className="container" style={{ display: "grid", placeItems: "center", minHeight: "90vh" }}>
      <div className="card" style={{ width: "100%", maxWidth: 460 }}>
        <div className="h1">JSON Auth Demo</div>
        <div className="p">Sign in with a username from <span className="badge">users.json</span></div>

        <form onSubmit={onSubmit}>
          <div style={{ marginBottom: 12 }}>
            <label className="p" style={{ display: "block", marginBottom: 6 }}>Username</label>
            <input className="input" value={username} onChange={(e) => setUsername(e.target.value)} />
          </div>

          <div style={{ marginBottom: 12 }}>
            <label className="p" style={{ display: "block", marginBottom: 6 }}>Password</label>
            <input className="input" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
          </div>

          <button className="btn" style={{ width: "100%" }}>Login</button>

          {err && <div className="error">{err}</div>}
        </form>

        <hr className="hr" />
        <div className="p" style={{ marginBottom: 0 }}>
          API: <span className="badge">https://localhost:7238</span>
        </div>
      </div>
    </div>
  );
}