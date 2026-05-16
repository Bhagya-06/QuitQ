import { useEffect, useState } from "react";
import { showSuccess, showError } from "../utils/toast";
import { validateReview } from "../utils/validations/userValidation";
import {
  createReview,
  updateReview,
} from "../services/reviewService";

function ReviewModal({
  productId,
  review,
  show,
  onClose,
  onSuccess,
}) {

  const isEdit = !!review;

  const [rating, setRating] = useState(5);

  const [comment, setComment] = useState("");

  useEffect(() => {

    if (review) {

      setRating(review.rating);

      setComment(review.comment);

    } else {

      setRating(5);

      setComment("");
    }

  }, [review]);

  if (!show) return null;

  const submitReview = async (e) => {
    e.preventDefault();

    try {
      const errorMessage = validateReview(rating, comment);

      if (errorMessage) {
        showError(errorMessage);
        return;
      }

      if (isEdit) {

        await updateReview(review.id, {
          rating,
          comment,
        });

        showSuccess("Review updated");

      } else {

        await createReview({
          productId,
          rating: Number(rating),
          comment,
        });

        showSuccess("Review added");
      }

      onSuccess();

      onClose();

    } catch (err) {

      showError(
        err.response?.data?.message ||
        "Review submission failed"
      );
    }
  };

  return (

    <div
      className="modal d-block"
      style={{
        backgroundColor: "rgba(0,0,0,0.5)",
      }}
    >

      <div className="modal-dialog">

        <div className="modal-content">

          <div className="modal-header">

            <h5>
              {isEdit
                ? "Edit Review"
                : "Add Review"}
            </h5>

            <button
              className="btn-close"
              onClick={onClose}
            />

          </div>

          <div className="modal-body">

            <select
              className="form-control mb-3"
              value={rating}
              onChange={(e) =>
                setRating(Number(e.target.value))
              }
            >

              <option value={5}>5 ⭐</option>
              <option value={4}>4 ⭐</option>
              <option value={3}>3 ⭐</option>
              <option value={2}>2 ⭐</option>
              <option value={1}>1 ⭐</option>

            </select>

            <textarea
              className="form-control"
              rows="4"
              placeholder="Write review"
              value={comment}
              onChange={(e) =>
                setComment(e.target.value)
              }
            />

          </div>

          <div className="modal-footer">

            <button
              className="btn btn-secondary"
              onClick={onClose}
            >
              Cancel
            </button>

            <button
              className="btn btn-primary"
              onClick={submitReview}
            >
              {isEdit
                ? "Update Review"
                : "Submit Review"}
            </button>

          </div>

        </div>

      </div>

    </div>
  );
}

export default ReviewModal;