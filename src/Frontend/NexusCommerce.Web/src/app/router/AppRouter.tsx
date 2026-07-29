import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { MainLayout } from '../layouts/MainLayout'
import { LoginPage } from '../../features/auth/pages/LoginPage'
import { CatalogPage } from '../../features/catalog/pages/CatalogPage'
import { PlatformHealthPage } from '../../features/platform-health/pages/PlatformHealthPage'
import { HomePage } from '../../features/dashboard/pages/HomePage'
import { UsersPage } from '../../features/users/pages/UsersPage'

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<MainLayout />}>
          <Route index element={<HomePage />} />
          <Route path="catalogo" element={<CatalogPage />} />
          <Route path="usuarios" element={<UsersPage />} />
          <Route path="plataforma" element={<PlatformHealthPage />} />
          <Route path="acceso" element={<LoginPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
