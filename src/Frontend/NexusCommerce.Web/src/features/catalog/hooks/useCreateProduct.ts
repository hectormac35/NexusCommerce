import { useMutation, useQueryClient } from '@tanstack/react-query'
import {
  createProduct,
  type CreateProductRequest,
} from '../api/catalogApi'

export function useCreateProduct() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (request: CreateProductRequest) =>
      createProduct(request),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['products'],
      })
    },
  })
}
