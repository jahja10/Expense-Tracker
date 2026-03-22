type Props = {
  totalTransactions: number
  upcomingPayments: number
  averagePerDay: number
  money: (n: number) => string
}

export default function QuickSummaryCard({
  totalTransactions,
  upcomingPayments,
  averagePerDay,
  money,
}: Props) {
  return (
    <section>
      <div className="rounded-[28px] min-h-[240px] border border-white/10 bg-white/5 px-6 py-5 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
        
        <h2 className="mb-4 text-lg font-semibold text-white">
          Quick Summary
        </h2>

        <div className="space-y-4">
          
          <div className="flex items-center justify-between rounded-xl bg-white/5 px-4 py-3">
            <span className="text-sm text-white/60">
              Transactions this month
            </span>
            <span className="text-sm font-semibold text-white">
              {totalTransactions}
            </span>
          </div>

          <div className="flex items-center justify-between rounded-xl bg-white/5 px-4 py-3">
            <span className="text-sm text-white/60">
              Upcoming payments
            </span>
            <span className="text-sm font-semibold text-white">
              ${money(upcomingPayments)}
            </span>
          </div>

          <div className="flex items-center justify-between rounded-xl bg-white/5 px-4 py-3">
            <span className="text-sm text-white/60">
              Avg / day
            </span>
            <span className="text-sm font-semibold text-white">
              ${money(averagePerDay)}
            </span>
          </div>

        </div>
      </div>
    </section>
  )
}