import {
  useEffect,
  useState,
} from "react";

import api from "../../api/axios";

import {
  showSuccess,
  showError,
} from "../../utils/toast";

function ManageCategories() {

  const [categories, setCategories]
    = useState([]);

  const [formData, setFormData]
    = useState({
      name: "",
      description: "",
    });

  const [editingId, setEditingId]
    = useState(null);

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {

    try {

      const res = await api.get(
        "/category"
      );

      setCategories(
        res.data.data
      );

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  const saveCategory = async () => {

    try {

      if (editingId) {

        await api.put(
          `/category/${editingId}`,
          null,
          {
            params: formData,
          }
        );

        showSuccess(
          "Category updated"
        );

      } else {

        await api.post(
          "/category",
          null,
          {
            params: formData,
          }
        );

        showSuccess(
          "Category added"
        );
      }

      setFormData({
        name: "",
        description: "",
      });

      setEditingId(null);

      loadCategories();

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  const deleteCategory = async (
    id
  ) => {

    try {

      await api.delete(
        `/category/${id}`
      );

      showSuccess(
        "Category deleted"
      );

      loadCategories();

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  return (
    <div className="container mt-4">

      <h2 className="mb-4">
        Manage Categories
      </h2>

      <div className="card p-4 mb-4">

        <input
          className="form-control mb-3"
          placeholder="Category Name"
          value={formData.name}
          onChange={(e) =>
            setFormData({
              ...formData,
              name: e.target.value,
            })
          }
        />

        <textarea
          className="form-control mb-3"
          placeholder="Description"
          value={
            formData.description
          }
          onChange={(e) =>
            setFormData({
              ...formData,
              description:
                e.target.value,
            })
          }
        />

        <button
          className="btn btn-primary"
          onClick={saveCategory}
        >
          {editingId
            ? "Update Category"
            : "Add Category"}
        </button>

      </div>

      <table className="table table-bordered">

        <thead className="table-dark">

          <tr>
            <th>Name</th>
            <th>Products</th>
            <th>Actions</th>
          </tr>

        </thead>

        <tbody>

          {categories.map((cat) => (

            <tr key={cat.id}>

              <td>{cat.name}</td>

              <td>
                {cat.products.length}
              </td>

              <td>

                <button
                  className="btn btn-warning btn-sm me-2"
                  onClick={() => {

                    setEditingId(cat.id);

                    setFormData({
                      name: cat.name,
                      description:
                        cat.description ||
                        "",
                    });
                  }}
                >
                  Edit
                </button>

                <button
                  className="btn btn-danger btn-sm"
                  onClick={() =>
                    deleteCategory(
                      cat.id
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

export default ManageCategories;