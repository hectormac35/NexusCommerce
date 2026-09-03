import { AxiosError } from 'axios'
import {
  ArrowLeft,
  CalendarDays,
  CircleDollarSign,
  Package,
  ReceiptText,
  UserRound,
} from 'lucide-react'
import { Link, useParams } from 'react-router-dom'
import { useOrder } from '../hooks/useOrder'
import { useChangeOrderStatus } from '../hooks/useChangeOrderStatus'

function formatPrice(price: number) {
  return new Intl.NumberFormat('es-ES', {
    style: 'currency',
    currency: 'EUR',
  }).format(price)
}

function formatDate(date: string) {
  return new Intl.DateTimeFormat('es-ES', {
    dateStyle: 'long',
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

export function OrderDetailPage() {
  const { pedidoId } = useParams()
  const changeStatus = useChangeOrderStatus()
  const {
    data: order,
    isLoading,
    isError,
    error,
    refetch,
  } = useOrder(pedidoId)

  if (isLoading) {
    return (
      <section>
        <div className="h-10 w-56 animate-pulse rounded-lg bg-slate-900" />

        <div className="mt-8 grid gap-5 lg:grid-cols-3">
          {Array.from({ length: 3 }).map((_, index) => (
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

  if (isError || !order) {
    const status =
      error instanceof AxiosError ? error.response?.status : undefined

    return (
      <section>
        <Link
          to="/pedidos"
          className="inline-flex items-center gap-2 text-sm font-semibold text-slate-400 transition hover:text-white"
        >
          <ArrowLeft size={17} />
          Volver a pedidos
        </Link>

        <p className="mt-8 text-sm font-semibold uppercase tracking-wider text-red-400">
          Error
        </p>

        <h1 className="mt-2 text-4xl font-bold text-white">
          No se pudo cargar el pedido
        </h1>

        <p className="mt-4 text-slate-400">
          {status
            ? `El servicio respondió con el código ${status}.`
            : 'Comprueba que el Gateway y Orders.Api estén funcionando.'}
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
      <Link
        to="/pedidos"
        className="inline-flex items-center gap-2 text-sm font-semibold text-slate-400 transition hover:text-white"
      >
        <ArrowLeft size={17} />
        Volver a pedidos
      </Link>

      <div className="mt-6 flex flex-col justify-between gap-5 sm:flex-row sm:items-end">
        <div>
          <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
            Detalle del pedido
          </p>

          <h1 className="mt-2 font-mono text-3xl font-bold text-white sm:text-4xl">
            #{order.pedidoId.slice(0, 8)}
          </h1>

          <p
            title={order.pedidoId}
            className="mt-3 break-all text-sm text-slate-500"
          >
            {order.pedidoId}
          </p>
        </div>

        <div className="flex flex-col items-start gap-3 sm:items-end">
          <span
            className={`inline-flex self-start rounded-full border px-4 py-2 text-sm font-semibold sm:self-auto ${getStatusStyles(
              order.estado,
            )}`}
          >
            {formatStatus(order.estado)}
          </span>

          <div className="flex flex-wrap gap-2">
            {order.estado === 'Pendiente' && (
              <>
                <button
                  type="button"
                  disabled={changeStatus.isPending}
                  onClick={() =>
                    changeStatus.mutate({
                      orderId: order.pedidoId,
                      request: { nuevoEstado: 'Confirmado' },
                    })
                  }
                  className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-blue-500 disabled:opacity-50"
                >
                  Confirmar pedido
                </button>

                <button
                  type="button"
                  disabled={changeStatus.isPending}
                  onClick={() =>
                    changeStatus.mutate({
                      orderId: order.pedidoId,
                      request: { nuevoEstado: 'Cancelado' },
                    })
                  }
                  className="rounded-lg border border-red-500/40 bg-red-500/10 px-4 py-2 text-sm font-semibold text-red-300 transition hover:bg-red-500/20 disabled:opacity-50"
                >
                  Cancelar
                </button>
              </>
            )}

            {order.estado === 'Confirmado' && (
              <>
                <button
                  type="button"
                  disabled={changeStatus.isPending}
                  onClick={() =>
                    changeStatus.mutate({
                      orderId: order.pedidoId,
                      request: { nuevoEstado: 'EnPreparacion' },
                    })
                  }
                  className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-blue-500 disabled:opacity-50"
                >
                  Iniciar preparación
                </button>

                <button
                  type="button"
                  disabled={changeStatus.isPending}
                  onClick={() =>
                    changeStatus.mutate({
                      orderId: order.pedidoId,
                      request: { nuevoEstado: 'Cancelado' },
                    })
                  }
                  className="rounded-lg border border-red-500/40 bg-red-500/10 px-4 py-2 text-sm font-semibold text-red-300 transition hover:bg-red-500/20 disabled:opacity-50"
                >
                  Cancelar
                </button>
              </>
            )}

            {order.estado === 'EnPreparacion' && (
              <>
                <button
                  type="button"
                  disabled={changeStatus.isPending}
                  onClick={() =>
                    changeStatus.mutate({
                      orderId: order.pedidoId,
                      request: { nuevoEstado: 'Enviado' },
                    })
                  }
                  className="rounded-lg bg-violet-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-violet-500 disabled:opacity-50"
                >
                  Marcar como enviado
                </button>

                <button
                  type="button"
                  disabled={changeStatus.isPending}
                  onClick={() =>
                    changeStatus.mutate({
                      orderId: order.pedidoId,
                      request: { nuevoEstado: 'Cancelado' },
                    })
                  }
                  className="rounded-lg border border-red-500/40 bg-red-500/10 px-4 py-2 text-sm font-semibold text-red-300 transition hover:bg-red-500/20 disabled:opacity-50"
                >
                  Cancelar
                </button>
              </>
            )}

            {order.estado === 'Enviado' && (
              <button
                type="button"
                disabled={changeStatus.isPending}
                onClick={() =>
                  changeStatus.mutate({
                    orderId: order.pedidoId,
                    request: { nuevoEstado: 'Entregado' },
                  })
                }
                className="rounded-lg bg-emerald-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-emerald-500 disabled:opacity-50"
              >
                Marcar como entregado
              </button>
            )}
          </div>

          {changeStatus.isError && (
            <p className="text-sm text-red-400">
              No se pudo cambiar el estado del pedido.
            </p>
          )}
        </div>
      </div>

      <div className="mt-8 grid gap-5 md:grid-cols-2 xl:grid-cols-4">
        <article className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex items-start gap-4">
            <div className="rounded-xl bg-blue-500/10 p-3 text-blue-400">
              <UserRound size={22} />
            </div>

            <div>
              <p className="text-sm text-slate-500">Cliente</p>
              <p
                title={order.clienteId}
                className="mt-2 font-mono text-sm font-semibold text-white"
              >
                {order.clienteId.slice(0, 12)}
              </p>
            </div>
          </div>
        </article>

        <article className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex items-start gap-4">
            <div className="rounded-xl bg-violet-500/10 p-3 text-violet-400">
              <CalendarDays size={22} />
            </div>

            <div>
              <p className="text-sm text-slate-500">Fecha</p>
              <p className="mt-2 text-sm font-semibold text-white">
                {formatDate(order.fechaCreacionUtc)}
              </p>
            </div>
          </div>
        </article>

        <article className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex items-start gap-4">
            <div className="rounded-xl bg-amber-500/10 p-3 text-amber-400">
              <Package size={22} />
            </div>

            <div>
              <p className="text-sm text-slate-500">Líneas</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {order.lineas.length}
              </p>
            </div>
          </div>
        </article>

        <article className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex items-start gap-4">
            <div className="rounded-xl bg-emerald-500/10 p-3 text-emerald-400">
              <CircleDollarSign size={22} />
            </div>

            <div>
              <p className="text-sm text-slate-500">Total</p>
              <p className="mt-2 text-2xl font-bold text-white">
                {formatPrice(order.total)}
              </p>
            </div>
          </div>
        </article>
      </div>

      <div className="mt-8 overflow-hidden rounded-2xl border border-slate-800 bg-slate-900">
        <div className="flex items-center gap-3 border-b border-slate-800 px-6 py-5">
          <ReceiptText size={20} className="text-blue-400" />

          <div>
            <h2 className="font-semibold text-white">
              Líneas del pedido
            </h2>
            <p className="mt-1 text-sm text-slate-500">
              Productos incluidos en esta operación.
            </p>
          </div>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full min-w-[760px]">
            <thead className="bg-slate-950/40">
              <tr>
                <th className="px-6 py-4 text-left text-xs font-semibold uppercase tracking-wider text-slate-500">
                  Producto
                </th>
                <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wider text-slate-500">
                  Precio unitario
                </th>
                <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wider text-slate-500">
                  Cantidad
                </th>
                <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wider text-slate-500">
                  Subtotal
                </th>
              </tr>
            </thead>

            <tbody>
              {order.lineas.map((line) => (
                <tr
                  key={line.productoId}
                  className="border-t border-slate-800"
                >
                  <td className="px-6 py-5">
                    <p className="font-semibold text-white">
                      {line.nombreProducto}
                    </p>
                    <p
                      title={line.productoId}
                      className="mt-1 font-mono text-xs text-slate-500"
                    >
                      {line.productoId.slice(0, 12)}
                    </p>
                  </td>

                  <td className="px-6 py-5 text-right text-slate-300">
                    {formatPrice(line.precioUnitario)}
                  </td>

                  <td className="px-6 py-5 text-right text-slate-300">
                    {line.cantidad}
                  </td>

                  <td className="px-6 py-5 text-right font-semibold text-white">
                    {formatPrice(line.subtotal)}
                  </td>
                </tr>
              ))}
            </tbody>

            <tfoot className="border-t border-slate-700 bg-slate-950/30">
              <tr>
                <td
                  colSpan={3}
                  className="px-6 py-5 text-right font-semibold text-slate-400"
                >
                  Total del pedido
                </td>

                <td className="px-6 py-5 text-right text-xl font-bold text-white">
                  {formatPrice(order.total)}
                </td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>
    </section>
  )
}
