import { Plus } from "lucide-react"

type Props = {
  userName: string
  spentThisMonth: number
  monthlyBudget: number
  remaining: number
  loading: boolean
  error: string | null
  money: (n: number) => string
}

export default function MonthlyOverviewCard({
  userName,
  spentThisMonth,
  monthlyBudget,
  remaining,
  loading,
  error,
  money,
}: Props) {
  return (
    <section className="col-span-8">
      <div className="relative  h-[170px] overflow-hidden rounded-[28px] border border-white/10 bg-white/5 px-7 py-3 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
        
        <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_30%_0%,rgba(255,255,255,0.07),rgba(0,0,0,0.0)_55%),radial-gradient(120%_80%_at_80%_120%,rgba(0,0,0,0.18),rgba(0,0,0,0.0)_55%)]" />

        <div className="relative flex items-start justify-between">
          <div>
            <div className="text-xs uppercase tracking-widest text-white/45">
              Monthly overview
            </div>

            {loading ? (
              <div className="mt-1 text-xl font-semibold text-white/60">
                Loading...
              </div>
            ) : (
              <div className="mt-1 text-xl font-semibold tracking-tight text-white/90">
                Welcome, {userName}!
              </div>
            )}

            {error && (
              <div className="mt-2 text-sm text-red-400">
                {error}
              </div>
            )}
          </div>

          <button className="grid h-8 w-8 cursor-pointer place-items-center rounded-lg border border-white/10 bg-white/5 text-white/70 transition-colors hover:bg-white/10 hover:text-emerald-400">
            <Plus className="h-4 w-4" />
          </button>
        </div>

        <div className="relative mt-2 flex items-end justify-between gap-6">

          <div>
            <div className="text-3xl font-semibold tracking-tight text-white">
              ${money(spentThisMonth)}
            </div>
            <div className="mt-1 text-sm text-emerald-300/90">
              Spent this month
            </div>
          </div>

          <div className="text-right">
            <div className="text-[11px] uppercase tracking-widest text-white/50">
              Remaining
            </div>

            <div className="mt-1 inline-flex items-center rounded-xl border border-white/10 bg-white/5 px-4 py-1.5 text-lg font-semibold text-white shadow-[inset_0_1px_0_rgba(255,255,255,0.06)]">
              ${money(remaining)}
            </div>

            <div className="mt-1 text-xs text-white/45">
              of ${money(monthlyBudget)} budget
            </div>
          </div>

        </div>
      </div>
    </section>
  )
}
