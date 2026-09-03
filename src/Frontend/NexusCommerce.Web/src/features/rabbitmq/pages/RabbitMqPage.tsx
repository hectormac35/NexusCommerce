import {
  Activity,
  Boxes,
  CheckCircle2,
  Clock3,
  Network,
  Radio,
  RefreshCw,
  Send,
  Server,
  Users,
} from 'lucide-react'

import { Card } from '../../../shared/ui/card/Card'
import { useRabbitMqOverview } from '../hooks/useRabbitMqOverview'

export function RabbitMqPage() {
  const {
    data: rabbitMq,
    isLoading,
    isError,
    dataUpdatedAt,
    refetch,
    isFetching,
  } = useRabbitMqOverview()

  const checkedAt = rabbitMq?.checkedAt
    ? new Date(rabbitMq.checkedAt).toLocaleTimeString('es-ES')
    : 'Sin comprobar'

  const lastUpdated = dataUpdatedAt
    ? new Date(dataUpdatedAt).toLocaleTimeString('es-ES')
    : 'Sin actualizar'

  if (isLoading) {
    return (
      <div className="flex min-h-[60vh] items-center justify-center">
        <div className="text-center">
          <Network
            size={32}
            className="mx-auto animate-pulse text-orange-400"
          />

          <p className="mt-4 text-sm text-slate-400">
            Consultando RabbitMQ...
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
            No se ha podido obtener información de RabbitMQ.
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
            <span className="h-2 w-2 rounded-full bg-orange-400 shadow-[0_0_12px_rgba(251,146,60,0.6)]" />

            <p className="text-sm font-semibold text-orange-400">
              Mensajería asíncrona
            </p>
          </div>

          <h1 className="mt-3 text-3xl font-bold tracking-tight text-white sm:text-4xl">
            RabbitMQ
          </h1>

          <p className="mt-2 max-w-2xl text-slate-400">
            Supervisa el broker, el flujo de mensajes y el estado de las colas
            de NexusCommerce.
          </p>
        </div>

        <button
          type="button"
          onClick={() => refetch()}
          disabled={isFetching}
          className="inline-flex items-center justify-center gap-2 self-start rounded-xl border border-slate-700 bg-slate-900/70 px-4 py-2.5 text-sm font-semibold text-slate-200 transition hover:border-orange-500/40 hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60 lg:self-auto"
        >
          <RefreshCw
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
              <p className="text-sm text-slate-500">Estado</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {rabbitMq?.status ?? 'Desconocido'}
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
              <p className="text-sm text-slate-500">Publicados</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {rabbitMq?.published ?? 0}
              </p>
            </div>

            <div className="rounded-xl border border-blue-500/20 bg-blue-500/10 p-3 text-blue-400">
              <Send size={22} />
            </div>
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">ACK</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {rabbitMq?.acknowledged ?? 0}
              </p>
            </div>

            <div className="rounded-xl border border-violet-500/20 bg-violet-500/10 p-3 text-violet-400">
              <Activity size={22} />
            </div>
          </div>
        </Card>

        <Card className="p-5">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-slate-500">No enrutados</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {rabbitMq?.unroutable ?? 0}
              </p>
            </div>

            <div className="rounded-xl border border-amber-500/20 bg-amber-500/10 p-3 text-amber-400">
              <Radio size={22} />
            </div>
          </div>
        </Card>
      </section>

      <section className="grid gap-5 sm:grid-cols-2 xl:grid-cols-4">
        <Card className="p-5">
          <p className="text-sm text-slate-500">Conexiones</p>
          <div className="mt-3 flex items-center gap-3">
            <Network size={20} className="text-slate-400" />
            <p className="text-2xl font-bold text-white">
              {rabbitMq?.connections ?? 0}
            </p>
          </div>
        </Card>

        <Card className="p-5">
          <p className="text-sm text-slate-500">Canales</p>
          <div className="mt-3 flex items-center gap-3">
            <Server size={20} className="text-slate-400" />
            <p className="text-2xl font-bold text-white">
              {rabbitMq?.channels ?? 0}
            </p>
          </div>
        </Card>

        <Card className="p-5">
          <p className="text-sm text-slate-500">Consumidores</p>
          <div className="mt-3 flex items-center gap-3">
            <Users size={20} className="text-slate-400" />
            <p className="text-2xl font-bold text-white">
              {rabbitMq?.consumers ?? 0}
            </p>
          </div>
        </Card>

        <Card className="p-5">
          <p className="text-sm text-slate-500">Colas</p>
          <div className="mt-3 flex items-center gap-3">
            <Boxes size={20} className="text-slate-400" />
            <p className="text-2xl font-bold text-white">
              {rabbitMq?.queues ?? 0}
            </p>
          </div>
        </Card>
      </section>

      <section className="grid gap-5 lg:grid-cols-3">
        <Card className="p-5">
          <p className="text-sm text-slate-500">Versión RabbitMQ</p>
          <p className="mt-2 text-lg font-semibold text-white">
            {rabbitMq?.version || 'Desconocida'}
          </p>
        </Card>

        <Card className="p-5">
          <p className="text-sm text-slate-500">Versión Erlang</p>
          <p className="mt-2 text-lg font-semibold text-white">
            {rabbitMq?.erlangVersion || 'Desconocida'}
          </p>
        </Card>

        <Card className="p-5">
          <div className="flex items-start justify-between gap-4">
            <div>
              <p className="text-sm text-slate-500">Última comprobación</p>
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
            Colas
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            Estado actual de las colas y mensajes pendientes del broker.
          </p>
        </div>

        <Card className="overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-left text-sm">
              <thead className="border-b border-slate-800 bg-slate-900/60 text-xs uppercase tracking-wide text-slate-500">
                <tr>
                  <th className="px-5 py-4">Cola</th>
                  <th className="px-5 py-4">Estado</th>
                  <th className="px-5 py-4">Mensajes</th>
                  <th className="px-5 py-4">Preparados</th>
                  <th className="px-5 py-4">Sin ACK</th>
                  <th className="px-5 py-4">Consumidores</th>
                </tr>
              </thead>

              <tbody>
                {rabbitMq?.queueDetails.map((queue) => (
                  <tr
                    key={queue.name}
                    className="border-b border-slate-800/70 last:border-0"
                  >
                    <td className="px-5 py-4 font-medium text-slate-200">
                      {queue.name}
                    </td>

                    <td className="px-5 py-4">
                      <span className="inline-flex rounded-full border border-emerald-500/20 bg-emerald-500/10 px-2.5 py-1 text-xs font-semibold text-emerald-300">
                        {queue.state}
                      </span>
                    </td>

                    <td className="px-5 py-4 text-slate-300">
                      {queue.messages}
                    </td>

                    <td className="px-5 py-4 text-slate-300">
                      {queue.messagesReady}
                    </td>

                    <td className="px-5 py-4 text-slate-300">
                      {queue.messagesUnacknowledged}
                    </td>

                    <td className="px-5 py-4 text-slate-300">
                      {queue.consumers}
                    </td>
                  </tr>
                ))}

                {rabbitMq?.queueDetails.length === 0 && (
                  <tr>
                    <td
                      colSpan={6}
                      className="px-5 py-10 text-center text-slate-500"
                    >
                      No hay colas registradas.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </Card>
      </section>

      <Card className="p-5">
        <p className="text-sm text-slate-500">Cluster</p>
        <p className="mt-2 break-all font-mono text-sm text-slate-300">
          {rabbitMq?.clusterName || 'Desconocido'}
        </p>
      </Card>
    </div>
  )
}
