import type { ReactNode } from 'react'

type BadgeVariant = 'neutral' | 'success' | 'warning' | 'danger' | 'primary'

type BadgeProps = {
  children: ReactNode
  variant?: BadgeVariant
}

const variants: Record<BadgeVariant, string> = {
  neutral: 'border-slate-700 bg-slate-800 text-slate-300',
  success: 'border-emerald-500/30 bg-emerald-500/10 text-emerald-300',
  warning: 'border-amber-500/30 bg-amber-500/10 text-amber-300',
  danger: 'border-red-500/30 bg-red-500/10 text-red-300',
  primary: 'border-blue-500/30 bg-blue-500/10 text-blue-300',
}

export function Badge({
  children,
  variant = 'neutral',
}: BadgeProps) {
  return (
    <span
      className={[
        'inline-flex items-center rounded-full border px-2.5 py-1',
        'text-xs font-semibold',
        variants[variant],
      ].join(' ')}
    >
      {children}
    </span>
  )
}
