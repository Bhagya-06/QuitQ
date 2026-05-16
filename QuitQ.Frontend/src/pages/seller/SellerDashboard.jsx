import { useEffect, useState } from "react";
import api from "../../api/axios";
import { showError } from "../../utils/toast";
import hiCharacter from "../../assets/hi-character.png";
import { ResponsiveContainer, BarChart, Bar, PieChart, Pie, Cell, Tooltip, XAxis, YAxis, LineChart, Line, CartesianGrid, Legend, } from "recharts";

function SellerDashboard() {

  const [profile, setProfile] = useState(null);

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    try {
      const res = await api.get("/auth/profile");
      setProfile(res.data.data);
    } catch (err) {
      showError(err.response?.data?.message);
    }
  };

  const [report, setReport] =
    useState(null);

  useEffect(() => {
    loadReport();
  }, []);

  const loadReport = async () => {

    try {

      const [
        reportRes,
        productsRes
      ] = await Promise.all([
        api.get("/auth/sales-report"),
        api.get("/auth/seller-products")
      ]);

      const reportData =
        reportRes.data.data;

      const products =
        productsRes.data.data;

      setReport({
        ...reportData,
        totalProducts: products.length
      });

    } catch (err) {

      showError(
        err.response?.data?.Message ||
        "Failed to load report"
      );
    }
  };

  if (!report) {

    return (
      <div className="container mt-4 text-center">
        <h3>Loading...</h3>
      </div>
    );
  }

  const chartData = [
    {
      name: "Sales",
      value: report.totalSales,
    },
    {
      name: "Orders",
      value: report.totalOrders,
    },
    {
      name: "Products",
      value: report.totalProducts,
    },
  ];

  const COLORS = [
    "#0d6efd",
    "#198754",
    "#ffc107",
  ];

  return (
    <div className="container mt-4">

      {/* HERO SECTION WITH FLEXBOX ALIGNMENT */}
      <div className="bg-dark text-white p-5 rounded mb-5 d-flex flex-column flex-md-row align-items-center justify-content-between position-relative overflow-visible">
        {/* TEXT CONTENT (LEFT SIDE) */}
        <div className="mb-4 mb-md-0">
          <h1 className="display-5 fw-bold">
            Welcome, {profile?.name || "Guest"} 👋
          </h1>
          <p className="lead mb-0">
            Sell your amazing products for the best prices.
          </p>
        </div>

        {/* CHARACTER PNG */}
        <div
          className="position-relative"
          style={{
            width: "240px",
            height: "220px", overflow: "visible",
          }}
        >
          <img
            src={hiCharacter}
            alt="Character Saying Hi"
            style={{
              position: "absolute",
              right: "0",
              width: "100%",
              height: "auto",
              objectFit: "contain",
              transform: "scale(1.8)", // makes character larger
              filter: "drop-shadow(0px 8px 16px rgba(0,0,0,0.35))",
            }}
          />
        </div>

      </div>

      {/* KPI */}

      <div className="row mb-4">

        <div className="col-md-4">

          <div className="card p-4 shadow-sm text-center">

            <h5>Total Sales</h5>

            <h2>
              ₹ {report.totalSales}
            </h2>

          </div>

        </div>

        <div className="col-md-4">

          <div className="card p-4 shadow-sm text-center">

            <h5>Total Orders</h5>

            <h2>
              {report.totalOrders}
            </h2>

          </div>

        </div>

        <div className="col-md-4">

          <div className="card p-4 shadow-sm text-center">

            <h5>Total Products</h5>

            <h2>
              {report.totalProducts}
            </h2>

          </div>

        </div>

      </div>

      {/* CHARTS */}

      <div className="row">

        {/* BAR */}

        <div className="col-md-6 mb-4">

          <div className="card p-4 shadow-sm">

            <h5 className="mb-3">
              Sales Overview
            </h5>

            <ResponsiveContainer
              width="100%"
              height={300}
            >

              <BarChart data={chartData}>

                <XAxis dataKey="name" />

                <YAxis />

                <Tooltip />

                <Bar
                  dataKey="value"
                  fill="#198754"
                />

              </BarChart>

            </ResponsiveContainer>

          </div>

        </div>

        {/* PIE */}

        <div className="col-md-6 mb-4">

          <div className="card p-4 shadow-sm">

            <h5 className="mb-3">
              Analytics Distribution
            </h5>

            <ResponsiveContainer
              width="100%"
              height={300}
            >

              <PieChart>

                <Pie
                  data={chartData}
                  dataKey="value"
                  outerRadius={100}
                  label
                >

                  {chartData.map((entry, index) => (

                    <Cell
                      key={index}
                      fill={
                        COLORS[
                        index % COLORS.length
                        ]
                      }
                    />

                  ))}

                </Pie>

                <Tooltip />

              </PieChart>

            </ResponsiveContainer>

          </div>

        </div>

        {/* LINE */}

        <div className="col-12">

          <div className="card p-4 shadow-sm">

            <h5 className="mb-3">
              Seller Performance
            </h5>

            <ResponsiveContainer
              width="100%"
              height={350}
            >

              <LineChart data={chartData}>

                <CartesianGrid strokeDasharray="3 3" />

                <XAxis dataKey="name" />

                <YAxis />

                <Tooltip />

                <Legend />

                <Line
                  type="monotone"
                  dataKey="value"
                  stroke="#0d6efd"
                  strokeWidth={3}
                />

              </LineChart>

            </ResponsiveContainer>

          </div>

        </div>

      </div>

    </div>
  );
}

export default SellerDashboard;