import Home from "../pages/common/Home";
import BuyerDashboard from "../pages/user/BuyerDashboard";
import Login from "../pages/auth/Login";
import Register from "../pages/auth/Register";
import Cart from "../pages/user/Cart";
import Wishlist from "../pages/user/Wishlist";
import ProductDetails from "../pages/user/ProductDetails";
import Profile from "../pages/user/Profile";
import EditProfile from "../pages/user/EditProfile";
import SearchProducts from "../pages/user/SearchProducts";
import Checkout from "../pages/user/Checkout";
import MyOrders from "../pages/user/MyOrders";
import OrderDetails from "../pages/user/OrderDetails";
import SellerDashboard from "../pages/seller/SellerDashboard";
import AddProduct from "../pages/seller/AddProduct";
import EditProduct from "../pages/seller/EditProduct";
import AdminDashboard from "../pages/admin/AdminDashboard";
import ManageBrands from "../pages/admin/ManageBrands";
import ManageCategories from "../pages/admin/ManageCategories";
import ManageOffers from "../pages/admin/ManageOffers";
import Users from "../pages/admin/Users";
import Sellers from "../pages/admin/Sellers";
import SellerProducts from "../pages/seller/SellerProducts";
import SellerOrders from "../pages/seller/SellerOrders";
import NotFound from "../pages/common/NotFound";

export const ROUTES = {
  HOME: "/",
  LOGIN: "/login",
  REGISTER: "/register",
  BUYER_DASHBOARD: "/buyer",
  CART: "/cart",
  WISHLIST: "/wishlist",
  PRODUCT_DETAILS: "/product/:id",
  PROFILE: "/profile",
  EDIT_PROFILE: "/profile/edit",
  SEARCH: "/search",
  CHECKOUT: "/checkout",
  MY_ORDERS: "/my-orders",
  ORDER_DETAILS: "/order/:id",
  SELLER_DASHBOARD: "/seller",
  SELLER_ADD_PRODUCT: "/seller/add-product",
  SELLER_EDIT_PRODUCT: "/seller/edit-product/:id",
  SELLER_PRODUCTS: "/seller/products",
  SELLER_ORDERS: "/seller/orders",
  ADMIN_DASHBOARD: "/admin",
  ADMIN_USERS: "/admin/users",
  ADMIN_SELLERS: "/admin/sellers",
  ADMIN_BRANDS: "/admin/brands",
  ADMIN_CATEGORIES: "/admin/categories",
  ADMIN_OFFERS: "/admin/offers",
};

export const publicRoutes = [
  { path: ROUTES.HOME, element: <Home /> },
  { path: ROUTES.LOGIN, element: <Login /> },
  { path: ROUTES.REGISTER, element: <Register /> },
];

export const protectedRoutes = [
  { path: ROUTES.BUYER_DASHBOARD, element: <BuyerDashboard /> },
  { path: ROUTES.CART, element: <Cart /> },
  { path: ROUTES.WISHLIST, element: <Wishlist /> },
  { path: ROUTES.PROFILE, element: <Profile /> },
  { path: ROUTES.EDIT_PROFILE, element: <EditProfile /> },
  { path: ROUTES.SEARCH, element: <SearchProducts /> },
  { path: ROUTES.CHECKOUT, element: <Checkout /> },
  { path: ROUTES.MY_ORDERS, element: <MyOrders /> },
  { path: ROUTES.ORDER_DETAILS, element: <OrderDetails /> },
  { path: ROUTES.PRODUCT_DETAILS, element: <ProductDetails /> },
];

export const sellerRoutes = [
  { path: ROUTES.SELLER_DASHBOARD, element: <SellerDashboard /> },
  { path: ROUTES.SELLER_ADD_PRODUCT, element: <AddProduct /> },
  { path: ROUTES.SELLER_EDIT_PRODUCT, element: <EditProduct /> },
  { path: ROUTES.SELLER_PRODUCTS, element: <SellerProducts /> },
  { path: ROUTES.SELLER_ORDERS, element: <SellerOrders /> },
];

export const adminRoutes = [
  { path: ROUTES.ADMIN_DASHBOARD, element: <AdminDashboard /> },
  { path: ROUTES.ADMIN_USERS, element: <Users /> },
  { path: ROUTES.ADMIN_SELLERS, element: <Sellers /> },
  { path: ROUTES.ADMIN_BRANDS, element: <ManageBrands /> },
  { path: ROUTES.ADMIN_CATEGORIES, element: <ManageCategories /> },
  { path: ROUTES.ADMIN_OFFERS, element: <ManageOffers /> },
];

export const fallbackRoute = { path: "*", element: <NotFound /> };
