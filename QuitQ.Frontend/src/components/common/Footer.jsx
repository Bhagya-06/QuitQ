import React from "react";

const Footer = () => {
  return (
    <footer className="bg-dark text-white py-2 mt-auto">
      <div className="container text-center">
        <p className="mb-0 small">© {new Date().getFullYear()} QuitQ. All rights reserved.</p>
      </div>
    </footer>
  );
};

export default Footer;
