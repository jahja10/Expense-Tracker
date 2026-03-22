type Props = {
  transactions: {
    price: number
    transactionDate: string
    transactionType: string
    description: string | null
    location: string | null
    categoryName: string
    paymentMethodName: string
  }[]
}

export default function RecentTransactionsCard({ transactions }: Props) {
  return (
    <section>
      <div className="rounded-[28px] min-h-[250px] border border-white/10 bg-white/5 px-6 py-5 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
       <div className="mb-4 flex items-center justify-between">
  <h2 className="text-lg font-semibold text-white">
    Recent Transactions
  </h2>

  <button className=" cursor-pointer rounded-xl border border-white/10 bg-white/5 px-3 py-1.5 text-xs font-medium text-white/70 transition-colors hover:bg-white/10 hover:text-emerald-400">
    View all
  </button>
</div>

        {transactions.length === 0 ? (
          <p className="text-sm text-white/60">
            No recent transactions
          </p>
        ) : (
          <div className="flex flex-col gap-3">
           {transactions.map((t, index) => {
  const isExpense = t.transactionType.toLowerCase() === "expense"

  return (
    <div
      key={index}
      className="group flex cursor-pointer items-center justify-between rounded-xl bg-white/5 px-4 py-3 transition-colors duration-200 hover:bg-white/10"
    >
      <div>
       <p className="text-sm font-medium text-white transition-colors duration-200 group-hover:text-emerald-400">
          {t.categoryName}
        </p>
        <p className="text-xs text-white/60 transition-colors duration-200 group-hover:text-emerald-300">
          {t.description ?? t.paymentMethodName}
        </p>
      </div>

      <div className="text-right">
        <p
          className={`text-sm font-semibold ${
            isExpense ? "text-red-400" : "text-emerald-400"
          }`}
        >
          {isExpense ? "-" : "+"}${t.price}
        </p>

        <p className="text-xs text-white/50 transition-colors duration-200 group-hover:text-emerald-300">
          {new Date(t.transactionDate).toLocaleDateString()}
        </p>
      </div>
    </div>
  )
})}
          </div>
        )}
      </div>
    </section>
  )
}