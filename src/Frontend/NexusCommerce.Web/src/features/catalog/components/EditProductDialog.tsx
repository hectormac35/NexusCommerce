import { useEffect, useState, type FormEvent } from 'react'
import { AxiosError } from 'axios'
import { useUpdateProduct } from '../hooks/useUpdateProduct'
import type { Product } from '../types/product'

type EditProductDialogProps = {
  product: Product | null
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
  categoria: string
}

const initialForm: ProductForm = {
  nombre: '',
  descripcion: '',
  precio: '',
  categoria: '',
}

export function EditProductDialog({
  product,
  isOpen,
  onClose,
}: EditProductDialogProps) {
  const [form, setForm] = useState(initialForm)
  const [validationError, setValidationError] = useState<string | null>(null)

  const updateProductMutation = useUpdateProduct()

  useEffect(() => {
    if (!product) {
      setForm(initialForm)
      return
    }

    setForm({
      nombre: product.nombre,
      descripcion: product.descripcion,
      precio: product.precio.toString(),
      categoria: product.categoria,
    })
  }, [product])

  if (!isOpen || !product) {
    return null
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

    if (!product) {
      return
    }

    const nombre = form.nombre.trim()
    const descripcion = form.descripcion.trim()
    const categoria = form.categoria.trim()
    const precio = Number(form.precio)

    if (!nombre || !descripcion || !categoria) {
      setValidationError('Completa todos los campos.')
      return
    }

    if (!Number.isFinite(precio) || precio <= 0) {
      setValidationError('El precio debe ser mayor que 0.')
      return
    }

    try {
      await updateProductMutation.mutateAsync({
        productId: product.id,
        request: {
          nombre,
          descripcion,
          precio,
          categoria,
        },
      })

      onClose()
    } catch {
      // El error se muestra debajo del formulario.
    }
  }

  const requestError =
    updateProductMutation.error instanceof AxiosError
      ? ((updateProductMutation.error.response?.data as ProblemDetails | undefined)
          ?.detail ??
        (updateProductMutation.error.response?.data as ProblemDetails | undefined)
          ?.title ??
        'No se pudo actualizar el producto.')
      : updateProductMutation.error
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
      <div className="w-full max-w-2xl rounded-2xl border border-slate-700 bg-slate-900">
        <div className="border-b border-slate-800 px-6 py-5">
          <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
            Gestión del catálogo
          </p>

          <h2 className="mt-2 text-2xl font-bold text-white">
            Editar producto
          </h2>
        </div>

        <form
          onSubmit={handleSubmit}
          className="space-y-5 px-6 py-6"
        >
          <input
            value={form.nombre}
            onChange={(e) => updateField('nombre', e.target.value)}
            placeholder="Nombre"
            className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white"
          />

          <textarea
            rows={4}
            value={form.descripcion}
            onChange={(e) =>
              updateField('descripcion', e.target.value)
            }
            placeholder="Descripción"
            className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white"
          />

          <input
            type="number"
            step="0.01"
            value={form.precio}
            onChange={(e) =>
              updateField('precio', e.target.value)
            }
            placeholder="Precio"
            className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white"
          />

          <input
            value={form.categoria}
            onChange={(e) =>
              updateField('categoria', e.target.value)
            }
            placeholder="Categoría"
            className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white"
          />

          {(validationError || requestError) && (
            <div className="rounded-lg border border-red-500/30 bg-red-500/10 px-4 py-3 text-sm text-red-300">
              {validationError ?? requestError}
            </div>
          )}

          <div className="flex justify-end gap-3 border-t border-slate-800 pt-5">
            <button
              type="button"
              onClick={onClose}
              className="rounded-lg border border-slate-700 px-5 py-3 text-slate-300"
            >
              Cancelar
            </button>

            <button
              type="submit"
              disabled={updateProductMutation.isPending}
              className="rounded-lg bg-blue-600 px-5 py-3 font-semibold text-white"
            >
              {updateProductMutation.isPending
                ? 'Guardando...'
                : 'Guardar cambios'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
