export type UserRole =
  | 'Administrador'
  | 'Empleado'
  | 'Cliente'
  | string

export type UserStatus =
  | 'Activo'
  | 'Bloqueado'
  | 'Desactivado'
  | string

export interface User {
  id: string
  nombre: string
  apellidos: string
  nombreCompleto: string
  correo: string
  rol: UserRole
  estado: UserStatus
  estaActivo: boolean
  fechaCreacionUtc: string
  fechaActualizacionUtc: string | null
}
