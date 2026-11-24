import { afterEach, describe, expect, it, vi } from 'vitest'
import { apiClient } from '../../../shared/api/apiClient'
import { createOrder } from './ordersApi'

describe('createOrder', () => {
  afterEach(() => vi.restoreAllMocks())

  it('solo envía producto y cantidad: ni cliente ni precio', async () => {
    const post = vi.spyOn(apiClient, 'post').mockResolvedValue({
      data: { pedidoId: 'p1', total: 50, fechaCreacionUtc: '2026-11-24T10:00:00Z' },
    })

    await createOrder({ lineas: [{ productoId: 'prod-1', cantidad: 2 }] })

    expect(post).toHaveBeenCalledWith('/api/pedidos', {
      lineas: [{ productoId: 'prod-1', cantidad: 2 }],
    })
    const cuerpo = JSON.stringify(post.mock.calls[0][1])
    expect(cuerpo).not.toMatch(/clienteId|precioUnitario|nombreProducto/)
  })
})
