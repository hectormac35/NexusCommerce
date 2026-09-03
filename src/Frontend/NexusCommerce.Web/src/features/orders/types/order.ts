export interface Order {
  pedidoId: string
  clienteId: string
  estado: string
  total: number
  fechaCreacionUtc: string
}

export interface OrderLine {
  productoId: string
  nombreProducto: string
  precioUnitario: number
  cantidad: number
  subtotal: number
}

export interface OrderDetail extends Order {
  lineas: OrderLine[]
}
