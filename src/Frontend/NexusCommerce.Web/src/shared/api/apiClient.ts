import axios, {
  AxiosError,
  type InternalAxiosRequestConfig,
} from 'axios'
import type { AuthSession } from '../../features/auth/types/auth'
import { useAuthStore } from '../../features/auth/store/authStore'

const apiUrl = import.meta.env.VITE_API_URL

if (!apiUrl) {
  throw new Error('La variable VITE_API_URL no está configurada')
}

type RetryRequestConfig =
  InternalAxiosRequestConfig & {
    _retry?: boolean
  }

export const apiClient = axios.create({
  baseURL: apiUrl,
  timeout: 10_000,
  headers: {
    'Content-Type': 'application/json',
  },
})

const refreshClient = axios.create({
  baseURL: apiUrl,
  timeout: 10_000,
  headers: {
    'Content-Type': 'application/json',
  },
})

let refreshPromise: Promise<AuthSession> | null = null

apiClient.interceptors.request.use((config) => {
  const accessToken =
    useAuthStore.getState().accessToken

  if (accessToken) {
    config.headers.Authorization =
      `Bearer ${accessToken}`
  }

  return config
})

apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest =
      error.config as RetryRequestConfig | undefined

    const isUnauthorized =
      error.response?.status === 401

    const isRefreshRequest =
      originalRequest?.url?.includes(
        '/api/autenticacion/refresh',
      )

    if (
      !isUnauthorized ||
      !originalRequest ||
      originalRequest._retry ||
      isRefreshRequest
    ) {
      return Promise.reject(error)
    }

    const authStore = useAuthStore.getState()
    const refreshToken =
      authStore.refreshToken

    if (!refreshToken) {
      authStore.limpiarSesion()
      window.location.href = '/acceso'

      return Promise.reject(error)
    }

    originalRequest._retry = true

    try {
      if (!refreshPromise) {
        refreshPromise = refreshClient
          .post<AuthSession>(
            '/api/autenticacion/refresh',
            {
              refreshToken,
            },
          )
          .then((response) => response.data)
          .finally(() => {
            refreshPromise = null
          })
      }

      const nuevaSesion =
        await refreshPromise

      useAuthStore
        .getState()
        .establecerSesion(nuevaSesion)

      originalRequest.headers.Authorization =
        `Bearer ${nuevaSesion.accessToken}`

      return apiClient(originalRequest)
    } catch (refreshError) {
      useAuthStore
        .getState()
        .limpiarSesion()

      window.location.href = '/acceso'

      return Promise.reject(refreshError)
    }
  },
)
