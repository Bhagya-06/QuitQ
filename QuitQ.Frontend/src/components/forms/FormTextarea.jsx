import React from "react";

/**
 * Reusable form textarea component
 */
const FormTextarea = ({
  label,
  name,
  value,
  onChange,
  onBlur,
  error,
  placeholder,
  required = false,
  disabled = false,
  rows = 3,
  className = "",
  helpText,
  maxLength,
  showCharCount = false,
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
      <textarea
        className={`form-control ${error ? "is-invalid" : ""} ${className}`}
        id={name}
        name={name}
        value={value}
        onChange={onChange}
        onBlur={onBlur}
        placeholder={placeholder}
        disabled={disabled}
        rows={rows}
        maxLength={maxLength}
        {...props}
      />
      <div className="d-flex justify-content-between">
        {error && (
          <div className="invalid-feedback d-block">{error}</div>
        )}
        {showCharCount && maxLength && (
          <small className="form-text text-muted ms-auto">
            {value.length}/{maxLength}
          </small>
        )}
      </div>
      {helpText && !error && (
        <small className="form-text text-muted">{helpText}</small>
      )}
    </div>
  );
};

export default FormTextarea;
