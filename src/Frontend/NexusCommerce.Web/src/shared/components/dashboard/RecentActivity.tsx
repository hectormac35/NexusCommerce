import {
  Package,
  User,
  Server,
  Rocket,
} from 'lucide-react'

const events = [
  {
    title: 'Producto creado',
    description: 'Monitor de 27 pulgadas',
    time: 'Hace 2 minutos',
    icon: Package,
  },
  {
    title: 'Inicio de sesión',
    description: 'Administrador',
    time: 'Hace 8 minutos',
    icon: User,
  },
  {
    title: 'RabbitMQ operativo',
    description: 'Todos los consumidores conectados',
    time: 'Hace 15 minutos',
    icon: Server,
  },
  {
    title: 'Nuevo despliegue',
    description: 'Versión 1.0.0',
    time: 'Hace 1 hora',
    icon: Rocket,
  },
]

export function RecentActivity() {
  return (
    <div className="space-y-4">
      {events.map((event) => {
        const Icon = event.icon

        return (
          <div
            key={event.title + event.time}
            className="flex items-start gap-4 rounded-xl border border-slate-800 bg-slate-900/60 p-4"
          >
            <div className="rounded-lg border border-slate-700 bg-slate-800 p-2 text-blue-400">
              <Icon size={18} />
            </div>

            <div className="min-w-0">
              <p className="font-medium text-white">
                {event.title}
              </p>

              <p className="text-sm text-slate-400">
                {event.description}
              </p>

              <p className="mt-1 text-xs text-slate-500">
                {event.time}
              </p>
            </div>
          </div>
        )
      })}
    </div>
  )
}
