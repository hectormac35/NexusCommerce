import { AxiosError } from 'axios'
import { useProducts } from '../hooks/useProducts'

function formatPrice(price: number) {
  return new Intl.NumberFormat('es-ES', {
    style: 'currency',
    currency: 'EUR',
  }).format(price)
}

export function CatalogPage() {
  const { data: products, isLoading, isError, error, refetch } = useProducts()

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
    <section>
      <div className="flex flex-col justify-between gap-4 sm:flex-row sm:items-end">
        <div>
          <p className="text-sm font-semibold uppercase tracking-wider text-blue-400">
            Productos
          </p>

          <h1 className="mt-2 text-4xl font-bold text-white">Catálogo</h1>

          <p className="mt-3 text-slate-400">
            Productos obtenidos mediante el API Gateway de NexusCommerce.
          </p>
        </div>

        <div className="rounded-lg border border-slate-800 bg-slate-900 px-4 py-2 text-sm text-slate-300">
          {products?.length ?? 0} productos
        </div>
      </div>

      {!products?.length ? (
        <div className="mt-10 rounded-2xl border border-dashed border-slate-700 bg-slate-900/50 p-12 text-center">
          <p className="text-lg font-medium text-slate-300">
            No hay productos disponibles
          </p>

          <p className="mt-2 text-sm text-slate-500">
            Crea productos desde la API para que aparezcan aquí.
          </p>
        </div>
      ) : (
        <div className="mt-10 grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {products.map((product) => (
            <article
              key={product.id}
              className="group flex flex-col rounded-2xl border border-slate-800 bg-slate-900 p-6 transition hover:-translate-y-1 hover:border-blue-500/50"
            >
              <div className="flex items-start justify-between gap-4">
                <span className="rounded-full bg-blue-500/10 px-3 py-1 text-xs font-semibold text-blue-300">
                  {product.categoria}
                </span>

                <span
                  className={
                    product.tieneStock
                      ? 'text-xs font-semibold text-emerald-400'
                      : 'text-xs font-semibold text-red-400'
                  }
                >
                  {product.tieneStock ? 'Disponible' : 'Sin stock'}
                </span>
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
        </div>
      )}
    </section>
  )
}
