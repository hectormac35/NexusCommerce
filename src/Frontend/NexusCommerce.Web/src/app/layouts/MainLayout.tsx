import { Outlet } from 'react-router-dom'
import { Sidebar } from '../../shared/components/layout/Sidebar'
import { Topbar } from '../../shared/components/layout/Topbar'

export function MainLayout() {
  return (
    <div className="min-h-screen bg-slate-950 text-slate-100">
      <Sidebar />

      <div className="lg:pl-72">
        <Topbar />

        <main className="px-6 py-8 lg:px-8">
          <Outlet />
        </main>
      </div>
    </div>
  )
}
