import { zodResolver } from '@hookform/resolvers/zod'
import { AxiosError } from 'axios'
import { useForm } from 'react-hook-form'
import { Link, useNavigate } from 'react-router-dom'
import { useRegistro } from '../hooks/useRegistro'
import { registroSchema, type RegistroFormValues } from '../schemas/registroSchema'

type ApiProblem = {
  title?: string
  detail?: string
  errors?: Record<string, string[]>
}

const campos: {
  nombre: keyof RegistroFormValues
  etiqueta: string
  tipo: string
  autoComplete: string
  placeholder: string
}[] = [
  { nombre: 'nombre', etiqueta: 'Nombre', tipo: 'text', autoComplete: 'given-name', placeholder: 'Ana' },
  { nombre: 'apellidos', etiqueta: 'Apellidos', tipo: 'text', autoComplete: 'family-name', placeholder: 'García López' },
  { nombre: 'correo', etiqueta: 'Correo electrónico', tipo: 'email', autoComplete: 'email', placeholder: 'nombre@correo.com' },
  { nombre: 'contrasena', etiqueta: 'Contraseña', tipo: 'password', autoComplete: 'new-password', placeholder: '••••••••••' },
  { nombre: 'confirmacionContrasena', etiqueta: 'Repite la contraseña', tipo: 'password', autoComplete: 'new-password', placeholder: '••••••••••' },
]

function mensajeDeError(error: unknown): string {
  if (!(error instanceof AxiosError)) {
    return 'No se pudo conectar con el servidor.'
  }

  const problema = error.response?.data as ApiProblem | undefined

  if (error.response?.status === 409) {
    return 'Ya existe una cuenta con ese correo.'
  }

  const primerError = problema?.errors && Object.values(problema.errors).flat()[0]

  return primerError ?? problema?.detail ?? 'Revisa los datos e inténtalo de nuevo.'
}

export function RegistroPage() {
  const navigate = useNavigate()
  const registro = useRegistro()

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegistroFormValues>({
    resolver: zodResolver(registroSchema),
    defaultValues: {
      nombre: '',
      apellidos: '',
      correo: '',
      contrasena: '',
      confirmacionContrasena: '',
    },
  })

  async function onSubmit(valores: RegistroFormValues) {
    await registro.mutateAsync(valores)
    navigate('/', { replace: true })
  }

  return (
    <section className="mx-auto w-full max-w-md">
      <div className="rounded-2xl border border-slate-800 bg-slate-900 p-8 shadow-2xl">
        <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
          Nueva cuenta
        </p>

        <h1 className="mt-2 text-3xl font-bold">Crear cuenta</h1>

        <p className="mt-3 text-sm text-slate-400">
          Las cuentas nuevas empiezan con el rol <strong className="text-slate-200">Cliente</strong>.
          Un administrador puede cambiarlo después desde Usuarios.
        </p>

        <form className="mt-8 space-y-4" onSubmit={handleSubmit(onSubmit)} noValidate>
          {campos.map((campo) => (
            <div key={campo.nombre}>
              <label htmlFor={campo.nombre} className="text-sm font-medium text-slate-300">
                {campo.etiqueta}
              </label>

              <input
                id={campo.nombre}
                type={campo.tipo}
                autoComplete={campo.autoComplete}
                placeholder={campo.placeholder}
                {...register(campo.nombre)}
                className="mt-2 w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 outline-none transition placeholder:text-slate-600 focus:border-blue-500"
              />

              {errors[campo.nombre] && (
                <p className="mt-2 text-sm text-red-400">{errors[campo.nombre]?.message}</p>
              )}
            </div>
          ))}

          <p className="text-xs text-slate-500">
            Mínimo 10 caracteres, con mayúscula, minúscula, número y un carácter especial.
          </p>

          {registro.isError && (
            <div className="rounded-lg border border-red-500/30 bg-red-500/10 p-4">
              <p className="text-sm font-semibold text-red-300">No se pudo crear la cuenta</p>
              <p className="mt-1 text-sm text-red-400">{mensajeDeError(registro.error)}</p>
            </div>
          )}

          <button
            type="submit"
            disabled={registro.isPending}
            className="w-full rounded-lg bg-blue-600 px-5 py-3 font-semibold transition hover:bg-blue-500 disabled:cursor-not-allowed disabled:opacity-60"
          >
            {registro.isPending ? 'Creando cuenta...' : 'Crear cuenta'}
          </button>
        </form>

        <p className="mt-6 text-center text-sm text-slate-400">
          ¿Ya tienes cuenta?{' '}
          <Link to="/acceso" className="font-semibold text-blue-400 hover:text-blue-300">
            Inicia sesión
          </Link>
        </p>
      </div>
    </section>
  )
}
