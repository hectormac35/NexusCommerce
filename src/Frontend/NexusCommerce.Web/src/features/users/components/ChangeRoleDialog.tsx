import { useEffect, useState } from 'react'
import { X } from 'lucide-react'
import type {
  User,
  UserRole,
} from '../types/user'

type ChangeRoleDialogProps = {
  user: User | null
  isOpen: boolean
  isPending: boolean
  errorMessage?: string
  onClose: () => void
  onConfirm: (
    userId: string,
    newRole: UserRole,
  ) => void
}

const roles: UserRole[] = [
  'Cliente',
  'Empleado',
  'Administrador',
]

export function ChangeRoleDialog({
  user,
  isOpen,
  isPending,
  errorMessage,
  onClose,
  onConfirm,
}: ChangeRoleDialogProps) {
  const [selectedRole, setSelectedRole] =
    useState<UserRole>('Cliente')

  useEffect(() => {
    if (user) {
      setSelectedRole(user.rol)
    }
  }, [user])

  if (!isOpen || !user) {
    return null
  }

  const hasChanged = selectedRole !== user.rol

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 px-4 backdrop-blur-sm">
      <div className="w-full max-w-md rounded-2xl border border-slate-800 bg-slate-900 shadow-2xl">
        <div className="flex items-center justify-between border-b border-slate-800 px-6 py-5">
          <div>
            <h2 className="text-lg font-semibold text-white">
              Cambiar rol
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              {user.nombreCompleto}
            </p>
          </div>

          <button
            type="button"
            onClick={onClose}
            disabled={isPending}
            aria-label="Cerrar"
            className="rounded-lg p-2 text-slate-500 transition hover:bg-slate-800 hover:text-white disabled:cursor-not-allowed disabled:opacity-50"
          >
            <X size={18} />
          </button>
        </div>

        <div className="space-y-4 px-6 py-5">
          <div>
            <label
              htmlFor="user-role"
              className="mb-2 block text-sm font-medium text-slate-300"
            >
              Nuevo rol
            </label>

            <select
              id="user-role"
              value={selectedRole}
              disabled={isPending}
              onChange={(event) =>
                setSelectedRole(
                  event.target.value as UserRole,
                )
              }
              className="h-11 w-full rounded-xl border border-slate-700 bg-slate-950 px-4 text-sm text-white outline-none transition focus:border-blue-500/60 disabled:cursor-not-allowed disabled:opacity-60"
            >
              {roles.map((role) => (
                <option key={role} value={role}>
                  {role}
                </option>
              ))}
            </select>
          </div>

          {errorMessage && (
            <p className="rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-sm text-red-300">
              {errorMessage}
            </p>
          )}
        </div>

        <div className="flex justify-end gap-3 border-t border-slate-800 px-6 py-4">
          <button
            type="button"
            onClick={onClose}
            disabled={isPending}
            className="h-10 rounded-xl border border-slate-700 px-4 text-sm font-medium text-slate-300 transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-50"
          >
            Cancelar
          </button>

          <button
            type="button"
            disabled={!hasChanged || isPending}
            onClick={() =>
              onConfirm(user.id, selectedRole)
            }
            className="h-10 rounded-xl bg-blue-600 px-4 text-sm font-semibold text-white transition hover:bg-blue-500 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {isPending
              ? 'Guardando...'
              : 'Guardar cambios'}
          </button>
        </div>
      </div>
    </div>
  )
}
