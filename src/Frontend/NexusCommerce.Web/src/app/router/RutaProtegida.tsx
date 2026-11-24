import { Navigate, Outlet, useLocation } from 'react-router-dom'
import { useAuthStore } from '../../features/auth/store/authStore'

/**
 * Deja pasar solo a usuarios con sesión. El resto va a /acceso y,
 * después de iniciar sesión, vuelve a la página que quería ver.
 */
export function RutaProtegida() {
  const estaAutenticado = useAuthStore((estado) => estado.estaAutenticado)
  const location = useLocation()

  if (!estaAutenticado) {
    return (
      <Navigate
        to="/acceso"
        replace
        state={{ desde: location.pathname + location.search }}
      />
    )
  }

  return <Outlet />
}
