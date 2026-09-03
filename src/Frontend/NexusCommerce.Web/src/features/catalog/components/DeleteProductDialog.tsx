import { AxiosError } from 'axios'
import { useDeleteProduct } from '../hooks/useDeleteProduct'
import type { Product } from '../types/product'

type DeleteProductDialogProps = {
  product: Product | null
  isOpen: boolean
  onClose: () => void
}

type ProblemDetails = {
  title?: string
  detail?: string
}

export function DeleteProductDialog({
  product,
  isOpen,
  onClose,
}: DeleteProductDialogProps) {
  const deleteProductMutation = useDeleteProduct()

  if (!isOpen || !product) {
    return null
  }

  async function handleDelete() {
    if (!product) {
      return
    }

    try {
      await deleteProductMutation.mutateAsync(product.id)
      onClose()
    } catch {
      // El error se muestra dentro del diálogo.
    }
  }

  const requestError =
    deleteProductMutation.error instanceof AxiosError
      ? ((deleteProductMutation.error.response?.data as
          | ProblemDetails
          | undefined)?.detail ??
        (deleteProductMutation.error.response?.data as
          | ProblemDetails
          | undefined)?.title ??
        'No se pudo desactivar el producto.')
      : deleteProductMutation.error
        ? 'No se pudo conectar con el servicio.'
        : null

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 p-4 backdrop-blur-sm"
      onMouseDown={(event) => {
        if (event.target === event.currentTarget) {
          onClose()
        }
      }}
    >
      <div className="w-full max-w-lg rounded-2xl border border-slate-700 bg-slate-900 shadow-2xl">
        <div className="border-b border-slate-800 px-6 py-5">
          <p className="text-sm font-semibold uppercase tracking-wider text-red-400">
            Acción administrativa
          </p>

          <h2 className="mt-2 text-2xl font-bold text-white">
            Desactivar producto
          </h2>
        </div>

        <div className="space-y-5 px-6 py-6">
          <p className="leading-7 text-slate-300">
            Vas a desactivar el producto{' '}
            <span className="font-semibold text-white">
              {product.nombre}
            </span>
            .
          </p>

          <div className="rounded-xl border border-amber-500/30 bg-amber-500/10 px-4 py-4 text-sm leading-6 text-amber-200">
            El producto dejará de estar disponible en el catálogo.
          </div>

          {requestError && (
            <div className="rounded-xl border border-red-500/30 bg-red-500/10 px-4 py-3 text-sm text-red-300">
              {requestError}
            </div>
          )}

          <div className="flex justify-end gap-3 border-t border-slate-800 pt-5">
            <button
              type="button"
              onClick={onClose}
              disabled={deleteProductMutation.isPending}
              className="rounded-lg border border-slate-700 px-5 py-3 font-semibold text-slate-300 transition hover:bg-slate-800 hover:text-white disabled:cursor-not-allowed disabled:opacity-50"
            >
              Cancelar
            </button>

            <button
              type="button"
              onClick={handleDelete}
              disabled={deleteProductMutation.isPending}
              className="rounded-lg bg-red-600 px-5 py-3 font-semibold text-white transition hover:bg-red-500 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {deleteProductMutation.isPending
                ? 'Desactivando...'
                : 'Desactivar producto'}
            </button>
          </div>
        </div>
      </div>
    </div>
  )
}
