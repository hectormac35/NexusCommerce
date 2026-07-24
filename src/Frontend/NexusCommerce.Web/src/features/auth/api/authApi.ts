import { apiClient } from '../../../shared/api/apiClient'
import type { AuthSession, LoginRequest } from '../types/auth'

export async function login(
  credentials: LoginRequest,
): Promise<AuthSession> {
  const response = await apiClient.post<AuthSession>(
    '/api/autenticacion/login',
    credentials,
  )

  return response.data
}
