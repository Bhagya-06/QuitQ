import { useEffect, useState } from "react";
import ProductCard from "../../components/ProductCard";
import { showError } from "../../utils/toast";
import { getProducts } from "../../services/productService";

function SearchProducts() {

  const [products, setProducts] = useState([]);

  const [filters, setFilters] = useState({
    name: "",
    categoryName: "",
    brandName: "",
    minPrice: "",
    maxPrice: "",
    sortBy: "newest",
    page: 1,
  });

  useEffect(() => {
    searchProducts();
  }, [filters]);

  const handleChange = (e) => {

    setFilters({
      ...filters,
      [e.target.name]: e.target.value,
      page: 1,
    });
  };

  const searchProducts = async () => {

    try {

      const res = await getProducts(filters);

      setProducts(res.data.data.data);

    } catch (err) {

      showError(
        err.response?.data?.message ||
        "Unable to search products"
      );
    }
  };

  return (
    <div className="container mt-4">

      <h2 className="mb-4">
        Search Products
      </h2>

      <div className="row g-3 mb-4">

        <div className="col-md-3">
          <input
            className="form-control"
            placeholder="Product Name"
            name="name"
            value={filters.name}
            onChange={handleChange}
          />
        </div>

        <div className="col-md-2">
          <input
            className="form-control"
            placeholder="Category"
            name="categoryName"
            value={filters.categoryName}
            onChange={handleChange}
          />
        </div>

        <div className="col-md-2">
          <input
            className="form-control"
            placeholder="Brand"
            name="brandName"
            value={filters.brandName}
            onChange={handleChange}
          />
        </div>

        <div className="col-md-2">
          <input
            type="number"
            className="form-control"
            placeholder="Min Price"
            name="minPrice"
            value={filters.minPrice}
            onChange={handleChange}
          />
        </div>

        <div className="col-md-2">
          <input
            type="number"
            className="form-control"
            placeholder="Max Price"
            name="maxPrice"
            value={filters.maxPrice}
            onChange={handleChange}
          />
        </div>

        <div className="col-md-3">
          <select
            className="form-control"
            name="sortBy"
            value={filters.sortBy}
            onChange={handleChange}
          >
            <option value="newest">Newest</option>
            <option value="price_low_high">
              Price Low-High
            </option>

            <option value="price_high_low">
              Price High-Low
            </option>

            <option value="rating">
              Rating
            </option>

            <option value="popular">
              Popular
            </option>
          </select>
        </div>

      </div>

      <div className="row">

        {products.map((product) => (

          <div
            className="col-md-3 mb-4"
            key={product.id}
          >
            <ProductCard product={product} />
          </div>
        ))}

      </div>

    </div>
  );
}

export default SearchProducts;