import type { LucideIcon } from 'lucide-react'
import { Card } from '../../ui/card/Card'

type StatCardProps = {
  title: string
  value: string | number
  description: string
  icon: LucideIcon
  accent?: 'blue' | 'emerald' | 'violet' | 'amber'
}

const accentStyles = {
  blue: 'border-blue-500/20 bg-blue-500/10 text-blue-400',
  emerald: 'border-emerald-500/20 bg-emerald-500/10 text-emerald-400',
  violet: 'border-violet-500/20 bg-violet-500/10 text-violet-400',
  amber: 'border-amber-500/20 bg-amber-500/10 text-amber-400',
}

export function StatCard({
  title,
  value,
  description,
  icon: Icon,
  accent = 'blue',
}: StatCardProps) {
  return (
    <Card className="group relative overflow-hidden p-6 transition duration-200 hover:-translate-y-0.5 hover:border-slate-700">
      <div className="relative flex items-start justify-between gap-4">
        <div className="min-w-0">
          <p className="text-sm font-medium text-slate-400">{title}</p>

          <p className="mt-3 text-3xl font-bold tracking-tight text-white">
            {value}
          </p>

          <p className="mt-2 truncate text-sm text-slate-500">
            {description}
          </p>
        </div>

        <div
          className={[
            'rounded-xl border p-3 transition duration-200 group-hover:scale-105',
            accentStyles[accent],
          ].join(' ')}
        >
          <Icon size={22} strokeWidth={1.8} />
        </div>
      </div>
    </Card>
  )
}
