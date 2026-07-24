import {
  Activity,
  Boxes,
  Gauge,
  Network,
  Package,
  Settings,
  ShoppingCart,
  Users,
} from 'lucide-react'
import { NavLink } from 'react-router-dom'
import { useAuthStore } from '../../../features/auth/store/authStore'

const navigation = [
  { to: '/', label: 'Dashboard', icon: Gauge },
  { to: '/catalogo', label: 'Productos', icon: Package },
  { to: '/usuarios', label: 'Usuarios', icon: Users },
  { to: '/pedidos', label: 'Pedidos', icon: ShoppingCart },
]

const infrastructure = [
  { to: '/plataforma', label: 'Plataforma', icon: Boxes },
  { to: '/rabbitmq', label: 'RabbitMQ', icon: Network },
  { to: '/trazas', label: 'Jaeger', icon: Activity },
  { to: '/configuracion', label: 'Configuración', icon: Settings },
]

function SidebarLink({
  to,
  label,
  icon: Icon,
}: {
  to: string
  label: string
  icon: typeof Gauge
}) {
  return (
    <NavLink
      to={to}
      end={to === '/'}
      className={({ isActive }) =>
        [
          'flex items-center gap-3 rounded-xl px-3 py-2.5',
          'text-sm font-medium transition',
          isActive
            ? 'bg-blue-600 text-white shadow-lg shadow-blue-950/30'
            : 'text-slate-400 hover:bg-slate-800 hover:text-white',
        ].join(' ')
      }
    >
      <Icon size={19} strokeWidth={1.8} />
      <span>{label}</span>
    </NavLink>
  )
}

export function Sidebar() {
  const usuario = useAuthStore((state) => state.usuario)

  const initials = usuario
    ? `${usuario.nombre.charAt(0)}${usuario.apellidos.charAt(0)}`
    : 'NC'

  return (
    <aside className="fixed inset-y-0 left-0 z-30 hidden w-72 border-r border-slate-800 bg-slate-950 lg:flex lg:flex-col">
      <div className="flex h-20 items-center border-b border-slate-800 px-6">
        <NavLink to="/" className="text-xl font-bold tracking-tight text-white">
          Nexus<span className="text-blue-500">Commerce</span>
        </NavLink>
      </div>

      <div className="flex-1 overflow-y-auto px-4 py-6">
        <p className="px-3 text-xs font-semibold uppercase tracking-[0.18em] text-slate-600">
          Comercio
        </p>

        <nav className="mt-3 space-y-1">
          {navigation.map((item) => (
            <SidebarLink key={item.to} {...item} />
          ))}
        </nav>

        <p className="mt-8 px-3 text-xs font-semibold uppercase tracking-[0.18em] text-slate-600">
          Infraestructura
        </p>

        <nav className="mt-3 space-y-1">
          {infrastructure.map((item) => (
            <SidebarLink key={item.to} {...item} />
          ))}
        </nav>
      </div>

      <div className="border-t border-slate-800 p-4">
        <div className="flex items-center gap-3 rounded-xl bg-slate-900 p-3">
          <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-blue-600 text-sm font-bold text-white">
            {initials}
          </div>

          <div className="min-w-0">
            <p className="truncate text-sm font-semibold text-white">
              {usuario
                ? `${usuario.nombre} ${usuario.apellidos}`
                : 'NexusCommerce'}
            </p>
            <p className="truncate text-xs text-slate-500">
              {usuario?.rol ?? 'Invitado'}
            </p>
          </div>
        </div>
      </div>
    </aside>
  )
}
