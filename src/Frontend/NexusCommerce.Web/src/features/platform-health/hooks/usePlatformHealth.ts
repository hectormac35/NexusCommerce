import { useQuery } from '@tanstack/react-query'
import { getPlatformHealth } from '../api/platformHealthApi'

export function usePlatformHealth() {
  return useQuery({
    queryKey: ['platform-health'],
    queryFn: getPlatformHealth,
    refetchInterval: 10_000,
    refetchIntervalInBackground: true,
    staleTime: 5_000,
    retry: 2,
  })
}
