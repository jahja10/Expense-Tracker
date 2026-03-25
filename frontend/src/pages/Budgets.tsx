import { useEffect, useState } from "react"
import { Wallet, Plus} from "lucide-react"

type Budget = {
  id: number
  amount: number
  savings: number | null
  startDate: string
  endDate: string | null
  userId: number
}

export default function Budgets() {
  const [budget, setBudget] = useState<Budget | null>(null)

  const [amount, setAmount] = useState("")
  const [savings, setSavings] = useState("")
  const [endDate, setEndDate] = useState("")

  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const token = localStorage.getItem("token")
  const apiUrl = import.meta.env.VITE_API_URL

  useEffect(() => {
    const loadBudget = async () => {
      try {

        const response = await fetch(`${apiUrl}/Budgets/active`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })

        if (response.status === 404) {
          setBudget(null)
          return
        }

        if (!response.ok) {
          throw new Error("Failed to load budget")
        }

        const data = await response.json()
        setBudget(data)
      } catch (err) {
        setError("Failed to load budget")
      } finally {
      }
    }

    loadBudget()
  }, [apiUrl, token])

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault()

    try {
      setLoading(true)
      setError(null)

      const response = await fetch(`${apiUrl}/Budgets`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          amount: Number(amount),
          savings: savings ? Number(savings) : null,
          endDate: endDate || null,
        }),
      })

      if (!response.ok) {
        throw new Error("Failed to create budget")
      }

      const data = await response.json()
      setBudget(data)

      setAmount("")
      setSavings("")
      setEndDate("")
    } catch (err) {
      setError("Failed to create budget")
    } finally {
      setLoading(false)
    }
  }

  const handleClose = async () => {
    if (!budget) return

    try {
      setLoading(true)

      const response = await fetch(
        `${apiUrl}/Budgets/${budget.id}/close`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            endDate: new Date().toISOString().split("T")[0],
          }),
        }
      )

      if (!response.ok) {
        throw new Error("Failed to close budget")
      }

      setBudget(null)
    } catch {
      setError("Failed to close budget")
    } finally {
      setLoading(false)
    }
  }

  return (
    <main className="flex-1">
      <div className="flex flex-col gap-6">
        {/* FORM */}
        {!budget && (
          <section>
            <div className="rounded-[28px] border border-white/10 bg-white/5 px-6 py-6 backdrop-blur-2xl">
              <div className="mb-6 flex items-center justify-between">
                <div>
                  <p className="text-xs uppercase text-emerald-300/70">
                    New Budget
                  </p>
                  <h2 className="text-xl font-semibold text-white">
                    Create Budget
                  </h2>
                </div>

                <div className="rounded-2xl bg-emerald-500/15 p-3 text-emerald-300">
                  <Plus />
                </div>
              </div>

              <form
                onSubmit={handleCreate}
                className="grid gap-4 md:grid-cols-3"
                >
                <div>
                    <label className="mb-2 block text-sm text-white/65">
                    Budget Amount
                    </label>
                    <input
                    type="number"
                    placeholder="Enter amount"
                    value={amount}
                    onChange={(e) => setAmount(e.target.value)}
                    className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-3 text-white outline-none transition focus:border-emerald-400/30"
                    />
                </div>

                <div>
                    <label className="mb-2 block text-sm text-white/65">
                    Savings
                    </label>
                    <input
                    type="number"
                    placeholder="Optional savings"
                    value={savings}
                    onChange={(e) => setSavings(e.target.value)}
                    className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-3 text-white outline-none transition focus:border-emerald-400/30"
                    />
                </div>

                <div>
                    <label className="mb-2 block text-sm text-white/65">
                    End Date
                    </label>
                    <input
                    type="date"
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-3 text-white outline-none transition focus:border-emerald-400/30"
                    />
                </div>

                <button
                    type="submit"
                    disabled={loading}
                    className="cursor-pointer rounded-xl border border-emerald-400/20 bg-emerald-500/20 py-3 font-semibold text-emerald-300 transition hover:bg-emerald-500/25 md:col-span-3"
                >
                    {loading ? "Saving..." : "Create Budget"}
                </button>
                </form>

              {error && <p className="mt-3 text-red-400">{error}</p>}
            </div>
          </section>
        )}

        {/* ACTIVE BUDGET */}
        {budget && (
          <section>
            <div className="rounded-[28px] border border-white/10 bg-white/5 px-6 py-6 backdrop-blur-2xl">
              <div className="mb-6 flex items-center justify-between">
                <div>
                  <p className="text-xs uppercase text-blue-300/70">
                    Active Budget
                  </p>
                  <h2 className="text-xl font-semibold text-white">
                    Your Budget
                  </h2>
                </div>

                <div className="rounded-2xl bg-emerald-500/15 p-3 text-emerald-300">
                  <Wallet />
                </div>
              </div>

              <div className="grid gap-4 md:grid-cols-3">
                <div>
                  <p className="text-xs text-white/50">Amount</p>
                  <p className="text-lg text-white">${budget.amount}</p>
                </div>

                <div>
                  <p className="text-xs text-white/50">Savings</p>
                  <p className="text-lg text-white">
                    ${budget.savings ?? 0}
                  </p>
                </div>

                <div>
                  <p className="text-xs text-white/50">End Date</p>
                  <p className="text-lg text-white">
                    {budget.endDate ?? "No limit"}
                  </p>
                </div>
              </div>

              <button
                onClick={handleClose}
                className="mt-6 w-full rounded-xl bg-red-500/20 py-3 text-red-300"
              >
                Close Budget
              </button>
            </div>
          </section>
        )}
      </div>
    </main>
  )
}