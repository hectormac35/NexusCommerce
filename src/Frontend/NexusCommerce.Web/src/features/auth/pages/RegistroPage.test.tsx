import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import { afterEach, describe, expect, it, vi } from 'vitest'
import { apiClient } from '../../../shared/api/apiClient'
import { useAuthStore } from '../store/authStore'
import { RegistroPage } from './RegistroPage'

function renderizar() {
  render(
    <QueryClientProvider client={new QueryClient()}>
      <MemoryRouter initialEntries={['/registro']}>
        <Routes>
          <Route path="/registro" element={<RegistroPage />} />
          <Route path="/" element={<p>Inicio</p>} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>,
  )
}

function rellenar(datos: Record<string, string>) {
  for (const [id, valor] of Object.entries(datos)) {
    fireEvent.change(screen.getByLabelText(id), { target: { value: valor } })
  }
}

describe('RegistroPage', () => {
  afterEach(() => {
    vi.restoreAllMocks()
    useAuthStore.getState().limpiarSesion()
  })

  it('no envía nada si la contraseña no cumple las reglas', async () => {
    const post = vi.spyOn(apiClient, 'post')
    renderizar()

    rellenar({
      Nombre: 'Ana',
      Apellidos: 'García',
      'Correo electrónico': 'ana@correo.com',
      Contraseña: 'corta',
      'Repite la contraseña': 'corta',
    })
    fireEvent.click(screen.getByRole('button', { name: 'Crear cuenta' }))

    expect(await screen.findByText('Mínimo 10 caracteres')).toBeInTheDocument()
    expect(post).not.toHaveBeenCalled()
  })

  it('registra la cuenta, inicia sesión y va al inicio', async () => {
    const post = vi.spyOn(apiClient, 'post').mockImplementation(async (url: string) => {
      if (url.endsWith('/registro')) return { data: {} }
      return {
        data: {
          accessToken: 'a',
          refreshToken: 'r',
          expiraEnSegundos: 900,
          usuario: { id: '1', nombre: 'Ana', apellidos: 'García', correo: 'ana@correo.com', rol: 'Cliente' },
        },
      }
    })
    renderizar()

    rellenar({
      Nombre: 'Ana',
      Apellidos: 'García',
      'Correo electrónico': 'ana@correo.com',
      Contraseña: 'Segura.2026!',
      'Repite la contraseña': 'Segura.2026!',
    })
    fireEvent.click(screen.getByRole('button', { name: 'Crear cuenta' }))

    expect(await screen.findByText('Inicio')).toBeInTheDocument()
    await waitFor(() => expect(useAuthStore.getState().usuario?.rol).toBe('Cliente'))
    expect(post.mock.calls.map((llamada) => llamada[0])).toEqual([
      '/api/autenticacion/registro',
      '/api/autenticacion/login',
    ])
  })
})
