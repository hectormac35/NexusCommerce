import {
  Bell,
  LogOut,
  Search,
  Settings,
  UserRound,
} from 'lucide-react'
import { useNavigate } from 'react-router-dom'

import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { Input } from '@/components/ui/input'
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip'
import { useAuthStore } from '@/features/auth/store/authStore'

export function Topbar() {
  const navigate = useNavigate()

  const usuario = useAuthStore((state) => state.usuario)
  const limpiarSesion = useAuthStore((state) => state.limpiarSesion)

  const iniciales = usuario
    ? `${usuario.nombre.charAt(0)}${usuario.apellidos.charAt(0)}`
    : 'NC'

  function cerrarSesion() {
    limpiarSesion()
    navigate('/acceso')
  }

  return (
    <header className="sticky top-0 z-20 border-b border-slate-800 bg-slate-950/95 backdrop-blur-xl">
      <div className="flex h-20 items-center justify-between gap-4 px-6 lg:px-8">
        <div className="hidden max-w-md flex-1 md:block">
          <div className="relative">
            <Search
              size={18}
              className="pointer-events-none absolute left-4 top-1/2 -translate-y-1/2 text-slate-500"
            />

            <Input
              type="search"
              placeholder="Buscar productos, usuarios o pedidos..."
              className="h-11 rounded-2xl border-slate-700 bg-slate-900/80 pl-11 text-slate-200 placeholder:text-slate-500 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20"
            />
          </div>
        </div>

        <div className="ml-auto flex items-center gap-2">
          <Tooltip>
            <TooltipTrigger
              render={
                <Button
                  variant="ghost"
                  size="icon"
                  aria-label="Notificaciones"
                  className="rounded-xl border border-slate-800 bg-slate-900/60 hover:bg-slate-800 hover:border-slate-700"
                />
              }
            >
              <Bell size={19} />
            </TooltipTrigger>

            <TooltipContent>Notificaciones</TooltipContent>
          </Tooltip>

          <DropdownMenu>
            <DropdownMenuTrigger
              render={
                <Button
                  variant="ghost"
                  className="h-auto rounded-2xl border border-slate-800 bg-slate-900/60 px-3 py-2 hover:bg-slate-800"
                />
              }
            >
              <Avatar className="h-9 w-9">
                <AvatarFallback className="bg-primary text-primary-foreground">
                  {iniciales}
                </AvatarFallback>
              </Avatar>

              <div className="hidden min-w-0 text-left sm:block">
                <p className="truncate text-sm font-semibold">
                  {usuario?.nombre ?? 'Invitado'}
                </p>

                <p className="truncate text-xs text-muted-foreground">
                  {usuario?.rol ?? 'Sin sesión'}
                </p>
              </div>
            </DropdownMenuTrigger>

            <DropdownMenuContent align="end" className="w-56">
              <DropdownMenuGroup>
                <DropdownMenuLabel>
                  <p className="font-semibold">
                    {usuario
                      ? `${usuario.nombre} ${usuario.apellidos}`
                      : 'NexusCommerce'}
                  </p>

                  <p className="mt-1 text-xs font-normal text-muted-foreground">
                    {usuario?.correo ?? 'No autenticado'}
                  </p>
                </DropdownMenuLabel>
              </DropdownMenuGroup>

              <DropdownMenuSeparator />

              <DropdownMenuGroup>
                <DropdownMenuItem>
                  <UserRound size={16} />
                  Perfil
                </DropdownMenuItem>

                <DropdownMenuItem
                  onClick={() => navigate('/configuracion')}
                >
                  <Settings size={16} />
                  Configuración
                </DropdownMenuItem>
              </DropdownMenuGroup>

              <DropdownMenuSeparator />

              <DropdownMenuGroup>
                <DropdownMenuItem
                  variant="destructive"
                  onClick={cerrarSesion}
                >
                  <LogOut size={16} />
                  Cerrar sesión
                </DropdownMenuItem>
              </DropdownMenuGroup>
            </DropdownMenuContent>
          </DropdownMenu>
        </div>
      </div>
    </header>
  )
}
