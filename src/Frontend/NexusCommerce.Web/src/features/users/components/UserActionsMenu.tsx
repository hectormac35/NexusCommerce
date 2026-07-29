import { useEffect, useRef, useState } from 'react'
import {
  MoreHorizontal,
  ShieldCheck,
} from 'lucide-react'
import type { User } from '../types/user'

type UserActionsMenuProps = {
  user: User
  onChangeRole: (user: User) => void
}

export function UserActionsMenu({
  user,
  onChangeRole,
}: UserActionsMenuProps) {
  const [isOpen, setIsOpen] = useState(false)
  const containerRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        containerRef.current &&
        !containerRef.current.contains(event.target as Node)
      ) {
        setIsOpen(false)
      }
    }

    document.addEventListener('mousedown', handleClickOutside)

    return () => {
      document.removeEventListener(
        'mousedown',
        handleClickOutside,
      )
    }
  }, [])

  return (
    <div
      ref={containerRef}
      className="relative inline-block"
    >
      <button
        type="button"
        aria-label={`Acciones para ${user.nombreCompleto}`}
        aria-expanded={isOpen}
        onClick={() =>
          setIsOpen((current) => !current)
        }
        className="rounded-lg p-2 text-slate-500 transition hover:bg-slate-800 hover:text-white"
      >
        <MoreHorizontal size={18} />
      </button>

      {isOpen && (
        <div className="absolute right-0 z-30 mt-2 w-48 overflow-hidden rounded-xl border border-slate-700 bg-slate-900 p-1 shadow-2xl">
          <button
            type="button"
            onClick={() => {
              setIsOpen(false)
              onChangeRole(user)
            }}
            className="flex w-full items-center gap-3 rounded-lg px-3 py-2.5 text-left text-sm text-slate-300 transition hover:bg-slate-800 hover:text-white"
          >
            <ShieldCheck size={16} />
            Cambiar rol
          </button>
        </div>
      )}
    </div>
  )
}
