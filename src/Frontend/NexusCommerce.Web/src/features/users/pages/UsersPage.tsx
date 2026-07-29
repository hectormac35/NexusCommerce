import { useMemo, useState } from 'react'
import {
  ShieldCheck,
  UserRoundCheck,
  UsersRound,
} from 'lucide-react'
import { Card } from '../../../shared/ui/card/Card'
import { Badge } from '../../../shared/ui/badge/Badge'
import { ChangeRoleDialog } from '../components/ChangeRoleDialog'
import { UserFilters } from '../components/UserFilters'
import { UserSearch } from '../components/UserSearch'
import { UserTable } from '../components/UserTable'
import { useChangeUserRole } from '../hooks/useChangeUserRole'
import { useUsers } from '../hooks/useUsers'
import type { User, UserRole } from '../types/user'

export function UsersPage() {
  const {
    data: users = [],
    isLoading,
    isError,
  } = useUsers()

  const changeUserRoleMutation = useChangeUserRole()

  const [search, setSearch] = useState('')
  const [role, setRole] = useState('')
  const [status, setStatus] = useState('')
  const [selectedUser, setSelectedUser] =
    useState<User | null>(null)

  const roles = useMemo(
    () =>
      [...new Set(users.map((user) => user.rol))].sort(),
    [users],
  )

  const statuses = useMemo(
    () =>
      [...new Set(users.map((user) => user.estado))].sort(),
    [users],
  )

  const filteredUsers = useMemo(() => {
    const normalizedSearch = search
      .trim()
      .toLowerCase()

    return users.filter((user) => {
      const matchesSearch =
        normalizedSearch.length === 0 ||
        user.nombreCompleto
          .toLowerCase()
          .includes(normalizedSearch) ||
        user.correo
          .toLowerCase()
          .includes(normalizedSearch)

      const matchesRole =
        role.length === 0 || user.rol === role

      const matchesStatus =
        status.length === 0 || user.estado === status

      return (
        matchesSearch &&
        matchesRole &&
        matchesStatus
      )
    })
  }, [users, search, role, status])

  const activeUsers = users.filter(
    (user) => user.estado === 'Activo',
  ).length

  const administratorUsers = users.filter(
    (user) => user.rol === 'Administrador',
  ).length

  function handleOpenChangeRole(user: User) {
    changeUserRoleMutation.reset()
    setSelectedUser(user)
  }

  function handleCloseChangeRole() {
    if (changeUserRoleMutation.isPending) {
      return
    }

    changeUserRoleMutation.reset()
    setSelectedUser(null)
  }

  function handleConfirmChangeRole(
    userId: string,
    newRole: UserRole,
  ) {
    changeUserRoleMutation.mutate(
      {
        userId,
        newRole,
      },
      {
        onSuccess: () => {
          setSelectedUser(null)
        },
      },
    )
  }

  return (
    <>
      <div className="space-y-6">
        <section className="flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
          <div>
            <div className="flex items-center gap-2">
              <UsersRound
                size={22}
                className="text-blue-400"
              />

              <h1 className="text-2xl font-bold text-white">
                Gestión de usuarios
              </h1>
            </div>

            <p className="mt-2 text-sm text-slate-500">
              Consulta y administra los usuarios de
              NexusCommerce.
            </p>
          </div>

          <Badge variant="primary">
            {users.length} usuarios
          </Badge>
        </section>

        <section className="grid gap-4 md:grid-cols-3">
          <Card className="p-5">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-slate-500">
                  Usuarios totales
                </p>

                <p className="mt-2 text-2xl font-bold text-white">
                  {users.length}
                </p>
              </div>

              <UsersRound className="text-blue-400" />
            </div>
          </Card>

          <Card className="p-5">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-slate-500">
                  Usuarios activos
                </p>

                <p className="mt-2 text-2xl font-bold text-white">
                  {activeUsers}
                </p>
              </div>

              <UserRoundCheck className="text-emerald-400" />
            </div>
          </Card>

          <Card className="p-5">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-slate-500">
                  Administradores
                </p>

                <p className="mt-2 text-2xl font-bold text-white">
                  {administratorUsers}
                </p>
              </div>

              <ShieldCheck className="text-violet-400" />
            </div>
          </Card>
        </section>

        <Card className="overflow-hidden">
          <div className="flex flex-col gap-4 border-b border-slate-800 p-5 lg:flex-row lg:items-center">
            <UserSearch
              value={search}
              onChange={setSearch}
            />

            <UserFilters
              role={role}
              status={status}
              roles={roles}
              statuses={statuses}
              onRoleChange={setRole}
              onStatusChange={setStatus}
            />
          </div>

          {isLoading && (
            <div className="flex min-h-80 items-center justify-center">
              <p className="text-sm text-slate-500">
                Cargando usuarios...
              </p>
            </div>
          )}

          {isError && (
            <div className="flex min-h-80 items-center justify-center">
              <p className="text-sm text-red-300">
                No se ha podido obtener el listado de
                usuarios.
              </p>
            </div>
          )}

          {!isLoading &&
            !isError &&
            filteredUsers.length === 0 && (
              <div className="flex min-h-80 items-center justify-center">
                <p className="text-sm text-slate-500">
                  No hay usuarios que coincidan con los
                  filtros.
                </p>
              </div>
            )}

          {!isLoading &&
            !isError &&
            filteredUsers.length > 0 && (
              <UserTable
                users={filteredUsers}
                onChangeRole={handleOpenChangeRole}
              />
            )}

          {!isLoading && !isError && (
            <div className="border-t border-slate-800 px-6 py-4 text-sm text-slate-500">
              Mostrando {filteredUsers.length} de{' '}
              {users.length} usuarios
            </div>
          )}
        </Card>
      </div>

      <ChangeRoleDialog
        user={selectedUser}
        isOpen={selectedUser !== null}
        isPending={changeUserRoleMutation.isPending}
        errorMessage={
          changeUserRoleMutation.isError
            ? 'No se ha podido cambiar el rol. Comprueba la conexión y vuelve a intentarlo.'
            : undefined
        }
        onClose={handleCloseChangeRole}
        onConfirm={handleConfirmChangeRole}
      />
    </>
  )
}
