import React, { createContext, useEffect, useMemo, useState } from "react";
import { api, setBearer } from "../api/client";

export type Role = "admin" | "user";
export type Me = { username: string; role: Role };

type AuthState = {
  token: string | null;
  me: Me | null;
  loading: boolean;
  login: (username: string, password: string) => Promise<Me>;
  logout: () => void;
};

export const AuthContext = createContext<AuthState | null>(null);

const TOKEN_KEY = "token";

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [token, setToken] = useState<string | null>(() => localStorage.getItem(TOKEN_KEY));
  const [me, setMe] = useState<Me | null>(null);
  const [loading, setLoading] = useState(true);

  async function fetchMe(tkn: string): Promise<Me> {
    setBearer(tkn);
    const res = await api.get<Me>("/auth/me");
    setMe(res.data);
    return res.data;
  }

  async function login(username: string, password: string): Promise<Me> {
    // spec: POST /auth/login -> { token, username, role }
    const res = await api.post<{ token: string; username: string; role: Role }>("/auth/login", {
      username,
      password,
    });

    const tkn = res.data.token;
    localStorage.setItem(TOKEN_KEY, tkn);
    setToken(tkn);

    // always confirm identity from /auth/me (source of truth)
    return await fetchMe(tkn);
  }

  function logout() {
    localStorage.removeItem(TOKEN_KEY);
    setBearer(null);
    setToken(null);
    setMe(null);
  }

  useEffect(() => {
    (async () => {
      try {
        if (token) await fetchMe(token);
      } catch {
        logout();
      } finally {
        setLoading(false);
      }
    })();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const value = useMemo(() => ({ token, me, loading, login, logout }), [token, me, loading]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}