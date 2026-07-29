type UserFiltersProps = {
  role: string
  status: string
  roles: string[]
  statuses: string[]
  onRoleChange: (value: string) => void
  onStatusChange: (value: string) => void
}

export function UserFilters({
  role,
  status,
  roles,
  statuses,
  onRoleChange,
  onStatusChange,
}: UserFiltersProps) {
  const selectClassName =
    'h-11 rounded-xl border border-slate-800 bg-slate-950 px-4 text-sm text-slate-300 outline-none transition focus:border-blue-500/60'

  return (
    <div className="flex flex-col gap-3 sm:flex-row">
      <select
        value={role}
        onChange={(event) => onRoleChange(event.target.value)}
        className={selectClassName}
      >
        <option value="">Todos los roles</option>

        {roles.map((currentRole) => (
          <option key={currentRole} value={currentRole}>
            {currentRole}
          </option>
        ))}
      </select>

      <select
        value={status}
        onChange={(event) => onStatusChange(event.target.value)}
        className={selectClassName}
      >
        <option value="">Todos los estados</option>

        {statuses.map((currentStatus) => (
          <option key={currentStatus} value={currentStatus}>
            {currentStatus}
          </option>
        ))}
      </select>
    </div>
  )
}
