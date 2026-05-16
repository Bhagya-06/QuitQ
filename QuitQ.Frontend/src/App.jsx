import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import AppRoutes from "./routes/AppRoutes";
import { AuthProvider } from "./context/AuthContext";
import { CartProvider } from "./context/CartContext";
import { ThemeProvider } from "./context/ThemeContext";
import "./styles/theme.css";
import "./styles/responsive.css";

function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <CartProvider>
          <AppRoutes />
          <ToastContainer
            position="top-right"
            autoClose={3000}
            hideProgressBar={false}
            newestOnTop={true}
            closeOnClick
            pauseOnHover
            theme="colored"
          />
        </CartProvider>
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;
