export type JaegerTrace = {
  traceId: string
  rootService: string
  operation: string
  durationMs: number
  spanCount: number
  services: string[]
  hasError: boolean
  startedAt: string
}

export type JaegerOverview = {
  status: string

  serviceCount: number
  traceCount: number
  errorCount: number

  averageDurationMs: number

  checkedAt: string

  services: string[]
  recentTraces: JaegerTrace[]
}
