import { apiClient } from '../../../shared/api/apiClient'
import type { RabbitMqOverview } from '../types/rabbitMq'

export async function getRabbitMqOverview(): Promise<RabbitMqOverview> {
  const response = await apiClient.get<RabbitMqOverview>(
    '/api/platform/rabbitmq',
  )

  return response.data
}
