import { useState, useEffect } from "react";
import api from "../../api/axios";
import { useNavigate } from "react-router-dom";
import { showSuccess, showError } from "../../utils/toast";
import { validateProductForm } from "../../utils/validations/sellerValidation";

function AddProduct() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    price: 0,
    stock: 0,
    imageUrl: "",
    categoryId: "",
    brandId: "",
  });

  const [errors, setErrors] = useState({});

  const [categories, setCategories] = useState([]);

  const [brands, setBrands] = useState([]);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {

    try {

      const categoryRes =
        await api.get("/category");

      const brandRes =
        await api.get("/brand");

      setCategories(categoryRes.data.data);
      setBrands(brandRes.data.data);

    } catch (err) {

      showError("Failed to load data");
    }
  };

  const handleSubmit = async (e) => {

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

      await api.post("/product", formData);

      showSuccess("Product added");

      navigate("/seller");

    } catch (err) {

      showError(err.response?.data?.message);
    }
  };

  return (
    <div className="container mt-4">

      <div
        className="card p-4 mx-auto"
        style={{ maxWidth: "600px" }}
      >

        <h2>Add Product</h2>

        <form onSubmit={handleSubmit}>

          <input
            name="name"
            placeholder="Product Name"
            className="form-control mb-3"
            onChange={handleChange}
          />

          {errors.name && (
            <small className="text-danger mb-3">
              {errors.name}
            </small>
          )}

          <textarea
            name="description"
            placeholder="Description"
            className="form-control mb-3"
            onChange={handleChange}
          />

          {errors.description && (
            <small className="text-danger mb-3">
              {errors.description}
            </small>
          )}

          <input
            type="number"
            name="price"
            placeholder="Price"
            className="form-control mb-3"
            onChange={handleChange}
          />

          {errors.price && (
            <small className="text-danger mb-3">
              {errors.price}
            </small>
          )}

          <input
            type="number"
            name="stock"
            placeholder="Stock"
            className="form-control mb-3"
            onChange={handleChange}
          />

          {errors.stock && (
            <small className="text-danger mb-3">
              {errors.stock}
            </small>
          )}

          <input
            name="imageUrl"
            placeholder="Image URL"
            className="form-control mb-3"
            onChange={handleChange}
          />

          {errors.imageUrl && (
            <small className="text-danger mb-3">
              {errors.imageUrl}
            </small>
          )}

          <select
            name="categoryId"
            className="form-control mb-3"
            value={formData.categoryId}
            onChange={handleChange}
          >

            <option value="">
              Select Category
            </option>

            {categories.map((category) => (

              <option
                key={category.id}
                value={category.id}
              >
                {category.name}
              </option>

            ))}

          </select>

          {errors.categoryId && (
            <small className="text-danger mb-3">
              {errors.categoryId}
            </small>
          )}

          <select
            name="brandId"
            className="form-control mb-3"
            value={formData.brandId}
            onChange={handleChange}
          >

            <option value="">
              Select Brand
            </option>

            {brands.map((brand) => (

              <option
                key={brand.id}
                value={brand.id}
              >
                {brand.name}
              </option>

            ))}

          </select>

          {errors.brandId && (
            <small className="text-danger mb-3">
              {errors.brandId}
            </small>
          )}

          <button className="btn btn-success w-100">
            Add Product
          </button>

        </form>
      </div>
    </div>
  );
}

export default AddProduct;