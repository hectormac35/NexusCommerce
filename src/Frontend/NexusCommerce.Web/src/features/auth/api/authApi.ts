import { apiClient } from '../../../shared/api/apiClient'
import type { AuthSession, LoginRequest, RegistroRequest } from '../types/auth'

export async function login(
  credentials: LoginRequest,
): Promise<AuthSession> {
  const response = await apiClient.post<AuthSession>(
    '/api/autenticacion/login',
    credentials,
  )

  return response.data
}

/** Crea una cuenta nueva. Identity.Api siempre la crea con el rol Cliente. */
export async function registrar(datos: RegistroRequest): Promise<void> {
  await apiClient.post('/api/autenticacion/registro', datos)
}
