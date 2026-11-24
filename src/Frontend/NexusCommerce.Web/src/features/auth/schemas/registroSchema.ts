import { z } from 'zod'

// Mismas reglas que valida Identity.Api, para avisar antes de enviar.
export const registroSchema = z
  .object({
    nombre: z.string().trim().min(1, 'El nombre es obligatorio').max(100),
    apellidos: z.string().trim().min(1, 'Los apellidos son obligatorios').max(150),
    correo: z.string().trim().min(1, 'El correo es obligatorio').email('Introduce un correo válido'),
    contrasena: z
      .string()
      .min(10, 'Mínimo 10 caracteres')
      .regex(/[A-Z]/, 'Debe incluir una mayúscula')
      .regex(/[a-z]/, 'Debe incluir una minúscula')
      .regex(/[0-9]/, 'Debe incluir un número')
      .regex(/[^a-zA-Z0-9]/, 'Debe incluir un carácter especial'),
    confirmacionContrasena: z.string().min(1, 'Repite la contraseña'),
  })
  .refine((datos) => datos.contrasena === datos.confirmacionContrasena, {
    path: ['confirmacionContrasena'],
    message: 'Las contraseñas no coinciden',
  })

export type RegistroFormValues = z.infer<typeof registroSchema>
