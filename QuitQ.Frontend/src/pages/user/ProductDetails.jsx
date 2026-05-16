import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ReviewSection from "../../components/ReviewSection";
import { showSuccess, showError } from "../../utils/toast";
import { getProductById } from "../../services/productService";
import { addToCart } from "../../services/cartService";

function ProductDetails() {
  const { id } = useParams();

  const [product, setProduct] = useState(null);

  useEffect(() => {
    loadProduct();
  }, []);

  const loadProduct = async () => {
    try {
      const res = await getProductById(id);
      setProduct(res.data.data);
    } catch (err) {
      showError(err.response?.data?.message || "Unable to load product");
    }
  };

  const handleAddToCart = async () => {
    try {
      await addToCart(product.id, 1);
      showSuccess("Product added to cart");
    } catch (err) {
      showError(err.response?.data?.message || "Unable to add product to cart");
    }
  };

  if (!product) {
    return <h3 className="text-center mt-5">Loading...</h3>;
  }

  return (
    <div className="container mt-4">

      <div className="row">

        <div className="col-md-5">

          <img
            src={product.imageUrl}
            alt={product.name}
            className="img-fluid rounded shadow"
          />

        </div>

        <div className="col-md-7">

          <h2>{product.name}</h2>

          <p className="text-muted">
            {product.brandName}
          </p>

          <h4 className="text-success">
            ₹ {product.price}
          </h4>

          <p>{product.description}</p>

          <p>
            <strong>Category:</strong>{" "}
            {product.categoryName}
          </p>

          <p>
            <strong>Seller:</strong>{" "}
            {product.storeName}
          </p>

          <p>
            <strong>Stock:</strong>{" "}
            {product.stock}
          </p>

          <p>
            ⭐ {product.averageRating?.toFixed(1)}
          </p>

          <button className="btn btn-primary" onClick={handleAddToCart}>
            Add To Cart
          </button>

          <ReviewSection productId={product.id} />
        </div>

      </div>
    </div>
  );
}

export default ProductDetails;