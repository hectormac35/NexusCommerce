import { useMutation, useQueryClient } from '@tanstack/react-query'
import { deleteProduct } from '../api/catalogApi'

export function useDeleteProduct() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (productId: string) =>
      deleteProduct(productId),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['products'],
      })
    },
  })
}
