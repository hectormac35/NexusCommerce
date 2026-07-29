import {
  Activity,
  CheckCircle2,
  Clock3,
  Server,
  ShieldCheck,
} from 'lucide-react'

import { Card } from '../../../shared/ui/card/Card'
import { ServiceCard } from '../components/ServiceCard'
import { usePlatformHealth } from '../hooks/usePlatformHealth'

const serviceDescriptions: Record<string, string> = {
  Gateway: 'Punto de entrada y enrutamiento mediante YARP',
  Identity: 'Autenticación, autorización y gestión de usuarios',
  Catalog: 'Gestión de productos e inventario',
}

export function PlatformHealthPage() {
  const {
    data: platformHealth,
    isLoading,
    isError,
    dataUpdatedAt,
    refetch,
    isFetching,
  } = usePlatformHealth()

  const services = platformHealth?.services ?? []
  const healthyServices = services.filter(
    (service) => service.status === 'Healthy',
  ).length

  const availability =
    services.length > 0
      ? Math.round((healthyServices / services.length) * 100)
      : 0

  const checkedAt = platformHealth?.checkedAt
    ? new Date(platformHealth.checkedAt).toLocaleTimeString('es-ES')
    : 'Sin comprobar'

  const lastUpdated = dataUpdatedAt
    ? new Date(dataUpdatedAt).toLocaleTimeString('es-ES')
    : 'Sin actualizar'

  if (isLoading) {
    return (
      <div className="flex min-h-[60vh] items-center justify-center">
        <div className="text-center">
          <Activity
            size={32}
            className="mx-auto animate-pulse text-blue-400"
          />

          <p className="mt-4 text-sm text-slate-400">
            Comprobando el estado de la plataforma...
          </p>
        </div>
      </div>
    )
  }

  if (isError) {
    return (
      <div className="mx-auto max-w-3xl">
        <Card className="border-red-500/20 p-8 text-center">
          <p className="font-semibold text-red-300">
            No se ha podido obtener el estado de la plataforma.
          </p>

          <button
            type="button"
            onClick={() => refetch()}
            className="mt-5 rounded-xl border border-red-500/30 bg-red-500/10 px-4 py-2 text-sm font-semibold text-red-200 transition hover:bg-red-500/20"
          >
            Reintentar
          </button>
        </Card>
      </div>
    )
  }

  return (
    <div className="mx-auto max-w-7xl space-y-8">
      <header className="flex flex-col gap-5 lg:flex-row lg:items-end lg:justify-between">
        <div>
          <div className="flex items-center gap-2">
            <span className="h-2 w-2 rounded-full bg-emerald-400 shadow-[0_0_12px_rgba(52,211,153,0.6)]" />

            <p className="text-sm font-semibold text-emerald-400">
              Observabilidad
            </p>
          </div>

          <h1 className="mt-3 text-3xl font-bold tracking-tight text-white sm:text-4xl">
            Platform Health
          </h1>

          <p className="mt-2 max-w-2xl text-slate-400">
            Supervisa la disponibilidad y el tiempo de respuesta de los
            servicios principales de NexusCommerce.
          </p>
        </div>

        <button
          type="button"
          onClick={() => refetch()}
          disabled={isFetching}
          className="inline-flex items-center justify-center gap-2 self-start rounded-xl border border-slate-700 bg-slate-900/70 px-4 py-2.5 text-sm font-semibold text-slate-200 transition hover:border-blue-500/40 hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60 lg:self-auto"
        >
          <Activity
            size={17}
            className={isFetching ? 'animate-spin' : ''}
          />

          {isFetching ? 'Actualizando...' : 'Actualizar ahora'}
        </button>
      </header>

      <section className="grid gap-5 sm:grid-cols-2 xl:grid-cols-4">
        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">Estado general</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {platformHealth?.status ?? 'Desconocido'}
              </p>
            </div>

            <div className="rounded-xl border border-emerald-500/20 bg-emerald-500/10 p-3 text-emerald-400">
              <ShieldCheck size={22} />
            </div>
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">Disponibilidad</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {availability} %
              </p>
            </div>

            <div className="rounded-xl border border-blue-500/20 bg-blue-500/10 p-3 text-blue-400">
              <CheckCircle2 size={22} />
            </div>
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">
                Servicios monitorizados
              </p>
              <p className="mt-2 text-2xl font-bold text-white">
                {services.length}
              </p>
            </div>

            <div className="rounded-xl border border-violet-500/20 bg-violet-500/10 p-3 text-violet-400">
              <Server size={22} />
            </div>
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">Última comprobación</p>
              <p className="mt-2 text-lg font-bold text-white">
                {checkedAt}
              </p>
              <p className="mt-1 text-xs text-slate-600">
                Cliente actualizado: {lastUpdated}
              </p>
            </div>

            <div className="rounded-xl border border-amber-500/20 bg-amber-500/10 p-3 text-amber-400">
              <Clock3 size={22} />
            </div>
          </div>
        </Card>
      </section>

      <section>
        <div className="mb-5">
          <h2 className="text-xl font-semibold text-white">
            Servicios
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            Estado y latencia obtenidos en tiempo real desde el Gateway.
          </p>
        </div>

        <div className="grid gap-5 md:grid-cols-2 xl:grid-cols-3">
          {services.map((service) => (
            <ServiceCard
              key={service.name}
              service={service}
              description={
                serviceDescriptions[service.name] ??
                'Servicio interno de NexusCommerce'
              }
            />
          ))}
        </div>
      </section>
    </div>
  )
}
