import { useState } from 'react'
import { AxiosError } from 'axios'
import { CreateProductDialog } from '../components/CreateProductDialog'
import { EditProductDialog } from '../components/EditProductDialog'
import { DeleteProductDialog } from '../components/DeleteProductDialog'
import { ProductActionsMenu } from '../components/ProductActionsMenu'
import { useProducts } from '../hooks/useProducts'
import type { Product } from '../types/product'

function formatPrice(price: number) {
  return new Intl.NumberFormat('es-ES', {
    style: 'currency',
    currency: 'EUR',
  }).format(price)
}

export function CatalogPage() {
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false)
  const [searchTerm, setSearchTerm] = useState('')
  const [selectedCategory, setSelectedCategory] = useState('Todas')



  const [editingProduct, setEditingProduct] = useState<Product | null>(null)
  const [deletingProduct, setDeletingProduct] = useState<Product | null>(null)

  const {
    data: products,
    isLoading,
    isError,
    error,
    refetch,
  } = useProducts()

  const categories = [
    'Todas',
    ...Array.from(
      new Set((products ?? []).map((product) => product.categoria)),
    ).sort(),
  ]

  const filteredProducts = (products ?? []).filter((product) => {
    const normalizedSearch = searchTerm.trim().toLowerCase()

    const matchesSearch =
      !normalizedSearch ||
      product.nombre.toLowerCase().includes(normalizedSearch) ||
      product.descripcion.toLowerCase().includes(normalizedSearch)

    const matchesCategory =
      selectedCategory === 'Todas' ||
      product.categoria === selectedCategory

    return matchesSearch && matchesCategory
  })

  if (isLoading) {
    return (
      <section>
        <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
          Productos
        </p>

        <h1 className="mt-2 text-4xl font-bold text-white">Catálogo</h1>

        <div className="mt-10 grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {Array.from({ length: 6 }).map((_, index) => (
            <div
              key={index}
              className="h-64 animate-pulse rounded-2xl border border-slate-800 bg-slate-900"
            />
          ))}
        </div>
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
          No se pudo cargar el catálogo
        </h1>

        <p className="mt-4 max-w-2xl text-slate-400">
          {status
            ? `El API Gateway respondió con el código ${status}.`
            : 'Comprueba que el Gateway y el servicio de catálogo estén funcionando.'}
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
    <>
      <section>
        <div className="flex flex-col justify-between gap-5 sm:flex-row sm:items-end">
          <div>
            <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
              Productos
            </p>

            <h1 className="mt-2 text-4xl font-bold text-white">Catálogo</h1>

            <p className="mt-3 text-slate-400">
              Gestiona los productos disponibles a través del API Gateway de
              NexusCommerce.
            </p>
          </div>

          <div className="flex flex-col gap-3 sm:flex-row sm:items-center">
            <div className="rounded-lg border border-slate-800 bg-slate-900 px-4 py-2 text-center text-sm text-slate-300">
              {products?.length ?? 0} productos
            </div>

            <button
              type="button"
              onClick={() => setIsCreateDialogOpen(true)}
              className="rounded-lg bg-blue-600 px-5 py-3 font-semibold text-white transition hover:bg-blue-500"
            >
              Nuevo producto
            </button>
          </div>
        </div>

        <div className="mt-8 grid gap-4 rounded-2xl border border-slate-800 bg-slate-900 p-5 md:grid-cols-[1fr_240px]">
          <div>
            <label
              htmlFor="catalog-search"
              className="mb-2 block text-sm font-medium text-slate-300"
            >
              Buscar productos
            </label>

            <input
              id="catalog-search"
              type="search"
              value={searchTerm}
              onChange={(event) => setSearchTerm(event.target.value)}
              placeholder="Nombre o descripción..."
              className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white outline-none transition placeholder:text-slate-600 focus:border-blue-500"
            />
          </div>

          <div>
            <label
              htmlFor="catalog-category"
              className="mb-2 block text-sm font-medium text-slate-300"
            >
              Categoría
            </label>

            <select
              id="catalog-category"
              value={selectedCategory}
              onChange={(event) => setSelectedCategory(event.target.value)}
              className="w-full rounded-lg border border-slate-700 bg-slate-950 px-4 py-3 text-white outline-none transition focus:border-blue-500"
            >
              {categories.map((category) => (
                <option key={category} value={category}>
                  {category}
                </option>
              ))}
            </select>
          </div>
        </div>

        {!products?.length ? (
          <div className="mt-10 rounded-2xl border border-dashed border-slate-700 bg-slate-900/50 p-12 text-center">
            <p className="text-lg font-medium text-slate-300">
              No hay productos disponibles
            </p>

            <p className="mt-2 text-sm text-slate-500">
              Crea el primer producto para comenzar a gestionar el catálogo.
            </p>

            <button
              type="button"
              onClick={() => setIsCreateDialogOpen(true)}
              className="mt-6 rounded-lg bg-blue-600 px-5 py-3 font-semibold text-white transition hover:bg-blue-500"
            >
              Crear primer producto
            </button>
          </div>
        ) : filteredProducts.length === 0 ? (
          <div className="mt-10 rounded-2xl border border-dashed border-slate-700 bg-slate-900/50 p-12 text-center">
            <p className="text-lg font-medium text-slate-300">
              No se encontraron productos
            </p>

            <p className="mt-2 text-sm text-slate-500">
              Prueba con otro término de búsqueda o cambia la categoría.
            </p>

            <button
              type="button"
              onClick={() => {
                setSearchTerm('')
                setSelectedCategory('Todas')
              }}
              className="mt-6 rounded-lg border border-slate-700 px-5 py-3 font-semibold text-slate-300 transition hover:bg-slate-800 hover:text-white"
            >
              Limpiar filtros
            </button>
          </div>
        ) : (
          <div className="mt-10 grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
            {filteredProducts.map((product) => (
              <article
                key={product.id}
                className="group flex flex-col rounded-2xl border border-slate-800 bg-slate-900 p-6 transition hover:-translate-y-1 hover:border-blue-500/50"
              >
                <div className="flex items-start justify-between gap-4">
                  <span className="rounded-full bg-blue-500/10 px-3 py-1 text-xs font-semibold text-blue-300">
                    {product.categoria}
                  </span>

                  <div className="flex items-center gap-3">
                    <span
                      className={
                        product.tieneStock
                          ? 'text-xs font-semibold text-emerald-400'
                          : 'text-xs font-semibold text-red-400'
                      }
                    >
                      {product.tieneStock ? 'Disponible' : 'Sin stock'}
                    </span>

                    <ProductActionsMenu
                      productName={product.nombre}
                      onEdit={() => setEditingProduct(product)}
                      onDeactivate={() => setDeletingProduct(product)}
                    />
                  </div>
                </div>

                <h2 className="mt-5 text-xl font-bold text-white">
                  {product.nombre}
                </h2>

                <p className="mt-3 flex-1 text-sm leading-6 text-slate-400">
                  {product.descripcion}
                </p>

                <div className="mt-6 flex items-end justify-between border-t border-slate-800 pt-5">
                  <div>
                    <p className="text-xs text-slate-500">Precio</p>

                    <p className="text-2xl font-bold text-white">
                      {formatPrice(product.precio)}
                    </p>
                  </div>

                  <div className="text-right">
                    <p className="text-xs text-slate-500">Stock</p>

                    <p className="font-semibold text-slate-300">
                      {product.stock} unidades
                    </p>
                  </div>
                </div>
              </article>
            ))}


      <EditProductDialog
        product={editingProduct}
        isOpen={editingProduct !== null}
        onClose={() => setEditingProduct(null)}
      />
</div>
        )}
      </section>

      <DeleteProductDialog
        product={deletingProduct}
        isOpen={deletingProduct !== null}
        onClose={() => setDeletingProduct(null)}
      />

      <CreateProductDialog
        isOpen={isCreateDialogOpen}
        onClose={() => setIsCreateDialogOpen(false)}
      />
    </>
  )
}
