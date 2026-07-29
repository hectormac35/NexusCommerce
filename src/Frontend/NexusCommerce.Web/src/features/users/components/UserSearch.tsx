import { Search } from 'lucide-react'

type UserSearchProps = {
  value: string
  onChange: (value: string) => void
}

export function UserSearch({
  value,
  onChange,
}: UserSearchProps) {
  return (
    <div className="relative flex-1">
      <Search
        size={18}
        className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-slate-500"
      />

      <input
        type="search"
        value={value}
        onChange={(event) => onChange(event.target.value)}
        placeholder="Buscar por nombre o correo..."
        className="h-11 w-full rounded-xl border border-slate-800 bg-slate-950 pl-10 pr-4 text-sm text-white outline-none transition placeholder:text-slate-600 focus:border-blue-500/60"
      />
    </div>
  )
}
