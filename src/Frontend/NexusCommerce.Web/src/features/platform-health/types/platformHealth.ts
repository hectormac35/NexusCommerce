export type PlatformHealthStatus =
  | 'Healthy'
  | 'Degraded'
  | 'Unhealthy'
  | 'Unavailable'

export type ServiceHealth = {
  name: string
  status: PlatformHealthStatus
  responseTimeMs: number
}

export type PlatformHealthResponse = {
  status: PlatformHealthStatus
  checkedAt: string
  services: ServiceHealth[]
}
