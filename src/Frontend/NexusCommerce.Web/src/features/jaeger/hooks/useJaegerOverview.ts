import { useQuery } from '@tanstack/react-query'

import { useSettingsStore } from '@/features/settings/store/settingsStore'
import { getJaegerOverview } from '../api/jaegerApi'

export function useJaegerOverview() {
  const monitoringInterval = useSettingsStore(
    (state) => state.monitoringInterval,
  )

  const refreshInBackground = useSettingsStore(
    (state) => state.refreshInBackground,
  )

  return useQuery({
    queryKey: ['jaeger-overview'],
    queryFn: getJaegerOverview,
    refetchInterval:
      monitoringInterval === 0
        ? false
        : monitoringInterval,
    refetchIntervalInBackground: refreshInBackground,
    staleTime: 5_000,
    retry: 2,
  })
}
