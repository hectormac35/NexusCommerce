import { apiClient } from '../../../shared/api/apiClient'
import type { Order, OrderDetail } from '../types/order'

export type CreateOrderLineRequest = {
  productoId: string
  nombreProducto: string
  precioUnitario: number
  cantidad: number
}

export type CreateOrderRequest = {
  clienteId: string
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
