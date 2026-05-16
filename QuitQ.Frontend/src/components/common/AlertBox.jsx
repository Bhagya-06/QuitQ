import React, { useState, useEffect } from "react";

/**
 * Alert box component
 */
const AlertBox = ({
  message,
  type = "info",
  dismissible = true,
  autoClose = null,
  onClose,
  className = "",
}) => {
  const [isVisible, setIsVisible] = useState(!!message);

  useEffect(() => {
    setIsVisible(!!message);
    if (autoClose && message) {
      const timer = setTimeout(() => {
        handleClose();
      }, autoClose);
      return () => clearTimeout(timer);
    }
  }, [message, autoClose]);

  const handleClose = () => {
    setIsVisible(false);
    if (onClose) onClose();
  };

  if (!isVisible) return null;

  const alertClass = {
    success: "alert-success",
    danger: "alert-danger",
    warning: "alert-warning",
    info: "alert-info",
    primary: "alert-primary",
  }[type];

  return (
    <div className={`alert ${alertClass} ${className}`} role="alert">
      <div className="d-flex justify-content-between align-items-start">
        <div>{message}</div>
        {dismissible && (
          <button
            type="button"
            className="btn-close"
            onClick={handleClose}
          ></button>
        )}
      </div>
    </div>
  );
};

export default AlertBox;
