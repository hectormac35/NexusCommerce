import type { HTMLAttributes, ReactNode } from 'react'

type CardProps = HTMLAttributes<HTMLDivElement> & {
  children: ReactNode
}

export function Card({
  children,
  className = '',
  ...props
}: CardProps) {
  return (
    <div
      className={[
        'rounded-2xl border border-slate-800 bg-slate-900/80',
        'shadow-sm shadow-black/10',
        className,
      ].join(' ')}
      {...props}
    >
      {children}
    </div>
  )
}
