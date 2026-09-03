import { useMemo, useState } from 'react'
import { Minus, Plus, ShoppingCart, X } from 'lucide-react'

import { useAuthStore } from '../../auth/store/authStore'
import { useProducts } from '../../catalog/hooks/useProducts'
import type { Product } from '../../catalog/types/product'
import { useCreateOrder } from '../hooks/useCreateOrder'

type Props = {
  open: boolean
  onClose: () => void
  onCreated: (pedidoId: string) => void
}

export function CreateOrderDialog({
  open,
  onClose,
  onCreated,
}: Props) {
  const usuario = useAuthStore((state) => state.usuario)
  const { data: products = [], isLoading, isError } = useProducts()
  const createOrder = useCreateOrder()

  const [quantities, setQuantities] = useState<Record<string, number>>({})

  const availableProducts = useMemo(
    () =>
      products.filter(
        (product) =>
          product.estaActivo &&
          product.tieneStock &&
          product.stock > 0,
      ),
    [products],
  )

  const selectedProducts = useMemo(
    () =>
      availableProducts.filter(
        (product) => (quantities[product.id] ?? 0) > 0,
      ),
    [availableProducts, quantities],
  )

  const total = useMemo(
    () =>
      selectedProducts.reduce(
        (sum, product) =>
          sum + product.precio * (quantities[product.id] ?? 0),
        0,
      ),
    [selectedProducts, quantities],
  )

  if (!open) {
    return null
  }

  function updateQuantity(product: Product, quantity: number) {
    const safeQuantity = Math.max(
      0,
      Math.min(quantity, product.stock),
    )

    setQuantities((current) => ({
      ...current,
      [product.id]: safeQuantity,
    }))
  }

  async function handleCreateOrder() {
    if (!usuario || selectedProducts.length === 0) {
      return
    }

    const result = await createOrder.mutateAsync({
      clienteId: usuario.id,
      lineas: selectedProducts.map((product) => ({
        productoId: product.id,
        nombreProducto: product.nombre,
        precioUnitario: product.precio,
        cantidad: quantities[product.id],
      })),
    })

    setQuantities({})
    onCreated(result.pedidoId)
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 p-4 backdrop-blur-sm">
      <div className="flex max-h-[90vh] w-full max-w-4xl flex-col overflow-hidden rounded-2xl border border-slate-700 bg-slate-900 shadow-2xl">
        <header className="flex items-center justify-between border-b border-slate-800 px-6 py-5">
          <div>
            <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
              Gestión comercial
            </p>

            <h2 className="mt-1 text-2xl font-bold text-white">
              Nuevo pedido
            </h2>
          </div>

          <button
            type="button"
            onClick={onClose}
            className="rounded-lg p-2 text-slate-400 transition hover:bg-slate-800 hover:text-white"
          >
            <X size={22} />
          </button>
        </header>

        <main className="overflow-y-auto p-6">
          {isLoading && (
            <p className="text-slate-400">
              Cargando productos...
            </p>
          )}

          {isError && (
            <div className="rounded-xl border border-red-500/30 bg-red-500/10 p-4 text-red-300">
              No se pudo cargar el catálogo.
            </div>
          )}

          <div className="space-y-3">
            {availableProducts.map((product) => {
              const quantity = quantities[product.id] ?? 0

              return (
                <article
                  key={product.id}
                  className="grid gap-4 rounded-xl border border-slate-800 bg-slate-950 p-4 md:grid-cols-[1fr_auto_auto] md:items-center"
                >
                  <div>
                    <p className="font-semibold text-white">
                      {product.nombre}
                    </p>

                    <p className="mt-1 text-sm text-slate-500">
                      {product.categoria} · Stock: {product.stock}
                    </p>
                  </div>

                  <p className="font-semibold text-slate-200">
                    {product.precio.toLocaleString('es-ES', {
                      style: 'currency',
                      currency: 'EUR',
                    })}
                  </p>

                  <div className="flex items-center gap-2">
                    <button
                      type="button"
                      onClick={() =>
                        updateQuantity(product, quantity - 1)
                      }
                      disabled={quantity === 0}
                      className="rounded-lg border border-slate-700 p-2 text-slate-300 disabled:opacity-40"
                    >
                      <Minus size={16} />
                    </button>

                    <span className="w-8 text-center font-semibold text-white">
                      {quantity}
                    </span>

                    <button
                      type="button"
                      onClick={() =>
                        updateQuantity(product, quantity + 1)
                      }
                      disabled={quantity >= product.stock}
                      className="rounded-lg border border-slate-700 p-2 text-slate-300 disabled:opacity-40"
                    >
                      <Plus size={16} />
                    </button>
                  </div>
                </article>
              )
            })}
          </div>

          {createOrder.isError && (
            <div className="mt-5 rounded-xl border border-red-500/30 bg-red-500/10 p-4 text-red-300">
              No se pudo crear el pedido.
            </div>
          )}
        </main>

        <footer className="border-t border-slate-800 bg-slate-950/60 px-6 py-5">
          <div className="flex flex-col justify-between gap-4 sm:flex-row sm:items-center">
            <div>
              <p className="text-sm text-slate-500">
                {selectedProducts.length} productos seleccionados
              </p>

              <p className="text-2xl font-bold text-white">
                {total.toLocaleString('es-ES', {
                  style: 'currency',
                  currency: 'EUR',
                })}
              </p>
            </div>

            <div className="flex gap-3">
              <button
                type="button"
                onClick={onClose}
                className="rounded-lg border border-slate-700 px-5 py-3 font-semibold text-slate-300"
              >
                Cancelar
              </button>

              <button
                type="button"
                onClick={handleCreateOrder}
                disabled={
                  selectedProducts.length === 0 ||
                  createOrder.isPending
                }
                className="inline-flex items-center gap-2 rounded-lg bg-blue-600 px-5 py-3 font-semibold text-white disabled:opacity-50"
              >
                <ShoppingCart size={18} />

                {createOrder.isPending
                  ? 'Creando...'
                  : 'Crear pedido'}
              </button>
            </div>
          </div>
        </footer>
      </div>
    </div>
  )
}
