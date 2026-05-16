import React from "react";

/**
 * Data table component for displaying tabular data
 */
const DataTable = ({
  columns,
  data,
  loading = false,
  emptyMessage = "No data available",
  striped = true,
  hover = true,
  bordered = false,
  responsive = true,
  size = "md",
  onRowClick,
  rowActions,
  pagination,
  className = "",
}) => {
  const tableClass = `table ${striped ? "table-striped" : ""} ${hover ? "table-hover" : ""} ${bordered ? "table-bordered" : ""} ${
    size === "sm" ? "table-sm" : ""
  } mb-0`;

  return (
    <div className={responsive ? "table-responsive" : ""}>
      <table className={`${tableClass} ${className}`}>
        <thead>
          <tr>
            {columns.map((col) => (
              <th key={col.key} style={col.width ? { width: col.width } : {}}>
                {col.label}
              </th>
            ))}
            {rowActions && <th style={{ width: "150px" }}>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {loading ? (
            <tr>
              <td colSpan={columns.length + (rowActions ? 1 : 0)} className="text-center py-5">
                <div className="spinner-border text-primary" role="status">
                  <span className="visually-hidden">Loading...</span>
                </div>
              </td>
            </tr>
          ) : data.length === 0 ? (
            <tr>
              <td colSpan={columns.length + (rowActions ? 1 : 0)} className="text-center py-5 text-muted">
                {emptyMessage}
              </td>
            </tr>
          ) : (
            data.map((row, idx) => (
              <tr
                key={row.id || idx}
                onClick={() => onRowClick && onRowClick(row)}
                style={onRowClick ? { cursor: "pointer" } : {}}
              >
                {columns.map((col) => (
                  <td key={col.key}>
                    {col.render
                      ? col.render(row[col.key], row)
                      : row[col.key]}
                  </td>
                ))}
                {rowActions && (
                  <td>
                    <div className="btn-group btn-group-sm" role="group">
                      {rowActions.map((action, idx) => (
                        <button
                          key={idx}
                          className={`btn btn-outline-${action.variant || "secondary"}`}
                          onClick={(e) => {
                            e.stopPropagation();
                            action.onClick(row);
                          }}
                          disabled={action.disabled}
                        >
                          {action.label}
                        </button>
                      ))}
                    </div>
                  </td>
                )}
              </tr>
            ))
          )}
        </tbody>
      </table>

      {pagination && (
        <nav aria-label="Table pagination" className="mt-3">
          <ul className="pagination justify-content-end">
            {pagination.currentPage > 1 && (
              <li className="page-item">
                <button
                  className="page-link"
                  onClick={() => pagination.onPageChange(pagination.currentPage - 1)}
                >
                  Previous
                </button>
              </li>
            )}

            {[...Array(pagination.totalPages)].map((_, idx) => (
              <li
                key={idx + 1}
                className={`page-item ${pagination.currentPage === idx + 1 ? "active" : ""}`}
              >
                <button
                  className="page-link"
                  onClick={() => pagination.onPageChange(idx + 1)}
                >
                  {idx + 1}
                </button>
              </li>
            ))}

            {pagination.currentPage < pagination.totalPages && (
              <li className="page-item">
                <button
                  className="page-link"
                  onClick={() => pagination.onPageChange(pagination.currentPage + 1)}
                >
                  Next
                </button>
              </li>
            )}
          </ul>
        </nav>
      )}
    </div>
  );
};

export default DataTable;
