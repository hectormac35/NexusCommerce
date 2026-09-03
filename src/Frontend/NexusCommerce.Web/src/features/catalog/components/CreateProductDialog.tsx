import { useState, type FormEvent } from 'react'
import { AxiosError } from 'axios'
import { useCreateProduct } from '../hooks/useCreateProduct'

type CreateProductDialogProps = {
  isOpen: boolean
  onClose: () => void
}

type ProblemDetails = {
  title?: string
  detail?: string
}

type ProductForm = {
  nombre: string
  descripcion: string
  precio: string
  stock: string
  categoria: string
}

const initialForm: ProductForm = {
  nombre: '',
  descripcion: '',
  precio: '',
  stock: '',
  categoria: '',
}

export function CreateProductDialog({
  isOpen,
  onClose,
}: CreateProductDialogProps) {
  const [form, setForm] = useState<ProductForm>(initialForm)
  const [validationError, setValidationError] = useState<string | null>(null)

  const createProductMutation = useCreateProduct()

  if (!isOpen) {
    return null
  }

  function handleClose() {
    if (createProductMutation.isPending) {
      return
    }

    setForm(initialForm)
    setValidationError(null)
    createProductMutation.reset()
    onClose()
  }

  function updateField(field: keyof ProductForm, value: string) {
    setForm((current) => ({
      ...current,
      [field]: value,
    }))

    setValidationError(null)
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setValidationError(null)

    const nombre = form.nombre.trim()
    const descripcion = form.descripcion.trim()
    const categoria = form.categoria.trim()
    const precio = Number(form.precio)
    const stock = Number(form.stock)

    if (!nombre || !descripcion || !categoria) {
      setValidationError('Completa todos los campos del formulario.')
      return
    }

    if (!Number.isFinite(precio) || precio <= 0) {
      setValidationError('El precio debe ser superior a 0.')
      return
    }

    if (!Number.isInteger(stock) || stock < 0) {
      setValidationError('El stock debe ser un número entero igual o superior a 0.')
      return
    }

    try {
      await createProductMutation.mutateAsync({
        nombre,
        descripcion,
        precio,
        stock,
        categoria,
      })

      setForm(initialForm)
      onClose()
    } catch {
      // El error se muestra debajo del formulario mediante mutation.error.
    }
  }

  const requestError =
    createProductMutation.error instanceof AxiosError
      ? (createProductMutation.error.response?.data as ProblemDetails | undefined)
          ?.detail ??
        (createProductMutation.error.response?.data as ProblemDetails | undefined)
          ?.title ??
        'No se pudo crear el producto.'
      : createProductMutation.error
        ? 'No se pudo conectar con el servicio de catálogo.'
        : null

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 p-4 backdrop-blur-sm"
      role="dialog"
      aria-modal="true"
      aria-labelledby="create-product-title"
      onMouseDown={(event) => {
        if (event.target === event.currentTarget) {
          handleClose()
        }
      }}
    >
      <div className="w-full max-w-2xl rounded-2xl border border-slate-700 bg-slate-900 shadow-2xl">
        <div className="flex items-start justify-between border-b border-slate-800 px-6 py-5">
          <div>
            <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
              Gestión del catálogo
            </p>

            <h2
              id="create-product-title"
              className="mt-1 text-2xl font-bold text-white"
            >
              Nuevo producto
            </h2>

            <p className="mt-2 text-sm text-slate-400">
              Introduce la información del producto que quieres añadir.
            </p>
          </div>

          <button
            type="button"
            onClick={handleClose}
            disabled={createProductMutation.isPending}
            className="rounded-lg px-3 py-2 text-xl text-slate-400 transition hover:bg-slate-800 hover:text-white disabled:cursor-not-allowed disabled:opacity-50"
            aria-label="Cerrar diálogo"
          >
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-5 px-6 py-6">
          <div>
            <label
              htmlFor="product-name"
              className="mb-2 block text-sm font-medium text-slate-300"
            >
              Nombre
            </label>

            <input
              id="product-name"
              type="text"
              value={form.nombre}
              onChange={(event) => updateField('nombre', event.target.value)}
              placeholder="Ejemplo: Teclado mecánico"
              maxLength={150}
              disabled={createProductMutation.isPending}
              className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white outline-none transition placeholder:text-slate-600 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:opacity-60"
            />
          </div>

          <div>
            <label
              htmlFor="product-description"
              className="mb-2 block text-sm font-medium text-slate-300"
            >
              Descripción
            </label>

            <textarea
              id="product-description"
              value={form.descripcion}
              onChange={(event) =>
                updateField('descripcion', event.target.value)
              }
              placeholder="Describe las características principales del producto"
              rows={4}
              maxLength={1000}
              disabled={createProductMutation.isPending}
              className="w-full resize-none rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white outline-none transition placeholder:text-slate-600 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:opacity-60"
            />
          </div>

          <div className="grid gap-5 sm:grid-cols-2">
            <div>
              <label
                htmlFor="product-price"
                className="mb-2 block text-sm font-medium text-slate-300"
              >
                Precio
              </label>

              <div className="relative">
                <input
                  id="product-price"
                  type="number"
                  value={form.precio}
                  onChange={(event) =>
                    updateField('precio', event.target.value)
                  }
                  placeholder="0,00"
                  min="0.01"
                  step="0.01"
                  disabled={createProductMutation.isPending}
                  className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 pr-12 text-white outline-none transition placeholder:text-slate-600 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:opacity-60"
                />

                <span className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-slate-500">
                  €
                </span>
              </div>
            </div>

            <div>
              <label
                htmlFor="product-stock"
                className="mb-2 block text-sm font-medium text-slate-300"
              >
                Stock inicial
              </label>

              <input
                id="product-stock"
                type="number"
                value={form.stock}
                onChange={(event) => updateField('stock', event.target.value)}
                placeholder="0"
                min="0"
                step="1"
                disabled={createProductMutation.isPending}
                className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white outline-none transition placeholder:text-slate-600 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:opacity-60"
              />
            </div>
          </div>

          <div>
            <label
              htmlFor="product-category"
              className="mb-2 block text-sm font-medium text-slate-300"
            >
              Categoría
            </label>

            <input
              id="product-category"
              type="text"
              value={form.categoria}
              onChange={(event) =>
                updateField('categoria', event.target.value)
              }
              placeholder="Ejemplo: Periféricos"
              maxLength={100}
              disabled={createProductMutation.isPending}
              className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white outline-none transition placeholder:text-slate-600 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:opacity-60"
            />
          </div>

          {(validationError || requestError) && (
            <div className="rounded-lg border border-red-500/30 bg-red-500/10 px-4 py-3 text-sm text-red-300">
              {validationError ?? requestError}
            </div>
          )}

          <div className="flex flex-col-reverse gap-3 border-t border-slate-800 pt-5 sm:flex-row sm:justify-end">
            <button
              type="button"
              onClick={handleClose}
              disabled={createProductMutation.isPending}
              className="rounded-lg border border-slate-700 px-5 py-3 font-semibold text-slate-300 transition hover:bg-slate-800 hover:text-white disabled:cursor-not-allowed disabled:opacity-50"
            >
              Cancelar
            </button>

            <button
              type="submit"
              disabled={createProductMutation.isPending}
              className="rounded-lg bg-blue-600 px-5 py-3 font-semibold text-white transition hover:bg-blue-500 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {createProductMutation.isPending
                ? 'Creando producto...'
                : 'Crear producto'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
