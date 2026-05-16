import {
  useEffect,
  useState,
} from "react";

import api from "../../api/axios";

import {
  showSuccess,
  showError,
} from "../../utils/toast";

function ManageOffers() {

  const [offers, setOffers] =
    useState([]);

  const [formData, setFormData]
    = useState({
      title: "",
      discountPercentage: "",
      startDate: "",
      endDate: "",
      isActive: true,
    });

  useEffect(() => {
    loadOffers();
  }, []);

  const loadOffers = async () => {

    try {

      const res = await api.get(
        "/offer"
      );

      setOffers(res.data.data);

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  const createOffer = async () => {

    try {

      await api.post(
        "/offer",
        formData
      );

      showSuccess(
        "Offer created"
      );

      loadOffers();

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  const deleteOffer = async (id) => {

    try {

      await api.delete(
        `/offer/${id}`
      );

      showSuccess(
        "Offer deleted"
      );

      loadOffers();

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  return (
    <div className="container mt-4">

      <h2 className="mb-4">
        Manage Offers
      </h2>

      <div className="card p-4 mb-4">

        <input
          className="form-control mb-3"
          placeholder="Title"
          onChange={(e) =>
            setFormData({
              ...formData,
              title: e.target.value,
            })
          }
        />

        <input
          type="number"
          className="form-control mb-3"
          placeholder="Discount %"
          onChange={(e) =>
            setFormData({
              ...formData,
              discountPercentage:
                e.target.value,
            })
          }
        />

        <input
          type="datetime-local"
          className="form-control mb-3"
          onChange={(e) =>
            setFormData({
              ...formData,
              startDate:
                e.target.value,
            })
          }
        />

        <input
          type="datetime-local"
          className="form-control mb-3"
          onChange={(e) =>
            setFormData({
              ...formData,
              endDate:
                e.target.value,
            })
          }
        />

        <button
          className="btn btn-primary"
          onClick={createOffer}
        >
          Create Offer
        </button>

      </div>

      <table className="table table-bordered">

        <thead className="table-dark">

          <tr>
            <th>Title</th>
            <th>Discount</th>
            <th>Status</th>
            <th>Actions</th>
          </tr>

        </thead>

        <tbody>

          {offers.map((offer) => (

            <tr key={offer.id}>

              <td>{offer.title}</td>

              <td>
                {
                  offer.discountPercentage
                }%
              </td>

              <td>
                {offer.isActive
                  ? "Active"
                  : "Inactive"}
              </td>

              <td>

                <button
                  className="btn btn-danger btn-sm"
                  onClick={() =>
                    deleteOffer(
                      offer.id
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

export default ManageOffers;