import {
  ResponsiveContainer,
  AreaChart,
  Area,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
} from "recharts"

type SpendingTrendItem = {
  month: string
  spent: number
  budget: number
}

type Props = {
  data: SpendingTrendItem[]
}

type CustomTooltipProps = {
  active?: boolean
  payload?: Array<{
    value: number
    dataKey: string
    color: string
  }>
  label?: string
}

function CustomTooltip({ active, payload, label }: CustomTooltipProps) {
  if (!active || !payload || payload.length === 0) return null

  const spent = payload.find((item) => item.dataKey === "spent")
  const budget = payload.find((item) => item.dataKey === "budget")

  const formatMoney = (value: number) =>
    `$${value.toLocaleString(undefined, {
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    })}`

  return (
    <div className="min-w-[170px] rounded-2xl border border-white/10 bg-[#0b1220]/95 px-4 py-3 shadow-[0_10px_30px_rgba(0,0,0,0.45)] backdrop-blur-xl">
      <p className="mb-3 text-sm font-semibold text-white">{label}</p>

      {spent && (
        <div className="mb-2 flex items-center justify-between gap-4">
          <div className="flex items-center gap-2">
            <span className="h-2.5 w-2.5 rounded-full bg-teal-400" />
            <span className="text-sm text-white/70">Spent</span>
          </div>
          <span className="text-sm font-medium text-white">
            {formatMoney(spent.value)}
          </span>
        </div>
      )}

      {budget && (
        <div className="flex items-center justify-between gap-4">
          <div className="flex items-center gap-2">
            <span className="h-2.5 w-2.5 rounded-full bg-sky-400" />
            <span className="text-sm text-white/70">Budget</span>
          </div>
          <span className="text-sm font-medium text-white">
            {formatMoney(budget.value)}
          </span>
        </div>
      )}
    </div>
  )
}

export default function SpendingTrendChart({ data }: Props) {
  if (!data || data.length === 0) {
    return null
  }

  return (
    <section className="w-full">
      <div className="relative h-[440px] overflow-hidden rounded-[28px] border border-white/10 bg-white/[0.04] px-6 py-5 shadow-[0_20px_60px_rgba(0,0,0,0.45)] backdrop-blur-2xl">
        <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(90%_70%_at_15%_0%,rgba(59,130,246,0.16),transparent_45%),radial-gradient(70%_70%_at_100%_100%,rgba(45,212,191,0.14),transparent_40%)]" />

        <div className="relative z-10 mb-5 flex items-start justify-between">
          <div>
            <p className="text-[11px] uppercase tracking-[0.32em] text-white/40">
              Spending analytics
            </p>
            <h2 className="mt-2 text-3xl font-semibold text-white">
              Monthly spending trend
            </h2>
            <p className="mt-1 text-sm text-white/55">
              Last 6 months overview
            </p>
          </div>

          <div className="mt-1 flex items-center gap-4">
            <div className="flex items-center gap-2">
              <span className="h-2.5 w-2.5 rounded-full bg-teal-400 shadow-[0_0_12px_rgba(45,212,191,0.9)]" />
              <span className="text-xs text-white/60">Spent</span>
            </div>

            <div className="flex items-center gap-2">
              <span className="h-2.5 w-2.5 rounded-full bg-sky-400 shadow-[0_0_12px_rgba(56,189,248,0.9)]" />
              <span className="text-xs text-white/60">Budget</span>
            </div>
          </div>
        </div>

        <div className="relative z-10">
          <ResponsiveContainer width="100%" height={305}>
            <AreaChart
              data={data}
              margin={{ top: 10, right: 10, left: -20, bottom: 0 }}
            >
              <defs>
                <linearGradient id="spentStrokeGlow" x1="0" y1="0" x2="1" y2="0">
                  <stop offset="0%" stopColor="#2dd4bf" />
                  <stop offset="100%" stopColor="#22d3ee" />
                </linearGradient>

                <linearGradient id="spentFill" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor="#2dd4bf" stopOpacity={0.3} />
                  <stop offset="55%" stopColor="#22d3ee" stopOpacity={0.12} />
                  <stop offset="95%" stopColor="#22d3ee" stopOpacity={0.02} />
                </linearGradient>
              </defs>

              <CartesianGrid
                stroke="rgba(255,255,255,0.06)"
                vertical={false}
                strokeDasharray="3 3"
              />

              <XAxis
                dataKey="month"
                tick={{ fill: "rgba(255,255,255,0.45)", fontSize: 12 }}
                axisLine={false}
                tickLine={false}
              />

              <YAxis
                tick={{ fill: "rgba(255,255,255,0.45)", fontSize: 12 }}
                axisLine={false}
                tickLine={false}
                width={52}
              />

              <Tooltip
                cursor={{
                  stroke: "rgba(255,255,255,0.18)",
                  strokeWidth: 1,
                  strokeDasharray: "4 4",
                }}
                content={<CustomTooltip />}
              />

              <Area
                type="monotone"
                dataKey="budget"
                stroke="#60a5fa"
                strokeWidth={2}
                fillOpacity={0}
                dot={false}
                activeDot={{
                  r: 5,
                  stroke: "#bfdbfe",
                  strokeWidth: 2,
                  fill: "#60a5fa",
                }}
              />

              <Area
                type="monotone"
                dataKey="spent"
                stroke="url(#spentStrokeGlow)"
                strokeWidth={3}
                fill="url(#spentFill)"
                dot={false}
                activeDot={{
                  r: 5,
                  stroke: "#ccfbf1",
                  strokeWidth: 2,
                  fill: "#2dd4bf",
                }}
              />
            </AreaChart>
          </ResponsiveContainer>
        </div>
      </div>
    </section>
  )
}