import { lazy, Suspense } from 'react'
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { MainLayout } from '../layouts/MainLayout'
import { LoginPage } from '../../features/auth/pages/LoginPage'
import { RutaProtegida } from './RutaProtegida'
import { RutaSoloAdministrador } from './RutaSoloAdministrador'
import { RegistroPage } from '../../features/auth/pages/RegistroPage'

// Cada página se descarga cuando se visita por primera vez (bundle inicial más pequeño).
const HomePage = lazy(() =>
  import('../../features/dashboard/pages/HomePage').then((m) => ({ default: m.HomePage })))
const CatalogPage = lazy(() =>
  import('../../features/catalog/pages/CatalogPage').then((m) => ({ default: m.CatalogPage })))
const UsersPage = lazy(() =>
  import('../../features/users/pages/UsersPage').then((m) => ({ default: m.UsersPage })))
const OrdersPage = lazy(() =>
  import('../../features/orders/pages/OrdersPage').then((m) => ({ default: m.OrdersPage })))
const OrderDetailPage = lazy(() =>
  import('../../features/orders/pages/OrderDetailPage').then((m) => ({ default: m.OrderDetailPage })))
const PlatformHealthPage = lazy(() =>
  import('../../features/platform-health/pages/PlatformHealthPage').then((m) => ({ default: m.PlatformHealthPage })))
const RabbitMqPage = lazy(() =>
  import('../../features/rabbitmq/pages/RabbitMqPage').then((m) => ({ default: m.RabbitMqPage })))
const JaegerPage = lazy(() =>
  import('../../features/jaeger/pages/JaegerPage').then((m) => ({ default: m.JaegerPage })))
const SettingsPage = lazy(() =>
  import('../../features/settings/pages/SettingsPage').then((m) => ({ default: m.SettingsPage })))

function CargandoPagina() {
  return (
    <div className="flex h-64 items-center justify-center text-sm text-slate-500">
      Cargando…
    </div>
  )
}

export function AppRouter() {
  return (
    <BrowserRouter>
      <Suspense fallback={<CargandoPagina />}>
        <Routes>
          <Route
            path="acceso"
            element={
              <div className="flex min-h-screen items-center bg-slate-950 px-4 text-slate-100">
                <LoginPage />
              </div>
            }
          />

          <Route
            path="registro"
            element={
              <div className="flex min-h-screen items-center bg-slate-950 px-4 py-10 text-slate-100">
                <RegistroPage />
              </div>
            }
          />

          <Route element={<RutaProtegida />}>
            <Route element={<MainLayout />}>
              <Route index element={<HomePage />} />
              <Route path="catalogo" element={<CatalogPage />} />
              <Route element={<RutaSoloAdministrador />}>
                <Route path="usuarios" element={<UsersPage />} />
              </Route>
              <Route path="pedidos" element={<OrdersPage />} />
              <Route path="pedidos/:pedidoId" element={<OrderDetailPage />} />
              <Route path="plataforma" element={<PlatformHealthPage />} />
              <Route path="rabbitmq" element={<RabbitMqPage />} />
              <Route path="trazas" element={<JaegerPage />} />
              <Route path="configuracion" element={<SettingsPage />} />
            </Route>
          </Route>

          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  )
}
