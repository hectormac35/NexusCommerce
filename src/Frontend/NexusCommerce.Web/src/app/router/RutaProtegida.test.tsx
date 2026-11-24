import { render, screen } from '@testing-library/react'
import { MemoryRouter, Route, Routes, useLocation } from 'react-router-dom'
import { beforeEach, describe, expect, it } from 'vitest'
import { useAuthStore } from '../../features/auth/store/authStore'
import { RutaProtegida } from './RutaProtegida'

function PantallaAcceso() {
  const location = useLocation()
  const desde = (location.state as { desde?: string } | null)?.desde
  return <p>Acceso (desde {desde})</p>
}

function renderizarEn(ruta: string) {
  render(
    <MemoryRouter initialEntries={[ruta]}>
      <Routes>
        <Route path="/acceso" element={<PantallaAcceso />} />
        <Route element={<RutaProtegida />}>
          <Route path="/pedidos" element={<p>Pedidos privados</p>} />
        </Route>
      </Routes>
    </MemoryRouter>,
  )
}

describe('RutaProtegida', () => {
  beforeEach(() => useAuthStore.getState().limpiarSesion())

  it('manda al login a quien no tiene sesión y recuerda adónde iba', () => {
    renderizarEn('/pedidos?pagina=2')

    expect(screen.queryByText('Pedidos privados')).not.toBeInTheDocument()
    expect(screen.getByText('Acceso (desde /pedidos?pagina=2)')).toBeInTheDocument()
  })

  it('deja pasar a quien ha iniciado sesión', () => {
    useAuthStore.getState().establecerSesion({
      accessToken: 'a',
      refreshToken: 'r',
      expiraEnSegundos: 900,
      usuario: {
        id: '1',
        nombre: 'Ana',
        apellidos: 'López',
        correo: 'ana@nexus.test',
        rol: 'Cliente',
      },
    })

    renderizarEn('/pedidos')

    expect(screen.getByText('Pedidos privados')).toBeInTheDocument()
  })
})
