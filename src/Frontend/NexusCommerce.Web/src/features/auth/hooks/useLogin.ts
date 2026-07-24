import { useMutation } from '@tanstack/react-query'
import { login } from '../api/authApi'
import { useAuthStore } from '../store/authStore'

export function useLogin() {
  const establecerSesion = useAuthStore(
    (state) => state.establecerSesion,
  )

  return useMutation({
    mutationFn: login,
    onSuccess: establecerSesion,
  })
}
