import axios from 'axios'
import { useAuthStore } from '../../features/auth/store/authStore'

const apiUrl = import.meta.env.VITE_API_URL

if (!apiUrl) {
  throw new Error('La variable VITE_API_URL no está configurada')
}

export const apiClient = axios.create({
  baseURL: apiUrl,
  timeout: 10_000,
  headers: {
    'Content-Type': 'application/json',
  },
})

apiClient.interceptors.request.use((config) => {
  const accessToken = useAuthStore.getState().accessToken

  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`
  }

  return config
})
