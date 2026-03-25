import { BrowserRouter, Routes, Route, Navigate, Outlet} from "react-router-dom"
import Login from "./pages/LogIn"
import Dashboard from "./pages/Dashboard"
import Sidebar from "./components/Sidebar"
import SignUp from "./pages/SignUp"
import Transactions from "./pages/Transactions"
import RecurringTransactions from "./pages/RecurringTransactions"
import Categories from "./pages/Categories"
import PaymentMethods from "./pages/PaymentMethods"
import Budgets from "./pages/Budgets"
import UserProfile from "./pages/UserProfile"
import type { JSX } from "react"

function AppLayout() {
  return (
    <div className="min-h-screen bg-zinc-950 text-white">
      <div className="absolute inset-0 -z-10 bg-[radial-gradient(1200px_600px_at_30%_0%,rgba(59,130,246,0.18),transparent_60%),radial-gradient(900px_500px_at_80%_20%,rgba(16,185,129,0.12),transparent_55%),radial-gradient(900px_700px_at_50%_100%,rgba(255,255,255,0.06),transparent_60%)]" />

      <div className="flex items-stretch gap-6 p-6">
        <Sidebar />
        <Outlet />
      </div>
    </div>
  )
}

function ProtectedRoute() {
  const token = localStorage.getItem("token")
  return token ? <AppLayout /> : <Navigate to="/login" replace />
}

function PublicRoute({ children }: { children: JSX.Element }) {
  const token = localStorage.getItem("token")
  return token ? <Navigate to="/dashboard" replace /> : children
}

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to="/dashboard" replace />} />

        <Route
          path="/login"
          element={
            <PublicRoute>
              <Login />
            </PublicRoute>
          }
        />

        <Route
          path="/register"
          element={
            <PublicRoute>
              <SignUp />
            </PublicRoute>
          }
        />

        <Route element={<ProtectedRoute />}>
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/transactions" element={<Transactions />} />
          <Route path="/recurring-transactions" element={<RecurringTransactions />} />
          <Route path="/categories" element={<Categories/>}/>
          <Route path="/payment-methods" element={<PaymentMethods/>}/>
          <Route path="/budgets" element={<Budgets/>}/>
          <Route path="/profile" element={<UserProfile/>}/>
        </Route>
      </Routes>
    </BrowserRouter>
  )
}