import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { MainLayout } from '../layouts/MainLayout'
import { LoginPage } from '../../features/auth/pages/LoginPage'
import { CatalogPage } from '../../features/catalog/pages/CatalogPage'
import { PlatformHealthPage } from '../../features/platform-health/pages/PlatformHealthPage'
import { RabbitMqPage } from '../../features/rabbitmq/pages/RabbitMqPage'
import { JaegerPage } from '../../features/jaeger/pages/JaegerPage'
import { SettingsPage } from '../../features/settings/pages/SettingsPage'
import { HomePage } from '../../features/dashboard/pages/HomePage'
import { UsersPage } from '../../features/users/pages/UsersPage'
import { OrdersPage } from '../../features/orders/pages/OrdersPage'
import { OrderDetailPage } from '../../features/orders/pages/OrderDetailPage'

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<MainLayout />}>
          <Route index element={<HomePage />} />
          <Route path="catalogo" element={<CatalogPage />} />
          <Route path="usuarios" element={<UsersPage />} />
          <Route path="pedidos" element={<OrdersPage />} />
          <Route path="pedidos/:pedidoId" element={<OrderDetailPage />} />
          <Route path="plataforma" element={<PlatformHealthPage />} />
          <Route path="rabbitmq" element={<RabbitMqPage />} />
          <Route path="trazas" element={<JaegerPage />} />
          <Route path="configuracion" element={<SettingsPage />} />
          <Route path="acceso" element={<LoginPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
