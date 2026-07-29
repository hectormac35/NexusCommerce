import { apiClient } from '../../../shared/api/apiClient'
import type { PlatformHealthResponse } from '../types/platformHealth'

export async function getPlatformHealth(): Promise<PlatformHealthResponse> {
  const response = await apiClient.get<PlatformHealthResponse>(
    '/api/platform/health',
  )

  return response.data
}
