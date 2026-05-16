import { useEffect, useState } from "react";
import api from "../../api/axios";
import { Link } from "react-router-dom";
import { showSuccess, showError } from "../../utils/toast";

function MyOrders() {

  const [orders, setOrders] = useState([]);

  useEffect(() => {
    loadOrders();
  }, []);

  const loadOrders = async () => {
    try {

      const res = await api.get("/order/my-orders");

      setOrders(res.data.data);

    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  return (
    <div className="container mt-4">

      <h2 className="mb-4">My Orders</h2>

      {orders.length === 0 ? (
        <div className="alert alert-info">
          No orders found
        </div>
      ) : (
        <div className="row">

          {orders.map((order) => (

            <div className="col-md-6 mb-3" key={order.id}>

              <div className="card p-3">

                <h5>Order #{order.id}</h5>

                <p>Status: {order.status}</p>

                <p>Total: ₹ {order.total}</p>

                <p>
                  Tracking:
                  {" "}
                  {order.trackingNumber}
                </p>

                <Link
                  to={`/order/${order.id}`}
                  className="btn btn-primary btn-sm"
                >
                  View Details
                </Link>

              </div>

            </div>

          ))}

        </div>
      )}
    </div>
  );
}

export default MyOrders;