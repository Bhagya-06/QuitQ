import { Link, useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import logo from "../../assets/logo.jpg";

function Navbar() {
  const navigate = useNavigate();
  const location = useLocation();
  const { isAuthenticated, role, logout } = useAuth();

  // Hide entire navbar on authentication screens
  if (location.pathname === "/login" || location.pathname === "/register") {
    return null;
  }

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  const buyerLinks = [
    { label: "Search", to: "/search" },
    { label: "Cart", to: "/cart" },
    { label: "Wishlist", to: "/wishlist" },
    { label: "My Orders", to: "/my-orders" },
    { label: "Profile", to: "/profile" },
  ];

  const sellerLinks = [
    { label: "Dashboard", to: "/seller" },
    { label: "My Shop", to: "/seller/products" },
    { label: "Add Product", to: "/seller/add-product" },
    { label: "Orders", to: "/seller/orders" },
    { label: "Profile", to: "/profile" },
  ];

  const adminLinks = [
    { label: "Dashboard", to: "/admin" },
    { label: "Users", to: "/admin/users" },
    { label: "Sellers", to: "/admin/sellers" },
    { label: "Brands", to: "/admin/brands" },
    { label: "Categories", to: "/admin/categories" },
    { label: "Offers", to: "/admin/offers" },
  ];

  const getLinksByRole = () => {
    if (role === "Buyer") return buyerLinks;
    if (role === "Seller") return sellerLinks;
    if (role === "Admin") return adminLinks;
    return [];
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <div className="container">
        
        {/* LEFT SIDE */}
        <img src={logo} alt="QuitQ Logo" style={{ height: "40px", width: "auto", objectFit: "contain", marginRight: "10px" }}/>
        <Link className="navbar-brand fw-bold" to="/">
          QuitQ
        </Link>

        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>

        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ms-auto align-items-lg-center">
            
            {/* GUEST LINKS */}
            {!isAuthenticated && (
              <>
                <li className="nav-item">
                  <Link className="nav-link" to="/login">
                    Login
                  </Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/register">
                    Register
                  </Link>
                </li>
              </>
            )}

            {/* ROLE-BASED AUTHENTICATED LINKS */}
            {isAuthenticated && (
              <>
                {getLinksByRole().map((link) => (
                  <li className="nav-item" key={link.to}>
                    <Link className="nav-link" to={link.to}>
                      {link.label}
                    </Link>
                  </li>
                ))}
                
                {/* LOGOUT BUTTON */}
                <li className="nav-item ms-lg-3">
                  <button className="btn btn-danger" onClick={handleLogout}>
                    Logout
                  </button>
                </li>
              </>
            )}

          </ul>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
