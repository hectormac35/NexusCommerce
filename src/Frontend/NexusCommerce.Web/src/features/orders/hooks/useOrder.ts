import { useQuery } from '@tanstack/react-query'
import { getOrderById } from '../api/ordersApi'

export function useOrder(orderId: string | undefined) {
  return useQuery({
    queryKey: ['orders', orderId],
    queryFn: () => getOrderById(orderId!),
    enabled: Boolean(orderId),
  })
}
