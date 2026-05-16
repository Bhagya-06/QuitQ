import { useEffect, useState } from "react";
import api from "../../api/axios";
import {
  showError,
  showSuccess,
} from "../../utils/toast";

function SellerOrders() {

  const [orders, setOrders] =
    useState([]);

  useEffect(() => {
    loadOrders();
  }, []);

  const loadOrders = async () => {

    try {

      const res = await api.get(
        "/auth/seller-orders"
      );

      setOrders(res.data.data);

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  const updateStatus = async (
    orderId,
    status
  ) => {

    try {

      await api.put(
        `/auth/order-status/${orderId}?status=${status}`,
        null,
        {
          params: { status },
        }
      );

      showSuccess(
        "Status updated"
      );

      loadOrders();

    } catch (err) {

      showError(
        err.response?.data?.message
      );
    }
  };

  return (
    <div className="container mt-4">

      <h2 className="mb-4">
        Seller Orders
      </h2>

      <div className="table-responsive">

        <table className="table table-bordered">

          <thead className="table-dark">

            <tr>
              <th>Order Id</th>
              <th>Product Name</th>
              <th>Quantity</th>
              <th>Total</th>
              <th>Status</th>
              <th>Action</th>
            </tr>

          </thead>

          <tbody>

            {orders.map((order) => (

              <tr key={order.orderId}>

                <td>{order.orderId}</td>

                <td>
                  {order.productName}
                </td>

                <td>
                  {order.quantity}
                </td>

                <td>
                  {(order.price)*(order.quantity)}
                </td>

                <td>
                  {order.status}
                </td>

                <td>

                  <select
                    className="form-control"
                    value={order.status}
                    onChange={(e) =>
                      updateStatus(
                        order.orderId,
                        e.target.value
                      )
                    }
                  >

                    <option>
                      Pending
                    </option>

                    <option>
                      Processing
                    </option>

                    <option>
                      Shipped
                    </option>

                    <option>
                      Delivered
                    </option>

                    <option>
                      Cancelled
                    </option>

                  </select>

                </td>

              </tr>

            ))}

          </tbody>

        </table>

      </div>

    </div>
  );
}

export default SellerOrders;