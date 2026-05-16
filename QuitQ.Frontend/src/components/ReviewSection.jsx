import { useEffect, useState } from "react";
import api from "../api/axios";
import { showSuccess, showError } from "../utils/toast";

function ReviewSection({ productId }) {

  const [reviews, setReviews] = useState([]);

  useEffect(() => {
    loadReviews();
  }, [productId]);

  const loadReviews = async () => {

    try {

      const res = await api.get(
        `/review/product/${productId}`
      );

      setReviews(res.data.data);

    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  return (
    <div className="mt-5">

      <h3 className="mb-4">
        Customer Reviews
      </h3>

      {reviews.length === 0 ? (

        <div className="alert alert-info">
          No reviews yet
        </div>

      ) : (

        reviews.map((review) => (

          <div
            className="card p-3 mb-3"
            key={review.id}
          >

            <div className="d-flex justify-content-between">

              <div>

                <h6 className="mb-1">
                  {review.userName}
                </h6>

                <div className="text-warning">
                  {"⭐".repeat(review.rating)}
                </div>

              </div>

              <small>
                {new Date(
                  review.createdDate
                ).toLocaleDateString()}
              </small>

            </div>

            <p className="mt-3 mb-0">
              {review.comment}
            </p>

          </div>
        ))
      )}

    </div>
  );
}

export default ReviewSection;