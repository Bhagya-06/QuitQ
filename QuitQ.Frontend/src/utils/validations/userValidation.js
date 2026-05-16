export const validateRequired = (value, fieldName) => {
  if (
    value === null ||
    value === undefined ||
    value.toString().trim() === ""
  ) {
    return `${fieldName} is required`;
  }

  return "";
};

export const validateEmail = (email) => {
  if (!email) {
    return "Email is required";
  }

  const regex =
    /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  if (!regex.test(email)) {
    return "Invalid email address";
  }

  return "";
};

export const validatePhone = (phone) => {
  if (!phone) {
    return "Phone number is required";
  }

  const regex = /^[0-9]{10}$/;

  if (!regex.test(phone)) {
    return "Phone number must be 10 digits";
  }

  return "";
};

export const validatePassword = (password) => {
  if (!password) {
    return "Password is required";
  }

  if (password.length < 6) {
    return "Password must be at least 6 characters";
  }

  return "";
};

export const validatePrice = (price) => {
  if (price <= 0) {
    return "Price must be greater than 0";
  }

  return "";
};

export const validateQuantity = (quantity) => {
  if (quantity < 1) {
    return "Quantity must be at least 1";
  }

  return "";
};

export const validateAddress = (addressId) => {
  if (!addressId || addressId <= 0) {
    return "Please select an address";
  }

  return "";
};

export const validateAddressFields = (city, country) => {
  if (!city?.trim()) {
    return "City is required";
  }

  if (!country?.trim()) {
    return "Country is required";
  }

  return "";
};

export const validateProfile = (profileData) => {
  const { name, username, phone } = profileData;

  if (!name?.trim()) {
    return "Name is required";
  }

  if (!username?.trim()) {
    return "Username is required";
  }

  if (!phone?.trim()) {
    return "Phone is required";
  }

  return "";
};

export const validateReview = (
  rating,
  comment
) => {

  if (!rating || rating < 1 || rating > 5) {
    return "Rating must be between 1 and 5";
  }

  if (!comment?.trim()) {
    return "Review comment is required";
  }

  if (comment.trim().length < 5) {
    return "Review comment is too short";
  }

  return "";
};

export const validateCheckout = (
  addressId,
  paymentMethod
) => {

  if (!addressId) {
    return "Please select an address";
  }

  if (
    paymentMethod === null ||
    paymentMethod === undefined
  ) {
    return "Please select payment method";
  }

  return "";
};