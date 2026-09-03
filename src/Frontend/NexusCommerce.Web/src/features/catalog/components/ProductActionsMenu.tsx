import { useEffect, useRef, useState } from 'react'

type ProductActionsMenuProps = {
  productName: string
  onEdit: () => void
  onDeactivate: () => void
}

export function ProductActionsMenu({
  productName,
  onEdit,
  onDeactivate,
}: ProductActionsMenuProps) {
  const [isOpen, setIsOpen] = useState(false)
  const menuRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        menuRef.current &&
        !menuRef.current.contains(event.target as Node)
      ) {
        setIsOpen(false)
      }
    }

    function handleEscape(event: KeyboardEvent) {
      if (event.key === 'Escape') {
        setIsOpen(false)
      }
    }

    document.addEventListener('mousedown', handleClickOutside)
    document.addEventListener('keydown', handleEscape)

    return () => {
      document.removeEventListener('mousedown', handleClickOutside)
      document.removeEventListener('keydown', handleEscape)
    }
  }, [])

  function handleEdit() {
    setIsOpen(false)
    onEdit()
  }

  function handleDeactivate() {
    setIsOpen(false)
    onDeactivate()
  }

  return (
    <div ref={menuRef} className="relative">
      <button
        type="button"
        onClick={() => setIsOpen((current) => !current)}
        className="flex h-9 w-9 items-center justify-center rounded-lg text-xl font-bold text-slate-400 transition hover:bg-slate-800 hover:text-white"
        aria-label={`Abrir acciones de ${productName}`}
        aria-expanded={isOpen}
        aria-haspopup="menu"
      >
        ⋮
      </button>

      {isOpen && (
        <div
          role="menu"
          className="absolute right-0 top-11 z-20 w-48 overflow-hidden rounded-xl border border-slate-700 bg-slate-900 p-1 shadow-2xl"
        >
          <button
            type="button"
            role="menuitem"
            onClick={handleEdit}
            className="flex w-full items-center gap-3 rounded-lg px-4 py-3 text-left text-sm font-medium text-slate-300 transition hover:bg-slate-800 hover:text-white"
          >
            <span aria-hidden="true">✏️</span>
            Editar producto
          </button>

          <button
            type="button"
            role="menuitem"
            onClick={handleDeactivate}
            className="flex w-full items-center gap-3 rounded-lg px-4 py-3 text-left text-sm font-medium text-red-300 transition hover:bg-red-500/10 hover:text-red-200"
          >
            <span aria-hidden="true">🗑️</span>
            Desactivar
          </button>
        </div>
      )}
    </div>
  )
}
