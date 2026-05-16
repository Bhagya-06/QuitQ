import React from "react";

/**
 * Reusable form input component
 */
const FormInput = ({
  label,
  name,
  type = "text",
  value,
  onChange,
  onBlur,
  error,
  placeholder,
  required = false,
  disabled = false,
  className = "",
  helpText,
  ...props
}) => {
  return (
    <div className="mb-3">
      {label && (
        <label htmlFor={name} className="form-label">
          {label}
          {required && <span className="text-danger">*</span>}
        </label>
      )}
      <input
        type={type}
        className={`form-control ${error ? "is-invalid" : ""} ${className}`}
        id={name}
        name={name}
        value={value}
        onChange={onChange}
        onBlur={onBlur}
        placeholder={placeholder}
        disabled={disabled}
        {...props}
      />
      {error && (
        <div className="invalid-feedback d-block">{error}</div>
      )}
      {helpText && !error && (
        <small className="form-text text-muted">{helpText}</small>
      )}
    </div>
  );
};

export default FormInput;
