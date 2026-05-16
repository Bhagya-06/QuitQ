import React from "react";

/**
 * Page header component
 */
const PageHeader = ({
  title,
  subtitle,
  icon,
  actions,
  breadcrumbs,
  className = "",
}) => {
  return (
    <div className={`mb-4 ${className}`}>
      {breadcrumbs && (
        <nav aria-label="breadcrumb" className="mb-3">
          <ol className="breadcrumb">
            {breadcrumbs.map((item, index) => (
              <li
                key={index}
                className={`breadcrumb-item ${item.active ? "active" : ""}`}
              >
                {item.href ? <a href={item.href}>{item.label}</a> : item.label}
              </li>
            ))}
          </ol>
        </nav>
      )}

      <div className="d-flex justify-content-between align-items-start">
        <div>
          <div className="d-flex align-items-center gap-3">
            {icon && <div style={{ fontSize: "2rem" }}>{icon}</div>}
            <div>
              <h2 className="mb-0">{title}</h2>
              {subtitle && <p className="text-muted mb-0">{subtitle}</p>}
            </div>
          </div>
        </div>

        {actions && <div className="d-flex gap-2">{actions}</div>}
      </div>
    </div>
  );
};

export default PageHeader;
