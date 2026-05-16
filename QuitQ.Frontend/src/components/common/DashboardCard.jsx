import React from "react";

/**
 * Dashboard card component - for showing statistics
 */
const DashboardCard = ({
  title,
  value,
  icon,
  bgColor = "bg-primary",
  textColor = "text-white",
  trend,
  trendIcon = "📈",
  onClick,
  className = "",
  suffix = "",
  description = "",
}) => {
  return (
    <div
      className={`card h-100 ${className}`}
      onClick={onClick}
      style={onClick ? { cursor: "pointer" } : {}}
    >
      <div className={`card-body ${bgColor} ${textColor} rounded-top`}>
        <div className="d-flex justify-content-between align-items-start">
          <div>
            <p className="card-text opacity-75 mb-2">{title}</p>
            <h3 className="card-title mb-0">
              {value}
              {suffix && <small className="ms-1">{suffix}</small>}
            </h3>
            {description && (
              <p className="card-text opacity-75 mt-2 mb-0">{description}</p>
            )}
          </div>
          {icon && <div style={{ fontSize: "2rem" }}>{icon}</div>}
        </div>
      </div>
      {trend && (
        <div className="card-footer bg-light">
          <span className="text-success">
            {trendIcon} {trend}
          </span>
        </div>
      )}
    </div>
  );
};

export default DashboardCard;
