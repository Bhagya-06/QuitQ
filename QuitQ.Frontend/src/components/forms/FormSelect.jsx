import React from "react";

/**
 * Reusable form select component
 */
const FormSelect = ({
  label,
  name,
  value,
  onChange,
  onBlur,
  error,
  options = [],
  placeholder = "Select an option",
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
      <select
        className={`form-select ${error ? "is-invalid" : ""} ${className}`}
        id={name}
        name={name}
        value={value}
        onChange={onChange}
        onBlur={onBlur}
        disabled={disabled}
        {...props}
      >
        <option value="">{placeholder}</option>
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
      {error && (
        <div className="invalid-feedback d-block">{error}</div>
      )}
      {helpText && !error && (
        <small className="form-text text-muted">{helpText}</small>
      )}
    </div>
  );
};

export default FormSelect;
