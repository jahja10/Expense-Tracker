import Sidebar from "./components/Sidebar"
import Dashboard from "./pages/Dashboard"

export default function App() {
  return (
    <div className="min-h-screen bg-zinc-950 text-white">
      
      <div className="absolute inset-0 -z-10 bg-[radial-gradient(1200px_600px_at_30%_0%,rgba(59,130,246,0.18),transparent_60%),radial-gradient(900px_500px_at_80%_20%,rgba(16,185,129,0.12),transparent_55%),radial-gradient(900px_700px_at_50%_100%,rgba(255,255,255,0.06),transparent_60%)]" />

      <div className="flex items-stretch gap-6 p-6">
        <Sidebar />
        <Dashboard />
      </div>

    </div>
  )
}