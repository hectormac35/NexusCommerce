import {
  Activity,
  Database,
  Globe,
  Package,
  Rabbit,
  UserRound,
} from 'lucide-react'

import { Badge } from '../../../shared/ui/badge/Badge'
import { Card } from '../../../shared/ui/card/Card'

import type { ServiceHealth } from '../types/platformHealth'

type Props = {
  service: ServiceHealth
  description: string
}

export function ServiceCard({
  service,
  description,
}: Props) {
  const healthy = service.status === 'Healthy'

  const latency = service.responseTimeMs

  const percentage = Math.min((latency / 150) * 100, 100)

  const barColor =
    latency < 50
      ? 'bg-emerald-400'
      : latency < 150
      ? 'bg-amber-400'
      : 'bg-red-400'

  const serviceConfig: Record<
    string,
    {
      icon: typeof Globe
      title: string
      description: string
    }
  > = {
    Gateway: {
      icon: Globe,
      title: 'Gateway',
      description: 'API Gateway y Reverse Proxy',
    },
    Identity: {
      icon: UserRound,
      title: 'Identity',
      description: 'Autenticación y autorización',
    },
    Catalog: {
      icon: Package,
      title: 'Catalog',
      description: 'Productos e inventario',
    },
    RabbitMQ: {
      icon: Rabbit,
      title: 'RabbitMQ',
      description: 'Mensajería asíncrona',
    },
    Jaeger: {
      icon: Activity,
      title: 'Jaeger',
      description: 'Distributed Tracing',
    },
    PostgreSQL: {
      icon: Database,
      title: 'PostgreSQL',
      description: 'Base de datos',
    },
  }

  const config =
    serviceConfig[service.name] ?? {
      icon: Globe,
      title: service.name,
      description,
    }

  const Icon = config.icon

  return (
    <Card className="group p-5 transition hover:-translate-y-0.5 hover:border-slate-700">
      <div className="flex items-start justify-between gap-4">
        <div className="flex items-start gap-4">
          <div
            className={[
              'rounded-xl border p-3',
              healthy
                ? 'border-emerald-500/20 bg-emerald-500/10 text-emerald-400'
                : 'border-red-500/20 bg-red-500/10 text-red-400',
            ].join(' ')}
          >
            <Icon size={20} />
          </div>

          <div>
            <h3 className="font-semibold text-white">
              {config.title}
            </h3>

            <p className="mt-1 text-sm text-slate-500">
              {config.description}
            </p>
          </div>
        </div>

        <Badge
          variant={healthy ? 'success' : 'danger'}
        >
          {service.status}
        </Badge>
      </div>

      <div className="mt-6">
        <div className="mb-2 flex items-center justify-between">
          <span className="text-sm text-slate-500">
            Latencia
          </span>

          <span className="font-semibold text-white">
            {latency} ms
          </span>
        </div>

        <div className="h-2 overflow-hidden rounded-full bg-slate-800">
          <div
            className={[
              'h-full rounded-full transition-all',
              barColor,
            ].join(' ')}
            style={{
              width: `${percentage}%`,
            }}
          />
        </div>
      </div>
    </Card>
  )
}
