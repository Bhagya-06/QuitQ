import { useEffect, useState } from "react";
import api from "../../api/axios";
import { Link } from "react-router-dom";
import { showSuccess, showError } from "../../utils/toast";

function Profile() {

  const [profile, setProfile] = useState(null);

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    try {

      const res = await api.get("/auth/profile");

      setProfile(res.data.data);

    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  if (!profile) {
    return (
      <div className="container mt-5 text-center">
        <h4>Loading Profile...</h4>
      </div>
    );
  }

  return (
    <div className="container mt-4">

      <div className="card shadow p-4">

        <div className="d-flex justify-content-between align-items-center mb-4">

          <h2>My Profile</h2>

          <Link
            to="/profile/edit"
            className="btn btn-primary"
          >
            Edit Profile
          </Link>

        </div>

        {/* USER DETAILS */}

        <div className="row">

          <div className="col-md-6">

            <h5 className="mb-3">User Information</h5>

            <p>
              <strong>Name:</strong> {profile.name}
            </p>

            <p>
              <strong>Email:</strong> {profile.email}
            </p>

            <p>
              <strong>Username:</strong> {profile.username}
            </p>

            <p>
              <strong>Phone:</strong> {profile.phone}
            </p>

            <p>
              <strong>Gender:</strong> {profile.gender || "Not Added"}
            </p>

            <p>
              <strong>Role:</strong> {profile.role}
            </p>

          </div>

          <div className="col-md-6">

            <h5 className="mb-3">Addresses</h5>

            {profile.addresses?.length > 0 ? (

              profile.addresses.map((a) => (

                <div
                  key={a.id}
                  className="border rounded p-3 mb-3"
                >
                  <h6>{a.addressName}</h6>

                  <p className="mb-1">{a.address1}</p>

                  <p className="mb-1">{a.city}</p>

                  <p className="mb-0">{a.pincode}</p>
                </div>

              ))

            ) : (
              <p>No addresses added</p>
            )}

          </div>

        </div>
        {/* SELLER DETAILS */}

        {profile.role === "Seller" && profile.seller && (

          <>
            <hr className="my-4" />

            <h4 className="mb-4">
              Seller Information
            </h4>

            <div className="row">

              {/* LEFT SIDE */}
              <div className="col-md-6">

                <div className="card border-0 shadow-sm p-3 h-100">

                  <h5 className="mb-3">
                    Store Details
                  </h5>

                  <p>
                    <strong>Seller ID:</strong>{" "}
                    {profile.seller.id}
                  </p>

                  <p>
                    <strong>Store Name:</strong>{" "}
                    {profile.seller.storeName}
                  </p>

                  <p>
                    <strong>City:</strong>{" "}
                    {profile.seller.city || "Not Added"}
                  </p>

                  <p>
                    <strong>Country:</strong>{" "}
                    {profile.seller.country || "Not Added"}
                  </p>

                  <p>
                    <strong>Verification Status:</strong>{" "}

                    <span
                      className={
                        profile.seller.verificationStatus === "Verified"
                          ? "text-success fw-bold"
                          : profile.seller.verificationStatus === "Rejected"
                            ? "text-danger fw-bold"
                            : "text-warning fw-bold"
                      }
                    >
                      {profile.seller.verificationStatus || "Pending"}
                    </span>

                  </p>

                </div>

              </div>

              {/* RIGHT SIDE */}
              <div className="col-md-6">

                <div className="card border-0 shadow-sm p-3 h-100">

                  <h5 className="mb-3">
                    Seller Documents
                  </h5>

                  <p>
                    <strong>GSTIN:</strong>{" "}
                    {profile.seller.gstin || "Not Added"}
                  </p>

                  <p>
                    <strong>ID Proof Number:</strong>{" "}
                    {profile.seller.idProofNumber || "Not Added"}
                  </p>

                  <p>
                    <strong>Bank Account:</strong>{" "}
                    {profile.seller.bankAccountNumber || "Not Added"}
                  </p>

                  <p>
                    <strong>IFSC:</strong>{" "}
                    {profile.seller.bankIfsc || "Not Added"}
                  </p>

                  <div className="d-flex gap-3 flex-wrap mt-3">

                    {profile.seller.idProofDocument && (

                      <a
                        href={profile.seller.idProofDocument}
                        target="_blank"
                        rel="noreferrer"
                        className="btn btn-outline-primary"
                      >
                        View ID Proof
                      </a>

                    )}

                    {profile.seller.businessLicense && (

                      <a
                        href={profile.seller.businessLicense}
                        target="_blank"
                        rel="noreferrer"
                        className="btn btn-outline-dark"
                      >
                        View Business License
                      </a>

                    )}

                  </div>

                </div>

              </div>

            </div>

          </>

        )}

      </div>

    </div>
  );
}

export default Profile;