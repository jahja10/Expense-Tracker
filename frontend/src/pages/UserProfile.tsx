import { useEffect, useState } from "react"
import { KeyRound, LogOut, ShieldCheck, UserCircle2 } from "lucide-react"
import { useNavigate } from "react-router-dom"

type MeResponse = {
  id: number
  name: string
  email: string
  createdAt: string
  updatedAt: string
  isActive: boolean
}

export default function UserProfile() {
  const navigate = useNavigate()

  const [user, setUser] = useState<MeResponse | null>(null)

  const [currentPassword, setCurrentPassword] = useState("")
  const [newPassword, setNewPassword] = useState("")
  const [confirmNewPassword, setConfirmNewPassword] = useState("")

  const [loadingUser, setLoadingUser] = useState(true)
  const [changingPassword, setChangingPassword] = useState(false)
  const [loggingOut, setLoggingOut] = useState(false)

  const [error, setError] = useState<string | null>(null)
  const [successMessage, setSuccessMessage] = useState<string | null>(null)

  const token = localStorage.getItem("token")
  const apiUrl = import.meta.env.VITE_API_URL

  useEffect(() => {
    const loadMe = async () => {
      try {
        setLoadingUser(true)
        setError(null)

        const response = await fetch(`${apiUrl}/me`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })

        if (!response.ok) {
          throw new Error("Failed to load user profile")
        }

        const data: MeResponse = await response.json()
        setUser(data)
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Failed to load user profile"
        )
      } finally {
        setLoadingUser(false)
      }
    }

    loadMe()
  }, [apiUrl, token])

  const handleChangePassword = async (e: React.FormEvent) => {
    e.preventDefault()

    try {
      setChangingPassword(true)
      setError(null)
      setSuccessMessage(null)

      if (
        currentPassword.trim() === "" ||
        newPassword.trim() === "" ||
        confirmNewPassword.trim() === ""
      ) {
        setError("All password fields are required.")
        return
      }

      if (newPassword !== confirmNewPassword) {
        setError("New password and confirm password do not match.")
        return
      }

      const response = await fetch(`${apiUrl}/me/password`, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          currentPassword,
          newPassword,
          confirmNewPassword,
        }),
      })

      if (!response.ok) {
        const errorData = await response.json().catch(() => null)
        const message =
          errorData?.[0]?.description ||
          errorData?.title ||
          "Failed to change password"

        throw new Error(message)
      }

      setCurrentPassword("")
      setNewPassword("")
      setConfirmNewPassword("")
      setSuccessMessage("Password changed successfully.")
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Failed to change password"
      )
    } finally {
      setChangingPassword(false)
    }
  }

  const handleLogout = async () => {
    try {
      setLoggingOut(true)

      localStorage.removeItem("token")
      navigate("/login", { replace: true })
    } finally {
      setLoggingOut(false)
    }
  }

  return (
    <main className="flex-1">
      <div className="flex flex-col gap-6">
        <section className="w-full">
          <div className="relative overflow-hidden rounded-[28px] border border-white/10 bg-white/5 px-6 py-6 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
            <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_15%_0%,rgba(255,255,255,0.07),transparent_45%),radial-gradient(100%_100%_at_100%_100%,rgba(59,130,246,0.12),transparent_52%)]" />

            <div className="relative z-10">
              <div className="mb-6 flex items-center justify-between">
                <div>
                  <p className="text-xs uppercase tracking-[0.25em] text-blue-300/70">
                    Account
                  </p>
                  <h2 className="mt-2 text-xl font-semibold text-white">
                    User Profile
                  </h2>
                </div>

                <div className="flex h-11 w-11 items-center justify-center rounded-2xl border border-white/10 bg-white/5 text-white/70">
                  <UserCircle2 className="h-5 w-5" />
                </div>
              </div>

              {loadingUser ? (
                <p className="text-sm text-white/60">Loading profile...</p>
              ) : user ? (
                <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
                  <div className="rounded-2xl border border-white/10 bg-white/[0.04] px-4 py-4">
                    <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                      Name
                    </p>
                    <p className="mt-2 text-base font-semibold text-white">
                      {user.name}
                    </p>
                  </div>

                  <div className="rounded-2xl border border-white/10 bg-white/[0.04] px-4 py-4">
                    <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                      Email
                    </p>
                    <p className="mt-2 break-all text-base font-semibold text-white">
                      {user.email}
                    </p>
                  </div>

                  <div className="rounded-2xl border border-white/10 bg-white/[0.04] px-4 py-4">
                    <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                      Status
                    </p>
                    <p className="mt-2 inline-flex items-center gap-2 text-base font-semibold text-emerald-300">
                      <ShieldCheck className="h-4 w-4" />
                      {user.isActive ? "Active" : "Inactive"}
                    </p>
                  </div>

                  <div className="rounded-2xl border border-white/10 bg-white/[0.04] px-4 py-4">
                    <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                      Created
                    </p>
                    <p className="mt-2 text-base font-semibold text-white">
                      {new Date(user.createdAt).toLocaleDateString()}
                    </p>
                  </div>
                </div>
              ) : (
                <p className="text-sm text-red-400">Could not load profile.</p>
              )}
            </div>
          </div>
        </section>

        <section className="w-full">
          <div className="relative overflow-hidden rounded-[28px] border border-white/10 bg-white/5 px-6 py-6 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
            <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_15%_0%,rgba(255,255,255,0.07),transparent_45%),radial-gradient(100%_100%_at_100%_100%,rgba(16,185,129,0.12),transparent_52%)]" />

            <div className="relative z-10">
              <div className="mb-6 flex items-center justify-between">
                <div>
                  <p className="text-xs uppercase tracking-[0.25em] text-emerald-300/70">
                    Security
                  </p>
                  <h2 className="mt-2 text-xl font-semibold text-white">
                    Change Password
                  </h2>
                </div>

                <div className="flex h-11 w-11 items-center justify-center rounded-2xl border border-emerald-400/20 bg-emerald-500/15 text-emerald-300 shadow-[0_0_30px_rgba(16,185,129,0.15)]">
                  <KeyRound className="h-5 w-5" />
                </div>
              </div>

              <form
                onSubmit={handleChangePassword}
                className="grid grid-cols-1 gap-4 md:grid-cols-3"
              >
                <div>
                  <label className="mb-2 block text-sm text-white/65">
                    Current Password
                  </label>
                  <input
                    type="password"
                    value={currentPassword}
                    onChange={(e) => setCurrentPassword(e.target.value)}
                    placeholder="Enter current password"
                    className="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-white outline-none placeholder:text-white/30 transition focus:border-emerald-400/30 focus:bg-white/[0.07]"
                  />
                </div>

                <div>
                  <label className="mb-2 block text-sm text-white/65">
                    New Password
                  </label>
                  <input
                    type="password"
                    value={newPassword}
                    onChange={(e) => setNewPassword(e.target.value)}
                    placeholder="Enter new password"
                    className="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-white outline-none placeholder:text-white/30 transition focus:border-emerald-400/30 focus:bg-white/[0.07]"
                  />
                </div>

                <div>
                  <label className="mb-2 block text-sm text-white/65">
                    Confirm New Password
                  </label>
                  <input
                    type="password"
                    value={confirmNewPassword}
                    onChange={(e) => setConfirmNewPassword(e.target.value)}
                    placeholder="Confirm new password"
                    className="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-white outline-none placeholder:text-white/30 transition focus:border-emerald-400/30 focus:bg-white/[0.07]"
                  />
                </div>

                <div className="md:col-span-3">
                  <button
                    type="submit"
                    disabled={changingPassword}
                    className="w-full cursor-pointer rounded-2xl border border-emerald-400/20 bg-emerald-500/15 px-4 py-3 font-semibold text-emerald-300 shadow-[0_0_30px_rgba(16,185,129,0.15)] transition hover:bg-emerald-500/20 disabled:cursor-not-allowed disabled:opacity-50"
                  >
                    {changingPassword ? "Updating..." : "Change Password"}
                  </button>
                </div>
              </form>

              {error && <p className="mt-4 text-sm text-red-400">{error}</p>}
              {successMessage && (
                <p className="mt-4 text-sm text-emerald-300">
                  {successMessage}
                </p>
              )}
            </div>
          </div>
        </section>

        <section className="w-full">
          <div className="relative overflow-hidden rounded-[28px] border border-white/10 bg-white/5 px-6 py-6 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
            <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_15%_0%,rgba(255,255,255,0.07),transparent_45%),radial-gradient(100%_100%_at_100%_100%,rgba(239,68,68,0.12),transparent_52%)]" />

            <div className="relative z-10 flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
              <div>
                <p className="text-xs uppercase tracking-[0.25em] text-red-300/70">
                  Session
                </p>
                <h2 className="mt-2 text-xl font-semibold text-white">
                  Logout
                </h2>
                <p className="mt-2 text-sm text-white/60">
                  Sign out from your account on this device.
                </p>
              </div>

              <button
                type="button"
                onClick={handleLogout}
                disabled={loggingOut}
                className="inline-flex cursor-pointer items-center justify-center gap-2 rounded-2xl border border-red-400/20 bg-red-500/15 px-5 py-3 font-semibold text-red-300 shadow-[0_0_20px_rgba(239,68,68,0.2)] transition hover:bg-red-500/25 disabled:cursor-not-allowed disabled:opacity-50"
              >
                <LogOut className="h-4 w-4" />
                {loggingOut ? "Logging out..." : "Logout"}
              </button>
            </div>
          </div>
        </section>
      </div>
    </main>
  )
}