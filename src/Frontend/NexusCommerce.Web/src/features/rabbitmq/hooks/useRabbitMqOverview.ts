import { useQuery } from '@tanstack/react-query'
import { getRabbitMqOverview } from '../api/rabbitMqApi'

export function useRabbitMqOverview() {
  return useQuery({
    queryKey: ['rabbitmq-overview'],
    queryFn: getRabbitMqOverview,
    refetchInterval: 10_000,
    refetchIntervalInBackground: true,
    staleTime: 5_000,
    retry: 2,
  })
}
