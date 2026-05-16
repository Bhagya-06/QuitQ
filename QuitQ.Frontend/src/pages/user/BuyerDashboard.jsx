import { useEffect, useState } from "react";
import ProductCard from "../../components/ProductCard";
import { showError } from "../../utils/toast";
import { getProducts } from "../../services/productService";
import api from "../../api/axios";
import hiCharacter from "../../assets/hi-character.png";

function BuyerDashboard() {
  const [products, setProducts] = useState([]);
  const [profile, setProfile] = useState(null);

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    try {
      const res = await api.get("/auth/profile");
      setProfile(res.data.data);
    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      const res = await getProducts({
        page: 1,
        sortBy: "popular",
      });
      setProducts(res.data.data.data);
    } catch (err) {
      showError(err.response?.data?.message || "Unable to load products");
    }
  };

  return (
    <div className="container mt-4">

      {/* HERO SECTION WITH FLEXBOX ALIGNMENT */}
      <div className="bg-dark text-white p-5 rounded mb-5 d-flex flex-column flex-md-row align-items-center justify-content-between position-relative overflow-visible">
        {/* TEXT CONTENT (LEFT SIDE) */}
        <div className="mb-4 mb-md-0">
          <h1 className="display-5 fw-bold">
            Welcome, {profile?.name || "Guest"} 👋
          </h1>
          <p className="lead mb-0">
            Discover amazing products at the best prices.
          </p>
        </div>

        {/* CHARACTER PNG */}
        <div
          className="position-relative"
          style={{
            width: "240px",
            height: "220px",overflow: "visible",
          }}
        >
          <img
            src={hiCharacter}
            alt="Character Saying Hi"
            style={{
              position: "absolute",
              right: "0",
              width: "100%",
              height: "auto",
              objectFit: "contain",
              transform: "scale(1.8)", // makes character larger
              filter: "drop-shadow(0px 8px 16px rgba(0,0,0,0.35))",
            }}
          />
        </div>

      </div>

      {/* FEATURED */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>Featured Products</h2>
      </div>

      <div className="row">
        {products.map((product) => (
          <div key={product.id} className="col-12 col-md-6 col-lg-3 mb-4">
            <ProductCard product={product} />
          </div>
        ))}
      </div>

    </div>
  );
}

export default BuyerDashboard;