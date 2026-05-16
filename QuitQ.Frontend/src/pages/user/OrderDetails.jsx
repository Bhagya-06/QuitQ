import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import api from "../../api/axios";
import ReviewModal from "../../components/ReviewModal";
import { showSuccess, showError } from "../../utils/toast";

function OrderDetails() {

  const { id } = useParams();

  const [order, setOrder] = useState(null);

  const [reviews, setReviews] = useState([]);

  const [showReviewModal, setShowReviewModal]
    = useState(false);

  const [selectedProductId, setSelectedProductId]
    = useState(null);

  const [selectedReview, setSelectedReview]
    = useState(null);

  const currentUserId = Number(localStorage.getItem("userId"));

  useEffect(() => {

    loadOrder();

  }, []);

  useEffect(() => {
    if (order?.orderItems?.length) {
      loadReviews();
    }
  }, [order?.id]);

  const loadOrder = async () => {

    try {

      const res = await api.get(`/order/${id}`);

      setOrder(res.data.data);

    } catch (err) {

      showError(err.response?.data?.message);
    }
  };

  const loadReviews = async () => {
    try {
      const responses = await Promise.all(
        order.orderItems.map((item) =>
          api.get(`/review/product/${item.productId}`)
        )
      );

      const allReviews = responses.flatMap(
        (res) => res.data.data
      );

      setReviews(allReviews);

    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  const getUserReview = (productId) => {

    return reviews.find(
      (r) =>
        r.productId === productId &&
        r.userId === currentUserId
    );
  };

  const deleteReview = async (reviewId) => {

    try {

      await api.delete(`/review/${reviewId}`);

      showSuccess("Review deleted");

      loadReviews();

    } catch (err) {

      showError(err.response?.data?.message);
    }
  };

  if (!order) {

    return (
      <div className="container mt-4">
        Loading...
      </div>
    );
  }

  return (
    <div className="container mt-4">

      <div className="card p-4">

        <h2>Order #{order.id}</h2>

        <p>Status: {order.status}</p>

        <p>Total: ₹ {order.total}</p>

        <p>Tracking: {order.trackingNumber}</p>

        <hr />

        <h4>Items</h4>

        <table className="table">

          <thead>

            <tr>
              <th>Product Id</th>
              <th>Quantity</th>
              <th>Price</th>
              <th>Review</th>
            </tr>

          </thead>

          <tbody>

            {order.orderItems.map((item, index) => {

              const existingReview =
                reviews.find(
                  (r) =>
                    r.userId === currentUserId &&
                    r.productId === item.productId
                );

              console.log("Current User:", currentUserId);

              console.log("Reviews:", reviews);

              console.log(
                "Matching Review:",
                reviews.find(
                  (r) =>
                    r.userId === currentUserId &&
                    r.productId === item.productId
                )
              );

              return (

                <tr key={index}>

                  <td>{item.productId}</td>

                  <td>{item.quantity}</td>

                  <td>₹ {item.price}</td>

                  <td>

                    {existingReview ? (

                      <div className="d-flex gap-2">

                        <button
                          className="btn btn-warning btn-sm"
                          onClick={() => {

                            setSelectedReview(
                              existingReview
                            );

                            setSelectedProductId(
                              item.productId
                            );

                            setShowReviewModal(true);
                          }}
                        >
                          Edit Review
                        </button>

                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() =>
                            deleteReview(
                              existingReview.id
                            )
                          }
                        >
                          Delete
                        </button>

                      </div>

                    ) : (

                      <button
                        className="btn btn-primary btn-sm"
                        onClick={() => {

                          setSelectedReview(null);

                          setSelectedProductId(
                            item.productId
                          );

                          setShowReviewModal(true);
                        }}
                      >
                        Add Review
                      </button>

                    )}

                  </td>

                </tr>
              );
            })}

          </tbody>

        </table>

        <ReviewModal
          productId={selectedProductId}
          review={selectedReview}
          show={showReviewModal}
          onClose={() => {
            setShowReviewModal(false);
            setSelectedReview(null);
            setSelectedProductId(null);
          }}
          onSuccess={() => {
            loadReviews();
            setShowReviewModal(false);
          }}
        />

      </div>

    </div>
  );
}

export default OrderDetails;