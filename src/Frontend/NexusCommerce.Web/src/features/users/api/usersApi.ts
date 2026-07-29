import { apiClient } from '../../../shared/api/apiClient'
import type { User, UserRole } from '../types/user'

export async function getUsers(): Promise<User[]> {
  const response = await apiClient.get<User[]>('/api/usuarios')
  return response.data
}

export async function changeUserRole(
  userId: string,
  newRole: UserRole,
): Promise<void> {
  await apiClient.patch(
    `/api/usuarios/${userId}/rol`,
    {
      nuevoRol: newRole,
    },
  )
}
