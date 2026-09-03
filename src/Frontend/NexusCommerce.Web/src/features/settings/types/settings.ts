export type MonitoringInterval =
  | 0
  | 5_000
  | 10_000
  | 30_000
  | 60_000

export type SettingsState = {
  monitoringInterval: MonitoringInterval
  refreshInBackground: boolean
}
