import { useEffect, useState } from "react";
import api from "../../api/axios";
import { useNavigate } from "react-router-dom";
import { showSuccess, showError } from "../../utils/toast";
import { validateProfile, validatePhone, validateRequired, validateAddressFields } from "../../utils/validations/userValidation";

function EditProfile() {
  const navigate = useNavigate();

  const [isSeller, setIsSeller] = useState(false);

  const [formData, setFormData] = useState({
    name: "",
    username: "",
    phone: "",
    gender: "",

    storeName: "",
    city: "",
    country: "",

    gstin: "",
    idProofNumber: "",
    idProofDocument: "",
    businessLicense: "",
    bankAccountNumber: "",
    bankIfsc: "",
  });

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {
    try {
      const res = await api.get("/auth/profile");

      const data = res.data.data;

      setIsSeller(data.role === "Seller");

      setFormData({
        name: data.name || "",
        username: data.username || "",
        phone: data.phone || "",
        gender: data.gender || "",
        city: data.city || "",
        country: data.country || "",

        storeName: data.seller?.storeName || "",
        gstin: data.seller?.gstin || "",
        idProofNumber: data.seller?.idProofNumber || "",
        idProofDocument: data.seller?.idProofDocument || "",
        businessLicense: data.seller?.businessLicense || "",
        bankAccountNumber:
          data.seller?.bankAccountNumber || "",
        bankIfsc: data.seller?.bankIfsc || "",
      });
    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const updateProfile = async (e) => {
    e.preventDefault();

    const errorMessage = validateProfile(formData);
    if (errorMessage) {
      showError(errorMessage);
      return;
    }

    const phoneError = validatePhone(formData.phone);
    if (phoneError) {
      showError(phoneError);
      return;
    }

    const addressError = validateAddressFields(formData.city, formData.country);
    if (addressError) {
      showError(addressError);
      return;
    }

    try {
      await api.put("/auth/updateprofile", formData);

      showSuccess("Profile updated");

      navigate("/profile");
    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  return (
    <div className="container mt-4">
      <div className="card p-4 mx-auto" style={{ maxWidth: "600px" }}>

        <h2 className="mb-4">Edit Profile</h2>

        <form onSubmit={updateProfile}>

          <input
            name="name"
            value={formData.name}
            onChange={handleChange}
            className="form-control mb-3"
            placeholder="Name"
          />

          <input
            name="username"
            value={formData.username}
            onChange={handleChange}
            className="form-control mb-3"
            placeholder="Username"
          />

          <input
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            className="form-control mb-3"
            placeholder="Phone"
          />

          <select
            name="gender"
            value={formData.gender}
            onChange={handleChange}
            className="form-control mb-3"
          >
            <option value="">Select Gender</option>
            <option>Male</option>
            <option>Female</option>
          </select>

          <input
            name="city"
            value={formData.city}
            onChange={handleChange}
            className="form-control mb-3"
            placeholder="City"
          />

          <input
            name="country"
            value={formData.country}
            onChange={handleChange}
            className="form-control mb-3"
            placeholder="Country"
          />

          {isSeller && (
            <>
              <input
                name="storeName"
                value={formData.storeName}
                onChange={handleChange}
                className="form-control mb-3"
                placeholder="Store Name"
              />

              <hr className="my-3" />

              <h5 className="mb-3">
                Seller Documents
              </h5>

              <input
                name="gstin"
                value={formData.gstin}
                onChange={handleChange}
                className="form-control mb-3"
                placeholder="GSTIN"
              />

              <input
                name="idProofNumber"
                value={formData.idProofNumber}
                onChange={handleChange}
                className="form-control mb-3"
                placeholder="ID Proof Number"
              />

              <input
                name="idProofDocument"
                value={formData.idProofDocument}
                onChange={handleChange}
                className="form-control mb-3"
                placeholder="ID Proof Document URL"
              />

              <input
                name="businessLicense"
                value={formData.businessLicense}
                onChange={handleChange}
                className="form-control mb-3"
                placeholder="Business License URL"
              />

              <input
                name="bankAccountNumber"
                value={formData.bankAccountNumber}
                onChange={handleChange}
                className="form-control mb-3"
                placeholder="Bank Account Number"
              />

              <input
                name="bankIfsc"
                value={formData.bankIfsc}
                onChange={handleChange}
                className="form-control mb-3"
                placeholder="Bank IFSC"
              />

            </>
          )}

          <button className="btn btn-primary w-100">
            Update Profile
          </button>

        </form>
      </div>
    </div>
  );
}

export default EditProfile;