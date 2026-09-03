import { useQuery } from '@tanstack/react-query'
import { getJaegerOverview } from '../api/jaegerApi'

export function useJaegerOverview() {
  return useQuery({
    queryKey: ['jaeger-overview'],
    queryFn: getJaegerOverview,
    refetchInterval: 10_000,
    refetchIntervalInBackground: true,
    staleTime: 5_000,
    retry: 2,
  })
}
