import { useEffect, useRef, useState } from "react"
import {
  CalendarDays,
  Check,
  ChevronDown,
  CreditCard,
  List,
  Pencil,
  Plus,
  Repeat,
  Shapes,
  Trash2,
  X,
} from "lucide-react"

type FrequencyOfTransaction = "monthly" | "yearly"

type Category = {
  id: number
  name: string
}

type PaymentMethod = {
  id: number
  name: string
}

type RecurringTransactionItem = {
  id: number
  name: string
  frequencyOfTransaction: FrequencyOfTransaction | string
  userId: number
  categoryId: number
  paymentMethodId: number
  amount: number
  startDate: string
  lastGeneratedDate: string | null
  isActive: boolean
  categoryName?: string
  paymentMethodName?: string
}

type GetRecurringTransactionsResponse = {
  recurringTransactions: RecurringTransactionItem[]
  totalCount: number
}

type CreateRecurringTransactionRequest = {
  name: string
  frequencyOfTransaction: FrequencyOfTransaction
  categoryId: number
  paymentMethodId: number
  amount: number
  startDate: string
}

type UpdateRecurringTransactionRequest = {
  name?: string
  frequencyOfTransaction?: FrequencyOfTransaction
  categoryId?: number
  paymentMethodId?: number
  amount?: number
  startDate?: string
}

type CustomSelectOption = {
  value: string
  label: string
}

type CustomSelectProps = {
  label: string
  value: string
  onChange: (value: string) => void
  options: CustomSelectOption[]
  placeholder: string
  icon?: React.ElementType
}

function CustomSelect({
  label,
  value,
  onChange,
  options,
  placeholder,
  icon: Icon,
}: CustomSelectProps) {
  const [open, setOpen] = useState(false)
  const wrapperRef = useRef<HTMLDivElement | null>(null)

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        wrapperRef.current &&
        !wrapperRef.current.contains(event.target as Node)
      ) {
        setOpen(false)
      }
    }

    document.addEventListener("mousedown", handleClickOutside)
    return () => {
      document.removeEventListener("mousedown", handleClickOutside)
    }
  }, [])

  const selectedOption = options.find((option) => option.value === value)

  return (
    <div ref={wrapperRef} className="relative">
      <label className="mb-2 block text-sm text-white/65">{label}</label>

      <button
        type="button"
        onClick={() => setOpen((prev) => !prev)}
        className={`flex w-full cursor-pointer items-center gap-3 rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-left shadow-[inset_0_1px_0_rgba(255,255,255,0.03)] transition hover:bg-white/[0.07] ${
          open
            ? "border-emerald-400/30 bg-white/[0.07]"
            : "focus:border-emerald-400/30"
        }`}
      >
        {Icon && <Icon className="h-5 w-5 shrink-0 text-white/40" />}

        <span
          className={`flex-1 truncate ${
            selectedOption ? "text-white" : "text-white/30"
          }`}
        >
          {selectedOption ? selectedOption.label : placeholder}
        </span>

        <ChevronDown
          className={`h-4 w-4 shrink-0 text-white/45 transition ${
            open ? "rotate-180" : ""
          }`}
        />
      </button>

      {open && (
        <div className="absolute left-0 right-0 top-[calc(100%+10px)] z-50 overflow-hidden rounded-2xl border border-white/10 bg-zinc-950/95 shadow-[0_20px_50px_rgba(0,0,0,0.45)] backdrop-blur-2xl">
          <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(120%_80%_at_20%_0%,rgba(255,255,255,0.08),transparent_45%),radial-gradient(100%_100%_at_100%_100%,rgba(16,185,129,0.10),transparent_55%)]" />

          <div className="relative max-h-56 overflow-y-auto p-2 [scrollbar-width:thin] [scrollbar-color:rgba(52,211,153,0.35)_transparent] [&::-webkit-scrollbar]:w-2 [&::-webkit-scrollbar-track]:bg-transparent [&::-webkit-scrollbar-thumb]:rounded-full [&::-webkit-scrollbar-thumb]:bg-white/10 hover:[&::-webkit-scrollbar-thumb]:bg-emerald-400/30">
            {options.map((option) => {
              const isSelected = option.value === value

              return (
                <button
                  key={option.value}
                  type="button"
                  onClick={() => {
                    onChange(option.value)
                    setOpen(false)
                  }}
                  className={`flex w-full cursor-pointer items-center justify-between rounded-xl px-3 py-2.5 text-sm transition ${
                    isSelected
                      ? "bg-emerald-500/15 text-emerald-300"
                      : "text-white/80 hover:bg-white/7 hover:text-white"
                  }`}
                >
                  <span className="truncate">{option.label}</span>
                  {isSelected && <Check className="h-4 w-4 shrink-0" />}
                </button>
              )
            })}
          </div>
        </div>
      )}
    </div>
  )
}

export default function RecurringTransactions() {
  const [recurringTransactions, setRecurringTransactions] = useState<
    RecurringTransactionItem[]
  >([])
  const [categories, setCategories] = useState<Category[]>([])
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([])

  const [name, setName] = useState("")
  const [frequencyOfTransaction, setFrequencyOfTransaction] =
    useState<FrequencyOfTransaction>("monthly")
  const [categoryId, setCategoryId] = useState("")
  const [paymentMethodId, setPaymentMethodId] = useState("")
  const [amount, setAmount] = useState("")
  const [startDate, setStartDate] = useState("")

  const [editingId, setEditingId] = useState<number | null>(null)
  const [deleteId, setDeleteId] = useState<number | null>(null)

  const [loading, setLoading] = useState(false)
  const [loadingRecurringTransactions, setLoadingRecurringTransactions] =
    useState(true)
  const [error, setError] = useState<string | null>(null)

  const token = localStorage.getItem("token")
  const apiUrl = import.meta.env.VITE_API_URL

  const resetForm = () => {
    setEditingId(null)
    setName("")
    setFrequencyOfTransaction("monthly")
    setCategoryId("")
    setPaymentMethodId("")
    setAmount("")
    setStartDate("")
    setError(null)
  }

  const enrichRecurringTransaction = (
    recurringTransaction: RecurringTransactionItem,
    categoriesList: Category[],
    paymentMethodsList: PaymentMethod[]
  ): RecurringTransactionItem => {
    return {
      ...recurringTransaction,
      categoryName:
        recurringTransaction.categoryName ??
        categoriesList.find(
          (category) => category.id === recurringTransaction.categoryId
        )?.name ??
        "Unknown category",
      paymentMethodName:
        recurringTransaction.paymentMethodName ??
        paymentMethodsList.find(
          (method) => method.id === recurringTransaction.paymentMethodId
        )?.name ??
        "Unknown payment method",
    }
  }

  useEffect(() => {
    const loadPageData = async () => {
      try {
        setLoadingRecurringTransactions(true)
        setError(null)

        const headers = {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        }

        const [
          recurringTransactionsResponse,
          categoriesResponse,
          paymentMethodsResponse,
        ] = await Promise.all([
          fetch(`${apiUrl}/RecurringTransactions`, { headers }),
          fetch(`${apiUrl}/Categories`, { headers }),
          fetch(`${apiUrl}/PaymentMethods`, { headers }),
        ])

        if (!recurringTransactionsResponse.ok) {
          throw new Error("Failed to load recurring transactions")
        }

        if (!categoriesResponse.ok) {
          throw new Error("Failed to load categories")
        }

        if (!paymentMethodsResponse.ok) {
          throw new Error("Failed to load payment methods")
        }

        const recurringTransactionsData: GetRecurringTransactionsResponse =
          await recurringTransactionsResponse.json()
        const categoriesData = await categoriesResponse.json()
        const paymentMethodsData = await paymentMethodsResponse.json()

        const recurringTransactionsResult: RecurringTransactionItem[] =
          recurringTransactionsData.recurringTransactions ?? []
        const categoriesResult: Category[] = categoriesData.categories ?? []
        const paymentMethodsResult: PaymentMethod[] =
          paymentMethodsData.paymentMethods ?? []

        setCategories(categoriesResult)
        setPaymentMethods(paymentMethodsResult)

        const recurringTransactionsWithNames = recurringTransactionsResult
          .map((recurringTransaction) =>
            enrichRecurringTransaction(
              recurringTransaction,
              categoriesResult,
              paymentMethodsResult
            )
          )
          .sort(
            (a, b) =>
              new Date(b.startDate).getTime() - new Date(a.startDate).getTime()
          )

        setRecurringTransactions(recurringTransactionsWithNames)
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Failed to load page data"
        )
      } finally {
        setLoadingRecurringTransactions(false)
      }
    }

    loadPageData()
  }, [apiUrl, token])

  const handleEdit = (recurringTransaction: RecurringTransactionItem) => {
    setEditingId(recurringTransaction.id)
    setName(recurringTransaction.name)
    setFrequencyOfTransaction(
      recurringTransaction.frequencyOfTransaction as FrequencyOfTransaction
    )
    setCategoryId(String(recurringTransaction.categoryId))
    setPaymentMethodId(String(recurringTransaction.paymentMethodId))
    setAmount(String(recurringTransaction.amount))
    setStartDate(recurringTransaction.startDate?.slice(0, 10) ?? "")
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

      const response = await fetch(`${apiUrl}/RecurringTransactions/${deleteId}`, {
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
          "Failed to delete recurring transaction"

        throw new Error(message)
      }

      setRecurringTransactions((prev) =>
        prev.filter((recurringTransaction) => recurringTransaction.id !== deleteId)
      )

      if (editingId === deleteId) {
        resetForm()
      }

      setDeleteId(null)
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Failed to delete recurring transaction"
      )
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    try {
      setLoading(true)
      setError(null)

      if (
        name.trim() === "" ||
        amount.trim() === "" ||
        startDate.trim() === "" ||
        categoryId === "" ||
        paymentMethodId === ""
      ) {
        setError(
          "Name, amount, start date, category and payment method are required."
        )
        return
      }

      const isEditing = editingId !== null

      const createBody: CreateRecurringTransactionRequest = {
        name: name.trim(),
        frequencyOfTransaction,
        categoryId: Number(categoryId),
        paymentMethodId: Number(paymentMethodId),
        amount: Number(amount),
        startDate,
      }

      const updateBody: UpdateRecurringTransactionRequest = {
        name: name.trim(),
        frequencyOfTransaction,
        categoryId: Number(categoryId),
        paymentMethodId: Number(paymentMethodId),
        amount: Number(amount),
        startDate,
      }

      const url = isEditing
        ? `${apiUrl}/RecurringTransactions/${editingId}`
        : `${apiUrl}/RecurringTransactions`

      const method = isEditing ? "PUT" : "POST"

      const response = await fetch(url, {
        method,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(isEditing ? updateBody : createBody),
      })

      if (!response.ok) {
        const errorData = await response.json().catch(() => null)
        const message =
          errorData?.[0]?.description ||
          errorData?.title ||
          (isEditing
            ? "Failed to update recurring transaction"
            : "Failed to create recurring transaction")

        throw new Error(message)
      }

      const savedRecurringTransaction: RecurringTransactionItem =
        await response.json()

      const enrichedRecurringTransaction = enrichRecurringTransaction(
        savedRecurringTransaction,
        categories,
        paymentMethods
      )

      if (isEditing) {
        setRecurringTransactions((prev) =>
          prev
            .map((recurringTransaction) =>
              recurringTransaction.id === editingId
                ? enrichedRecurringTransaction
                : recurringTransaction
            )
            .sort(
              (a, b) =>
                new Date(b.startDate).getTime() - new Date(a.startDate).getTime()
            )
        )
      } else {
        setRecurringTransactions((prev) =>
          [enrichedRecurringTransaction, ...prev].sort(
            (a, b) =>
              new Date(b.startDate).getTime() - new Date(a.startDate).getTime()
          )
        )
      }

      resetForm()
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : editingId !== null
          ? "Failed to update recurring transaction"
          : "Failed to create recurring transaction"
      )
    } finally {
      setLoading(false)
    }
  }

  const frequencyOptions: CustomSelectOption[] = [
    { value: "monthly", label: "Monthly" },
    { value: "yearly", label: "Yearly" },
  ]

  const categoryOptions: CustomSelectOption[] = categories.map((category) => ({
    value: String(category.id),
    label: category.name,
  }))

  const paymentMethodOptions: CustomSelectOption[] = paymentMethods.map(
    (method) => ({
      value: String(method.id),
      label: method.name,
    })
  )

  const formatFrequency = (frequency: string) => {
    if (frequency.toLowerCase() === "monthly") return "Monthly"
    if (frequency.toLowerCase() === "yearly") return "Yearly"
    return frequency
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
                    {editingId
                      ? "Update Recurring Transaction"
                      : "Add Recurring Transaction"}
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
                className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3"
              >
                <div>
                  <label className="mb-2 block text-sm text-white/65">
                    Name
                  </label>
                  <div className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 shadow-[inset_0_1px_0_rgba(255,255,255,0.03)] transition focus-within:border-emerald-400/30 focus-within:bg-white/[0.07]">
                    <input
                      type="text"
                      value={name}
                      onChange={(e) => setName(e.target.value)}
                      placeholder="Enter recurring name"
                      className="w-full cursor-pointer bg-transparent text-white outline-none placeholder:text-white/30"
                    />
                  </div>
                </div>

                <div>
                  <label className="mb-2 block text-sm text-white/65">
                    Amount
                  </label>
                  <div className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 shadow-[inset_0_1px_0_rgba(255,255,255,0.03)] transition focus-within:border-emerald-400/30 focus-within:bg-white/[0.07]">
                    <input
                      type="number"
                      value={amount}
                      onChange={(e) => setAmount(e.target.value)}
                      placeholder="Enter amount"
                      className="w-full cursor-pointer appearance-none bg-transparent text-white outline-none placeholder:text-white/30 [&::-webkit-inner-spin-button]:appearance-none [&::-webkit-outer-spin-button]:appearance-none"
                    />
                  </div>
                </div>

                <CustomSelect
                  label="Frequency"
                  value={frequencyOfTransaction}
                  onChange={(value) =>
                    setFrequencyOfTransaction(value as FrequencyOfTransaction)
                  }
                  options={frequencyOptions}
                  placeholder="Select frequency"
                  icon={Repeat}
                />

                <CustomSelect
                  label="Category"
                  value={categoryId}
                  onChange={setCategoryId}
                  options={categoryOptions}
                  placeholder="Select category"
                  icon={Shapes}
                />

                <CustomSelect
                  label="Payment Method"
                  value={paymentMethodId}
                  onChange={setPaymentMethodId}
                  options={paymentMethodOptions}
                  placeholder="Select payment method"
                  icon={CreditCard}
                />

                <div>
                  <label className="mb-2 block text-sm text-white/65">
                    Start Date
                  </label>
                  <div className="flex items-center gap-3 rounded-2xl border border-white/10 bg-white/5 px-4 py-3 shadow-[inset_0_1px_0_rgba(255,255,255,0.03)] transition focus-within:border-emerald-400/30 focus-within:bg-white/[0.07]">
                    <CalendarDays className="h-5 w-5 text-white/40" />
                    <input
                      type="date"
                      value={startDate}
                      onChange={(e) => setStartDate(e.target.value)}
                      className="w-full cursor-pointer bg-transparent text-white outline-none [color-scheme:dark]"
                    />
                  </div>
                </div>

                {error && (
                  <p className="text-sm text-red-400 md:col-span-2 xl:col-span-3">
                    {error}
                  </p>
                )}

                <div className="md:col-span-2 xl:col-span-3">
                  <div className="flex flex-col gap-3 sm:flex-row">
                    <button
                      type="submit"
                      disabled={loading}
                      className="w-full cursor-pointer rounded-2xl border border-emerald-400/20 bg-emerald-500/15 px-4 py-3 font-semibold text-emerald-300 shadow-[0_0_30px_rgba(16,185,129,0.15)] transition hover:bg-emerald-500/20 disabled:cursor-not-allowed disabled:opacity-50"
                    >
                      {loading
                        ? editingId
                          ? "Updating..."
                          : "Saving..."
                        : editingId
                        ? "Update Recurring Transaction"
                        : "Add Recurring Transaction"}
                    </button>

                    {editingId && (
                      <button
                        type="button"
                        onClick={resetForm}
                        className="w-full cursor-pointer rounded-2xl border border-white/10 bg-white/5 px-4 py-3 font-semibold text-white/75 transition hover:bg-white/10 hover:text-white sm:max-w-[180px]"
                      >
                        Cancel
                      </button>
                    )}
                  </div>
                </div>
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
                    Scheduled
                  </p>
                  <h2 className="mt-2 text-xl font-semibold text-white">
                    All Recurring Transactions
                  </h2>
                </div>

                <div className="flex h-11 w-11 items-center justify-center rounded-2xl border border-white/10 bg-white/5 text-white/70">
                  <List className="h-5 w-5" />
                </div>
              </div>

              {loadingRecurringTransactions ? (
                <p className="text-sm text-white/60">
                  Loading recurring transactions...
                </p>
              ) : recurringTransactions.length === 0 ? (
                <p className="text-sm text-white/60">
                  No recurring transactions yet.
                </p>
              ) : (
                <div className="h-[390px] overflow-y-scroll pr-2 [scrollbar-width:thin] [scrollbar-color:rgba(52,211,153,0.35)_transparent] [&::-webkit-scrollbar]:w-2 [&::-webkit-scrollbar-track]:bg-transparent [&::-webkit-scrollbar-thumb]:rounded-full [&::-webkit-scrollbar-thumb]:bg-white/10 hover:[&::-webkit-scrollbar-thumb]:bg-emerald-400/30">
                  <div className="flex flex-col gap-2.5">
                    {recurringTransactions.map((recurringTransaction) => {
                      const isEditingThis = editingId === recurringTransaction.id
                      const isMonthly =
                        recurringTransaction.frequencyOfTransaction
                          .toLowerCase() === "monthly"

                      return (
                        <div
                          key={recurringTransaction.id}
                          onClick={() => handleEdit(recurringTransaction)}
                          className={`cursor-pointer rounded-[22px] border px-4 py-3 shadow-[inset_0_1px_0_rgba(255,255,255,0.04)] transition ${
                            isEditingThis
                              ? "border-emerald-400/30 bg-emerald-500/[0.08]"
                              : "border-white/10 bg-white/[0.04] hover:border-white/15 hover:bg-white/[0.06]"
                          }`}
                        >
                          <div className="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
                            <div className="min-w-0 flex-1">
                              <div className="flex items-center gap-3">
                                <div
                                  className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-2xl border ${
                                    isMonthly
                                      ? "border-emerald-400/20 bg-emerald-500/10 text-emerald-300"
                                      : "border-blue-400/20 bg-blue-500/10 text-blue-300"
                                  }`}
                                >
                                  <Repeat className="h-4.5 w-4.5" />
                                </div>

                                <div className="min-w-0">
                                  <p className="truncate text-sm font-semibold text-white">
                                    {recurringTransaction.name}
                                  </p>

                                  <p className="mt-1 text-xs text-white/50">
                                    {recurringTransaction.categoryName} •{" "}
                                    {recurringTransaction.paymentMethodName}
                                  </p>

                                  <p className="mt-1 truncate text-xs text-white/40">
                                    {formatFrequency(
                                      recurringTransaction.frequencyOfTransaction
                                    )}{" "}
                                    recurring payment
                                  </p>
                                </div>
                              </div>
                            </div>

                            <div className="grid gap-3 text-sm text-white/65 sm:grid-cols-3 lg:min-w-[390px] lg:max-w-[490px] lg:flex-1">
                              <div>
                                <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                                  Start Date
                                </p>
                                <p className="mt-1 text-sm text-white/75">
                                  {new Date(
                                    recurringTransaction.startDate
                                  ).toLocaleDateString()}
                                </p>
                              </div>

                              <div>
                                <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                                  Frequency
                                </p>
                                <p
                                  className={`mt-1 text-sm font-medium ${
                                    isMonthly
                                      ? "text-emerald-300"
                                      : "text-blue-300"
                                  }`}
                                >
                                  {formatFrequency(
                                    recurringTransaction.frequencyOfTransaction
                                  )}
                                </p>
                              </div>

                              <div>
                                <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                                  Status
                                </p>
                                <p
                                  className={`mt-1 text-sm font-medium ${
                                    recurringTransaction.isActive
                                      ? "text-emerald-300"
                                      : "text-red-300"
                                  }`}
                                >
                                  {recurringTransaction.isActive
                                    ? "Active"
                                    : "Inactive"}
                                </p>
                              </div>
                            </div>

                            <div className="lg:min-w-[160px] lg:text-right">
                              <p className="text-[10px] uppercase tracking-[0.2em] text-white/35">
                                Amount
                              </p>
                              <p className="mt-1 text-base font-semibold text-emerald-300">
                                ${recurringTransaction.amount.toFixed(2)}
                              </p>

                              <div className="mt-3 flex items-center gap-2 lg:justify-end">
                                <button
                                  type="button"
                                  onClick={(e) => {
                                    e.stopPropagation()
                                    handleEdit(recurringTransaction)
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
                                    setDeleteId(recurringTransaction.id)
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
                    Delete recurring transaction?
                  </h3>
                </div>

                <div className="flex h-11 w-11 items-center justify-center rounded-2xl border border-red-400/20 bg-red-500/15 text-red-300 shadow-[0_0_30px_rgba(239,68,68,0.18)]">
                  <Trash2 className="h-5 w-5" />
                </div>
              </div>

              <p className="text-sm leading-6 text-white/60">
                This action cannot be undone. The selected recurring transaction
                will be permanently removed.
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