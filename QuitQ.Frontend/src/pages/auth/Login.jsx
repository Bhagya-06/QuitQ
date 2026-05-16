import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { GoogleLogin } from "@react-oauth/google";
import { useAuth } from "../../hooks/useAuth";
import { showSuccess, showError } from "../../utils/toast";
import { validateLogin } from "../../utils/validations/authValidation";
import api from "../../api/axios";
import logo from "../../assets/logo.jpg";

function Login() {

  const navigate = useNavigate();

  const { login } = useAuth();

  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const handleChange = (e) => {

    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });

  };

  const parseJwt = (token) => {

    try {

      return JSON.parse(
        atob(token.split(".")[1])
      );

    } catch (e) {

      return null;

    }

  };

  const redirectByRole = (token) => {

    const decoded = parseJwt(token);

    const role =
      decoded?.role ||
      decoded?.[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];

    if (role === "Admin") {

      navigate("/admin");

    }
    else if (role === "Seller") {

      navigate("/seller");

    }
    else {

      navigate("/buyer");

    }

  };

  const handleSubmit = async (e) => {

    e.preventDefault();

    const errorMessage =
      validateLogin(formData);

    if (errorMessage) {

      showError(errorMessage);

      return;

    }

    try {

      const res = await login(formData);

      const token =
        res.data.data.token;

      localStorage.setItem(
        "token",
        token
      );

      redirectByRole(token);

      showSuccess(
        "Login successful"
      );

      

    } catch (err) {

      showError(
        err.response?.data?.message ||
        "Login failed"
      );

    }

  };

  const handleGoogleLogin = async (
    credentialResponse
  ) => {

    console.log(credentialResponse.credential);

    try {

      const res = await api.post(
        "/auth/google-login",
        {
          idToken:
            credentialResponse.credential,
        }
        
      );

      const token =
        res.data.data.token;

      localStorage.setItem(
        "token",
        token
      );

      localStorage.setItem(
        "role",
        "Buyer"
      );

      navigate("/buyer");
      
      showSuccess(
        "Google Login Success"
      );


    } catch (err) {

      showError(
        err.response?.data?.message ||
        "Google login failed"
      );

    }

  };

  return (

    <div className="container d-flex justify-content-center align-items-center">

      <div
        className="card border-0 shadow-lg p-4 position-relative"
        style={{
          maxWidth: "400px",
          width: "100%",
          borderRadius: "16px",
          marginTop: "45px"
        }}
      >

        {/* LOGO */}
        <div
          className="position-absolute start-50 translate-middle shadow"
          style={{
            top: "0",
            width: "50px",
            height: "50px",
            backgroundColor: "#212529",
            borderRadius: "16px",
            overflow: "hidden",
            padding: "8px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center"
          }}
        >

          <img
            src={logo}
            alt="QuitQ Logo"
            style={{
              width: "100%",
              height: "100%",
              objectFit: "contain"
            }}
          />

        </div>

        {/* FORM */}
        <div style={{ marginTop: "40px" }}>

          <h2 className="mb-4 text-center fw-bold text-dark">
            Login
          </h2>

          <form onSubmit={handleSubmit}>

            <div className="mb-3">

              <input
                type="email"
                name="email"
                placeholder="Email"
                className="form-control form-control-lg fs-6"
                value={formData.email}
                onChange={handleChange}
                style={{
                  borderRadius: "8px"
                }}
              />

            </div>

            <div className="mb-3">

              <input
                type="password"
                name="password"
                placeholder="Password"
                className="form-control form-control-lg fs-6"
                value={formData.password}
                onChange={handleChange}
                style={{
                  borderRadius: "8px"
                }}
              />

            </div>

            <button
              className="btn btn-dark w-100 fw-bold py-2 mb-3"
              style={{
                borderRadius: "8px",
                transition: "0.2s"
              }}
            >
              Login
            </button>

            <p className="text-center text-muted mb-4 small">

              Don't have an account?{" "}

              <Link
                to="/register"
                className="text-primary text-decoration-none fw-semibold"
              >
                Register
              </Link>

            </p>

            {/* SEPARATOR */}
            <div className="d-flex align-items-center my-3 text-muted">

              <hr className="flex-grow-1 m-0" />

              <span className="px-2 small text-uppercase">
                or
              </span>

              <hr className="flex-grow-1 m-0" />

            </div>

            {/* GOOGLE LOGIN */}
            <div className="d-flex justify-content-center">

              <GoogleLogin
                onSuccess={handleGoogleLogin}
                onError={() => {
                  showError(
                    "Google Login Failed"
                  );
                }}
              />

            </div>

          </form>

        </div>

      </div>

    </div>

  );

}

export default Login;