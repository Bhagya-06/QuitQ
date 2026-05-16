import React from "react";

/**
 * Reusable submit button component
 */
const SubmitButton = ({
  label = "Submit",
  loading = false,
  disabled = false,
  type = "submit",
  variant = "primary",
  size = "md",
  className = "",
  onClick,
  children,
  fullWidth = false,
  ...props
}) => {
  const sizeClass = {
    sm: "btn-sm",
    md: "",
    lg: "btn-lg",
  }[size];

  const widthClass = fullWidth ? "w-100" : "";

  return (
    <button
      type={type}
      className={`btn btn-${variant} ${sizeClass} ${widthClass} ${className}`}
      disabled={loading || disabled}
      onClick={onClick}
      {...props}
    >
      {loading ? (
        <>
          <span
            className="spinner-border spinner-border-sm me-2"
            role="status"
            aria-hidden="true"
          ></span>
          Loading...
        </>
      ) : (
        children || label
      )}
    </button>
  );
};

export default SubmitButton;
