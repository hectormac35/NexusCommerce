import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { AuthSession, AuthUser } from '../types/auth'

type AuthState = {
  accessToken: string | null
  refreshToken: string | null
  usuario: AuthUser | null
  estaAutenticado: boolean
  establecerSesion: (session: AuthSession) => void
  limpiarSesion: () => void
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      refreshToken: null,
      usuario: null,
      estaAutenticado: false,

      establecerSesion: (session) =>
        set({
          accessToken: session.accessToken,
          refreshToken: session.refreshToken,
          usuario: session.usuario,
          estaAutenticado: true,
        }),

      limpiarSesion: () =>
        set({
          accessToken: null,
          refreshToken: null,
          usuario: null,
          estaAutenticado: false,
        }),
    }),
    {
      name: 'nexuscommerce-auth',
    },
  ),
)
