import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import api from "../../api/axios";
import { showSuccess, showError } from "../../utils/toast";
import { validateProductForm } from "../../utils/validations/sellerValidation";

function EditProduct() {
  const { id } = useParams();

  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    price: "",
    stock: "",
    imageUrl: "",
    categoryId: "",
    brandId: "",
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    loadProduct();
  }, []);

  const loadProduct = async () => {
    try {
      const res = await api.get(`/product/${id}`);

      const data = res.data.data;

      setFormData({
        name: data.name,
        description: data.description,
        price: data.price,
        stock: data.stock,
        imageUrl: data.imageUrl,
        categoryId: data.categoryId || "",
        brandId: data.brandId || "",
      });
    } catch (err) {
      showError("Failed to load product");
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const updateProduct = async (e) => {

    e.preventDefault();

    const validationErrors =
      validateProductForm(formData);

    if (
      Object.keys(validationErrors).length > 0
    ) {
      setErrors(validationErrors);
      return;
    }

    setErrors({});

    try {

      await api.put(`/product/${id}`, formData);

      showSuccess("Product updated");

      navigate("/seller");

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  return (
    <div className="container mt-4">
      <div className="card p-4">

        <h2>Edit Product</h2>

        <form onSubmit={updateProduct}>

          <input
            className="form-control mb-3"
            placeholder="Name"
            name="name"
            value={formData.name}
            onChange={handleChange}
          />

          {errors.name && (
            <small className="text-danger mb-3">
              {errors.name}
            </small>
          )}

          <textarea
            className="form-control mb-3"
            placeholder="Description"
            name="description"
            value={formData.description}
            onChange={handleChange}
          />

          {errors.description && (
            <small className="text-danger mb-3">
              {errors.description}
            </small>
           )}

          <input
            type="number"
            className="form-control mb-3"
            placeholder="Price"
            name="price"
            value={formData.price}
            onChange={handleChange}
          />

          {errors.price && (
            <small className="text-danger mb-3">
              {errors.price}
            </small>
          )}

          <input
            type="number"
            className="form-control mb-3"
            placeholder="Stock"
            name="stock"
            value={formData.stock}
            onChange={handleChange}
          />

          {errors.stock && (
            <small className="text-danger mb-3">
              {errors.stock}
            </small>
          )}

          <input
            className="form-control mb-3"
            placeholder="Image URL"
            name="imageUrl"
            value={formData.imageUrl}
            onChange={handleChange}
          />

          {errors.imageUrl && (
            <small className="text-danger mb-3">
              {errors.imageUrl}
            </small>
          )}

          <input
            type="number"
            className="form-control mb-3"
            placeholder="Category Id"
            name="categoryId"
            value={formData.categoryId}
            onChange={handleChange}
          />

          {errors.categoryId && (
            <small className="text-danger mb-3">
              {errors.categoryId}
            </small>
          )}

          <input
            type="number"
            className="form-control mb-3"
            placeholder="Brand Id"
            name="brandId"
            value={formData.brandId}
            onChange={handleChange}
          />

          {errors.brandId && (
            <small className="text-danger mb-3">
              {errors.brandId}
            </small>
          )}

          <button className="btn btn-primary w-100">
            Update Product
          </button>

        </form>
      </div>
    </div>
  );
}

export default EditProduct;