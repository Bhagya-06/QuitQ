import React from "react";

/**
 * Loading spinner component
 */
const Loader = ({ size = "md", fullScreen = false }) => {
  const sizeClass = {
    sm: "spinner-border-sm",
    md: "",
    lg: "spinner-border spinner-border-lg",
  }[size];

  if (fullScreen) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: "100vh" }}>
        <div className={`spinner-border text-primary ${sizeClass}`} role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="d-flex justify-content-center py-5">
      <div className={`spinner-border text-primary ${sizeClass}`} role="status">
        <span className="visually-hidden">Loading...</span>
      </div>
    </div>
  );
};

export default Loader;
