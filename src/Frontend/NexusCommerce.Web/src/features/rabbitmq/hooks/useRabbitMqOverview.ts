import { useQuery } from '@tanstack/react-query'

import { useSettingsStore } from '@/features/settings/store/settingsStore'
import { getRabbitMqOverview } from '../api/rabbitMqApi'

export function useRabbitMqOverview() {
  const monitoringInterval = useSettingsStore(
    (state) => state.monitoringInterval,
  )

  const refreshInBackground = useSettingsStore(
    (state) => state.refreshInBackground,
  )

  return useQuery({
    queryKey: ['rabbitmq-overview'],
    queryFn: getRabbitMqOverview,
    refetchInterval:
      monitoringInterval === 0
        ? false
        : monitoringInterval,
    refetchIntervalInBackground: refreshInBackground,
    staleTime: 5_000,
    retry: 2,
  })
}
