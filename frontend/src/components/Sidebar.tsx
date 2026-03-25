import {
  AlignJustify,
  Search,
  LayoutDashboard,
  ArrowLeftRight,
  Wallet,
  Repeat,
  Shapes,
  CreditCard,
  User,
} from "lucide-react"
import { NavLink, useLocation } from "react-router-dom"

type NavItem = {
  label: string
  icon: React.ElementType
  path: string
}

const nav: NavItem[] = [
  { label: "Home", icon: LayoutDashboard, path: "/dashboard" },
  { label: "Transactions", icon: ArrowLeftRight, path: "/transactions" },
  { label: "Budgets", icon: Wallet, path: "/budgets" },
  { label: "Recurring Transactions", icon: Repeat, path: "/recurring-transactions" },
  { label: "Categories", icon: Shapes, path: "/categories" },
  { label: "Payment Methods", icon: CreditCard, path: "/payment-methods" },
  { label: "User Profile", icon: User, path: "/profile" },
]

export default function Sidebar() {
  const location = useLocation()

  const fullHeightPages = ["/budgets", "/profile", "/categories", "/payment-methods"]
  const isFullHeightPage = fullHeightPages.includes(location.pathname)

  return (
    <aside
      className={`w-[270px] shrink-0 ${
        isFullHeightPage ? "self-start" : ""
      }`}
    >
      <div
        className={`relative flex flex-col rounded-[28px] border border-white/10 bg-white/5 shadow-[0_30px_90px_rgba(0,0,0,0.45)] backdrop-blur-2xl ${
          isFullHeightPage ? "min-h-[calc(100vh-48px)]" : "h-full min-h-full"
        }`}
      >
        <div className="pointer-events-none absolute inset-0 rounded-[28px] bg-[radial-gradient(120%_80%_at_50%_0%,rgba(255,255,255,0.10),rgba(0,0,0,0.0)_55%),radial-gradient(120%_80%_at_50%_110%,rgba(0,0,0,0.25),rgba(0,0,0,0.0)_55%)]" />

        <div className="relative flex items-center justify-between px-5 pt-5">
          <button className="grid h-11 w-11 cursor-pointer place-items-center rounded-2xl border border-white/10 bg-white/5 text-white/70 transition-colors hover:bg-white/10 hover:text-emerald-400">
            <AlignJustify className="h-5 w-5" />
          </button>

          <button className="grid h-11 w-11 cursor-pointer place-items-center rounded-2xl border border-white/10 bg-white/5 text-white/70 transition-colors hover:bg-white/10 hover:text-emerald-400">
            <Search className="h-5 w-5" />
          </button>
        </div>

        <nav className="relative mt-6 flex-1 px-5">
          <ul className="space-y-2">
            {nav.map((item) => {
              const Icon = item.icon

              return (
                <li key={item.label}>
                  <NavLink
                    to={item.path}
                    className={({ isActive }) =>
                      [
                        "group flex items-center gap-3 rounded-2xl px-4 py-3 text-[15px] transition-colors duration-200",
                        isActive
                          ? "border border-white/10 bg-white/10 text-emerald-400 shadow-[0_0_12px_rgba(16,185,129,0.15)]"
                          : "text-white/60 hover:bg-white/7 hover:text-white/90",
                      ].join(" ")
                    }
                  >
                    {({ isActive }) => (
                      <>
                        <span
                          className={[
                            "grid h-8 w-8 place-items-center rounded-xl transition-colors duration-200",
                            isActive
                              ? "bg-white/10 text-emerald-400"
                              : "bg-white/5 text-white/70 group-hover:bg-white/10 group-hover:text-emerald-400",
                          ].join(" ")}
                        >
                          <Icon className="h-5 w-5" />
                        </span>

                        <span className="truncate">{item.label}</span>
                      </>
                    )}
                  </NavLink>
                </li>
              )
            })}
          </ul>
        </nav>

        <div className="mt-auto px-5 pb-6">
          <div className="flex items-center gap-3 rounded-2xl border border-white/10 bg-white/5 px-4 py-3">
            <div className="grid h-9 w-9 place-items-center rounded-xl bg-emerald-500/15 text-emerald-400">
              <Wallet className="h-5 w-5" />
            </div>

            <div className="leading-tight">
              <div className="text-sm font-semibold text-white">
                Expense Tracker
              </div>
              <div className="text-[11px] text-white/50">
                Personal finance
              </div>
            </div>
          </div>
        </div>
      </div>
    </aside>
  )
}