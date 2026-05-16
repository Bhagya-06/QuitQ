import { createContext, useState, useCallback, useEffect } from "react";
import { loginUser, registerUser, logoutUser, isTokenValid, getUserRole, getUserId } from "../services/authService";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [auth, setAuth] = useState({
    token: localStorage.getItem("token") || null,
    userId: localStorage.getItem("userId") || null,
    role: localStorage.getItem("role") || null,
    user: null,
    isAuthenticated: !!localStorage.getItem("token"),
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const login = useCallback(async (credentials) => {

    setLoading(true);
    setError(null);

    try {

      const response = await loginUser(credentials);

      const token = response.data.data.token;

      const role = getUserRole(token);

      const userId = getUserId(token);

      localStorage.setItem("token", token);
      localStorage.setItem("role", role);
      localStorage.setItem("userId", userId);

      setAuth({
        token,
        role,
        userId,
        isAuthenticated: true,
        user: null,
      });

      return response;

    } catch (err) {

      const errorMessage =
        err.response?.data?.message ||
        "Login failed";

      setError(errorMessage);

      throw err;

    } finally {

      setLoading(false);
    }

  }, []);

  const register = useCallback(async (credentials) => {
    setLoading(true);
    setError(null);
    try {
      const response = await registerUser(credentials);
      return response;
    } catch (err) {
      const errorMessage = err.response?.data?.message || "Registration failed";
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("role");
    setAuth({
      token: null,
      userId: null,
      role: null,
      isAuthenticated: false,
      user: null,
    });
    setError(null);
  }, []);

  const value = {
    auth,
    loading,
    error,
    login,
    register,
    logout,
    isAuthenticated: auth.isAuthenticated,
    role: auth.role,
    userId: auth.userId,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
