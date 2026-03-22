type RecurringTransaction = {

    id: number
    name: string
    amount: number
    frequency: string
    nextDueDate: string | null

}

type Props = {

    recurringTransactions: RecurringTransaction[]

}

export default function RecurringTransactionsCard ({

    recurringTransactions,


}: Props){

    return (

       <section>
      <div className="relative overflow-hidden rounded-[28px] h-[440px] border border-white/10 bg-white/5 px-6 py-5 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
        <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_20%_0%,rgba(255,255,255,0.06),rgba(0,0,0,0)_55%),radial-gradient(100%_70%_at_100%_100%,rgba(16,185,129,0.08),rgba(0,0,0,0)_55%)]" />

        <div className="relative">
          <div className="mb-4 text-center">
            <div className="text-xs uppercase tracking-[0.25em] text-white/40">
              Scheduled payments
            </div>

            <div className="mt-1 text-xl font-semibold tracking-tight text-white/90">
              Upcoming payments
            </div>
          </div>

          <div className="space-y-3">
           {recurringTransactions.slice(0, 4).map((t, index) => (
          <div
            key={`${t.id}-${index}`}
            className="group flex cursor-pointer items-center justify-between rounded-2xl border border-white/10 bg-white/5 px-4 py-3 transition-colors duration-200 hover:bg-white/10"
          >
            <div className="min-w-0 flex items-center gap-3">
              <div className="grid h-10 w-10 shrink-0 place-items-center rounded-xl border border-white/10 bg-white/5 text-sm font-semibold text-emerald-300 transition-colors duration-200 group-hover:bg-white/10 group-hover:text-emerald-400">
                {t.name.charAt(0).toUpperCase()}
              </div>

              <div className="min-w-0">
                <p className="truncate text-sm font-semibold text-white transition-colors duration-200 group-hover:text-emerald-400">
                  {t.name}
                </p>
                <p className="text-xs text-white/50 transition-colors duration-200 group-hover:text-emerald-300">
                  {t.nextDueDate}
                </p>
              </div>
            </div>

            <div className="rounded-xl border border-white/10 bg-white/5 px-3 py-1 text-sm font-semibold text-white transition-colors duration-200 group-hover:bg-white/10">
              ${t.amount}
            </div>
          </div>
        ))}
          </div>
        </div>
      </div>
    </section>


    )


}