import { useEffect, useState } from "react";
import api from "../../api/axios";
import { Link } from "react-router-dom";
import {
  showSuccess,
  showError,
} from "../../utils/toast";

function SellerProducts() {

  const [products, setProducts] =
    useState([]);

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {

    try {

      const res = await api.get(
        "/auth/seller-products"
      );

      setProducts(res.data.data);

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  const deleteProduct = async (id) => {

    if (
      !window.confirm(
        "Delete this product?"
      )
    ) return;

    try {

      await api.delete(`/product/${id}`);
      
      showSuccess("Product deleted");

      loadProducts();

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  return (
    <div className="container mt-4">

      <div className="d-flex justify-content-between align-items-center mb-4">

        <h2>My Products</h2>

        <Link
          to="/seller/add-product"
          className="btn btn-success"
        >
          Add Product
        </Link>

      </div>

      <div className="row">

        {products.map((product) => (

          <div
            className="col-md-4 mb-4"
            key={product.id}
          >

            <div className="card h-100 shadow-sm">

              <img
                src={product.imageUrl}
                className="card-img-top"
                style={{
                  height: "250px",
                  objectFit: "cover",
                }}
              />

              <div className="card-body">

                <h5>
                  {product.name}
                </h5>

                <p>
                  ₹ {product.price}
                </p>

                <p>
                  Stock:
                  {" "}
                  {product.stock}
                </p>

                <div className="d-flex gap-2">

                  <Link
                    to={`/seller/edit-product/${product.id}`}
                    className="btn btn-warning btn-sm"
                  >
                    Edit
                  </Link>

                  <button
                    className="btn btn-danger btn-sm"
                    onClick={() =>
                      deleteProduct(product.id)
                    }
                  >
                    Delete
                  </button>

                </div>

              </div>

            </div>

          </div>

        ))}

      </div>

    </div>
  );
}

export default SellerProducts;