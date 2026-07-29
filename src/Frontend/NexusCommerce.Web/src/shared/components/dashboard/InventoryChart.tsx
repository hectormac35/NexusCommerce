import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts'

type InventoryChartItem = {
  nombre: string
  stock: number
}

type InventoryChartProps = {
  data: InventoryChartItem[]
}

export function InventoryChart({
  data,
}: InventoryChartProps) {
  const chartData = data.map((item) => ({
    nombre:
      item.nombre.length > 18
        ? `${item.nombre.slice(0, 18)}…`
        : item.nombre,
    stock: item.stock,
  }))

  return (
    <div className="h-72 w-full">
      <ResponsiveContainer width="100%" height="100%">
        <BarChart
          data={chartData}
          margin={{
            top: 10,
            right: 10,
            left: -20,
            bottom: 0,
          }}
        >
          <CartesianGrid
            strokeDasharray="3 3"
            vertical={false}
            stroke="#1e293b"
          />

          <XAxis
            dataKey="nombre"
            axisLine={false}
            tickLine={false}
            tick={{
              fill: '#64748b',
              fontSize: 12,
            }}
          />

          <YAxis
            allowDecimals={false}
            axisLine={false}
            tickLine={false}
            tick={{
              fill: '#64748b',
              fontSize: 12,
            }}
          />

          <Tooltip
            cursor={{
              fill: 'rgba(30, 41, 59, 0.35)',
            }}
            contentStyle={{
              backgroundColor: '#0f172a',
              border: '1px solid #1e293b',
              borderRadius: '12px',
              color: '#f8fafc',
            }}
            labelStyle={{
              color: '#f8fafc',
              fontWeight: 600,
            }}
            itemStyle={{
              color: '#60a5fa',
            }}
            formatter={(value) => [`${value} unidades`, 'Stock']}
          />

          <Bar
            dataKey="stock"
            fill="#2563eb"
            radius={[8, 8, 0, 0]}
            maxBarSize={58}
          />
        </BarChart>
      </ResponsiveContainer>
    </div>
  )
}
