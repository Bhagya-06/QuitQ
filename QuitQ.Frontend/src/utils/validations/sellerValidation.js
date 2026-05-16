export const validateProductForm = (formData) => {

  const errors = {};

  // NAME

  if (!formData.name?.trim()) {
    errors.name = "Product name is required";
  } else if (formData.name.length < 3) {
    errors.name = "Product name must be at least 3 characters";
  }

  // DESCRIPTION

  if (!formData.description?.trim()) {
    errors.description = "Description is required";
  } else if (formData.description.length < 10) {
    errors.description =
      "Description must be at least 10 characters";
  }

  // PRICE

  if (
    formData.price === "" ||
    formData.price <= 0
  ) {
    errors.price =
      "Price must be greater than 0";
  }

  // STOCK

  if (
    formData.stock === "" ||
    formData.stock < 0
  ) {
    errors.stock =
      "Stock cannot be negative";
  }

  // IMAGE

  if (!formData.imageUrl?.trim()) {
    errors.imageUrl =
      "Image URL is required";
  } else if (
    !formData.imageUrl.startsWith("http")
  ) {
    errors.imageUrl =
      "Enter valid image URL";
  }

  // CATEGORY

  if (
    !formData.categoryId ||
    formData.categoryId <= 0
  ) {
    errors.categoryId =
      "Category is required";
  }

  // BRAND

  if (
    !formData.brandId ||
    formData.brandId <= 0
  ) {
    errors.brandId =
      "Brand is required";
  }

  return errors;
};