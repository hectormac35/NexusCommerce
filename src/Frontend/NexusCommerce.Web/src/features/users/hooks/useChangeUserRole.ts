import {
  useMutation,
  useQueryClient,
} from '@tanstack/react-query'
import { changeUserRole } from '../api/usersApi'
import type { UserRole } from '../types/user'

type ChangeUserRoleVariables = {
  userId: string
  newRole: UserRole
}

export function useChangeUserRole() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({
      userId,
      newRole,
    }: ChangeUserRoleVariables) =>
      changeUserRole(userId, newRole),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['users'],
      })
    },
  })
}
