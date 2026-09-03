import { useMutation, useQueryClient } from '@tanstack/react-query'
import {
  updateProduct,
  type UpdateProductRequest,
} from '../api/catalogApi'

export function useUpdateProduct() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({
      productId,
      request,
    }: {
      productId: string
      request: UpdateProductRequest
    }) => updateProduct(productId, request),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['products'],
      })
    },
  })
}
