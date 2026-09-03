import { apiClient } from '../../../shared/api/apiClient'
import type { Product } from '../types/product'

export type CreateProductRequest = {
  nombre: string
  descripcion: string
  precio: number
  stock: number
  categoria: string
}

export type UpdateProductRequest = {
  nombre: string
  descripcion: string
  precio: number
  categoria: string
}

export async function getProducts(): Promise<Product[]> {
  const response = await apiClient.get<Product[]>(
    '/api/catalogo/productos'
  )

  return response.data
}

export async function createProduct(
  request: CreateProductRequest
): Promise<void> {
  await apiClient.post(
    '/api/catalogo/productos',
    request
  )
}

export async function updateProduct(
  productId: string,
  request: UpdateProductRequest
): Promise<void> {
  await apiClient.put(
    `/api/catalogo/productos/${productId}`,
    request
  )
}

export async function deleteProduct(
  productId: string
): Promise<void> {
  await apiClient.delete(
    `/api/catalogo/productos/${productId}`
  )
}
