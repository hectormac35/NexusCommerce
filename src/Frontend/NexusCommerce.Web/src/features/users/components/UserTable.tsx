import type { User } from '../types/user'
import { UserRow } from './UserRow'

type UserTableProps = {
  users: User[]
  onChangeRole: (user: User) => void
}

export function UserTable({
  users,
  onChangeRole,
}: UserTableProps) {
  return (
    <div className="overflow-x-auto">
      <table className="w-full min-w-[760px]">
        <thead>
          <tr className="text-left text-xs uppercase tracking-wider text-slate-500">
            <th className="px-6 py-4 font-medium">
              Usuario
            </th>

            <th className="px-6 py-4 font-medium">
              Rol
            </th>

            <th className="px-6 py-4 font-medium">
              Estado
            </th>

            <th className="px-6 py-4 font-medium">
              Registro
            </th>

            <th className="px-6 py-4 text-right font-medium">
              Acciones
            </th>
          </tr>
        </thead>

        <tbody>
          {users.map((user) => (
            <UserRow
              key={user.id}
              user={user}
              onChangeRole={onChangeRole}
            />
          ))}
        </tbody>
      </table>
    </div>
  )
}
