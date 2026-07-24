import { zodResolver } from '@hookform/resolvers/zod'
import { AxiosError } from 'axios'
import { useForm } from 'react-hook-form'
import { useNavigate } from 'react-router-dom'
import { useLogin } from '../hooks/useLogin'
import {
  loginSchema,
  type LoginFormValues,
} from '../schemas/loginSchema'

type ApiProblem = {
  title?: string
  detail?: string
}

export function LoginPage() {
  const navigate = useNavigate()
  const loginMutation = useLogin()

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      correo: '',
      contrasena: '',
    },
  })

  async function onSubmit(values: LoginFormValues) {
    await loginMutation.mutateAsync(values)
    navigate('/')
  }

  const apiError =
    loginMutation.error instanceof AxiosError
      ? (loginMutation.error.response?.data as ApiProblem | undefined)
      : undefined

  return (
    <section className="mx-auto max-w-md">
      <div className="rounded-2xl border border-slate-800 bg-slate-900 p-8 shadow-2xl">
        <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
          Área privada
        </p>

        <h1 className="mt-2 text-3xl font-bold">Iniciar sesión</h1>

        <p className="mt-3 text-sm text-slate-400">
          Accede mediante el microservicio de identidad de NexusCommerce.
        </p>

        <form
          className="mt-8 space-y-5"
          onSubmit={handleSubmit(onSubmit)}
          noValidate
        >
          <div>
            <label
              htmlFor="correo"
              className="text-sm font-medium text-slate-300"
            >
              Correo electrónico
            </label>

            <input
              id="correo"
              type="email"
              autoComplete="email"
              placeholder="nombre@correo.com"
              {...register('correo')}
              className="mt-2 w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 outline-none transition placeholder:text-slate-600 focus:border-blue-500"
            />

            {errors.correo && (
              <p className="mt-2 text-sm text-red-400">
                {errors.correo.message}
              </p>
            )}
          </div>

          <div>
            <label
              htmlFor="contrasena"
              className="text-sm font-medium text-slate-300"
            >
              Contraseña
            </label>

            <input
              id="contrasena"
              type="password"
              autoComplete="current-password"
              placeholder="••••••••"
              {...register('contrasena')}
              className="mt-2 w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 outline-none transition placeholder:text-slate-600 focus:border-blue-500"
            />

            {errors.contrasena && (
              <p className="mt-2 text-sm text-red-400">
                {errors.contrasena.message}
              </p>
            )}
          </div>

          {loginMutation.isError && (
            <div className="rounded-lg border border-red-500/30 bg-red-500/10 p-4">
              <p className="text-sm font-semibold text-red-300">
                No se pudo iniciar sesión
              </p>

              <p className="mt-1 text-sm text-red-400">
                {apiError?.detail ??
                  'Comprueba el correo y la contraseña.'}
              </p>
            </div>
          )}

          <button
            type="submit"
            disabled={loginMutation.isPending}
            className="w-full rounded-lg bg-blue-600 px-5 py-3 font-semibold transition hover:bg-blue-500 disabled:cursor-not-allowed disabled:opacity-60"
          >
            {loginMutation.isPending
              ? 'Iniciando sesión...'
              : 'Acceder'}
          </button>
        </form>

        <div className="mt-6 rounded-lg border border-slate-800 bg-slate-950 p-4 text-xs text-slate-400">
          <p className="font-semibold text-slate-300">
            Usuario de demostración
          </p>
          <p className="mt-2">admin@nexuscommerce.local</p>
          <p>NexusCommerce123!</p>
        </div>
      </div>
    </section>
  )
}
