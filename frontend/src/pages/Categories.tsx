import { useEffect, useState } from "react"
import { Check, List, Pencil, Plus, Shapes, Trash2, X } from "lucide-react"

type Category = {
  id: number
  name: string
  createdAt: string
  updatedAt: string
}

type CreateCategoryRequest = {
  name: string
}

type UpdateCategoryRequest = {
  name: string
}

export default function Categories() {
  const [categories, setCategories] = useState<Category[]>([])

  const [name, setName] = useState("")
  const [editingId, setEditingId] = useState<number | null>(null)
  const [deleteId, setDeleteId] = useState<number | null>(null)

  const [loading, setLoading] = useState(false)
  const [loadingCategories, setLoadingCategories] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const token = localStorage.getItem("token")
  const apiUrl = import.meta.env.VITE_API_URL

  const resetForm = () => {
    setEditingId(null)
    setName("")
    setError(null)
  }

  useEffect(() => {
    const loadCategories = async () => {
      try {
        setLoadingCategories(true)
        setError(null)

        const response = await fetch(`${apiUrl}/Categories`, {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        })

        if (!response.ok) {
          throw new Error("Failed to load categories")
        }

        const data = await response.json()
        const result: Category[] = data.categories ?? []

        setCategories(result)
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Failed to load categories"
        )
      } finally {
        setLoadingCategories(false)
      }
    }

    loadCategories()
  }, [apiUrl, token])

  const handleEdit = (category: Category) => {
    setEditingId(category.id)
    setName(category.name)
    setError(null)

    window.scrollTo({
      top: 0,
      behavior: "smooth",
    })
  }

  const handleDelete = async () => {
    if (!deleteId) return

    try {
      setError(null)

      const response = await fetch(`${apiUrl}/Categories/${deleteId}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      if (!response.ok) {
        const errorData = await response.json().catch(() => null)
        const message =
          errorData?.[0]?.description ||
          errorData?.title ||
          "Failed to delete category"

        throw new Error(message)
      }

      setCategories((prev) =>
        prev.filter((category) => category.id !== deleteId)
      )

      if (editingId === deleteId) {
        resetForm()
      }

      setDeleteId(null)
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Failed to delete category"
      )
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    try {
      setLoading(true)
      setError(null)

      if (name.trim() === "") {
        setError("Category name is required.")
        return
      }

      const isEditing = editingId !== null
      const url = isEditing
        ? `${apiUrl}/Categories/${editingId}`
        : `${apiUrl}/Categories`

      const method = isEditing ? "PUT" : "POST"

      const body: CreateCategoryRequest | UpdateCategoryRequest = {
        name: name.trim(),
      }

      const response = await fetch(url, {
        method,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(body),
      })

      if (!response.ok) {
        const errorData = await response.json().catch(() => null)
        const message =
          errorData?.[0]?.description ||
          errorData?.title ||
          (isEditing
            ? "Failed to update category"
            : "Failed to create category")

        throw new Error(message)
      }

      const savedCategory: Category = await response.json()

      if (isEditing) {
        setCategories((prev) =>
          prev.map((category) =>
            category.id === editingId ? savedCategory : category
          )
        )
      } else {
        setCategories((prev) => [savedCategory, ...prev])
      }

      resetForm()
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : editingId !== null
          ? "Failed to update category"
          : "Failed to create category"
      )
    } finally {
      setLoading(false)
    }
  }

  return (
    <main className="flex-1">
      <div className="flex flex-col gap-6">
        <section className="w-full">
          <div className="relative overflow-visible rounded-[28px] border border-white/10 bg-white/5 px-6 py-6 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
            <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_15%_0%,rgba(255,255,255,0.07),transparent_45%),radial-gradient(100%_100%_at_100%_100%,rgba(16,185,129,0.12),transparent_52%)]" />

            <div className="relative z-10">
              <div className="mb-6 flex items-center justify-between">
                <div>
                  <p className="text-xs uppercase tracking-[0.25em] text-emerald-300/70">
                    {editingId ? "Edit Entry" : "New Entry"}
                  </p>
                  <h2 className="mt-2 text-xl font-semibold text-white">
                    {editingId ? "Update Category" : "Add Category"}
                  </h2>
                </div>

                <div className="flex items-center gap-2">
                  {editingId && (
                    <button
                      type="button"
                      onClick={resetForm}
                      className="flex h-11 w-11 cursor-pointer items-center justify-center rounded-2xl border border-white/10 bg-white/5 text-white/70 transition hover:bg-white/10 hover:text-white"
                    >
                      <X className="h-5 w-5" />
                    </button>
                  )}

                  <div className="flex h-11 w-11 items-center justify-center rounded-2xl border border-emerald-400/20 bg-emerald-500/15 text-emerald-300 shadow-[0_0_30px_rgba(16,185,129,0.15)]">
                    {editingId ? (
                      <Pencil className="h-5 w-5" />
                    ) : (
                      <Plus className="h-5 w-5" />
                    )}
                  </div>
                </div>
              </div>

              <form
                onSubmit={handleSubmit}
                className="grid grid-cols-1 gap-4 md:grid-cols-[1fr_auto]"
              >
                <div>
                  <label className="mb-2 block text-sm text-white/65">
                    Category Name
                  </label>
                  <div className="flex items-center gap-3 rounded-2xl border border-white/10 bg-white/5 px-4 py-3 shadow-[inset_0_1px_0_rgba(255,255,255,0.03)] transition focus-within:border-emerald-400/30 focus-within:bg-white/[0.07]">
                    <Shapes className="h-5 w-5 text-white/40" />
                    <input
                      type="text"
                      value={name}
                      onChange={(e) => setName(e.target.value)}
                      placeholder="Enter category name"
                      className="w-full cursor-pointer bg-transparent text-white outline-none placeholder:text-white/30"
                    />
                  </div>
                </div>

                <div className="flex items-end">
                  <button
                    type="submit"
                    disabled={loading}
                    className="w-full cursor-pointer rounded-2xl border border-emerald-400/20 bg-emerald-500/15 px-4 py-3 font-semibold text-emerald-300 shadow-[0_0_30px_rgba(16,185,129,0.15)] transition hover:bg-emerald-500/20 disabled:cursor-not-allowed disabled:opacity-50 md:min-w-[220px]"
                  >
                    {loading
                      ? editingId
                        ? "Updating..."
                        : "Saving..."
                      : editingId
                      ? "Update Category"
                      : "Add Category"}
                  </button>
                </div>

                {error && (
                  <p className="text-sm text-red-400 md:col-span-2">{error}</p>
                )}

                {editingId && (
                  <div className="md:col-span-2">
                    <button
                      type="button"
                      onClick={resetForm}
                      className="w-full cursor-pointer rounded-2xl border border-white/10 bg-white/5 px-4 py-3 font-semibold text-white/75 transition hover:bg-white/10 hover:text-white md:max-w-[180px]"
                    >
                      Cancel
                    </button>
                  </div>
                )}
              </form>
            </div>
          </div>
        </section>

        <section className="w-full">
          <div className="relative overflow-hidden rounded-[28px] border border-white/10 bg-white/5 px-6 py-6 shadow-[0_15px_40px_rgba(0,0,0,0.35)] backdrop-blur-2xl">
            <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_20%_0%,rgba(255,255,255,0.08),transparent_45%),radial-gradient(100%_100%_at_100%_100%,rgba(59,130,246,0.10),transparent_50%)]" />

            <div className="relative z-10">
              <div className="mb-6 flex items-center justify-between">
                <div>
                  <p className="text-xs uppercase tracking-[0.25em] text-blue-300/70">
                    Activity
                  </p>
                  <h2 className="mt-2 text-xl font-semibold text-white">
                    All Categories
                  </h2>
                </div>

                <div className="flex h-11 w-11 items-center justify-center rounded-2xl border border-white/10 bg-white/5 text-white/70">
                  <List className="h-5 w-5" />
                </div>
              </div>

              {loadingCategories ? (
                <p className="text-sm text-white/60">Loading categories...</p>
              ) : categories.length === 0 ? (
                <p className="text-sm text-white/60">No categories yet.</p>
              ) : (
                <div className="h-[390px] overflow-y-scroll pr-2 [scrollbar-width:thin] [scrollbar-color:rgba(52,211,153,0.35)_transparent] [&::-webkit-scrollbar]:w-2 [&::-webkit-scrollbar-track]:bg-transparent [&::-webkit-scrollbar-thumb]:rounded-full [&::-webkit-scrollbar-thumb]:bg-white/10 hover:[&::-webkit-scrollbar-thumb]:bg-emerald-400/30">
                  <div className="flex flex-col gap-2.5">
                    {categories.map((category) => {
                      const isEditingThis = editingId === category.id

                      return (
                        <div
                          key={category.id}
                          onClick={() => handleEdit(category)}
                          className={`cursor-pointer rounded-[22px] border px-4 py-4 shadow-[inset_0_1px_0_rgba(255,255,255,0.04)] transition ${
                            isEditingThis
                              ? "border-emerald-400/30 bg-emerald-500/[0.08]"
                              : "border-white/10 bg-white/[0.04] hover:border-white/15 hover:bg-white/[0.06]"
                          }`}
                        >
                          <div className="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
                            <div className="min-w-0 flex-1">
                              <div className="flex items-center gap-3">
                                <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-2xl border border-emerald-400/20 bg-emerald-500/10 text-emerald-300">
                                  <Shapes className="h-4.5 w-4.5" />
                                </div>

                                <div className="min-w-0">
                                  <p className="truncate text-sm font-semibold text-white">
                                    {category.name}
                                  </p>

                                  
                                </div>
                              </div>
                            </div>

                            <div className="grid gap-3 text-sm text-white/65 sm:grid-cols-2 lg:min-w-[320px] lg:max-w-[420px] lg:flex-1">
                              <div>
                                <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                                  Created
                                </p>
                                <p className="mt-1 text-sm text-white/75">
                                  {new Date(category.createdAt).toLocaleDateString()}
                                </p>
                              </div>

                              <div>
                                <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                                  Updated
                                </p>
                                <p className="mt-1 text-sm text-white/75">
                                  {new Date(category.updatedAt).toLocaleDateString()}
                                </p>
                              </div>
                            </div>

                            <div className="lg:min-w-[140px] lg:text-right">
                              <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                                Status
                              </p>
                              <p className="mt-1 inline-flex items-center gap-2 text-sm font-medium text-emerald-300 lg:justify-end">
                                <Check className="h-4 w-4" />
                                Active
                              </p>

                              <div className="mt-3 flex items-center gap-2 lg:justify-end">
                                <button
                                  type="button"
                                  onClick={(e) => {
                                    e.stopPropagation()
                                    handleEdit(category)
                                  }}
                                  className="inline-flex cursor-pointer items-center gap-1 rounded-xl border border-white/10 bg-white/5 px-2.5 py-1.5 text-xs text-white/70 transition hover:bg-white/10 hover:text-white"
                                >
                                  <Pencil className="h-3.5 w-3.5" />
                                  Edit
                                </button>

                                <button
                                  type="button"
                                  onClick={(e) => {
                                    e.stopPropagation()
                                    setDeleteId(category.id)
                                  }}
                                  className="inline-flex cursor-pointer items-center gap-1 rounded-xl border border-red-400/20 bg-red-500/10 px-2.5 py-1.5 text-xs text-red-300 transition hover:bg-red-500/15 hover:text-red-200"
                                >
                                  <Trash2 className="h-3.5 w-3.5" />
                                  Delete
                                </button>
                              </div>
                            </div>
                          </div>
                        </div>
                      )
                    })}
                  </div>
                </div>
              )}
            </div>
          </div>
        </section>
      </div>

      {deleteId && (
        <div className="fixed inset-0 z-[100] flex items-center justify-center px-4">
          <div
            className="absolute inset-0 bg-black/60 backdrop-blur-sm"
            onClick={() => setDeleteId(null)}
          />

          <div className="relative z-10 w-full max-w-md overflow-hidden rounded-[28px] border border-white/10 bg-white/5 p-6 shadow-[0_20px_60px_rgba(0,0,0,0.6)] backdrop-blur-2xl">
            <div className="pointer-events-none absolute inset-0 rounded-[28px] bg-[radial-gradient(120%_80%_at_20%_0%,rgba(255,255,255,0.08),transparent_45%),radial-gradient(100%_100%_at_100%_100%,rgba(239,68,68,0.12),transparent_55%)]" />

            <div className="relative z-10">
              <div className="mb-4 flex items-center justify-between">
                <div>
                  <p className="text-xs uppercase tracking-[0.25em] text-red-300/70">
                    Warning
                  </p>
                  <h3 className="mt-2 text-lg font-semibold text-white">
                    Delete category?
                  </h3>
                </div>

                <div className="flex h-11 w-11 items-center justify-center rounded-2xl border border-red-400/20 bg-red-500/15 text-red-300 shadow-[0_0_30px_rgba(239,68,68,0.18)]">
                  <Trash2 className="h-5 w-5" />
                </div>
              </div>

              <p className="text-sm leading-6 text-white/60">
                This action cannot be undone. The selected category will be
                permanently removed.
              </p>

              <div className="mt-6 flex gap-3">
                <button
                  type="button"
                  onClick={() => setDeleteId(null)}
                  className="w-full cursor-pointer rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm font-medium text-white/70 transition hover:bg-white/10 hover:text-white"
                >
                  Cancel
                </button>

                <button
                  type="button"
                  onClick={handleDelete}
                  className="w-full cursor-pointer rounded-2xl border border-red-400/20 bg-red-500/15 px-4 py-3 text-sm font-semibold text-red-300 shadow-[0_0_20px_rgba(239,68,68,0.2)] transition hover:bg-red-500/25"
                >
                  Delete
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </main>
  )
}