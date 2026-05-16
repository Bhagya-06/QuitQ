import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { showSuccess, showError } from "../../utils/toast";
import { validateRegister } from "../../utils/validations/authValidation";
import { registerUser } from "../../services/authService";
import logo from "../../assets/logo.jpg";

export default function Register() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    name: "",
    email: "",
    username: "",
    password: "",
    confirmPassword: "",
    phone: "",
    address: "",
    role: "Buyer",
    addressName: "",
    city: "",
    pinCode: "",
    storeName: "",
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const register = async (e) => {
    e.preventDefault();

    const errorMessage = validateRegister(formData);
    if (errorMessage) {
      showError(errorMessage);
      return;
    }

    try {
      await registerUser(formData);
      showSuccess("Registration successful");
      navigate("/login");
    } catch (err) {
      showError(err.response?.data?.Errors?.[0] || err.response?.data?.Message || "Registration failed");
    }
  };

  return (
    <div className="container d-flex justify-content-center align-items-center">
      <div
        className="card border-0 shadow-lg p-4 position-relative"
        style={{ 
          maxWidth: "500px", 
          width: "100%", 
          borderRadius: "16px",
          marginTop: "45px" 
        }}
      >
        {/* LOGO: Tweaked halfway alignment matching login exactly */}
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

        {/* FORM CONTENT */}
        <div style={{ marginTop: "40px" }}>
          <h2 className="mb-4 text-center fw-bold text-dark">Register</h2>

          <form onSubmit={register}>
            <div className="mb-3">
              <input
                name="name"
                placeholder="Name"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="email"
                placeholder="Email"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="username"
                placeholder="Username"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="password"
                type="password"
                placeholder="Password"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="confirmPassword"
                type="password"
                placeholder="Confirm Password"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="phone"
                placeholder="Phone"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="address"
                placeholder="Address"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="addressName"
                placeholder="Address Name"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="city"
                placeholder="City"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <input
                name="pinCode"
                placeholder="Pin Code"
                className="form-control form-control-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              />
            </div>

            <div className="mb-3">
              <select
                name="role"
                className="form-select form-select-lg fs-6"
                onChange={handleChange}
                style={{ borderRadius: "8px" }}
              >
                <option value="Buyer">Buyer</option>
                <option value="Seller">Seller</option>
              </select>
            </div>

            {formData.role === "Seller" && (
              <div className="mb-3">
                <input
                  name="storeName"
                  placeholder="Store Name"
                  className="form-control form-control-lg fs-6"
                  onChange={handleChange}
                  style={{ borderRadius: "8px" }}
                />
              </div>
            )}

            <button 
              className="btn btn-dark w-100 fw-bold py-2 mb-3"
              style={{ borderRadius: "8px", transition: "0.2s" }}
            >
              Register
            </button>

            <p className="text-center text-muted mb-0 small">
              Already have an account?{" "}
              <Link to="/login" className="text-primary text-decoration-none fw-semibold">
                Login
              </Link>
            </p>
          </form>
        </div>

      </div>
    </div>
  );
}
