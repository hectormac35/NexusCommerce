import { useMutation, useQueryClient } from '@tanstack/react-query'
import {
  changeOrderStatus,
  type ChangeOrderStatusRequest,
} from '../api/ordersApi'

type ChangeOrderStatusVariables = {
  orderId: string
  request: ChangeOrderStatusRequest
}

export function useChangeOrderStatus() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({
      orderId,
      request,
    }: ChangeOrderStatusVariables) =>
      changeOrderStatus(orderId, request),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['orders'],
      })
    },
  })
}
