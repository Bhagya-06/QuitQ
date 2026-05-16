import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { showSuccess, showError } from "../../utils/toast";
import { getWishlist, removeFromWishlist } from "../../services/wishlistService";
import { addToCart } from "../../services/cartService";

function Wishlist() {

  const [products, setProducts] = useState([]);

  useEffect(() => {
    loadWishlist();
  }, []);

  const loadWishlist = async () => {
    try {
      const res = await getWishlist();
      setProducts(res.data.data);
    } catch (err) {
      showError(err.response?.data?.message || "Unable to load wishlist");
    }
  };

  const removeItem = async (productId) => {
    try {
      await removeFromWishlist(productId);
      showSuccess("Removed from wishlist");
      loadWishlist();
    } catch (err) {
      showError(err.response?.data?.message || "Unable to remove item");
    }
  };

  const addProductToCart = async (productId) => {
    try {
      await addToCart(productId, 1);
      showSuccess("Added to cart");
    } catch (err) {
      showError(err.response?.data?.message || "Unable to add product to cart");
    }
  };

  return (
    <div className="container mt-4">

      <h2 className="mb-4">My Wishlist</h2>

      {products.length === 0 ? (
        <div className="alert alert-info">
          Wishlist is empty
        </div>
      ) : (
        <div className="row">

          {products.map((product) => (

            <div className="col-md-3 mb-4" key={product.id}>

              <div className="card h-100 shadow-sm">

                <img
                  src={product.imageUrl}
                  className="card-img-top"
                  style={{
                    height: "220px",
                    objectFit: "cover",
                  }}
                />

                <div className="card-body d-flex flex-column">

                  <h5>{product.name}</h5>

                  <p>{product.category?.name}</p>

                  <h6>₹ {product.price}</h6>

                  <div className="mt-auto">

                    <Link
                      to={`/product/${product.id}`}
                      className="btn btn-primary btn-sm me-2"
                    >
                      View
                    </Link>

                    <button
                      className="btn btn-success btn-sm me-2"
                      onClick={() => addProductToCart(product.id)}
                    >
                      Cart
                    </button>

                    <button
                      className="btn btn-danger btn-sm"
                      onClick={() => removeItem(product.id)}
                    >
                      Remove
                    </button>

                  </div>

                </div>

              </div>

            </div>

          ))}

        </div>
      )}

    </div>
  );
}

export default Wishlist;