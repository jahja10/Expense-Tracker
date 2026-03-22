type Props = {
  percentage: number
  status: string
  daysLeft: number
}

export default function BudgetStatusCard({ percentage, status, daysLeft }: Props) {
  const size = 110
  const stroke = 10
  const radius = (size - stroke) / 2
  const circumference = 2 * Math.PI * radius

  const pct = Math.min(100, Math.max(0, percentage))
  const displayPct = Math.max(0, percentage)
  const dash = (pct / 100) * circumference

  const statusColor =
    status === "Over budget"
      ? "text-red-400"
      : status === "Warning"
      ? "text-yellow-400"
      : "text-emerald-400"

  const ringColor =
    status === "Over budget"
      ? "rgb(248 113 113)"
      : status === "Warning"
      ? "rgb(250 204 21)"
      : "rgb(52 211 153)"

  return (
    <section className="col-span-4">
      <div className="relative h-[170px] overflow-hidden rounded-[28px] border border-white/10 bg-white/5 px-7 py-3 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl flex flex-col justify-center">
        <div className="text-xl font-semibold text-white/90 mb-3 text-center">
          Budget Status
        </div>

        <div className="flex items-center -translate-y-1">
          <div className="relative grid place-items-center">
            <svg width={size} height={size} className="-rotate-90">
              <circle
                cx={size / 2}
                cy={size / 2}
                r={radius}
                stroke="rgba(255,255,255,0.12)"
                strokeWidth={stroke}
                fill="none"
              />

              <circle
                cx={size / 2}
                cy={size / 2}
                r={radius}
                stroke={ringColor}
                strokeWidth={stroke}
                fill="none"
                strokeLinecap="round"
                strokeDasharray={`${dash} ${circumference}`}
                className="transition-all duration-500"
              />
            </svg>

            <div className="absolute text-center">
              <div className="text-lg font-semibold text-white">
                {Math.round(displayPct)}%
              </div>
              <div className="text-[11px] text-white/45">used</div>
            </div>
          </div>

          <div className="ml-auto text-right space-y-3">
            <div>
              <div className="text-sm text-white/50">Status</div>
              <div className={`text-sm font-semibold ${statusColor}`}>
                {status}
              </div>
            </div>

            <div>
              <div className="text-sm text-white/50">Days left</div>
              <div className="text-sm font-semibold text-white">
                {daysLeft} days
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  )
}