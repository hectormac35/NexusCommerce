import { useQuery } from '@tanstack/react-query'

import { useSettingsStore } from '@/features/settings/store/settingsStore'
import { getPlatformHealth } from '../api/platformHealthApi'

export function usePlatformHealth() {
  const monitoringInterval = useSettingsStore(
    (state) => state.monitoringInterval,
  )

  const refreshInBackground = useSettingsStore(
    (state) => state.refreshInBackground,
  )

  return useQuery({
    queryKey: ['platform-health'],
    queryFn: getPlatformHealth,
    refetchInterval:
      monitoringInterval === 0
        ? false
        : monitoringInterval,
    refetchIntervalInBackground: refreshInBackground,
    staleTime: 5_000,
    retry: 2,
  })
}
