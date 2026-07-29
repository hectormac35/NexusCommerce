import {
  Activity,
  ArrowRight,
  Boxes,
  PackagePlus,
  UserPlus,
} from 'lucide-react'
import { useNavigate } from 'react-router-dom'

const actions = [
  {
    title: 'Nuevo producto',
    description: 'Crear un producto en el catálogo',
    icon: PackagePlus,
    path: '/catalogo',
    enabled: true,
  },
  {
    title: 'Nuevo usuario',
    description: 'Gestión de usuarios próximamente',
    icon: UserPlus,
    path: null,
    enabled: false,
  },
  {
    title: 'Ver catálogo',
    description: 'Consultar todos los productos',
    icon: Boxes,
    path: '/catalogo',
    enabled: true,
  },
  {
    title: 'Salud de la plataforma',
    description: 'Monitorización próximamente',
    icon: Activity,
    path: null,
    enabled: false,
  },
]

export function QuickActions() {
  const navigate = useNavigate()

  return (
    <div className="space-y-3">
      {actions.map((action) => {
        const Icon = action.icon

        return (
          <button
            key={action.title}
            type="button"
            disabled={!action.enabled}
            onClick={() => {
              if (action.path) {
                navigate(action.path)
              }
            }}
            className={[
              'group flex w-full items-center justify-between rounded-xl border p-4 text-left transition',
              action.enabled
                ? 'border-slate-800 bg-slate-900/60 hover:border-blue-500/30 hover:bg-slate-800'
                : 'cursor-not-allowed border-slate-800/70 bg-slate-900/30 opacity-60',
            ].join(' ')}
          >
            <div className="flex min-w-0 items-center gap-4">
              <div className="shrink-0 rounded-lg border border-blue-500/20 bg-blue-500/10 p-2 text-blue-400">
                <Icon size={18} />
              </div>

              <div className="min-w-0">
                <div className="flex flex-wrap items-center gap-2">
                  <p className="font-medium text-white">{action.title}</p>

                  {!action.enabled && (
                    <span className="rounded-full border border-slate-700 bg-slate-800 px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide text-slate-400">
                      Próximamente
                    </span>
                  )}
                </div>

                <p className="mt-0.5 text-sm text-slate-500">
                  {action.description}
                </p>
              </div>
            </div>

            <ArrowRight
              size={18}
              className={[
                'shrink-0 transition',
                action.enabled
                  ? 'text-slate-600 group-hover:translate-x-1 group-hover:text-blue-400'
                  : 'text-slate-700',
              ].join(' ')}
            />
          </button>
        )
      })}
    </div>
  )
}
