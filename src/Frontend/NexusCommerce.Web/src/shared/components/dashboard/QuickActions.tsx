import {
  ArrowRight,
  PackagePlus,
  UserPlus,
  Boxes,
  Activity,
} from 'lucide-react'

const actions = [
  {
    title: 'Nuevo producto',
    description: 'Crear un producto en el catálogo',
    icon: PackagePlus,
  },
  {
    title: 'Nuevo usuario',
    description: 'Registrar un nuevo usuario',
    icon: UserPlus,
  },
  {
    title: 'Ver catálogo',
    description: 'Consultar todos los productos',
    icon: Boxes,
  },
  {
    title: 'Platform Health',
    description: 'Consultar el estado de los servicios',
    icon: Activity,
  },
]

export function QuickActions() {
  return (
    <div className="space-y-3">
      {actions.map((action) => {
        const Icon = action.icon

        return (
          <button
            key={action.title}
            className="group flex w-full items-center justify-between rounded-xl border border-slate-800 bg-slate-900/60 p-4 text-left transition hover:border-blue-500/30 hover:bg-slate-800"
          >
            <div className="flex items-center gap-4">
              <div className="rounded-lg border border-blue-500/20 bg-blue-500/10 p-2 text-blue-400">
                <Icon size={18} />
              </div>

              <div>
                <p className="font-medium text-white">
                  {action.title}
                </p>

                <p className="text-sm text-slate-500">
                  {action.description}
                </p>
              </div>
            </div>

            <ArrowRight
              size={18}
              className="text-slate-600 transition group-hover:translate-x-1 group-hover:text-blue-400"
            />
          </button>
        )
      })}
    </div>
  )
}
