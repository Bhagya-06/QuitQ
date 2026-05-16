import { useEffect, useState } from "react";
import api from "../../api/axios";

import {
  showSuccess,
  showError,
} from "../../utils/toast";

function Sellers() {

  const [sellers, setSellers] =
    useState([]);

  useEffect(() => {
    fetchSellers();
  }, []);

  const fetchSellers = async () => {

    try {

      const res = await api.get(
        "/admin/sellers"
      );

      setSellers(res.data.data);

    } catch (err) {

      showError(
        err.response?.data?.message ||
        "Failed to load sellers"
      );
    }
  };

  const deactivateSeller = async (
    id
  ) => {

    const confirmDelete =
      window.confirm(
        "Deactivate seller?"
      );

    if (!confirmDelete) return;

    try {

      await api.delete(
        `/admin/seller/${id}`
      );

      showSuccess(
        "Seller deactivated"
      );

      fetchSellers();

    } catch (err) {

      showError(
        err.response?.data?.message ||
        "Failed to deactivate"
      );
    }
  };

  const verifySeller = async (
    id,
    status
  ) => {

    try {

      await api.put(
        `/admin/seller/${id}/verify?status=${status}`
      );

      showSuccess(
        `Seller ${status}`
      );

      fetchSellers();

    } catch (err) {

      showError(
        err.response?.data?.message ||
        "Verification failed"
      );
    }
  };

  return (
    <div className="container mt-5">

      <div className="d-flex justify-content-between align-items-center mb-4">

        <h2>
          Seller Verification
        </h2>

      </div>

      <div className="row">

        {sellers.map((seller) => (

          <div
            className="col-lg-6 mb-4"
            key={seller.id}
          >

            <div className="card shadow-sm h-100 border-0">

              <div className="card-body">

                {/* HEADER */}

                <div className="d-flex justify-content-between align-items-start mb-3">

                  <div>

                    <h4 className="mb-1">
                      {seller.storeName}
                    </h4>

                    <p className="text-muted mb-0">
                      Seller ID :
                      {" "}
                      {seller.id}
                    </p>

                  </div>

                  <span
                    className={`badge ${
                      seller.verificationStatus ===
                      "Verified"
                        ? "bg-success"
                        : seller.verificationStatus ===
                          "Rejected"
                        ? "bg-danger"
                        : "bg-warning text-dark"
                    }`}
                  >

                    {
                      seller.verificationStatus
                    }

                  </span>

                </div>

                <hr />

                {/* BASIC INFO */}

                <div className="mb-3">

                  <p className="mb-2">
                    <strong>Name :</strong>
                    {" "}
                    {seller.name}
                  </p>

                  <p className="mb-2">
                    <strong>Email :</strong>
                    {" "}
                    {seller.email}
                  </p>

                  <p className="mb-2">
                    <strong>Phone :</strong>
                    {" "}
                    {seller.phone || "N/A"}
                  </p>

                  <p className="mb-2">
                    <strong>City :</strong>
                    {" "}
                    {seller.city || "N/A"}
                  </p>

                  <p className="mb-2">
                    <strong>Country :</strong>
                    {" "}
                    {seller.country || "N/A"}
                  </p>

                  <p className="mb-2">
                    <strong>GSTIN :</strong>
                    {" "}
                    {seller.gstin || "N/A"}
                  </p>

                  <p className="mb-2">
                    <strong>Status :</strong>
                    {" "}
                    {seller.isActive
                      ? "Active"
                      : "Inactive"}
                  </p>

                </div>

                {/* DOCUMENTS */}

                <div className="mb-4">

                  <h5 className="mb-3">
                    Documents
                  </h5>

                  <div className="d-grid gap-2">

                    {seller.idProofDocument ? (
                      <a
                        href={
                          seller.idProofDocument
                        }
                        target="_blank"
                        rel="noreferrer"
                        className="btn btn-outline-primary"
                      >
                        View ID Proof
                      </a>
                    ) : (
                      <button
                        className="btn btn-outline-secondary"
                        disabled
                      >
                        No ID Proof
                      </button>
                    )}

                    {seller.businessLicense ? (
                      <a
                        href={
                          seller.businessLicense
                        }
                        target="_blank"
                        rel="noreferrer"
                        className="btn btn-outline-dark"
                      >
                        View Business License
                      </a>
                    ) : (
                      <button
                        className="btn btn-outline-secondary"
                        disabled
                      >
                        No Business License
                      </button>
                    )}

                  </div>

                </div>

                {/* ACTIONS */}

                <div className="d-flex gap-2 flex-wrap">

                  <button
                    className="btn btn-success"
                    onClick={() =>
                      verifySeller(
                        seller.id,
                        "Verified"
                      )
                    }
                  >
                    Verify
                  </button>

                  <button
                    className="btn btn-warning"
                    onClick={() =>
                      verifySeller(
                        seller.id,
                        "Rejected"
                      )
                    }
                  >
                    Reject
                  </button>

                  <button
                    className="btn btn-danger"
                    onClick={() =>
                      deactivateSeller(
                        seller.id
                      )
                    }
                  >
                    Deactivate
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

export default Sellers;