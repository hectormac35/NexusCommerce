import { useMutation } from '@tanstack/react-query'
import { login, registrar } from '../api/authApi'
import { useAuthStore } from '../store/authStore'
import type { RegistroRequest } from '../types/auth'

/** Registra la cuenta y, si todo va bien, inicia sesión directamente. */
export function useRegistro() {
  const establecerSesion = useAuthStore((state) => state.establecerSesion)

  return useMutation({
    mutationFn: async (datos: RegistroRequest) => {
      await registrar(datos)
      return login({ correo: datos.correo, contrasena: datos.contrasena })
    },
    onSuccess: establecerSesion,
  })
}
