import React from "react";

/**
 * Empty state component - shows when no data is available
 */
const EmptyState = ({
  icon = "📦",
  title = "No Data",
  description = "No items found",
  actionLabel,
  onAction,
  className = "",
}) => {
  return (
    <div className={`text-center py-5 ${className}`}>
      <div style={{ fontSize: "4rem" }} className="mb-3">
        {icon}
      </div>
      <h5 className="card-title">{title}</h5>
      <p className="text-muted">{description}</p>
      {actionLabel && onAction && (
        <button
          className="btn btn-primary btn-sm mt-3"
          onClick={onAction}
        >
          {actionLabel}
        </button>
      )}
    </div>
  );
};

export default EmptyState;
