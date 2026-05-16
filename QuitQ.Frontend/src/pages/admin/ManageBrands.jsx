import {
  useEffect,
  useState,
} from "react";

import api from "../../api/axios";

import {
  showSuccess,
  showError,
} from "../../utils/toast";

function ManageBrands() {

  const [brands, setBrands] =
    useState([]);

  const [name, setName] =
    useState("");

  const [editingId, setEditingId] =
    useState(null);

  useEffect(() => {
    loadBrands();
  }, []);

  const loadBrands = async () => {

    try {

      const res = await api.get("/brand");

      setBrands(res.data.data);

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  const saveBrand = async () => {

    if (!name.trim()) {
      showError("Brand name required");
      return;
    }

    try {

      if (editingId) {

        await api.put(
          `/brand/${editingId}`,
          JSON.stringify(name),
          {
            headers: {
              "Content-Type":
                "application/json",
            },
          }
        );

        showSuccess(
          "Brand updated"
        );

      } else {

        await api.post(
          "/brand",
          JSON.stringify(name),
          {
            headers: {
              "Content-Type":
                "application/json",
            },
          }
        );

        showSuccess(
          "Brand added"
        );
      }

      setName("");
      setEditingId(null);

      loadBrands();

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  const deleteBrand = async (id) => {

    try {

      await api.delete(`/brand/${id}`);

      showSuccess(
        "Brand deleted"
      );

      loadBrands();

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  return (
    <div className="container mt-4">

      <h2 className="mb-4">
        Manage Brands
      </h2>

      <div className="card p-4 mb-4">

        <input
          className="form-control mb-3"
          placeholder="Brand Name"
          value={name}
          onChange={(e) =>
            setName(e.target.value)
          }
        />

        <button
          className="btn btn-primary"
          onClick={saveBrand}
        >
          {editingId
            ? "Update Brand"
            : "Add Brand"}
        </button>

      </div>

      <table className="table table-bordered">

        <thead className="table-dark">

          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Products</th>
            <th>Actions</th>
          </tr>

        </thead>

        <tbody>

          {brands.map((brand) => (

            <tr key={brand.id}>

              <td>{brand.id}</td>

              <td>{brand.name}</td>

              <td>
                {brand.products.length}
              </td>

              <td>

                <button
                  className="btn btn-warning btn-sm me-2"
                  onClick={() => {

                    setEditingId(
                      brand.id
                    );

                    setName(
                      brand.name
                    );
                  }}
                >
                  Edit
                </button>

                <button
                  className="btn btn-danger btn-sm"
                  onClick={() =>
                    deleteBrand(
                      brand.id
                    )
                  }
                >
                  Delete
                </button>

              </td>

            </tr>

          ))}

        </tbody>

      </table>

    </div>
  );
}

export default ManageBrands;