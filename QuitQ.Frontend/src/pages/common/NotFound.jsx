import { FaFaceFrown } from "react-icons/fa6";

function NotFound() {
  return (
    <div 
      className="container d-flex flex-column justify-content-center align-items-center text-center" 
      style={{ minHeight: "80vh" }}
    >  
      <FaFaceFrown size={80} className=" mb-3" />
      <h1 className="fw-bold text-dark">404 - Page Not Found</h1>
      
      <p className="text-muted small">The page you are looking for does not exist.</p>
    </div>
  );
}

export default NotFound;
