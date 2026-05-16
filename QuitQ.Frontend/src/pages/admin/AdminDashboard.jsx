import { useEffect, useState } from "react";
import api from "../../api/axios";
import { showError } from "../../utils/toast";
import hiCharacter from "../../assets/hi-character.png";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, PieChart, Pie, Cell, LineChart, Line, CartesianGrid, Legend, } from "recharts";

function AdminDashboard() {

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

  const [report, setReport] = useState(null);

  useEffect(() => {
    fetchReport();
  }, []);

  const fetchReport = async () => {

    try {

      const res = await api.get("/admin/reports/sales");

      setReport(res.data.data);

    } catch (err) {

      showError(err.response?.data?.message);

    }
  };

  if (!report) {
    return (
      <div className="container mt-5 text-center">
        <h3>Loading...</h3>
      </div>
    );
  }

  const summaryData = [
    {
      name: "Orders",
      value: report.totalOrders,
    },
    {
      name: "Products Sold",
      value: report.totalProductsSold,
    },
  ];

  const revenueData = [
    {
      name: "Revenue",
      amount: report.totalRevenue,
    },
  ];

  const trendData = [
    {
      name: "Orders",
      value: report.totalOrders,
    },
    {
      name: "Products",
      value: report.totalProductsSold,
    },
  ];

  const COLORS = [
    "#0088FE",
    "#00C49F",
    "#FFBB28",
    "#FF8042",
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
            Monitor your app's amazing performance and sales with real-time analytics.
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

      {/* KPI CARDS */}

      <div className="row mb-4">

        <div className="col-md-4">

          <div className="card shadow-sm p-4 text-center">

            <h5>Total Orders</h5>

            <h2>{report.totalOrders}</h2>

          </div>

        </div>

        <div className="col-md-4">

          <div className="card shadow-sm p-4 text-center">

            <h5>Total Revenue</h5>

            <h2>₹ {report.totalRevenue}</h2>

          </div>

        </div>

        <div className="col-md-4">

          <div className="card shadow-sm p-4 text-center">

            <h5>Products Sold</h5>

            <h2>{report.totalProductsSold}</h2>

          </div>

        </div>

      </div>

      {/* CHARTS */}

      <div className="row">

        {/* BAR CHART */}

        <div className="col-md-6 mb-4">

          <div className="card p-4 shadow-sm">

            <h5 className="mb-3">
              Revenue Analytics
            </h5>

            <ResponsiveContainer
              width="100%"
              height={300}
            >

              <BarChart data={revenueData}>

                <XAxis dataKey="name" />

                <YAxis />

                <Tooltip />

                <Bar
                  dataKey="amount"
                  fill="#198754"
                />

              </BarChart>

            </ResponsiveContainer>

          </div>

        </div>

        {/* PIE CHART */}

        <div className="col-md-6 mb-4">

          <div className="card p-4 shadow-sm">

            <h5 className="mb-3">
              Orders vs Products
            </h5>

            <ResponsiveContainer
              width="100%"
              height={300}
            >

              <PieChart>

                <Pie
                  data={summaryData}
                  dataKey="value"
                  nameKey="name"
                  outerRadius={100}
                  label
                >

                  {summaryData.map((entry, index) => (

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

        {/* LINE CHART */}

        <div className="col-12">

          <div className="card p-4 shadow-sm">

            <h5 className="mb-3">
              Performance Trend
            </h5>

            <ResponsiveContainer
              width="100%"
              height={350}
            >

              <LineChart data={trendData}>

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

export default AdminDashboard;