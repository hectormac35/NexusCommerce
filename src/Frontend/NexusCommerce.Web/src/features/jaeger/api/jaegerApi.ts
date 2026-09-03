import { apiClient } from '../../../shared/api/apiClient'
import type { JaegerOverview } from '../types/jaeger'

export async function getJaegerOverview(): Promise<JaegerOverview> {
  const response = await apiClient.get<JaegerOverview>(
    '/api/platform/jaeger',
  )

  return response.data
}
