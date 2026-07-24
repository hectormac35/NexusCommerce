import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { MainLayout } from '../layouts/MainLayout'
import { LoginPage } from '../../features/auth/pages/LoginPage'
import { CatalogPage } from '../../features/catalog/pages/CatalogPage'
import { HomePage } from '../../features/dashboard/pages/HomePage'

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<MainLayout />}>
          <Route index element={<HomePage />} />
          <Route path="catalogo" element={<CatalogPage />} />
          <Route path="acceso" element={<LoginPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
