import { Link } from "react-router-dom";
import {
  FaSearch,
  FaShoppingCart,
  FaTruck,
  FaUndo,
  FaShieldAlt,
  FaHeadphones,
  FaMobileAlt,
  FaTshirt,
  FaCouch,
  FaGamepad,
  FaBook,
  FaFootballBall,
} from "react-icons/fa";

import heroCharacter from "../../assets/hi-character.png";

function Home() {
  const categories = [
    {
      title: "Electronics",
      icon: <FaMobileAlt size={28} />,
    },
    {
      title: "Fashion",
      icon: <FaTshirt size={28} />,
    },
    {
      title: "Home & Living",
      icon: <FaCouch size={28} />,
    },
    {
      title: "Sports",
      icon: <FaFootballBall size={28} />,
    },
    {
      title: "Gaming",
      icon: <FaGamepad size={28} />,
    },
    {
      title: "Books",
      icon: <FaBook size={28} />,
    },
  ];

  const trendingProducts = [
    {
      name: "Wireless Headphones",
      price: "$99",
      image:
        "https://images.unsplash.com/photo-1505740420928-5e560c06d30e",
    },
    {
      name: "Smart Watch",
      price: "$129",
      image:
        "https://images.unsplash.com/photo-1523275335684-37898b6baf30",
    },
    {
      name: "Modern Chair",
      price: "$149",
      image:
        "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85",
    },
    {
      name: "Running Shoes",
      price: "$89",
      image:
        "https://images.unsplash.com/photo-1542291026-7eec264c27ff",
    },
  ];

  return (
    <div
      className="bg-white text-dark"
      style={{ minHeight: "100vh" }}
    >
      {/* HERO SECTION */}
      <section className="container py-5">
        <div className="row align-items-center">

          {/* LEFT */}
          <div className="col-lg-6 mb-5 mb-lg-0">
            <p
              className="fw-semibold text-uppercase mb-3"
              style={{ letterSpacing: "2px" }}
            >
              Discover. Shop. Enjoy.
            </p>

            <h1
              className="fw-bold lh-1 mb-4"
              style={{
                fontSize: "clamp(3rem, 8vw, 6rem)",
              }}
            >
              QUALITY
              <br />
              PRODUCTS
              <br />
              FOR EVERYONE
            </h1>

            <p
              className="text-secondary mb-4"
              style={{
                maxWidth: "500px",
                fontSize: "1.05rem",
              }}
            >
              Find everything you need in one place.
              Shop electronics, fashion, home essentials,
              books, sports gear and much more.
            </p>

            <div className="d-flex flex-wrap gap-3 mb-5">
              <Link
                to="/login"
                className="btn btn-dark px-4 py-3 fw-semibold"
              >
                Shop Now
              </Link>

              <Link
                to="/login"
                className="btn btn-outline-dark px-4 py-3 fw-semibold"
              >
                Explore Categories
              </Link>
            </div>

            {/* FEATURES */}
            <div className="row g-4">

              <div className="col-12 col-sm-4">
                <div className="d-flex align-items-start gap-3">
                  <FaTruck size={22} />
                  <div>
                    <h6 className="fw-bold mb-1">
                      Free Shipping
                    </h6>
                    <small className="text-secondary">
                      Orders over $50
                    </small>
                  </div>
                </div>
              </div>

              <div className="col-12 col-sm-4">
                <div className="d-flex align-items-start gap-3">
                  <FaUndo size={22} />
                  <div>
                    <h6 className="fw-bold mb-1">
                      Easy Returns
                    </h6>
                    <small className="text-secondary">
                      30-day return policy
                    </small>
                  </div>
                </div>
              </div>

              <div className="col-12 col-sm-4">
                <div className="d-flex align-items-start gap-3">
                  <FaShieldAlt size={22} />
                  <div>
                    <h6 className="fw-bold mb-1">
                      Secure Checkout
                    </h6>
                    <small className="text-secondary">
                      100% secure payment
                    </small>
                  </div>
                </div>
              </div>

            </div>
          </div>

          {/* RIGHT */}
          <div className="col-lg-6">
            <div
              className="position-relative d-flex justify-content-center"
            >
              {/* CIRCLE BG */}
              <div
                style={{
                  width: "500px",
                  height: "500px",
                  borderRadius: "50%",
                  background:
                    "linear-gradient(to bottom right, #f5f5f5, #e9e9e9)",
                  position: "absolute",
                  zIndex: 0,
                }}
              ></div>

              {/* CHARACTER */}
              <img
                src={heroCharacter}
                alt="QuitQ Character"
                className="img-fluid position-relative"
                style={{
                  maxHeight: "550px",
                  zIndex: 2,
                  objectFit: "contain",
                }}
              />

              {/* FLOATING CARD */}
              <div
                className="position-absolute bg-white shadow p-4"
                style={{
                  right: "-30px",
                  top: "-40px",
                  borderRadius: "20px",
                  maxWidth: "220px",
                  zIndex: 3,
                }}
              >
                <h5 className="fw-bold">
                  Hi there! 👋
                </h5>

                <p className="text-secondary mb-0">
                  Welcome to QuitQ.
                  Discover awesome products at amazing prices.
                </p>
              </div>
            </div>
          </div>

        </div>
      </section>

      {/* CATEGORY BAR */}
      <section className="bg-black text-white py-4">
        <div className="container">
          <div className="row text-center g-4">

            {categories.map((category) => (
              <div
                key={category.title}
                className="col-6 col-md-4 col-lg-2"
              >
                <div className="d-flex flex-column align-items-center">
                  <div className="mb-2">
                    {category.icon}
                  </div>

                  <span className="fw-semibold">
                    {category.title}
                  </span>
                </div>
              </div>
            ))}

          </div>
        </div>
      </section>

      {/* PROMO CARDS */}
      <section className="container py-5">
        <div className="row g-4">

          <div className="col-lg-4">
            <div
              className="p-4 h-100 border rounded-4 bg-light"
            >
              <p className="text-uppercase small fw-bold">
                Up to 50% Off
              </p>

              <h2 className="fw-bold mb-4">
                Deals Of
                <br />
                The Day
              </h2>

              <button className="btn btn-dark" to="/buyer">
                Shop Deals
              </button>
            </div>
          </div>

          <div className="col-lg-4">
            <div
              className="p-4 h-100 border rounded-4 bg-light"
            >
              <p className="text-uppercase small fw-bold">
                New Arrivals
              </p>

              <h2 className="fw-bold mb-4">
                Fresh Picks
                <br />
                Just For You
              </h2>

              <button className="btn btn-dark" to="/buyer">
                Explore Now
              </button>
            </div>
          </div>

          <div className="col-lg-4">
            <div
              className="p-4 h-100 border rounded-4 bg-light"
            >
              <p className="text-uppercase small fw-bold">
                Top Brands
              </p>

              <h2 className="fw-bold mb-4">
                Trusted By
                <br />
                Millions
              </h2>

              <button className="btn btn-dark" to="/buyer">
                View Brands
              </button>
            </div>
          </div>

        </div>
      </section>

      {/* POPULAR CATEGORIES */}
      <section className="container py-4">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h2 className="fw-bold">
            POPULAR CATEGORIES
          </h2>

          <Link
            className="text-dark text-decoration-none fw-semibold"
          >
            View All →
          </Link>
        </div>

        <div className="row g-4">

          {categories.map((category) => (
            <div
              key={category.title}
              className="col-6 col-md-4 col-lg-2"
            >
              <div
                className="border rounded-4 p-4 text-center h-100"
                style={{
                  transition: "0.3s",
                  cursor: "pointer",
                }}
              >
                <div className="mb-3">
                  {category.icon}
                </div>

                <h6 className="fw-bold mb-0">
                  {category.title}
                </h6>
              </div>
            </div>
          ))}

        </div>
      </section>

      {/* TRENDING PRODUCTS */}
      <section className="container py-5">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h2 className="fw-bold">
            TRENDING PRODUCTS
          </h2>

          <Link
            className="text-dark text-decoration-none fw-semibold"
          >
            View All Products →
          </Link>
        </div>

        <div className="row g-4">

          {trendingProducts.map((product) => (
            <div
              key={product.name}
              className="col-12 col-sm-6 col-lg-3"
            >
              <div
                className="card border-0 shadow-sm h-100 rounded-4 overflow-hidden"
              >
                <div
                  className="bg-light d-flex justify-content-center align-items-center"
                  style={{
                    height: "260px",
                  }}
                >
                  <img
                    src={product.image}
                    alt={product.name}
                    className="img-fluid"
                    style={{
                      height: "200px",
                      objectFit: "contain",
                    }}
                  />
                </div>

                <div className="card-body">
                  <h5 className="fw-bold">
                    {product.name}
                  </h5>

                  <p className="text-secondary mb-3">
                    Premium quality product
                  </p>

                  <div className="d-flex justify-content-between align-items-center">
                    <h5 className="fw-bold mb-0">
                      {product.price}
                    </h5>

                    <button className="btn btn-dark btn-sm">
                      View
                    </button>
                  </div>
                </div>
              </div>
            </div>
          ))}

        </div>
      </section>

      {/* JOIN BANNER */}
      <section className="container py-5">
        <div
          className="bg-black text-white rounded-5 p-5"
        >
          <div className="row align-items-center">

            <div className="col-lg-7">
              <h2
                className="fw-bold mb-3"
                style={{
                  fontSize: "3rem",
                }}
              >
                JOIN QUITQ CLUB!
              </h2>

              <p className="mb-4 text-light">
                Exclusive offers, member discounts,
                early access and rewards.
              </p>

              <Link
                to="/register"
                className="btn btn-light px-4 py-3 fw-semibold"
              >
                Join Now
              </Link>
            </div>

            <div className="col-lg-5 d-none d-lg-flex justify-content-center">
              <img
                src={heroCharacter}
                alt="QuitQ Character"
                style={{
                  height: "250px",
                  objectFit: "contain",
                }}
              />
            </div>

          </div>
        </div>
      </section>

      {/* FOOTER */}
      <footer className="bg-black text-white py-5 mt-5">
        <div className="container">

          <div className="row g-5">

            <div className="col-lg-4">
              <h2 className="fw-bold mb-3">
                QUITQ
              </h2>

              <p className="text-light">
                Your one-stop destination for
                quality products at unbeatable prices.
              </p>
            </div>

            <div className="col-6 col-lg-2">
              <h5 className="fw-bold mb-3">
                SHOP
              </h5>

              <ul className="list-unstyled">
                <li className="mb-2" to='/buyer'>
                  All Products
                </li>
                <li className="mb-2" to='/buyer'>
                  Featured
                </li>
                <li className="mb-2" to='/buyer'>
                  New Arrivals
                </li>
                <li className="mb-2" to='/buyer'>
                  Deals
                </li>
              </ul>
            </div>

            <div className="col-6 col-lg-3">
              <h5 className="fw-bold mb-3">
                SUPPORT
              </h5>

              <ul className="list-unstyled">
                <li className="mb-2" to='/contact'>
                  Contact Us
                </li>
                <li className="mb-2" to='/faqs'>
                  FAQs
                </li>
                <li className="mb-2" to='/shipping'>
                  Shipping
                </li>
                <li className="mb-2" to='/returns'>
                  Returns
                </li>
              </ul>
            </div>

            <div className="col-lg-3">
              <h5 className="fw-bold mb-3">
                STAY UPDATED
              </h5>

              <div className="input-group">
                <input
                  type="email"
                  className="form-control"
                  placeholder="Enter your email"
                />

                <button className="btn btn-light">
                  <FaSearch />
                </button>
              </div>
            </div>

          </div>

          <hr className="my-4" />

          <div className="text-center text-light">
            © 2026 QuitQ. All rights reserved.
          </div>

        </div>
      </footer>
    </div>
  );
}

export default Home;