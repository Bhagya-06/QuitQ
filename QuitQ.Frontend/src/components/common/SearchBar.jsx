import React from "react";

/**
 * Search bar component
 */
const SearchBar = ({
  placeholder = "Search...",
  value,
  onChange,
  onSearch,
  onClear,
  loading = false,
  className = "",
}) => {
  const handleKeyPress = (e) => {
    if (e.key === "Enter" && onSearch) {
      onSearch(value);
    }
  };

  const handleClear = () => {
    if (onClear) {
      onClear();
    }
  };

  return (
    <div className={`input-group ${className}`}>
      <input
        type="text"
        className="form-control"
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        onKeyPress={handleKeyPress}
        disabled={loading}
      />
      {value && (
        <button
          className="btn btn-outline-secondary"
          type="button"
          onClick={handleClear}
          disabled={loading}
        >
          ✕
        </button>
      )}
      <button
        className="btn btn-primary"
        type="button"
        onClick={() => onSearch && onSearch(value)}
        disabled={loading}
      >
        {loading ? (
          <span
            className="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></span>
        ) : (
          "🔍"
        )}
      </button>
    </div>
  );
};

export default SearchBar;
