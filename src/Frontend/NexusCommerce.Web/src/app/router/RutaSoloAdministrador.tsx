import { Navigate, Outlet } from 'react-router-dom'
import { useAuthStore } from '../../features/auth/store/authStore'

/** Páginas que solo tienen sentido para administradores (la API también lo exige). */
export function RutaSoloAdministrador() {
  const rol = useAuthStore((estado) => estado.usuario?.rol)

  return rol === 'Administrador' ? <Outlet /> : <Navigate to="/" replace />
}
