import {
  Activity,
  ArrowRight,
  Boxes,
  CircleCheck,
  Package,
  Server,
  ShoppingCart,
  Users,
  Warehouse,
} from 'lucide-react'
import { useProducts } from '../../catalog/hooks/useProducts'
import { useAuthStore } from '../../auth/store/authStore'
import { Badge } from '../../../shared/ui/badge/Badge'
import { Card } from '../../../shared/ui/card/Card'
import { StatCard } from '../../../shared/components/data-display/StatCard'
import { QuickActions } from '../../../shared/components/dashboard/QuickActions'
import { RecentActivity } from '../../../shared/components/dashboard/RecentActivity'

const services = [
  {
    name: 'API Gateway',
    description: 'Enrutamiento mediante YARP',
    status: 'Operativo',
    latency: '18 ms',
  },
  {
    name: 'Identity API',
    description: 'Autenticación y usuarios',
    status: 'Operativo',
    latency: '24 ms',
  },
  {
    name: 'Catalog API',
    description: 'Productos e inventario',
    status: 'Operativo',
    latency: '31 ms',
  },
  {
    name: 'RabbitMQ',
    description: 'Mensajería asíncrona',
    status: 'Operativo',
    latency: '12 ms',
  },
]

export function HomePage() {
  const { data: products = [], isLoading, isError } = useProducts()
  const usuario = useAuthStore((state) => state.usuario)

  const activeProducts = products.filter((product) => product.estaActivo).length
  const totalStock = products.reduce((total, product) => total + product.stock, 0)
  const lowStockProducts = products.filter(
    (product) => product.stock > 0 && product.stock <= 5,
  ).length

  return (
    <div className="mx-auto max-w-7xl space-y-8">
      <header className="flex flex-col gap-5 lg:flex-row lg:items-end lg:justify-between">
        <div>
          <div className="flex items-center gap-2">
            <span className="h-2 w-2 rounded-full bg-blue-400 shadow-[0_0_12px_rgba(96,165,250,0.7)]" />
            <p className="text-sm font-semibold text-blue-400">
              Vista general
            </p>
          </div>

          <h1 className="mt-3 text-3xl font-bold tracking-tight text-white sm:text-4xl">
            Dashboard
          </h1>

          <p className="mt-2 max-w-2xl text-slate-400">
            Bienvenido{usuario ? `, ${usuario.nombre}` : ''}. Consulta el estado
            operativo, el catálogo y la actividad principal de NexusCommerce.
          </p>
        </div>

        <div className="flex items-center gap-3 self-start rounded-xl border border-emerald-500/20 bg-emerald-500/5 px-4 py-3 lg:self-auto">
          <CircleCheck size={18} className="text-emerald-400" />

          <div>
            <p className="text-sm font-semibold text-emerald-300">
              Plataforma operativa
            </p>
            <p className="text-xs text-slate-500">
              Todos los servicios responden
            </p>
          </div>
        </div>
      </header>

      <section className="grid gap-5 sm:grid-cols-2 xl:grid-cols-4">
        <StatCard
          title="Productos"
          value={products.length}
          description={`${activeProducts} productos activos`}
          icon={Package}
          accent="blue"
        />

        <StatCard
          title="Stock disponible"
          value={totalStock}
          description={`${lowStockProducts} productos con stock bajo`}
          icon={Warehouse}
          accent="emerald"
        />

        <StatCard
          title="Usuarios"
          value={usuario ? 1 : 0}
          description="Sesión autenticada actualmente"
          icon={Users}
          accent="violet"
        />

        <StatCard
          title="Pedidos"
          value={0}
          description="Módulo planificado"
          icon={ShoppingCart}
          accent="amber"
        />
      </section>

      <section className="grid gap-6 xl:grid-cols-[1.45fr_1fr]">
        <Card className="overflow-hidden">
          <div className="flex flex-col gap-4 border-b border-slate-800 p-6 sm:flex-row sm:items-center sm:justify-between">
            <div>
              <div className="flex items-center gap-2">
                <Package size={18} className="text-blue-400" />
                <h2 className="font-semibold text-white">
                  Productos recientes
                </h2>
              </div>

              <p className="mt-1 text-sm text-slate-500">
                Información obtenida mediante el API Gateway
              </p>
            </div>

            <Badge variant="primary">
              {products.length} productos
            </Badge>
          </div>

          <div className="p-3 sm:p-4">
            {isLoading && (
              <div className="flex min-h-64 items-center justify-center">
                <div className="text-center">
                  <Activity
                    size={28}
                    className="mx-auto animate-pulse text-blue-400"
                  />
                  <p className="mt-3 text-sm text-slate-400">
                    Cargando productos...
                  </p>
                </div>
              </div>
            )}

            {isError && (
              <div className="flex min-h-64 items-center justify-center">
                <p className="text-sm text-red-300">
                  No se ha podido cargar el catálogo.
                </p>
              </div>
            )}

            {!isLoading && !isError && products.length === 0 && (
              <div className="flex min-h-64 items-center justify-center">
                <div className="text-center">
                  <Boxes size={30} className="mx-auto text-slate-600" />
                  <p className="mt-3 font-medium text-slate-300">
                    No hay productos
                  </p>
                  <p className="mt-1 text-sm text-slate-500">
                    El catálogo todavía está vacío.
                  </p>
                </div>
              </div>
            )}

            {!isLoading && !isError && products.length > 0 && (
              <div className="divide-y divide-slate-800">
                {products.slice(0, 5).map((product) => (
                  <div
                    key={product.id}
                    className="group flex flex-col gap-4 rounded-xl px-3 py-4 transition hover:bg-slate-800/40 sm:flex-row sm:items-center sm:justify-between"
                  >
                    <div className="flex min-w-0 items-center gap-4">
                      <div className="flex h-11 w-11 shrink-0 items-center justify-center rounded-xl border border-slate-700 bg-slate-800 text-slate-300">
                        <Package size={20} />
                      </div>

                      <div className="min-w-0">
                        <p className="truncate font-semibold text-white">
                          {product.nombre}
                        </p>
                        <p className="mt-1 truncate text-sm text-slate-500">
                          {product.categoria}
                        </p>
                      </div>
                    </div>

                    <div className="flex items-center justify-between gap-6 pl-15 sm:pl-0">
                      <div className="text-left sm:text-right">
                        <p className="font-semibold text-white">
                          {product.precio.toLocaleString('es-ES', {
                            style: 'currency',
                            currency: 'EUR',
                          })}
                        </p>
                        <p className="mt-1 text-xs text-slate-500">
                          Stock: {product.stock}
                        </p>
                      </div>

                      <ArrowRight
                        size={17}
                        className="text-slate-600 transition group-hover:translate-x-0.5 group-hover:text-blue-400"
                      />
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </Card>

        <Card className="overflow-hidden">
          <div className="border-b border-slate-800 p-6">
            <div className="flex items-center gap-2">
              <Server size={18} className="text-emerald-400" />
              <h2 className="font-semibold text-white">
                Estado de la plataforma
              </h2>
            </div>

            <p className="mt-1 text-sm text-slate-500">
              Servicios principales de NexusCommerce
            </p>
          </div>

          <div className="space-y-2 p-4">
            {services.map((service) => (
              <div
                key={service.name}
                className="rounded-xl border border-transparent p-4 transition hover:border-slate-800 hover:bg-slate-800/40"
              >
                <div className="flex items-center justify-between gap-4">
                  <div className="flex min-w-0 items-center gap-3">
                    <span className="h-2.5 w-2.5 shrink-0 rounded-full bg-emerald-400 shadow-[0_0_12px_rgba(52,211,153,0.55)]" />

                    <div className="min-w-0">
                      <p className="truncate font-medium text-slate-200">
                        {service.name}
                      </p>
                      <p className="mt-1 truncate text-xs text-slate-500">
                        {service.description}
                      </p>
                    </div>
                  </div>

                  <div className="shrink-0 text-right">
                    <Badge variant="success">{service.status}</Badge>
                    <p className="mt-2 text-xs text-slate-600">
                      {service.latency}
                    </p>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </Card>
      </section>

      <section className="grid gap-6 xl:grid-cols-[1.25fr_1fr]">
        <Card className="overflow-hidden">
          <div className="border-b border-slate-800 p-6">
            <h2 className="font-semibold text-white">
              Actividad reciente
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              Últimos eventos relevantes de la plataforma
            </p>
          </div>

          <div className="p-4">
            <RecentActivity />
          </div>
        </Card>

        <Card className="overflow-hidden">
          <div className="border-b border-slate-800 p-6">
            <h2 className="font-semibold text-white">
              Acciones rápidas
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              Accesos directos a las operaciones habituales
            </p>
          </div>

          <div className="p-4">
            <QuickActions />
          </div>
        </Card>
      </section>

      <Card className="overflow-hidden">
        <div className="grid gap-px bg-slate-800 sm:grid-cols-3">
          <div className="bg-slate-900 p-6">
            <p className="text-sm text-slate-500">Arquitectura</p>
            <p className="mt-2 font-semibold text-white">
              Microservicios distribuidos
            </p>
          </div>

          <div className="bg-slate-900 p-6">
            <p className="text-sm text-slate-500">Infraestructura</p>
            <p className="mt-2 font-semibold text-white">
              Kubernetes + Docker
            </p>
          </div>

          <div className="bg-slate-900 p-6">
            <p className="text-sm text-slate-500">Observabilidad</p>
            <p className="mt-2 font-semibold text-white">
              OpenTelemetry + Jaeger
            </p>
          </div>
        </div>
      </Card>
    </div>
  )
}
