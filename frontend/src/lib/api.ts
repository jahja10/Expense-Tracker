const API = import.meta.env.VITE_API_URL as string

export async function api<T>(path: string, options: RequestInit = {}) {
  const token = localStorage.getItem("token")

  const res = await fetch(`${API}${path}`, {
    ...options,
    headers: {
      ...(options.headers || {}),
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
  })

  if (!res.ok) throw new Error(await res.text())
  return (await res.json()) as T
}
