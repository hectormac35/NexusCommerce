import { UserRound } from 'lucide-react'
import { Badge } from '../../../shared/ui/badge/Badge'
import type { User } from '../types/user'
import { UserActionsMenu } from './UserActionsMenu'

type UserRowProps = {
  user: User
  onChangeRole: (user: User) => void
}

function getStatusVariant(
  status: string,
): 'success' | 'warning' | 'danger' {
  if (status === 'Activo') {
    return 'success'
  }

  if (status === 'Bloqueado') {
    return 'warning'
  }

  return 'danger'
}

function getRoleVariant(
  role: string,
): 'primary' | 'warning' | 'neutral' {
  if (role === 'Administrador') {
    return 'primary'
  }

  if (role === 'Empleado') {
    return 'warning'
  }

  return 'neutral'
}

export function UserRow({
  user,
  onChangeRole,
}: UserRowProps) {
  return (
    <tr className="border-t border-slate-800 transition hover:bg-slate-800/30">
      <td className="px-6 py-4">
        <div className="flex min-w-0 items-center gap-3">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl border border-slate-700 bg-slate-800 text-slate-300">
            <UserRound size={18} />
          </div>

          <div className="min-w-0">
            <p className="truncate font-medium text-white">
              {user.nombreCompleto}
            </p>

            <p className="mt-1 truncate text-xs text-slate-500">
              {user.correo}
            </p>
          </div>
        </div>
      </td>

      <td className="px-6 py-4">
        <Badge variant={getRoleVariant(user.rol)}>
          {user.rol}
        </Badge>
      </td>

      <td className="px-6 py-4">
        <Badge variant={getStatusVariant(user.estado)}>
          {user.estado}
        </Badge>
      </td>

      <td className="px-6 py-4 text-sm text-slate-400">
        {new Date(user.fechaCreacionUtc).toLocaleDateString(
          'es-ES',
        )}
      </td>

      <td className="px-6 py-4 text-right">
        <UserActionsMenu
          user={user}
          onChangeRole={onChangeRole}
        />
      </td>
    </tr>
  )
}
