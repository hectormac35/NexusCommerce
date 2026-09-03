import { create } from 'zustand'
import { persist } from 'zustand/middleware'

import type {
  MonitoringInterval,
  SettingsState,
} from '../types/settings'

type SettingsActions = {
  setMonitoringInterval: (
    interval: MonitoringInterval,
  ) => void
  setRefreshInBackground: (
    enabled: boolean,
  ) => void
  resetSettings: () => void
}

type SettingsStore = SettingsState & SettingsActions

const defaultSettings: SettingsState = {
  monitoringInterval: 10_000,
  refreshInBackground: true,
}

export const useSettingsStore =
  create<SettingsStore>()(
    persist(
      (set) => ({
        ...defaultSettings,

        setMonitoringInterval: (monitoringInterval) =>
          set({ monitoringInterval }),

        setRefreshInBackground: (refreshInBackground) =>
          set({ refreshInBackground }),

        resetSettings: () =>
          set(defaultSettings),
      }),
      {
        name: 'nexuscommerce-settings',
      },
    ),
  )
