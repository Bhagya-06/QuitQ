export const validateLogin = (formData) => {

  if (!formData.email.trim()) {
    return "Email is required";
  }

  const emailRegex =
    /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  if (!emailRegex.test(formData.email)) {
    return "Enter a valid email";
  }

  if (!formData.password.trim()) {
    return "Password is required";
  }

  if (formData.password.length < 6) {
    return "Password must be at least 6 characters";
  }

  return null;
};

export const validateRegister = (formData) => {

  if (!formData.name.trim()) {
    return "Name is required";
  }

  if (formData.name.length < 3) {
    return "Name must be at least 3 characters";
  }

  if (!formData.email.trim()) {
    return "Email is required";
  }

  const emailRegex =
    /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  if (!emailRegex.test(formData.email)) {
    return "Enter a valid email";
  }

  if (!formData.username.trim()) {
    return "Username is required";
  }

  if (formData.username.length < 3) {
    return "Username must be at least 3 characters";
  }

  if (!formData.password.trim()) {
    return "Password is required";
  }

  if (formData.password.length < 6) {
    return "Password must be at least 6 characters";
  }

  if (
    formData.password !==
    formData.confirmPassword
  ) {
    return "Passwords do not match";
  }

  if (!formData.phone.trim()) {
    return "Phone number is required";
  }

  const phoneRegex = /^[0-9]{10}$/;

  if (!phoneRegex.test(formData.phone)) {
    return "Phone number must be 10 digits";
  }

  if (!formData.address.trim()) {
    return "Address is required";
  }

  if (!formData.addressName.trim()) {
    return "Address name is required";
  }

  if (!formData.city.trim()) {
    return "City is required";
  }

  const pinRegex = /^[0-9]{6}$/;

  if (!pinRegex.test(formData.pinCode)) {
    return "Pincode must be 6 digits";
  }

  if (
    formData.role === "Seller" &&
    !formData.storeName.trim()
  ) {
    return "Store name is required";
  }

  return null;
};