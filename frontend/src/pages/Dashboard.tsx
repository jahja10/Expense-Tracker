import { useEffect, useMemo, useState } from "react"
import { useNavigate } from "react-router-dom"
import MonthlyOverviewCard from "../components/MonthlyOverviewCard"
import BudgetStatusCard from "../components/BudgetStatusCard"
import RecurringTransactionsCard from "../components/RecurringTransactionsCard"
import SpendingTrendChart from "../components/SpendingTrendChart"
import RecentTransactionsCard from "../components/RecentTransactionsCard"
import QuickSummaryCard from "../components/QuickSummaryCard"

type RecurringTransaction = {
  id: number
  name: string
  amount: number
  frequency: string
  nextDueDate: string | null
}

type DashboardSummary = {
  userName: string
  spentThisMonth: number
  monthlyBudget: number
  budgetEndDate: string | null
  recurringTransactions: RecurringTransaction[]
  totalTransactionsThisMonth: number
  upcomingPaymentsThisMonth: number
  averageDailySpending: number
}

type SpendingTrendItem = {
  month: string
  spent: number
  budget: number
}

type RecentTransaction = {
  price: number
  transactionDate: string
  transactionType: string
  description: string | null
  location: string | null
  categoryName: string
  paymentMethodName: string
}

export default function Dashboard() {
  const navigate = useNavigate()

  const [data, setData] = useState<DashboardSummary | null>(null)
  const [spendingTrend, setSpendingTrend] = useState<SpendingTrendItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [recentTransactions, setRecentTransactions] = useState<RecentTransaction[]>([])

  const recurringTransactions = data?.recurringTransactions ?? []

  useEffect(() => {
    const fetchDashboard = async () => {
      try {
        setLoading(true)
        setError(null)

        const token = localStorage.getItem("token")

        if (!token) {
          navigate("/login")
          return
        }

        const [summaryResponse, trendResponse, recentTransactionsResponse] =
          await Promise.all([
            fetch(`${import.meta.env.VITE_API_URL}/api/dashboard/summary`, {
              headers: {
                Authorization: `Bearer ${token}`,
              },
            }),
            fetch(`${import.meta.env.VITE_API_URL}/api/dashboard/spending-trend`, {
              headers: {
                Authorization: `Bearer ${token}`,
              },
            }),
            fetch(`${import.meta.env.VITE_API_URL}/api/dashboard/recent-transactions`, {
              headers: {
                Authorization: `Bearer ${token}`,
              },
            }),
          ])

        if (
          summaryResponse.status === 401 ||
          trendResponse.status === 401 ||
          recentTransactionsResponse.status === 401
        ) {
          localStorage.removeItem("token")
          navigate("/login")
          return
        }

        if (!summaryResponse.ok) {
          throw new Error("Failed to load dashboard data")
        }

        if (!trendResponse.ok) {
          throw new Error("Failed to load spending trend")
        }

        if (!recentTransactionsResponse.ok) {
          throw new Error("Failed to load recent transactions")
        }

        const summaryResult: DashboardSummary = await summaryResponse.json()
        const trendResult: SpendingTrendItem[] = await trendResponse.json()
        const recentTransactionsResult: RecentTransaction[] =
          await recentTransactionsResponse.json()

        setData(summaryResult)
        setSpendingTrend(trendResult)
        setRecentTransactions(recentTransactionsResult)
      } catch (err: any) {
        setError(err.message || "Unexpected error")
      } finally {
        setLoading(false)
      }
    }

    fetchDashboard()
  }, [navigate])

  const userName = data?.userName ?? "User"
  const spentThisMonth = data?.spentThisMonth ?? 0
  const monthlyBudget = data?.monthlyBudget ?? 0

  const totalTransactionsThisMonth = data?.totalTransactionsThisMonth ?? 0
  const upcomingPaymentsThisMonth = data?.upcomingPaymentsThisMonth ?? 0
  const averageDailySpending = data?.averageDailySpending ?? 0

  const remaining = useMemo(
    () => Math.max(0, monthlyBudget - spentThisMonth),
    [monthlyBudget, spentThisMonth]
  )

  const money = (n: number) =>
    n.toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    })

  const percentage =
    monthlyBudget > 0
      ? Math.min(100, (spentThisMonth / monthlyBudget) * 100)
      : 0

  const daysLeft = useMemo(() => {
    if (!data?.budgetEndDate) return 0

    const today = new Date()
    const endDate = new Date(data.budgetEndDate)

    const diffMs = endDate.getTime() - today.getTime()
    const diffDays = Math.ceil(diffMs / (1000 * 60 * 60 * 24))

    return Math.max(0, diffDays)
  }, [data?.budgetEndDate])

  let status

  if (percentage >= 100) {
    status = "Over budget"
  } else if (percentage >= 80) {
    status = "Warning"
  } else {
    status = "On track"
  }

  return (
    <div className="flex-1 px-6">
      <div className="grid grid-cols-12 gap-6">
        <div className="col-span-8">
          <MonthlyOverviewCard
            userName={userName}
            spentThisMonth={spentThisMonth}
            monthlyBudget={monthlyBudget}
            remaining={remaining}
            loading={loading}
            error={error}
            money={money}
          />
        </div>

        <div className="col-span-4">
          <BudgetStatusCard
            percentage={percentage}
            status={status}
            daysLeft={daysLeft}
          />
        </div>

        <div className="col-span-8">
          <SpendingTrendChart data={spendingTrend} />
        </div>

        <div className="col-span-4">
          <RecurringTransactionsCard
            recurringTransactions={recurringTransactions}
          />
        </div>

        <div className="col-span-8">
          <RecentTransactionsCard transactions={recentTransactions} />
        </div>

        <div className="col-span-4">
          <QuickSummaryCard
            totalTransactions={totalTransactionsThisMonth}
            upcomingPayments={upcomingPaymentsThisMonth}
            averagePerDay={averageDailySpending}
            money={money}
          />
        </div>
      </div>
    </div>
  )
}