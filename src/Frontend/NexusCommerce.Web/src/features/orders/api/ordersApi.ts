import { apiClient } from '../../../shared/api/apiClient'
import type { Order, OrderDetail } from '../types/order'

// El cliente solo envía qué producto y cuántas unidades.
// El usuario sale del token y el nombre/precio los pone Orders.Api desde el catálogo.
export type CreateOrderLineRequest = {
  productoId: string
  cantidad: number
}

export type CreateOrderRequest = {
  lineas: CreateOrderLineRequest[]
}

export type CreateOrderResponse = {
  pedidoId: string
  total: number
  fechaCreacionUtc: string
}

export async function getOrders(): Promise<Order[]> {
  const response = await apiClient.get<Order[]>(
    '/api/pedidos'
  )

  return response.data
}

export async function getOrderById(
  orderId: string
): Promise<OrderDetail> {
  const response = await apiClient.get<OrderDetail>(
    `/api/pedidos/${orderId}`
  )

  return response.data
}

export async function createOrder(
  request: CreateOrderRequest
): Promise<CreateOrderResponse> {
  const response =
    await apiClient.post<CreateOrderResponse>(
      '/api/pedidos',
      request
    )

  return response.data
}

export type ChangeOrderStatusRequest = {
  nuevoEstado: string
}

export async function changeOrderStatus(
  orderId: string,
  request: ChangeOrderStatusRequest,
): Promise<void> {
  await apiClient.patch(
    `/api/pedidos/${orderId}/estado`,
    request,
  )
}
