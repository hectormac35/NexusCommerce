import { useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { AxiosError } from 'axios'
import {
  CircleDollarSign,
  Clock3,
  Eye,
  ReceiptText,
  Search,
  ShoppingCart,
  TrendingUp,
} from 'lucide-react'
import { useOrders } from '../hooks/useOrders'
import { CreateOrderDialog } from '../components/CreateOrderDialog'

function formatPrice(price: number) {
  return new Intl.NumberFormat('es-ES', {
    style: 'currency',
    currency: 'EUR',
  }).format(price)
}

function formatDate(date: string) {
  return new Intl.DateTimeFormat('es-ES', {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(date))
}

function formatStatus(status: string) {
  if (status === 'EnPreparacion') {
    return 'En preparación'
  }

  return status
}

function getStatusStyles(status: string) {
  const normalizedStatus = status.toLowerCase()

  if (normalizedStatus === 'pendiente') {
    return 'border-amber-500/30 bg-amber-500/10 text-amber-300'
  }

  if (
    normalizedStatus === 'confirmado' ||
    normalizedStatus === 'enpreparacion'
  ) {
    return 'border-blue-500/30 bg-blue-500/10 text-blue-300'
  }

  if (normalizedStatus === 'enviado') {
    return 'border-violet-500/30 bg-violet-500/10 text-violet-300'
  }

  if (normalizedStatus === 'entregado') {
    return 'border-emerald-500/30 bg-emerald-500/10 text-emerald-300'
  }

  if (normalizedStatus === 'cancelado') {
    return 'border-red-500/30 bg-red-500/10 text-red-300'
  }

  return 'border-slate-600 bg-slate-800 text-slate-300'
}

export function OrdersPage() {
  const navigate = useNavigate()
  const [searchTerm, setSearchTerm] = useState('')
  const [selectedStatus, setSelectedStatus] = useState('Todos')
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false)

  const {
    data: orders,
    isLoading,
    isError,
    error,
    refetch,
  } = useOrders()

  const statuses = useMemo(
    () => [
      'Todos',
      ...Array.from(
        new Set((orders ?? []).map((order) => order.estado)),
      ).sort(),
    ],
    [orders],
  )

  const filteredOrders = useMemo(() => {
    const normalizedSearch = searchTerm.trim().toLowerCase()

    return (orders ?? []).filter((order) => {
      const matchesSearch =
        !normalizedSearch ||
        order.pedidoId.toLowerCase().includes(normalizedSearch) ||
        order.clienteId.toLowerCase().includes(normalizedSearch)

      const matchesStatus =
        selectedStatus === 'Todos' ||
        order.estado === selectedStatus

      return matchesSearch && matchesStatus
    })
  }, [orders, searchTerm, selectedStatus])

  const totalRevenue = useMemo(
    () =>
      (orders ?? []).reduce(
        (accumulator, order) => accumulator + order.total,
        0,
      ),
    [orders],
  )

  const pendingOrders = useMemo(
    () =>
      (orders ?? []).filter(
        (order) => order.estado.toLowerCase() === 'pendiente',
      ).length,
    [orders],
  )

  const averageTicket =
    orders && orders.length > 0
      ? totalRevenue / orders.length
      : 0

  if (isLoading) {
    return (
      <section>
        <div className="mb-8 h-10 w-56 animate-pulse rounded-lg bg-slate-900" />

        <div className="grid gap-5 md:grid-cols-2 xl:grid-cols-4">
          {Array.from({ length: 4 }).map((_, index) => (
            <div
              key={index}
              className="h-32 animate-pulse rounded-2xl border border-slate-800 bg-slate-900"
            />
          ))}
        </div>

        <div className="mt-8 h-96 animate-pulse rounded-2xl border border-slate-800 bg-slate-900" />
      </section>
    )
  }

  if (isError) {
    const status =
      error instanceof AxiosError ? error.response?.status : undefined

    return (
      <section>
        <p className="text-sm font-semibold uppercase tracking-wider text-red-400">
          Error de conexión
        </p>

        <h1 className="mt-2 text-4xl font-bold text-white">
          No se pudieron cargar los pedidos
        </h1>

        <p className="mt-4 max-w-2xl text-slate-400">
          {status
            ? `El API Gateway respondió con el código ${status}.`
            : 'Comprueba que el Gateway y el microservicio de pedidos estén funcionando.'}
        </p>

        <button
          type="button"
          onClick={() => refetch()}
          className="mt-6 rounded-lg bg-blue-600 px-5 py-3 font-semibold text-white transition hover:bg-blue-500"
        >
          Reintentar
        </button>
      </section>
    )
  }

  return (
    <section>
      <CreateOrderDialog
        open={isCreateDialogOpen}
        onClose={() => setIsCreateDialogOpen(false)}
        onCreated={(pedidoId) => {
          setIsCreateDialogOpen(false)
          navigate(`/pedidos/${pedidoId}`)
        }}
      />

      <div className="flex flex-col justify-between gap-5 sm:flex-row sm:items-end">
        <div>
          <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
            Gestión comercial
          </p>

          <h1 className="mt-2 text-4xl font-bold text-white">
            Pedidos
          </h1>

          <p className="mt-3 text-slate-400">
            Consulta el estado, importe y actividad de los pedidos de
            NexusCommerce.
          </p>
        </div>

        <button
          type="button"
          onClick={() => setIsCreateDialogOpen(true)}
          className="inline-flex items-center justify-center gap-2 rounded-lg bg-blue-600 px-5 py-3 font-semibold text-white transition hover:bg-blue-500"
        >
          <ShoppingCart size={18} />
          Nuevo pedido
        </button>
      </div>

      <div className="mt-8 grid gap-5 md:grid-cols-2 xl:grid-cols-4">
        <article className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex items-start justify-between gap-4">
            <div>
              <p className="text-sm text-slate-400">
                Pedidos totales
              </p>
              <p className="mt-2 text-3xl font-bold text-white">
                {orders?.length ?? 0}
              </p>
            </div>

            <div className="rounded-xl bg-blue-500/10 p-3 text-blue-400">
              <ReceiptText size={22} />
            </div>
          </div>
        </article>

        <article className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex items-start justify-between gap-4">
            <div>
              <p className="text-sm text-slate-400">
                Pendientes
              </p>
              <p className="mt-2 text-3xl font-bold text-white">
                {pendingOrders}
              </p>
            </div>

            <div className="rounded-xl bg-amber-500/10 p-3 text-amber-400">
              <Clock3 size={22} />
            </div>
          </div>
        </article>

        <article className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex items-start justify-between gap-4">
            <div>
              <p className="text-sm text-slate-400">
                Facturación
              </p>
              <p className="mt-2 text-3xl font-bold text-white">
                {formatPrice(totalRevenue)}
              </p>
            </div>

            <div className="rounded-xl bg-emerald-500/10 p-3 text-emerald-400">
              <TrendingUp size={22} />
            </div>
          </div>
        </article>

        <article className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex items-start justify-between gap-4">
            <div>
              <p className="text-sm text-slate-400">
                Ticket medio
              </p>
              <p className="mt-2 text-3xl font-bold text-white">
                {formatPrice(averageTicket)}
              </p>
            </div>

            <div className="rounded-xl bg-violet-500/10 p-3 text-violet-400">
              <CircleDollarSign size={22} />
            </div>
          </div>
        </article>
      </div>

      <div className="mt-8 grid gap-4 rounded-2xl border border-slate-800 bg-slate-900 p-5 md:grid-cols-[1fr_240px]">
        <div>
          <label
            htmlFor="orders-search"
            className="mb-2 block text-sm font-medium text-slate-300"
          >
            Buscar pedidos
          </label>

          <div className="relative">
            <Search
              size={18}
              className="pointer-events-none absolute left-4 top-1/2 -translate-y-1/2 text-slate-500"
            />

            <input
              id="orders-search"
              type="search"
              value={searchTerm}
              onChange={(event) => setSearchTerm(event.target.value)}
              placeholder="ID del pedido o cliente..."
              className="w-full rounded-lg border border-slate-700 bg-slate-950 py-3 pl-11 pr-4 text-white outline-none transition placeholder:text-slate-600 focus:border-blue-500"
            />
          </div>
        </div>

        <div>
          <label
            htmlFor="orders-status"
            className="mb-2 block text-sm font-medium text-slate-300"
          >
            Estado
          </label>

          <select
            id="orders-status"
            value={selectedStatus}
            onChange={(event) => setSelectedStatus(event.target.value)}
            className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white outline-none transition focus:border-blue-500"
          >
            {statuses.map((status) => (
              <option key={status} value={status}>
                {status}
              </option>
            ))}
          </select>
        </div>
      </div>

      {!orders?.length ? (
        <div className="mt-8 rounded-2xl border border-dashed border-slate-700 bg-slate-900/50 p-12 text-center">
          <ShoppingCart
            size={38}
            className="mx-auto text-slate-600"
          />

          <p className="mt-4 text-lg font-medium text-slate-300">
            No hay pedidos todavía
          </p>

          <p className="mt-2 text-sm text-slate-500">
            Crea el primer pedido para comenzar a gestionar ventas.
          </p>
        </div>
      ) : filteredOrders.length === 0 ? (
        <div className="mt-8 rounded-2xl border border-dashed border-slate-700 bg-slate-900/50 p-12 text-center">
          <p className="text-lg font-medium text-slate-300">
            No se encontraron pedidos
          </p>

          <p className="mt-2 text-sm text-slate-500">
            Prueba con otro término o cambia el estado seleccionado.
          </p>

          <button
            type="button"
            onClick={() => {
              setSearchTerm('')
              setSelectedStatus('Todos')
            }}
            className="mt-6 rounded-lg border border-slate-700 px-5 py-3 font-semibold text-slate-300 transition hover:bg-slate-800 hover:text-white"
          >
            Limpiar filtros
          </button>
        </div>
      ) : (
        <div className="mt-8 overflow-hidden rounded-2xl border border-slate-800 bg-slate-900">
          <div className="flex items-center justify-between border-b border-slate-800 px-5 py-4">
            <div>
              <h2 className="font-semibold text-white">
                Listado de pedidos
              </h2>
              <p className="mt-1 text-sm text-slate-500">
                {filteredOrders.length} resultados
              </p>
            </div>
          </div>

          <div className="overflow-x-auto">
            <table className="w-full min-w-[860px]">
              <thead className="bg-slate-950/40">
                <tr>
                  <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wider text-slate-500">
                    Pedido
                  </th>
                  <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wider text-slate-500">
                    Cliente
                  </th>
                  <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wider text-slate-500">
                    Estado
                  </th>
                  <th className="px-5 py-4 text-right text-xs font-semibold uppercase tracking-wider text-slate-500">
                    Total
                  </th>
                  <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wider text-slate-500">
                    Fecha
                  </th>
                  <th className="px-5 py-4 text-right text-xs font-semibold uppercase tracking-wider text-slate-500">
                    Acciones
                  </th>
                </tr>
              </thead>

              <tbody>
                {filteredOrders.map((order) => (
                  <tr
                    key={order.pedidoId}
                    className="border-t border-slate-800 transition hover:bg-slate-800/40"
                  >
                    <td className="px-5 py-4">
                      <p
                        title={order.pedidoId}
                        className="font-mono text-sm font-semibold text-white"
                      >
                        #{order.pedidoId.slice(0, 8)}
                      </p>
                    </td>

                    <td className="px-5 py-4">
                      <p
                        title={order.clienteId}
                        className="font-mono text-sm text-slate-300"
                      >
                        {order.clienteId.slice(0, 8)}
                      </p>
                    </td>

                    <td className="px-5 py-4">
                      <span
                        className={`inline-flex rounded-full border px-3 py-1 text-xs font-semibold ${getStatusStyles(
                          order.estado,
                        )}`}
                      >
                        {formatStatus(order.estado)}
                      </span>
                    </td>

                    <td className="px-5 py-4 text-right font-semibold text-white">
                      {formatPrice(order.total)}
                    </td>

                    <td className="px-5 py-4 text-sm text-slate-400">
                      {formatDate(order.fechaCreacionUtc)}
                    </td>

                    <td className="px-5 py-4 text-right">
                      <button
                        type="button"
                        title="Ver detalle"
                        onClick={() =>
                          navigate(`/pedidos/${order.pedidoId}`)
                        }
                        className="inline-flex items-center gap-2 rounded-lg border border-slate-700 px-3 py-2 text-sm font-semibold text-slate-300 transition hover:border-blue-500/50 hover:bg-blue-500/10 hover:text-blue-300"
                      >
                        <Eye size={16} />
                        Ver
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </section>
  )
}
