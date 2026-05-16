import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { showSuccess, showError } from "../utils/toast";
import {
  addToWishlist,
  removeFromWishlist,
  checkWishlist,
} from "../services/wishlistService";
import { addToCart } from "../services/cartService";

function ProductCard({ product }) {
  const navigate = useNavigate();
  const [isWishlisted, setIsWishlisted] = useState(false);

  useEffect(() => {
    const initWishlist = async () => {
      try {
        const res = await checkWishlist(product.id);
        setIsWishlisted(res.data.data);
      } catch (err) {
        showError(err.response?.data?.message || "Unable to check wishlist");
      }
    };

    initWishlist();
  }, [product.id]);

  const handleAddToCart = async () => {
    try {
      await addToCart(product.id, 1);

      if (isWishlisted) {
        await removeFromWishlist(product.id);
        setIsWishlisted(false);
      }

      showSuccess("Added to cart");
    } catch (err) {
      showError(err.response?.data?.message || "Unable to add to cart");
    }
  };

  const toggleWishlist = async () => {
    try {
      if (isWishlisted) {
        await removeFromWishlist(product.id);
        setIsWishlisted(false);
        showSuccess("Removed from wishlist");
      } else {
        await addToWishlist(product.id);
        setIsWishlisted(true);
        showSuccess("Added to wishlist");
      }
    } catch (err) {
      showError(err.response?.data?.message || "Wishlist update failed");
    }
  };

  const buyNow = () => {
    navigate(
      `/checkout?productId=${product.id}&quantity=1`
    );
  };

  return (
    <div className="card h-100 shadow-sm">

      <div className="position-relative">
        <img
          src={product.imageUrl}
          className="card-img-top"
          style={{
            height: "220px",
            objectFit: "cover",
          }}
        />

        <button
          onClick={toggleWishlist}
          className="btn rounded-circle position-absolute top-0 end-0 m-2 d-flex align-items-center justify-content-center"
          style={{
            width: "45px",
            height: "45px",
            backgroundColor: "rgba(203, 203, 203, 0.77)",
            color: isWishlisted ? "red" : "white",
            fontSize: "22px",
            fontWeight: "bold",
            transition: "0.2s ease",
          }}
        >
          ♥
        </button>

      </div>

      <div className="card-body d-flex flex-column">

        <h5>{product.name}</h5>

        <p>{product.categoryName}</p>

        <h6>₹ {product.price}</h6>

        <p>{product.storeName}</p>

        <div className="mt-auto d-flex gap-2 flex-wrap">

          <Link
            to={`/product/${product.id}`}
            className="btn btn-primary btn-sm"
          >
            View
          </Link>

          <button
            className="btn btn-success btn-sm"
            onClick={handleAddToCart}
          >
            Add To Cart
          </button>

          <button
            className="btn btn-warning btn-sm"
            onClick={buyNow}
          >
            Buy Now
          </button>

        </div>
      </div>
    </div>
  );
}

export default ProductCard;