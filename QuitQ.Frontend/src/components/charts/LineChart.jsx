import React from "react";
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from "recharts";

/**
 * Line chart component
 */
const LineChartComponent = ({
  data,
  dataKey = "value",
  xAxisKey = "name",
  title,
  height = 300,
  margin = { top: 20, right: 30, left: 0, bottom: 0 },
  color = "#0d6efd",
}) => {
  return (
    <div className="card">
      {title && <div className="card-header">{title}</div>}
      <div className="card-body">
        <ResponsiveContainer width="100%" height={height}>
          <LineChart data={data} margin={margin}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey={xAxisKey} />
            <YAxis />
            <Tooltip />
            <Legend />
            <Line
              type="monotone"
              dataKey={dataKey}
              stroke={color}
              dot={{ fill: color }}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default LineChartComponent;
