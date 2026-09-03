import {
  Activity,
  CheckCircle2,
  Clock3,
  GitBranch,
  RefreshCw,
  Server,
  Timer,
  TriangleAlert,
} from 'lucide-react'

import { Card } from '../../../shared/ui/card/Card'
import { useJaegerOverview } from '../hooks/useJaegerOverview'

function formatDuration(durationMs: number) {
  if (durationMs < 1) {
    return `${durationMs.toFixed(2)} ms`
  }

  if (durationMs < 1000) {
    return `${durationMs.toFixed(2)} ms`
  }

  return `${(durationMs / 1000).toFixed(2)} s`
}

function formatTime(value: string) {
  return new Date(value).toLocaleTimeString('es-ES')
}

export function JaegerPage() {
  const {
    data: jaeger,
    isLoading,
    isError,
    dataUpdatedAt,
    refetch,
    isFetching,
  } = useJaegerOverview()

  const checkedAt = jaeger?.checkedAt
    ? new Date(jaeger.checkedAt).toLocaleTimeString('es-ES')
    : 'Sin comprobar'

  const lastUpdated = dataUpdatedAt
    ? new Date(dataUpdatedAt).toLocaleTimeString('es-ES')
    : 'Sin actualizar'

  const distributedTraces =
    jaeger?.recentTraces.filter(
      (trace) => trace.services.length > 1,
    ).length ?? 0

  if (isLoading) {
    return (
      <div className="flex min-h-[60vh] items-center justify-center">
        <div className="text-center">
          <Activity
            size={32}
            className="mx-auto animate-pulse text-cyan-400"
          />

          <p className="mt-4 text-sm text-slate-400">
            Consultando trazas distribuidas...
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
            No se ha podido obtener información de Jaeger.
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
            <span className="h-2 w-2 rounded-full bg-cyan-400 shadow-[0_0_12px_rgba(34,211,238,0.6)]" />

            <p className="text-sm font-semibold text-cyan-400">
              Observabilidad distribuida
            </p>
          </div>

          <h1 className="mt-3 text-3xl font-bold tracking-tight text-white sm:text-4xl">
            Jaeger
          </h1>

          <p className="mt-2 max-w-2xl text-slate-400">
            Supervisa las trazas, servicios y operaciones distribuidas de
            NexusCommerce mediante OpenTelemetry.
          </p>
        </div>

        <button
          type="button"
          onClick={() => refetch()}
          disabled={isFetching}
          className="inline-flex items-center justify-center gap-2 self-start rounded-xl border border-slate-700 bg-slate-900/70 px-4 py-2.5 text-sm font-semibold text-slate-200 transition hover:border-cyan-500/40 hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60 lg:self-auto"
        >
          <RefreshCw
            size={17}
            className={isFetching ? 'animate-spin' : ''}
          />

          {isFetching ? 'Actualizando...' : 'Actualizar ahora'}
        </button>
      </header>

      <section className="grid gap-5 sm:grid-cols-2 xl:grid-cols-5">
        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">Estado</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {jaeger?.status ?? 'Desconocido'}
              </p>
            </div>

            <div className="rounded-xl border border-emerald-500/20 bg-emerald-500/10 p-3 text-emerald-400">
              <CheckCircle2 size={22} />
            </div>
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">Servicios</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {jaeger?.serviceCount ?? 0}
              </p>
            </div>

            <Server size={22} className="text-blue-400" />
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">Trazas</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {jaeger?.traceCount ?? 0}
              </p>
            </div>

            <Activity size={22} className="text-violet-400" />
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">Errores</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {jaeger?.errorCount ?? 0}
              </p>
            </div>

            <TriangleAlert
              size={22}
              className={
                jaeger?.errorCount
                  ? 'text-red-400'
                  : 'text-emerald-400'
              }
            />
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">
                Duración media
              </p>
              <p className="mt-2 text-2xl font-bold text-white">
                {formatDuration(jaeger?.averageDurationMs ?? 0)}
              </p>
            </div>

            <Timer size={22} className="text-amber-400" />
          </div>
        </Card>
      </section>

      <section className="grid gap-5 lg:grid-cols-3">
        <Card className="p-5">
          <p className="text-sm text-slate-500">
            Trazas distribuidas
          </p>

          <div className="mt-3 flex items-center gap-3">
            <GitBranch size={20} className="text-cyan-400" />

            <p className="text-2xl font-bold text-white">
              {distributedTraces}
            </p>
          </div>

          <p className="mt-2 text-xs text-slate-600">
            Trazas que atraviesan más de un servicio.
          </p>
        </Card>

        <Card className="p-5 lg:col-span-2">
          <div className="flex items-start justify-between gap-4">
            <div>
              <p className="text-sm text-slate-500">
                Última comprobación
              </p>

              <p className="mt-2 text-lg font-semibold text-white">
                {checkedAt}
              </p>

              <p className="mt-1 text-xs text-slate-600">
                Cliente actualizado: {lastUpdated}
              </p>
            </div>

            <Clock3 size={20} className="text-slate-500" />
          </div>
        </Card>
      </section>

      <section>
        <div className="mb-5">
          <h2 className="text-xl font-semibold text-white">
            Servicios instrumentados
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            Servicios detectados por Jaeger durante la captura de
            telemetría.
          </p>
        </div>

        <div className="flex flex-wrap gap-3">
          {jaeger?.services.map((service) => (
            <div
              key={service}
              translate="no"
              className="notranslate inline-flex items-center gap-2 rounded-xl border border-slate-800 bg-slate-900/60 px-4 py-3"
            >
              <span className="h-2 w-2 rounded-full bg-emerald-400" />

              <span className="font-mono text-sm text-slate-300">
                {service}
              </span>
            </div>
          ))}
        </div>
      </section>

      <section>
        <div className="mb-5">
          <h2 className="text-xl font-semibold text-white">
            Trazas recientes
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            Últimas trazas recopiladas y correlacionadas por Jaeger.
          </p>
        </div>

        <Card className="overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-left text-sm">
              <thead className="border-b border-slate-800 bg-slate-900/60 text-xs uppercase tracking-wide text-slate-500">
                <tr>
                  <th translate="no" className="notranslate px-5 py-4">Trace ID</th>
                  <th className="px-5 py-4">Operación</th>
                  <th className="px-5 py-4">Servicios</th>
                  <th translate="no" className="notranslate px-5 py-4">Spans</th>
                  <th className="px-5 py-4">Duración</th>
                  <th className="px-5 py-4">Estado</th>
                  <th className="px-5 py-4">Hora</th>
                </tr>
              </thead>

              <tbody>
                {jaeger?.recentTraces.map((trace) => {
                  const distributed = trace.services.length > 1

                  return (
                    <tr
                      key={trace.traceId}
                      className="border-b border-slate-800/70 last:border-0"
                    >
                      <td
                        translate="no"
                        className="notranslate px-5 py-4"
                      >
                        <span
                          title={trace.traceId}
                          className="font-mono text-xs text-cyan-300"
                        >
                          {trace.traceId.slice(0, 12)}...
                        </span>
                      </td>

                      <td
                        translate="no"
                        className="notranslate max-w-[260px] px-5 py-4"
                      >
                        <p
                          title={trace.operation}
                          className="truncate font-medium text-slate-200"
                        >
                          {trace.operation || 'Sin operación'}
                        </p>

                        <p className="mt-1 truncate font-mono text-xs text-slate-600">
                          {trace.rootService}
                        </p>
                      </td>

                      <td className="px-5 py-4">
                        <div className="flex flex-wrap items-center gap-1.5">
                          {trace.services.map((service) => (
                            <span
                              key={service}
                              translate="no"
                              className="notranslate inline-flex rounded-full border border-slate-700 bg-slate-800/70 px-2 py-1 font-mono text-[11px] text-slate-300"
                            >
                              {service.replace(
                                'NexusCommerce.',
                                '',
                              )}
                            </span>
                          ))}

                          {distributed && (
                            <span className="inline-flex rounded-full border border-cyan-500/20 bg-cyan-500/10 px-2 py-1 text-[11px] font-semibold text-cyan-300">
                              Distribuida
                            </span>
                          )}
                        </div>
                      </td>

                      <td className="px-5 py-4 text-slate-300">
                        {trace.spanCount}
                      </td>

                      <td className="px-5 py-4 text-slate-300">
                        {formatDuration(trace.durationMs)}
                      </td>

                      <td className="px-5 py-4">
                        {trace.hasError ? (
                          <span className="inline-flex rounded-full border border-red-500/20 bg-red-500/10 px-2.5 py-1 text-xs font-semibold text-red-300">
                            Error
                          </span>
                        ) : (
                          <span translate="no" className="notranslate inline-flex rounded-full border border-emerald-500/20 bg-emerald-500/10 px-2.5 py-1 text-xs font-semibold text-emerald-300">
                            OK
                          </span>
                        )}
                      </td>

                      <td className="whitespace-nowrap px-5 py-4 text-slate-400">
                        {formatTime(trace.startedAt)}
                      </td>
                    </tr>
                  )
                })}

                {jaeger?.recentTraces.length === 0 && (
                  <tr>
                    <td
                      colSpan={7}
                      className="px-5 py-10 text-center text-slate-500"
                    >
                      Todavía no hay trazas disponibles.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </Card>
      </section>
    </div>
  )
}
