import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { Mail, Lock } from "lucide-react"

export default function Login() {
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    try {
      setLoading(true)
      setError(null)

       if (email.trim() === "" || password.trim() === "") {
        setError("All fields are required!")
        return
    }

      const response = await fetch(`${import.meta.env.VITE_API_URL}/auth/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email,
          password,
        }),
      })

      if (!response.ok) {
        throw new Error("Invalid email or password")
      } 
      
      const result = await response.json()

      localStorage.setItem("token", result.token)
      navigate("/dashboard")
    } catch (err) {
      setError(err instanceof Error ? err.message : "Login failed")
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen bg-zinc-950 text-white">
      <div className="absolute inset-0 -z-10 bg-[radial-gradient(1200px_600px_at_30%_0%,rgba(59,130,246,0.18),transparent_60%),radial-gradient(900px_500px_at_80%_20%,rgba(16,185,129,0.12),transparent_55%),radial-gradient(900px_700px_at_50%_100%,rgba(255,255,255,0.06),transparent_60%)]" />

      <div className="flex min-h-screen items-center justify-center p-6">
        <div className="relative w-full max-w-md overflow-hidden rounded-[28px] border border-white/10 bg-white/5 px-6 py-7 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
          <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_20%_0%,rgba(255,255,255,0.08),transparent_45%),radial-gradient(100%_100%_at_100%_100%,rgba(16,185,129,0.10),transparent_50%)]" />

          <div className="relative z-10">
            <p className="text-sm uppercase tracking-[0.3em] text-white/40">
              Expense Tracker
            </p>

            <h1 className="mt-3 text-3xl font-semibold text-white">
              Login
            </h1>

            <p className="mt-2 text-sm text-white/55">
              Sign in to continue to your dashboard
            </p>

            <form onSubmit={handleSubmit} className="mt-8 flex flex-col gap-4">
              <div>
                <label className="mb-2 block text-sm text-white/65">
                  Email
                </label>

                <div className="flex items-center gap-3 rounded-2xl border border-white/10 bg-white/5 px-4 py-3 shadow-[inset_0_1px_0_rgba(255,255,255,0.03)]">
                  <Mail className="h-5 w-5 text-white/40" />
                  <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="Enter your email"
                    className="w-full bg-transparent text-white outline-none placeholder:text-white/30"
                  />
                </div>
              </div>

              <div>
                <label className="mb-2 block text-sm text-white/65">
                  Password
                </label>

                <div className="flex items-center gap-3 rounded-2xl border border-white/10 bg-white/5 px-4 py-3 shadow-[inset_0_1px_0_rgba(255,255,255,0.03)]">
                  <Lock className="h-5 w-5 text-white/40" />
                  <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Enter your password"
                    className="w-full bg-transparent text-white outline-none placeholder:text-white/30"
                  />
                </div>
              </div>

              {error && (
                <p className="text-sm text-red-400">{error}</p>
              )}

              <button
                type="submit"
                disabled={loading}
                className="cursor-pointer mt-2 rounded-2xl border border-emerald-400/20 bg-emerald-500/15 px-4 py-3 font-semibold text-emerald-300 shadow-[0_0_30px_rgba(16,185,129,0.15)] transition hover:bg-emerald-500/20 disabled:cursor-not-allowed disabled:opacity-50"
              >
                {loading ? "Logging in..." : "Login"}
              </button>

              <p className="mt-4 text-center text-sm text-white/50">
                Don't have an account?{" "}
                <span
                  onClick={() => navigate("/register")}
                  className="cursor-pointer text-emerald-400 hover:underline"
                >
                  Sign up
                </span>
              </p>
            </form>
          </div>
        </div>
      </div>
    </div>
  )
}