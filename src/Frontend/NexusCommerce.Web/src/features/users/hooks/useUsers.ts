import { useQuery } from '@tanstack/react-query'
import { getUsers } from '../api/usersApi'

/** Solo los administradores pueden listar usuarios; para el resto la consulta no se lanza. */
export function useUsers(habilitado = true) {
  return useQuery({
    queryKey: ['users'],
    queryFn: getUsers,
    enabled: habilitado,
  })
}
