import React from "react";
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from "recharts";

/**
 * Bar chart component
 */
const BarChartComponent = ({
  data,
  dataKey = "value",
  xAxisKey = "name",
  title,
  height = 300,
  margin = { top: 20, right: 30, left: 0, bottom: 0 },
}) => {
  return (
    <div className="card">
      {title && <div className="card-header">{title}</div>}
      <div className="card-body">
        <ResponsiveContainer width="100%" height={height}>
          <BarChart data={data} margin={margin}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey={xAxisKey} />
            <YAxis />
            <Tooltip />
            <Legend />
            <Bar dataKey={dataKey} fill="#0d6efd" />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default BarChartComponent;
