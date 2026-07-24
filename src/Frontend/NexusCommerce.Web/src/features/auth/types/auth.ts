export type AuthUser = {
  id: string
  nombre: string
  apellidos: string
  correo: string
  rol: string
}

export type LoginRequest = {
  correo: string
  contrasena: string
}

export type AuthSession = {
  accessToken: string
  refreshToken: string
  expiraEnSegundos: number
  usuario: AuthUser
}
