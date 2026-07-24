import { z } from 'zod'

export const loginSchema = z.object({
  correo: z
    .string()
    .min(1, 'El correo es obligatorio')
    .email('Introduce un correo válido'),

  contrasena: z
    .string()
    .min(1, 'La contraseña es obligatoria')
    .min(8, 'La contraseña debe tener al menos 8 caracteres'),
})

export type LoginFormValues = z.infer<typeof loginSchema>
